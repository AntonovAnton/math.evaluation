using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using MathEvaluation.Context;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;

namespace MathEvaluation;

/// <summary>
/// Compiles any mathematical expression string to the <see cref="Func{T, TResult}"/>.
/// </summary>
public class MathExpression
{
    private static readonly Expression DoubleZero = Expression.Constant(0.0);

    private ParameterExpression? _parameterExpression;
    private IMathParameters? _parameters;
    private NumberFormatInfo? _numberFormat;

    /// <summary>Gets the math expression string.</summary>
    /// <value>The math expression string.</value>
    public string MathString { get; }

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext" /> interface.</value>
    public IMathContext? Context { get; }

    /// <summary>Gets the expression tree.</summary>
    /// <value>The expression tree.</value>
    public Expression? ExpressionTree { get; private set; }

    /// <summary>Initializes a new instance of the <see cref="MathExpression" /> class.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="context">The math context.</param>
    /// <exception cref="System.ArgumentNullException">mathString</exception>
    /// <exception cref="System.ArgumentException">Expression string is empty or white space. - mathString</exception>
    public MathExpression(string mathString, IMathContext? context = null)
    {
        if (mathString == null)
            throw new ArgumentNullException(nameof(mathString));

        if (string.IsNullOrWhiteSpace(mathString))
            throw new ArgumentException("Expression string is empty or white space.", nameof(mathString));

        MathString = mathString;
        Context = context;
    }

    /// <summary>Compiles the <see cref="MathString">math expression string</see>.</summary>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException">expression</exception>
    /// <exception cref="MathEvaluationException">expression</exception>
    public Func<T, double> Compile<T>(IFormatProvider? provider = null)
        => Compile<T>(default, provider);

    /// <summary>Compiles the <see cref="MathString">math expression string</see>.</summary>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException">expression</exception>
    /// <exception cref="MathEvaluationException">expression</exception>
    public Func<T, double> Compile<T>(T? parameters, IFormatProvider? provider = null)
    {
        try
        {
            if (parameters != null)
                _parameters = new MathParameters(parameters);

            _parameterExpression = Expression.Parameter(typeof(T), nameof(parameters));
            _numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;

            var i = 0;
            ExpressionTree = Build(MathString, ref i, null, null, (int)EvalPrecedence.Unknown, false, DoubleZero);

            return Expression.Lambda<Func<T, double>>(ExpressionTree, _parameterExpression).Compile();
        }
        catch (Exception ex)
        {
            ex = ex is not MathEvaluationException ? new MathEvaluationException(ex.Message, ex) : ex;
            ex.Data[nameof(MathString)] = MathString;
            ex.Data[nameof(Context)] = Context;
            ex.Data[nameof(provider)] = provider;
            throw ex;
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
                    var left = expression is ConstantExpression c && (double)c.Value == 0.0 ? Expression.Constant(1.0) : expression;
                    expression = Expression.Multiply(left, right);
                    break;
                case '+' when mathString.Length == i + 1 || mathString[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && start != i && mathString[start..i].IsNotMeaningless())
                        return expression;

                    i++;
                    right = Build(mathString, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand, DoubleZero);
                    expression = Expression.Add(expression, right);
                    if (isOperand)
                        return expression;
                    break;
                case '-' when mathString.Length == i + 1 || mathString[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && mathString[start..i].IsNotMeaningless())
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
                    right = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, false, expression);
                    expression = Expression.Multiply(expression, right);
                    break;
                case '/' when mathString.Length == i + 1 || mathString[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, false, expression);
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

            //var decimalValue = (decimal)expression;
            //if (TryBuildContextDecimal(mathString, entity, _numberFormat, ref i, separator, closingSymbol, ref decimalValue))
            //    return (double)decimalValue;
        }

        if (!throwError)
            return expression;

        var end = mathString[i..].IndexOfAny("(0123456789.,٫+-*/ \t\n\r") + i;
        var unknownSubstring = end > i ? mathString[i..end] : mathString[i..];

        throw new MathEvaluationException($"'{unknownSubstring.ToString()}' is not recognizable.", i);
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
                    var left = expression == DoubleZero ? Expression.Constant(1.0) : expression;
                    expression = Expression.Multiply(left, right);
                    return true;
                }
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    Expression right = Expression.Constant(mathVariable.Value);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    var left = expression == DoubleZero ? Expression.Constant(1.0) : expression;
                    expression = Expression.Multiply(left, right);
                    return true;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double>> lambda = arg => mathOperator.Fn(arg);
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
                    Expression<Func<double, double, double>> lambda = (arg1, arg2) => mathOperator.Fn(arg1, arg2);
                    var rightOperand = BuildOperand(mathString, ref i, separator, closingSymbol);
                    rightOperand = BuildExponentiation(mathString, ref i, separator, closingSymbol, rightOperand);
                    expression = Expression.Invoke(lambda, expression, rightOperand);
                    return true;
                }
            case MathOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double, double>> lambda = (arg1, arg2) => mathOperator.Fn(arg1, arg2);
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
                    var left = expression is ConstantExpression c && (double)c.Value == 0.0 ? Expression.Constant(1.0) : expression;
                    expression = Expression.Multiply(left, right);
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

                    Expression<Func<double, double>> lambda = (arg) => mathFunction.Fn(arg);
                    Expression right = Expression.Invoke(lambda, arg);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    var left = expression is ConstantExpression c && (double)c.Value == 0.0 ? Expression.Constant(1.0) : expression;
                    expression = Expression.Multiply(left, right);
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

                    Expression<Func<double[], double>> lambda = (args) => mathFunction.Fn(args);
                    Expression right = Expression.Invoke(lambda, args);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    var left = expression is ConstantExpression c && (double)c.Value == 0.0 ? Expression.Constant(1.0) : expression;
                    expression = Expression.Multiply(left, right);
                    return true;
                }
            default:
                return false;
        }
    }
}