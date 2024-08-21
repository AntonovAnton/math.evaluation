﻿using System;
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
        ref int i, char? separator, char? closingSymbol, int precedence, decimal value = default)
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
                    result = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    break;
                case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                    value = GetDecimalNumber(expression, provider, ref i, separator, closingSymbol);
                    break;
                case '+':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !expression[start..i].IsWhiteSpace())
                        return value;

                    i++;
                    value += EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic);
                    break;
                case '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !expression[start..i].IsWhiteSpace())
                        return value;

                    i++;
                    while (expression.Length > i && expression[i] is ' ' or '\n' or '\r')
                        i++;

                    //two negatives should combine to make a positive
                    if (expression.Length > i && expression[i] is '-')
                    {
                        i++;
                        value += EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic);
                    }
                    else
                    {
                        value -= EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic);
                    }

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
                case ' ' or '\n' or '\r': //space or LF or CR
                    i++;
                    break;
                default:
                    var entity = context?.FirstMathEntity(expression[i..]);

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateFuncOrVarDecimal(expression, context, provider, ref i, separator, closingSymbol, precedence, value, false, entity);
                    break;
            }
        }

        if (value == 0m && expression[start..i].IsWhiteSpace())
            throw new ArgumentException("Expression cannot be evaluated.", nameof(expression));

        return value;
    }

    private static decimal EvaluateOperandDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        switch (expression[i])
        {
            case '(':
                {
                    i++;
                    var value = EvaluateDecimal(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    i++;
                    return value;
                }
            case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                return EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);
            case '-':
                i++;
                return -EvaluateOperandDecimal(expression, context, provider, ref i, separator, closingSymbol);
            default:
                return EvaluateFuncOrVarDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Function, 0m, true);
        }
    }

    private static decimal EvaluateFuncOrVarDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, decimal value, bool isOperand, IMathEntity? entity = null)
    {
        entity ??= context?.FirstMathEntity(expression[i..]);
        if (entity?.Precedence < precedence)
            return value;

        if (entity != null && TryEvaluateContextDecimal(expression, context!, entity, provider, ref i, separator, closingSymbol, ref value, isOperand))
            return value;

        var doubleValue = (double)value;
        if (entity != null && TryEvaluateContext(expression, context!, entity, provider, ref i, separator, closingSymbol, ref doubleValue, isOperand))
            return (decimal)doubleValue;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        var end = expression[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static decimal EvaluateExponentiationDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, decimal value, IMathEntity? entity = null)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        entity ??= (expression.Length > i ? context?.FirstMathEntity(expression[i..]) : null);
        switch (entity)
        {
            case MathOperandConverter<decimal> mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    var result = mathConverter.IsConvertingLeftOperand
                        ? fn(value)
                        : fn(EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic));
                    value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    break;
                }
            case MathOperandOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var rightOperand = EvaluateOperandDecimal(expression, context, provider, ref i, separator, closingSymbol);
                    rightOperand = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, rightOperand);
                    value = fn(value, rightOperand);
                    break;
                }
            case MathOperandConverter<double> or MathOperandOperator<double>:
                value = (decimal)EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, (double)value, entity);
                break;
        }

        return value;
    }

    private static bool TryEvaluateContextDecimal(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, ref decimal value, bool isOperand)
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
            case MathVariableFunction<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, mathVariable.GetValue());
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathOperandConverter<decimal> mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    value = mathConverter.IsConvertingLeftOperand
                        ? fn(value)
                        : fn(EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic));
                    return true;
                }
            case MathOperandOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var rightOperand = EvaluateOperandDecimal(expression, context, provider, ref i, separator, closingSymbol);
                    rightOperand = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, rightOperand);
                    value = fn(value, rightOperand);
                    return true;
                }
            case MathOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var rightOperand = EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, rightOperand);
                    return true;
                }
            case BasicMathFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.ClosingSymbol.HasValue
                        ? fn(EvaluateDecimal(expression, context, provider, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown))
                        : fn(EvaluateOperandDecimal(expression, context, provider, ref i, separator, closingSymbol));

                    if (mathFunction.ClosingSymbol.HasValue)
                        i++;

                    if (!isOperand)
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
                    if (!isOperand)
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