using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;

namespace MathEvaluation;

/// <summary>
/// Fast and comprehensive evaluator for dynamically evaluating mathematical expressions from strings.
/// </summary>
public partial class MathEvaluator(string mathString, IMathContext? context = null)
{
    /// <summary>Gets the math expression string.</summary>
    /// <value>The math expression string.</value>
    public string MathString { get; } = mathString;

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext" /> interface.</value>
    public IMathContext? Context { get; } = context;

    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    public double Evaluate(IFormatProvider? provider = null)
        => Evaluate(MathString, Context, null, provider);

    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public double Evaluate(object parameters, IFormatProvider? provider = null)
        => Evaluate(MathString, Context, new MathParameters(parameters), provider);

    /// <summary>Evaluates the <see cref="MathString">math expression string</see>.</summary>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException">expression</exception>
    /// <exception cref="MathEvaluationException">expression</exception>
    public double Evaluate(IMathParameters parameters, IFormatProvider? provider = null)
        => Evaluate(MathString, Context, parameters, provider);

    /// <summary>Evaluates the math expression string.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="context">The specified math context.</param>
    /// <param name="parameters">The parameters of the math expression string.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException">expression</exception>
    /// <exception cref="MathEvaluationException">expression</exception>
    public static double Evaluate(ReadOnlySpan<char> mathString,
        IMathContext? context = null, IMathParameters? parameters = null, IFormatProvider? provider = null)
    {
        if (mathString == null)
            throw new ArgumentNullException(nameof(mathString));

        if (mathString.IsWhiteSpace())
            throw new ArgumentException("Expression string is empty or white space.", nameof(mathString));

        try
        {
            var numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;
            var i = 0;
            return Evaluate(mathString, context, parameters, numberFormat, ref i, null, null, (int)EvalPrecedence.Unknown);
        }
        catch (Exception ex)
        {
            ex = ex is not MathEvaluationException ? new MathEvaluationException(ex.Message, ex) : ex;
            ex.Data[nameof(mathString)] = mathString.ToString();
            ex.Data[nameof(context)] = context;
            ex.Data[nameof(parameters)] = parameters;
            ex.Data[nameof(provider)] = provider;
            throw ex;
        }
    }

    private static double Evaluate(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand = false, double value = default)
    {
        var decimalSeparator = numberFormat?.NumberDecimalSeparator?.Length > 0 ? numberFormat.NumberDecimalSeparator[0] : '.';

        var start = i;
        while (mathString.Length > i)
        {
            if (separator.HasValue && mathString.IsParamsSeparator(start, i, separator.Value, decimalSeparator) ||
                closingSymbol.HasValue && mathString[i] == closingSymbol.Value)
            {
                if (value == default)
                    mathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return value;
            }

            if (mathString[i] is >= '0' and <= '9' || mathString[i] == decimalSeparator) //number
            {
                if (isOperand)
                    return Evaluate(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = mathString.ParseNumber(numberFormat, ref i);
                continue;
            }

            switch (mathString[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var startParenthesis = i;
                    i++;
                    var result = Evaluate(mathString, context, parameters, numberFormat, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    mathString.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiation(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    break;
                case '+' when mathString.Length == i + 1 || mathString[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && start != i && mathString[start..i].IsNotMeaningless())
                        return value;

                    i++;
                    value += Evaluate(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '-' when mathString.Length == i + 1 || mathString[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && mathString[start..i].IsNotMeaningless())
                        return value;

                    var isNegativity = start == i;
                    i++;
                    result = Evaluate(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);
                    value = isNegativity ? -result : value - result; //it keeps sign
                    if (isOperand)
                        return value;
                    break;
                case '*' when mathString.Length == i + 1 || mathString[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= Evaluate(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when mathString.Length == i + 1 || mathString[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= Evaluate(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = context?.FirstMathEntity(mathString[i..]) ?? parameters?.FirstMathEntity(mathString[i..]);
                    if (entity == null && numberFormat != null && mathString.TryParseCurrency(numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateMathEntity(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, precedence, value, entity);
                    if (isOperand)
                        return value;
                    break;
            }
        }

        if (value == default)
            mathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return value;
    }

    private static double EvaluateOperand(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = Evaluate(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (value == default)
            mathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return value;
    }

    private static double EvaluateMathEntity(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, double value, IMathEntity? entity, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryEvaluateEntity(mathString, context, parameters, entity, numberFormat, ref i, separator, closingSymbol, ref value))
                return value;

            var decimalValue = (decimal)value;
            if (TryEvaluateContextDecimal(mathString, context, parameters, entity, numberFormat, ref i, separator, closingSymbol, ref decimalValue))
                return (double)decimalValue;
        }

        if (!throwError)
            return value;

        var end = mathString[i..].IndexOfAny("(0123456789.,٫+-*/ \t\n\r") + i;
        var unknownSubstring = end > i ? mathString[i..end] : mathString[i..];

        throw new MathEvaluationException($"'{unknownSubstring.ToString()}' is not recognizable.", i);
    }

    private static double EvaluateExponentiation(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, double value)
    {
        mathString.SkipMeaningless(ref i);
        if (mathString.Length <= i)
            return value;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = context?.FirstMathEntity(mathString[i..]) ?? parameters?.FirstMathEntity(mathString[i..]);
        return EvaluateMathEntity(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, precedence, value, entity, false);
    }

    private static bool TryEvaluateEntity(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, IMathEntity entity, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, ref double value)
    {
        var start = i;
        switch (entity)
        {
            case MathConstant<double> mathConstant:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiation(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, mathConstant.Value);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiation(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, mathVariable.Value);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var result = mathOperator.IsProcessingLeft
                        ? fn(value)
                        : fn(EvaluateOperand(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol));
                    value = EvaluateExponentiation(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateOperand(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol);
                    right = EvaluateExponentiation(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, right);
                    value = fn(value, right);
                    return true;
                }
            case MathOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = Evaluate(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, right);
                    return true;
                }
            case MathGetValueFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.SkipParenthesis(ref i);

                    var result = mathFunction.Fn();
                    result = EvaluateExponentiation(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathUnaryFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    if (mathFunction.OpeningSymbol.HasValue)
                        mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol.Value, start, ref i);

                    var arg = mathFunction.ClosingSymbol.HasValue
                        ? Evaluate(mathString, context, parameters, numberFormat, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown)
                        : EvaluateOperand(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol);

                    if (mathFunction.ClosingSymbol.HasValue)
                        mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol.Value, start, ref i);

                    var result = mathFunction.Fn(arg);
                    result = EvaluateExponentiation(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol, start, ref i);

                    var args = new List<double>();
                    while (mathString.Length > i)
                    {
                        var arg = Evaluate(mathString, context, parameters, numberFormat, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
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
                    result = EvaluateExponentiation(mathString, context, parameters, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            default:
                return false;
        }
    }
}