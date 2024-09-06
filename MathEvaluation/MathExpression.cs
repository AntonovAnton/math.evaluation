﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Cryptography;
using MathEvaluation.Context;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;

namespace MathEvaluation;

/// <summary>
/// Compiles any mathematical expression string to the <see cref="Func{T, TResult}"/> with parameters or to the <see cref="Func{TResult}"/>.
/// </summary>
public partial class MathExpression
{
    private static readonly Expression DoubleZero = Expression.Constant(0.0);
    private static readonly Expression DoubleOne = Expression.Constant(1.0);

    private readonly NumberFormatInfo? _numberFormat;

    private ParameterExpression? _parameterExpression;
    private IMathParameters? _parameters;

    /// <summary>Gets the math expression string.</summary>
    /// <value>The math expression string.</value>
    public string MathString { get; }

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext" /> interface.</value>
    public IMathContext? Context { get; }

    /// <summary>Gets the specified format provider.</summary>
    /// <value>The specified format provider.</value>
    public IFormatProvider? Provider { get; }

    /// <summary>Gets the expression tree.</summary>
    /// <value>The expression tree.</value>
    public Expression? ExpressionTree { get; private set; }

    /// <summary>Initializes a new instance of the <see cref="MathExpression" /> class.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="context">The math context.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <exception cref="System.ArgumentNullException">mathString</exception>
    /// <exception cref="System.ArgumentException">Expression string is empty or white space. - mathString</exception>
    public MathExpression(string mathString, IMathContext? context = null, IFormatProvider? provider = null)
    {
        if (mathString == null)
            throw new ArgumentNullException(nameof(mathString));

        if (string.IsNullOrWhiteSpace(mathString))
            throw new ArgumentException("Expression string is empty or white space.", nameof(mathString));

        MathString = mathString;
        Context = context;
        Provider = provider;
        _numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;
    }

    /// <summary>Compiles the <see cref="MathString">math expression string</see>.</summary>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="MathEvaluationException"/>
    public Func<double> Compile()
    {
        try
        {
            var i = 0;
            ExpressionTree = Build(MathString, ref i, null, null, (int)EvalPrecedence.Unknown, false, DoubleZero);

            return Expression.Lambda<Func<double>>(ExpressionTree).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, null);
        }
    }

    /// <inheritdoc cref="Compile()"/>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException">parameters</exception>
    public Func<T, double> Compile<T>(T parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        try
        {
            _parameters = new MathParameters();
            _parameters.Bind(parameters);

            _parameterExpression = Expression.Parameter(typeof(T), nameof(parameters));

            var i = 0;
            ExpressionTree = Build(MathString, ref i, null, null, (int)EvalPrecedence.Unknown, false, DoubleZero);

            return Expression.Lambda<Func<T, double>>(ExpressionTree, _parameterExpression).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }

    private Expression Build(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand, Expression expression)
    {
        var decimalSeparator = _numberFormat?.NumberDecimalSeparator?.Length > 0 ? _numberFormat.NumberDecimalSeparator[0] : '.';

        var start = i;
        while (mathString.Length > i)
        {
            if (separator.HasValue && mathString.IsParamsSeparator(start, i, separator.Value, decimalSeparator) ||
                closingSymbol.HasValue && mathString[i] == closingSymbol.Value)
            {
                if (expression == DoubleZero)
                    mathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return expression;
            }

            if (mathString[i] is >= '0' and <= '9' || mathString[i] == decimalSeparator) //number
            {
                if (isOperand)
                    return Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Function, false, DoubleZero);

                expression = Expression.Constant(mathString.ParseNumber(_numberFormat, ref i));
                continue;
            }

            switch (mathString[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return expression;

                    var startParenthesis = i;
                    i++;
                    var right = Build(mathString, ref i, null, ')', (int)EvalPrecedence.Unknown, false, DoubleZero);
                    mathString.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return right;

                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right);
                    break;
                case '+' when mathString.Length == i + 1 || mathString[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return expression;

                    i++;
                    right = Build(mathString, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand, DoubleZero);
                    expression = Expression.Add(expression, right);
                    if (isOperand)
                        return expression;
                    break;
                case '-' when mathString.Length == i + 1 || mathString[i + 1] != '-':
                    if (precedence >=(int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return expression;

                    var isNegativity = start == i;
                    i++;
                    right = Build(mathString, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand, DoubleZero);
                    expression = isNegativity ? Expression.Negate(right) : Expression.Subtract(expression, right); //it keeps sign
                    if (isOperand)
                        return expression;
                    break;
                case '*' when mathString.Length == i + 1 || mathString[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, false, DoubleZero);
                    expression = Expression.Multiply(expression, right);
                    break;
                case '/' when mathString.Length == i + 1 || mathString[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, false, DoubleZero);
                    expression = Expression.Divide(expression, right);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = Context?.FirstMathEntity(mathString[i..]) ?? _parameters?.FirstMathEntity(mathString[i..]);
                    if (entity == null && _numberFormat != null && mathString.TryParseCurrency(_numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return expression;

                    expression = BuildMathEntity(mathString, ref i, separator, closingSymbol, precedence, expression, entity);
                    if (isOperand)
                        return expression;
                    break;
            }
        }

        if (expression == DoubleZero)
            mathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return expression;
    }

    private Expression BuildOperand(ReadOnlySpan<char> mathString, ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var expression = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true, DoubleZero);
        if (expression == DoubleZero)
            mathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return expression;
    }

    private Expression BuildMathEntity(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, int precedence, Expression expression, IMathEntity? entity, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryBuildEntity(mathString, entity, ref i, separator, closingSymbol, ref expression))
                return expression;

            Expression convertExpression = expression == DoubleZero ? DecimalZero : Expression.Convert(expression, typeof(decimal));
            if (TryBuildEntityDecimal(mathString, entity, ref i, separator, closingSymbol, ref convertExpression))
                return Expression.Convert(convertExpression, typeof(double));
        }

        if (throwError)
            mathString.ThrowExceptionInvalidToken(i);

        return expression;
    }

    private Expression BuildExponentiation(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, Expression expression)
    {
        mathString.SkipMeaningless(ref i);
        if (mathString.Length <= i)
            return expression;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = Context?.FirstMathEntity(mathString[i..]) ?? _parameters?.FirstMathEntity(mathString[i..]);
        return BuildMathEntity(mathString, ref i, separator, closingSymbol, precedence, expression, entity, false);
    }

    private bool TryBuildEntity(ReadOnlySpan<char> mathString, IMathEntity entity,
        ref int i, char? separator, char? closingSymbol, ref Expression expression)
    {
        var start = i;
        switch (entity)
        {
            case MathConstant<double> mathConstant:
                {
                    i += entity.Key.Length;
                    Expression right = Expression.Constant(mathConstant.Value);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    Expression right = Expression.Property(_parameterExpression, entity.Key);
                    if (right.Type != typeof(double))
                        right = Expression.Convert(right, typeof(double));
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double>> lambda = v => mathOperator.Fn(v);
                    if (mathOperator.IsProcessingLeft)
                        expression = Expression.Invoke(lambda, expression);
                    else
                    {
                        var right = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, false, DoubleZero);
                        expression = Expression.Invoke(lambda, right);
                    }
                    expression = BuildExponentiation(mathString, ref i, separator, closingSymbol, expression);
                    return true;
                }
            case MathOperandsOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double, double>> lambda = (v1, v2) => mathOperator.Fn(v1, v2);
                    var right = BuildOperand(mathString, ref i, separator, closingSymbol);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = Expression.Invoke(lambda, expression, right);
                    return true;
                }
            case MathOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double, double>> lambda = (v1, v2) => mathOperator.Fn(v1, v2);
                    var right = Build(mathString, ref i, separator, closingSymbol, mathOperator.Precedence, false, DoubleZero);
                    expression = Expression.Invoke(lambda, expression, right);
                    return true;
                }
            case MathGetValueFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.SkipParenthesis(ref i);

                    Expression<Func<double>> lambda = () => mathFunction.Fn();
                    Expression right = Expression.Invoke(lambda);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            case MathUnaryFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    if (mathFunction.OpeningSymbol.HasValue)
                        mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol.Value, start, ref i);

                    var arg = mathFunction.ClosingSymbol.HasValue
                        ? Build(mathString, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown, false, DoubleZero)
                        : BuildOperand(mathString, ref i, separator, closingSymbol);

                    if (mathFunction.ClosingSymbol.HasValue)
                        mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol.Value, start, ref i);

                    Expression<Func<double, double>> lambda = (value) => mathFunction.Fn(value);
                    Expression right = Expression.Invoke(lambda, arg);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            case MathFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol, start, ref i);

                    var args = new List<Expression>();
                    while (mathString.Length > i)
                    {
                        var arg = Build(mathString, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown, false, DoubleZero);
                        args.Add(arg);

                        if (mathString[i] == mathFunction.Separator)
                        {
                            i++; //other param
                            continue;
                        }
                        break;
                    }

                    mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol, start, ref i);

                    Expression<Func<double[], double>> lambda = (values) => mathFunction.Fn(values);
                    Expression right = Expression.Invoke(lambda, Expression.NewArrayInit(typeof(double), args));
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            default:
                return false;
        }
    }

    private static MathEvaluationException CreateException(Exception ex,
        string mathString, IMathContext? context, IFormatProvider? provider, object? parameters)
    {
        ex = ex is not MathEvaluationException ? new MathEvaluationException(ex.Message, ex) : ex;
        ex.Data[nameof(mathString)] = mathString;
        ex.Data[nameof(context)] = context;
        ex.Data[nameof(provider)] = provider;
        ex.Data[nameof(parameters)] = parameters;
        throw ex;
    }
}