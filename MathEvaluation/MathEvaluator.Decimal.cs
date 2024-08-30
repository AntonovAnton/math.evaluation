using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;
using MathEvaluation.Entities;

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
    public static decimal EvaluateDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider? provider = null)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));

        if (expression.IsWhiteSpace())
            throw new ArgumentException("Expression is empty or white space.", nameof(expression));

        try
        {
            var numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;
            var i = 0;
            return EvaluateDecimal(expression, context, numberFormat, ref i, null, null, (int)EvalPrecedence.Unknown);
        }
        catch (Exception ex)
        {
            ex = ex is not MathEvaluationException ? new MathEvaluationException(ex.Message, ex) : ex;
            ex.Data[nameof(expression)] = expression.ToString();
            ex.Data[nameof(context)] = context;
            ex.Data[nameof(provider)] = provider;
            throw ex;
        }
    }

    private static decimal EvaluateDecimal(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand = false, decimal value = default)
    {
        var decimalSeparator = numberFormat?.NumberDecimalSeparator?.Length > 0 ? numberFormat.NumberDecimalSeparator[0] : '.';

        var start = i;
        while (expression.Length > i)
        {
            if (separator.HasValue && expression[i] == separator.Value &&
                (numberFormat == null || decimalSeparator != separator.Value || IsNotMeaningless(expression[start..i])))
            {
                ThrowExceptionIfNotEvaluated(expression, value, true, start, i);
                return value;
            }

            if (closingSymbol.HasValue && expression[i] == closingSymbol.Value)
            {
                ThrowExceptionIfNotEvaluated(expression, value, true, start, i);
                return value;
            }

            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '٫' || expression[i] == decimalSeparator) //number
            {
                if (isOperand)
                    return EvaluateDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = GetDecimalNumber(expression, numberFormat, ref i, separator, closingSymbol);
                continue;
            }

            switch (expression[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var startParenthesis = i;
                    i++;
                    var result = EvaluateDecimal(expression, context, numberFormat, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    ThrowExceptionIfNotClosed(expression, ')', startParenthesis, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiationDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    break;
                case '+' when expression.Length == i + 1 || expression[i + 1] != '+':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && IsNotMeaningless(expression[start..i]))
                        return value;

                    i++;
                    value += EvaluateDecimal(expression, context, numberFormat, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '-' when expression.Length == i + 1 || expression[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && IsNotMeaningless(expression[start..i]))
                        return value;

                    i++;
                    value -= EvaluateDecimal(expression, context, numberFormat, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '*' when expression.Length == i + 1 || expression[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= EvaluateDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when expression.Length == i + 1 || expression[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= EvaluateDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = context?.FirstMathEntity(expression[i..]);
                    if (entity == null && numberFormat != null && TryParseCurrencySymbol(expression, numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateMathEntityDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, precedence, value, entity);
                    if (isOperand)
                        return value;
                    break;
            }
        }

        ThrowExceptionIfNotEvaluated(expression, value, isOperand, start, i);
        return value;
    }

    private static decimal EvaluateOperandDecimal(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = EvaluateDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        ThrowExceptionIfNotEvaluated(expression, value, true, start, i);
        return value;
    }

    private static decimal EvaluateMathEntityDecimal(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, decimal value, IMathEntity? entity = null, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryEvaluateContextDecimal(expression, context!, entity, numberFormat, ref i, separator, closingSymbol, ref value))
                return value;

            var doubleValue = (double)value;
            if (TryEvaluateContext(expression, context!, entity, numberFormat, ref i, separator, closingSymbol, ref doubleValue))
                return (decimal)doubleValue;
        }

        if (!throwError)
            return value;

        var end = expression[i..].IndexOfAny("(0123456789.,٫+-*/ \t\n\r") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new MathEvaluationException($"'{unknownSubstring.ToString()}' is not recognizable.", i);
    }

    private static decimal EvaluateExponentiationDecimal(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, decimal value)
    {
        SkipMeaninglessChars(expression, ref i);
        if (expression.Length <= i)
            return value;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = context?.FirstMathEntity(expression[i..]);
        return EvaluateMathEntityDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, precedence, value, entity, false);
    }

    private static bool TryEvaluateContextDecimal(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, ref decimal value)
    {
        var start = i;
        switch (entity)
        {
            case MathVariable<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiationDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, mathVariable.Value);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathOperandOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var result = mathOperator.IsProcessingLeft
                        ? fn(value)
                        : fn(EvaluateOperandDecimal(expression, context, numberFormat, ref i, separator, closingSymbol));
                    value = EvaluateExponentiationDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateOperandDecimal(expression, context, numberFormat, ref i, separator, closingSymbol);
                    right = EvaluateExponentiationDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, right);
                    value = fn(value, right);
                    return true;
                }
            case MathOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, right);
                    return true;
                }
            case MathGetValueFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    SkipParenthesisChars(expression, ref i);
                    var result = mathFunction.Fn();
                    result = EvaluateExponentiationDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathUnaryFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.ClosingSymbol.HasValue
                        ? fn(EvaluateDecimal(expression, context, numberFormat, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown))
                        : fn(EvaluateOperandDecimal(expression, context, numberFormat, ref i, separator, closingSymbol));

                    if (mathFunction.ClosingSymbol.HasValue)
                        ThrowExceptionIfNotClosed(expression, mathFunction.ClosingSymbol.Value, start, ref i);

                    result = EvaluateExponentiationDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, result);
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
                        var arg = EvaluateDecimal(expression, context, numberFormat, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
                        args.Add(arg);

                        //closing
                        if (expression[i] != mathFunction.Separator)
                        {
                            ThrowExceptionIfNotClosed(expression, mathFunction.ClosingSymbol, start, ref i);
                            break;
                        }

                        //other param
                        i++;
                    }

                    var result = mathFunction.Fn([.. args]);
                    result = EvaluateExponentiationDecimal(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            default:
                return false;
        }
    }

    private static decimal GetDecimalNumber(ReadOnlySpan<char> expression, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol)
    {
        var str = GetNumberString(expression, numberFormat, ref i, separator, closingSymbol);
        return decimal.Parse(str, NumberStyles.Number | NumberStyles.AllowExponent, numberFormat);
    }

    private static void ThrowExceptionIfNotEvaluated(
        ReadOnlySpan<char> expression, decimal value, bool isOperand, int invalidTokenPosition, int i)
    {
        if (value == default && !IsNotMeaningless(expression[invalidTokenPosition..i]))
            throw new MathEvaluationException($"{(isOperand ? "The operand" : "It")} is not recognizable.", invalidTokenPosition);
    }
}