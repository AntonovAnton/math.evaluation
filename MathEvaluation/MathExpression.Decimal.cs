using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;

namespace MathEvaluation;

/// <summary>
/// Compiles any mathematical expression string to the <see cref="Func{T, TResult}"/> with parameters or to the <see cref="Func{TResult}"/>.
/// </summary>
public partial class MathExpression
{
    private static readonly Expression DecimalZero = Expression.Constant(0.0m);
    private static readonly Expression DecimalOne = Expression.Constant(1.0m);

    /// <inheritdoc cref="Compile()"/>
    public Func<decimal> CompileDecimal()
    {
        try
        {
            var i = 0;
            ExpressionTree = BuildDecimal(MathString, ref i, null, null, (int)EvalPrecedence.Unknown, false, DecimalZero);

            return Expression.Lambda<Func<decimal>>(ExpressionTree).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, null);
        }
    }

    /// <inheritdoc cref="Compile{T}(T)"/>
    public Func<T, decimal> CompileDecimal<T>(T parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        try
        {
            _parameters = new MathParameters();
            _parameters.Bind(parameters);

            _parameterExpression = Expression.Parameter(typeof(T), nameof(parameters));

            var i = 0;
            ExpressionTree = BuildDecimal(MathString, ref i, null, null, (int)EvalPrecedence.Unknown, false, DecimalZero);

            return Expression.Lambda<Func<T, decimal>>(ExpressionTree, _parameterExpression).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }

    private Expression BuildDecimal(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand, Expression expression)
    {
        var decimalSeparator = _numberFormat?.NumberDecimalSeparator?.Length > 0 ? _numberFormat.NumberDecimalSeparator[0] : '.';

        var start = i;
        while (mathString.Length > i)
        {
            if (separator.HasValue && mathString.IsParamsSeparator(start, i, separator.Value, decimalSeparator) ||
                closingSymbol.HasValue && mathString[i] == closingSymbol.Value)
            {
                if (expression == DecimalZero)
                    mathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return expression;
            }

            if (mathString[i] is >= '0' and <= '9' || mathString[i] == decimalSeparator) //number
            {
                if (isOperand)
                    return BuildDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Function, false, DecimalZero);

                expression = Expression.Constant(mathString.ParseDecimalNumber(_numberFormat, ref i));
                continue;
            }

            switch (mathString[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return expression;

                    var startParenthesis = i;
                    i++;
                    var right = BuildDecimal(mathString, ref i, null, ')', (int)EvalPrecedence.Unknown, false, DecimalZero);
                    mathString.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return right;

                    right = BuildExponentiationDecimal(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DecimalZero ? right : Expression.Multiply(expression, right);
                    break;
                case '+' when mathString.Length == i + 1 || mathString[i + 1] != '+':
                    if (isOperand || precedence >=(int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return expression;

                    i++;
                    right = BuildDecimal(mathString, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand, DecimalZero);
                    expression = Expression.Add(expression, right);
                    if (isOperand)
                        return expression;
                    break;
                case '-' when mathString.Length == i + 1 || mathString[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return expression;

                    var isNegativity = start == i;
                    i++;
                    right = BuildDecimal(mathString, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand, DecimalZero);
                    expression = isNegativity ? Expression.Negate(right) : Expression.Subtract(expression, right); //it keeps sign
                    if (isOperand)
                        return expression;
                    break;
                case '*' when mathString.Length == i + 1 || mathString[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = BuildDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, false, DecimalZero);
                    expression = Expression.Multiply(expression, right);
                    break;
                case '/' when mathString.Length == i + 1 || mathString[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = BuildDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, false, DecimalZero);
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

                    expression = BuildMathEntityDecimal(mathString, ref i, separator, closingSymbol, precedence, expression, entity);
                    if (isOperand)
                        return expression;
                    break;
            }
        }

        if (expression == DecimalZero)
            mathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return expression;
    }

    private Expression BuildOperandDecimal(ReadOnlySpan<char> mathString, ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var expression = BuildDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true, DecimalZero);
        if (expression == DecimalZero)
            mathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return expression;
    }

    private Expression BuildMathEntityDecimal(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, int precedence, Expression expression, IMathEntity? entity, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryBuildEntityDecimal(mathString, entity, ref i, separator, closingSymbol, ref expression))
                return expression;

            Expression convertExpression = expression == DecimalZero ? DoubleZero : Expression.Convert(expression, typeof(double));
            if (TryBuildEntity(mathString, entity, ref i, separator, closingSymbol, ref convertExpression))
                return Expression.Convert(convertExpression, typeof(decimal));
        }

        if (throwError)
            mathString.ThrowExceptionInvalidToken(i);

        return expression;
    }

    private Expression BuildExponentiationDecimal(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, Expression expression)
    {
        mathString.SkipMeaningless(ref i);
        if (mathString.Length <= i)
            return expression;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = Context?.FirstMathEntity(mathString[i..]) ?? _parameters?.FirstMathEntity(mathString[i..]);
        return BuildMathEntityDecimal(mathString, ref i, separator, closingSymbol, precedence, expression, entity, false);
    }

    private bool TryBuildEntityDecimal(ReadOnlySpan<char> mathString, IMathEntity entity,
        ref int i, char? separator, char? closingSymbol, ref Expression expression)
    {
        var start = i;
        switch (entity)
        {
            case MathConstant<decimal> mathConstant:
                {
                    i += entity.Key.Length;
                    Expression right = Expression.Constant(mathConstant.Value);
                    right = BuildExponentiationDecimal(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DecimalZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            case MathVariable<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    Expression right = Expression.Property(_parameterExpression, entity.Key);
                    if (right.Type != typeof(decimal))
                        right = Expression.Convert(right, typeof(decimal));
                    var left = expression == DecimalZero ? DecimalOne : expression;
                    expression = Expression.Multiply(left, right);
                    return true;
                }
            case MathOperandOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<decimal, decimal>> lambda = v => mathOperator.Fn(v);
                    if (mathOperator.IsProcessingLeft)
                        expression = Expression.Invoke(lambda, expression);
                    else
                    {
                        var right = BuildDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, false, DecimalZero);
                        expression = Expression.Invoke(lambda, right);
                    }
                    expression = BuildExponentiationDecimal(mathString, ref i, separator, closingSymbol, expression);
                    return true;
                }
            case MathOperandsOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<decimal, decimal, decimal>> lambda = (v1, v2) => mathOperator.Fn(v1, v2);
                    var right = BuildOperandDecimal(mathString, ref i, separator, closingSymbol);
                    right = BuildExponentiationDecimal(mathString, ref i, separator, closingSymbol, right);
                    expression = Expression.Invoke(lambda, expression, right);
                    return true;
                }
            case MathOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<decimal, decimal, decimal>> lambda = (v1, v2) => mathOperator.Fn(v1, v2);
                    var right = BuildDecimal(mathString, ref i, separator, closingSymbol, mathOperator.Precedence, false, DecimalZero);
                    expression = Expression.Invoke(lambda, expression, right);
                    return true;
                }
            case MathGetValueFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.SkipParenthesis(ref i);

                    Expression<Func<decimal>> lambda = () => mathFunction.Fn();
                    Expression right = Expression.Invoke(lambda);
                    right = BuildExponentiationDecimal(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DecimalZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            case MathUnaryFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    if (mathFunction.OpeningSymbol.HasValue)
                        mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol.Value, start, ref i);

                    var arg = mathFunction.ClosingSymbol.HasValue
                        ? BuildDecimal(mathString, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown, false, DecimalZero)
                        : BuildOperandDecimal(mathString, ref i, separator, closingSymbol);

                    if (mathFunction.ClosingSymbol.HasValue)
                        mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol.Value, start, ref i);

                    Expression<Func<decimal, decimal>> lambda = (value) => mathFunction.Fn(value);
                    Expression right = Expression.Invoke(lambda, arg);
                    right = BuildExponentiationDecimal(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DecimalZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            case MathFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol, start, ref i);

                    var args = new List<Expression>();
                    while (mathString.Length > i)
                    {
                        var arg = BuildDecimal(mathString, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown, false, DecimalZero);
                        args.Add(arg);

                        if (mathString[i] == mathFunction.Separator)
                        {
                            i++; //other param
                            continue;
                        }
                        break;
                    }

                    mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol, start, ref i);

                    Expression<Func<decimal[], decimal>> lambda = (values) => mathFunction.Fn(values);
                    Expression right = Expression.Invoke(lambda, Expression.NewArrayInit(typeof(decimal), args));
                    right = BuildExponentiationDecimal(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DecimalZero ? right : Expression.Multiply(expression, right);
                    return true;
                }
            default:
                return false;
        }
    }
}