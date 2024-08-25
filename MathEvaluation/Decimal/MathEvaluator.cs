using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;

namespace MathEvaluation;

public partial class MathEvaluator
{
    /// <inheritdoc cref="Evaluate(IFormatProvider?)"/>
    public decimal EvaluateDecimal(IFormatProvider? provider = null)
    {
        return EvaluateDecimal(Expression.AsSpan(), Context, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(string expression, IFormatProvider? provider = null)
    {
        return EvaluateDecimal(expression.AsSpan(), null, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(ReadOnlySpan<char> expression, IFormatProvider? provider = null)
    {
        return EvaluateDecimal(expression, null, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IMathContext?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(string expression, IMathContext? context, IFormatProvider? provider = null)
    {
        return EvaluateDecimal(expression.AsSpan(), context, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IMathContext?, IFormatProvider?)"/>
    /// <exception cref="System.ArgumentNullException">expression</exception>
    /// <exception cref="System.ArgumentException">expression</exception>
    public static decimal EvaluateDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider? provider = null)
    {
        try
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.IsWhiteSpace())
                throw new ArgumentException("Expression is empty or white space.", nameof(expression));

            var i = 0;
            return EvaluateDecimal(expression, context, provider ?? CultureInfo.CurrentCulture, ref i, null, null, (int)EvalPrecedence.Unknown);
        }
        catch (Exception ex)
        {
            ex.Data[nameof(expression)] = expression.ToString();
            ex.Data[nameof(context)] = context;
            ex.Data[nameof(provider)] = provider;
            throw;
        }
    }

    private static decimal EvaluateDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand = false, decimal value = default)
    {
        var start = i;
        while (expression.Length > i)
        {
            if ((closingSymbol.HasValue && expression[i] == closingSymbol.Value) ||
                (separator.HasValue && expression[i] == separator.Value))
                return value;

            switch (expression[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    i++;
                    var result = EvaluateDecimal(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    i++;
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    break;
                case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                    if (isOperand)
                        return EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                    value = GetDecimalNumber(expression, provider, ref i, separator, closingSymbol);
                    break;
                case '+':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !expression[start..i].IsWhiteSpace())
                        return value;

                    i++;
                    value += EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !expression[start..i].IsWhiteSpace())
                        return value;

                    i++;
                    SkipMeaninglessChars(expression, ref i);

                    //two negatives should combine to make a positive
                    if (expression.Length > i && expression[i] is '-')
                    {
                        i++;
                        value += EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);
                    }
                    else
                    {
                        value -= EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);
                    }

                    if (isOperand)
                        return value;
                    break;
                case '*' when expression.Length == i + 1 || expression[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when expression.Length == i + 1 || expression[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = context?.FirstMathEntity(expression[i..]);

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateFuncOrVarDecimal(expression, context, provider, ref i, separator, closingSymbol, precedence, value, entity);
                    if (isOperand)
                        return value;
                    break;
            }
        }

        if (value == 0m && expression[start..i].IsWhiteSpace())
            throw new ArgumentException("Expression cannot be evaluated.", nameof(expression));

        return value;
    }

    private static decimal EvaluateFuncOrVarDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, decimal value, IMathEntity? entity = null)
    {
        entity ??= context?.FirstMathEntity(expression[i..]);
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryEvaluateContextDecimal(expression, context!, entity, provider, ref i, separator, closingSymbol, ref value))
                return value;

            var doubleValue = (double)value;
            if (TryEvaluateContext(expression, context!, entity, provider, ref i, separator, closingSymbol, ref doubleValue))
                return (decimal)doubleValue;
        }

        if (TryParseCurrencySymbol(expression, provider, ref i))
            return value;

        var end = expression[i..].IndexOfAny("(0123456789.,٫+-*/ \t\n\r") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static decimal EvaluateExponentiationDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, decimal value)
    {
        SkipMeaninglessChars(expression, ref i);

        var entity = expression.Length > i ? context?.FirstMathEntity(expression[i..]) : null;
        if (entity != null && entity.Precedence >= (int)EvalPrecedence.Exponentiation)
        {
            if (TryEvaluateContextDecimal(expression, context!, entity, provider, ref i, separator, closingSymbol, ref value))
                return value;

            var doubleValue = (double)value;
            if (TryEvaluateContext(expression, context!, entity, provider, ref i, separator, closingSymbol, ref doubleValue))
                return (decimal)doubleValue;
        }

        return value;
    }

    private static bool TryEvaluateContextDecimal(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, ref decimal value)
    {
        switch (entity)
        {
            case MathVariable<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, mathVariable.Value);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathOperandOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var result = mathOperator.IsProcessingLeft
                        ? fn(value)
                        : fn(EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true));
                    value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
                    right = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, right);
                    value = fn(value, right);
                    return true;
                }
            case MathOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, right);
                    return true;
                }
            case MathGetValueFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    SkipParenthesisChars(expression, ref i);
                    var result = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, mathFunction.Fn());
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathUnaryFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.ClosingSymbol.HasValue
                        ? fn(EvaluateDecimal(expression, context, provider, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown))
                        : fn(EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true));

                    if (mathFunction.ClosingSymbol.HasValue)
                        i++;

                    result = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    if (expression.Length <= i || expression[i] != mathFunction.OpenningSymbol)
                        return false;

                    i++; //openning
                    var args = new List<decimal>();
                    while (expression.Length > i)
                    {
                        var arg = EvaluateDecimal(expression, context, provider, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
                        args.Add(arg);

                        //closing
                        if (expression.Length <= i || expression[i] != mathFunction.Separator)
                        {
                            i++;
                            break;
                        }

                        //other param
                        i++;
                    }

                    var result = mathFunction.Fn([.. args]);
                    result = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            default:
                return false;
        }
    }

    private static decimal GetDecimalNumber(ReadOnlySpan<char> expression, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol)
    {
        var str = GetNumberString(expression, ref i, separator, closingSymbol);
        return decimal.Parse(str, NumberStyles.Number | NumberStyles.AllowExponent, provider);
    }
}