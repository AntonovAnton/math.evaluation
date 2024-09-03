using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;

namespace MathEvaluation;

public partial class MathEvaluator
{
    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    public decimal EvaluateDecimal(IFormatProvider? provider = null)
        => EvaluateDecimal(MathString, Context, null, provider);

    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    public decimal EvaluateDecimal(IMathParameters parameters, IFormatProvider? provider = null)
        => EvaluateDecimal(MathString, Context, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(ReadOnlySpan<char> mathString, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, null, null, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(ReadOnlySpan<char> mathString, IMathParameters parameters, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, null, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(ReadOnlySpan<char> mathString, IMathContext context, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, context, null, provider);

    #region object parameters

    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public decimal EvaluateDecimal(object parameters, IFormatProvider? provider = null)
        => EvaluateDecimal(MathString, Context, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static decimal EvaluateDecimal(ReadOnlySpan<char> mathString, object parameters, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, null, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static decimal EvaluateDecimal(ReadOnlySpan<char> mathString, IMathContext? context, object parameters, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, context, new MathParameters(parameters), provider);

    #endregion

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, IFormatProvider? provider = null)
    {
        if (mathString == null)
            throw new ArgumentNullException(nameof(mathString));

        if (mathString.IsWhiteSpace())
            throw new ArgumentException("Expression string is empty or white space.", nameof(mathString));

        try
        {
            var numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;
            var i = 0;
            return EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, null, null, (int)EvalPrecedence.Unknown);
        }
        catch (Exception ex)
        {
            ex = ex is not MathEvaluationException ? new MathEvaluationException(ex.Message, ex) : ex;
            ex.Data[nameof(mathString)] = mathString.ToString();
            ex.Data[nameof(context)] = context;
            ex.Data[nameof(provider)] = provider;
            throw ex;
        }
    }

    private static decimal EvaluateDecimal(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand = false, decimal value = default)
    {
        var decimalSeparator = numberFormat?.NumberDecimalSeparator?.Length > 0 ? numberFormat.NumberDecimalSeparator[0] : '.';

        var start = i;
        while (mathString.Length > i)
        {
            if (separator.HasValue && mathString[i] == separator.Value &&
                (numberFormat == null || decimalSeparator != separator.Value || mathString[start..i].IsNotMeaningless()))
            {
                mathString.ThrowExceptionIfNotEvaluated(value, true, start, i);
                return value;
            }

            if (closingSymbol.HasValue && mathString[i] == closingSymbol.Value)
            {
                mathString.ThrowExceptionIfNotEvaluated(value, true, start, i);
                return value;
            }

            if (mathString[i] is >= '0' and <= '9' || mathString[i] == decimalSeparator) //number
            {
                if (isOperand)
                    return EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = mathString.ParseDecimalNumber(numberFormat, ref i);
                continue;
            }

            switch (mathString[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var startParenthesis = i;
                    i++;
                    var result = EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    mathString.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiationDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    break;
                case '+' when mathString.Length == i + 1 || mathString[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && start != i && mathString[start..i].IsNotMeaningless())
                        return value;

                    i++;
                    value += EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '-' when mathString.Length == i + 1 || mathString[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && mathString[start..i].IsNotMeaningless())
                        return value;

                    i++;
                    value -= EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '*' when mathString.Length == i + 1 || mathString[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when mathString.Length == i + 1 || mathString[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = context?.FirstMathEntity(mathString[i..]) ?? parameters?.FirstMathEntity(mathString[i..]);
                    if (entity == null && numberFormat != null && mathString.TryParseCurrencySymbol(numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateMathEntityDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, precedence, value, entity);
                    if (isOperand)
                        return value;
                    break;
            }
        }

        mathString.ThrowExceptionIfNotEvaluated(value, isOperand, start, i);
        return value;
    }

    private static decimal EvaluateOperandDecimal(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        mathString.ThrowExceptionIfNotEvaluated(value, true, start, i);
        return value;
    }

    private static decimal EvaluateMathEntityDecimal(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, decimal value, IMathEntity? entity = null, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryEvaluateContextDecimal(mathString, context, parameters, entity, numberFormat, ref i, separator, closingSymbol, ref value))
                return value;

            var doubleValue = (double)value;
            if (TryEvaluateEntity(mathString, context, parameters, entity, numberFormat, ref i, separator, closingSymbol, ref doubleValue))
                return (decimal)doubleValue;
        }

        if (!throwError)
            return value;

        var end = mathString[i..].IndexOfAny("(0123456789.,٫+-*/ \t\n\r") + i;
        var unknownSubstring = end > i ? mathString[i..end] : mathString[i..];

        throw new MathEvaluationException($"'{unknownSubstring.ToString()}' is not recognizable.", i);
    }

    private static decimal EvaluateExponentiationDecimal(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, decimal value)
    {
        mathString.SkipMeaninglessChars(ref i);
        if (mathString.Length <= i)
            return value;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = context?.FirstMathEntity(mathString[i..]) ?? parameters?.FirstMathEntity(mathString[i..]);
        return EvaluateMathEntityDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, precedence, value, entity, false);
    }

    private static bool TryEvaluateContextDecimal(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, IMathEntity entity, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, ref decimal value)
    {
        var start = i;
        switch (entity)
        {
            case MathConstant<decimal> mathConstant:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiationDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, mathConstant.Value);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathVariable<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiationDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, mathVariable.Value);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathOperandOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var result = mathOperator.IsProcessingLeft
                        ? fn(value)
                        : fn(EvaluateOperandDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol));
                    value = EvaluateExponentiationDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateOperandDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol);
                    right = EvaluateExponentiationDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, right);
                    value = fn(value, right);
                    return true;
                }
            case MathOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, right);
                    return true;
                }
            case MathGetValueFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.SkipParenthesisChars(ref i);
                    var result = mathFunction.Fn();
                    result = EvaluateExponentiationDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathUnaryFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    if (mathFunction.OpeningSymbol.HasValue)
                        mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol.Value, start, ref i);

                    var arg = mathFunction.ClosingSymbol.HasValue
                        ? EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown)
                        : EvaluateOperandDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol);

                    if (mathFunction.ClosingSymbol.HasValue)
                        mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol.Value, start, ref i);

                    var result = mathFunction.Fn(arg);
                    result = EvaluateExponentiationDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol, start, ref i);

                    var args = new List<decimal>();
                    while (mathString.Length > i)
                    {
                        var arg = EvaluateDecimal(mathString, context, parameters, numberFormat, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
                        args.Add(arg);

                        if (mathString[i] == mathFunction.Separator)
                        {
                            i++; //other param
                            continue;
                        }
                        break;
                    }

                    mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol, start, ref i);

                    var result = mathFunction.Fn([.. args]);
                    result = EvaluateExponentiationDecimal(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            default:
                return false;
        }
    }
}