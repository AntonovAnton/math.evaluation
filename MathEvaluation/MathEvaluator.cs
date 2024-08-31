﻿using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;

namespace MathEvaluation;

/// <summary>
/// Fast and comprehensive evaluator for dynamically evaluating mathematical expressions from strings.
/// </summary>
public partial class MathEvaluator(string expression, IMathContext? context = null)
{
    /// <summary>Gets the math expression string.</summary>
    /// <value>The math expression string.</value>
    public string Expression { get; } = expression;

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext" /> interface.</value>
    public IMathContext? Context { get; internal set; } = context;

    /// <summary>Evaluates the <see cref="Expression">math expression string</see>.</summary>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="ArgumentNullException">expression</exception>
    /// <exception cref="ArgumentException">expression</exception>
    /// <exception cref="MathEvaluationException">expression</exception>
    public double Evaluate(IFormatProvider? provider = null)
    {
        return Evaluate(Expression.AsSpan(), Context, provider);
    }

    /// <summary>Evaluates the math <paramref name="expression" /> string.</summary>
    /// <param name="expression">The math expression string.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="ArgumentNullException">expression</exception>
    /// <exception cref="ArgumentException">expression</exception>
    /// <exception cref="MathEvaluationException">expression</exception>
    public static double Evaluate(string expression, IFormatProvider? provider = null)
    {
        return Evaluate(expression.AsSpan(), null, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IFormatProvider?)"/>
    public static double Evaluate(ReadOnlySpan<char> expression, IFormatProvider? provider = null)
    {
        return Evaluate(expression, null, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IFormatProvider?)"/>
    /// <param name="expression">The math expression string.</param>
    /// <param name="context">The specified math context.</param>
    /// <param name="provider">The specified format provider.</param>
    public static double Evaluate(string expression, IMathContext? context, IFormatProvider? provider = null)
    {
        return Evaluate(expression.AsSpan(), context, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IMathContext?, IFormatProvider?)"/>
    public static double Evaluate(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider? provider = null)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));

        if (expression.IsWhiteSpace())
            throw new ArgumentException("Expression is empty or white space.", nameof(expression));

        try
        {
            var numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;
            var i = 0;
            return Evaluate(expression, context, numberFormat, ref i, null, null, (int)EvalPrecedence.Unknown);
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

    private static double Evaluate(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand = false, double value = default)
    {
        var decimalSeparator = numberFormat?.NumberDecimalSeparator?.Length > 0 ? numberFormat.NumberDecimalSeparator[0] : '.';

        var start = i;
        while (expression.Length > i)
        {
            if (separator.HasValue && expression[i] == separator.Value &&
                (numberFormat == null || decimalSeparator != separator.Value || expression[start..i].IsNotMeaningless()))
            {
                expression.ThrowExceptionIfNotEvaluated(value, true, start, i);
                return value;
            }

            if (closingSymbol.HasValue && expression[i] == closingSymbol.Value)
            {
                expression.ThrowExceptionIfNotEvaluated(value, true, start, i);
                return value;
            }

            if (expression[i] is >= '0' and <= '9' || expression[i] == decimalSeparator) //number
            {
                if (isOperand)
                    return Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = expression.ParseNumber(numberFormat, ref i);
                continue;
            }

            switch (expression[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var startParenthesis = i;
                    i++;
                    var result = Evaluate(expression, context, numberFormat, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    expression.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    break;
                case '+' when expression.Length == i + 1 || expression[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && start != i && expression[start..i].IsNotMeaningless())
                        return value;

                    i++;
                    value += Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '-' when expression.Length == i + 1 || expression[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && expression[start..i].IsNotMeaningless())
                        return value;

                    i++;
                    value -= Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '*' when expression.Length == i + 1 || expression[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when expression.Length == i + 1 || expression[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = context?.FirstMathEntity(expression[i..]);
                    if (entity == null && numberFormat != null && expression.TryParseCurrencySymbol(numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateMathEntity(expression, context, numberFormat, ref i, separator, closingSymbol, precedence, value, entity);
                    if (isOperand)
                        return value;
                    break;
            }
        }

        expression.ThrowExceptionIfNotEvaluated(value, isOperand, start, i);
        return value;
    }

    private static double EvaluateOperand(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        expression.ThrowExceptionIfNotEvaluated(value, true, start, i);
        return value;
    }

    private static double EvaluateMathEntity(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, double value, IMathEntity? entity, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryEvaluateContext(expression, context!, entity, numberFormat, ref i, separator, closingSymbol, ref value))
                return value;

            var decimalValue = (decimal)value;
            if (TryEvaluateContextDecimal(expression, context!, entity, numberFormat, ref i, separator, closingSymbol, ref decimalValue))
                return (double)decimalValue;
        }

        if (!throwError)
            return value;

        var end = expression[i..].IndexOfAny("(0123456789.,٫+-*/ \t\n\r") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new MathEvaluationException($"'{unknownSubstring.ToString()}' is not recognizable.", i);
    }

    private static double EvaluateExponentiation(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, double value)
    {
        expression.SkipMeaninglessChars(ref i);
        if (expression.Length <= i)
            return value;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = context?.FirstMathEntity(expression[i..]);
        return EvaluateMathEntity(expression, context, numberFormat, ref i, separator, closingSymbol, precedence, value, entity, false);
    }

    private static bool TryEvaluateContext(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, ref double value)
    {
        var start = i;
        switch (entity)
        {
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, mathVariable.Value);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var result = mathOperator.IsProcessingLeft
                        ? fn(value)
                        : fn(EvaluateOperand(expression, context, numberFormat, ref i, separator, closingSymbol));
                    value = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateOperand(expression, context, numberFormat, ref i, separator, closingSymbol);
                    right = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, right);
                    value = fn(value, right);
                    return true;
                }
            case MathOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, right);
                    return true;
                }
            case MathGetValueFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    expression.SkipParenthesisChars(ref i);
                    var result = mathFunction.Fn();
                    result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathUnaryFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.ClosingSymbol.HasValue
                        ? fn(Evaluate(expression, context, numberFormat, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown))
                        : fn(EvaluateOperand(expression, context, numberFormat, ref i, separator, closingSymbol));

                    if (mathFunction.ClosingSymbol.HasValue)
                        expression.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol.Value, start, ref i);

                    result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    if (expression.Length <= i || expression[i] != mathFunction.OpenningSymbol)
                        return false;

                    i++; //openning
                    var args = new List<double>();
                    while (expression.Length > i)
                    {
                        var arg = Evaluate(expression, context, numberFormat, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
                        args.Add(arg);

                        //closing
                        if (expression[i] != mathFunction.Separator)
                        {
                            expression.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol, start, ref i);
                            break;
                        }

                        //other param
                        i++;
                    }

                    var result = mathFunction.Fn([.. args]);
                    result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            default:
                return false;
        }
    }
}