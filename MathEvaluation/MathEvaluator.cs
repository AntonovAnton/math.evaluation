﻿using System;
using System.Globalization;
using MathEvaluation.Context;
using MathTrigonometric;

namespace MathEvaluation;

public class MathEvaluator(string expression)
{
    public string Expression { get; } = expression;

    public IMathContext? Context { get; internal set; }

    public double Evaluate(IFormatProvider? provider = null)
    {
        return Evaluate(Expression.AsSpan(), Context, provider);
    }

    public static double Evaluate(string expression, IFormatProvider? provider = null)
    {
        return Evaluate(expression.AsSpan(), null, provider);
    }

    public static double Evaluate(ReadOnlySpan<char> expression, IFormatProvider? provider = null)
    {
        return Evaluate(expression, null, provider);
    }

    public static double Evaluate(string expression, IMathContext? context, IFormatProvider? provider = null)
    {
        return Evaluate(expression.AsSpan(), context, provider);
    }

    public static double Evaluate(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider? provider = null)
    {
        try
        {
            if (expression == null || expression.IsWhiteSpace())
                return double.NaN;

            var i = 0;
            return EvaluateLowestBasic(expression, context, provider ?? CultureInfo.CurrentCulture, ref i);
        }
        catch (Exception ex)
        {
            ex.Data[nameof(expression)] = expression.ToString();
            ex.Data[nameof(provider)] = provider;
            throw;
        }
    }

    private static double EvaluateLowestBasic(ReadOnlySpan<char> expression, IMathContext? context,
        IFormatProvider provider, ref int i, char? separator = null, bool isAbs = false, double value = default)
    {
        while (expression.Length > i)
        {
            if (separator.HasValue && expression[i] == separator.Value)
            {
                i++;
                return value;
            }

            switch (expression[i])
            {
                case '(' or '[':
                    i++;
                    value = (value == 0 ? 1 : value) * EvaluateLowestBasic(expression, context, provider, ref i);
                    break;
                case ')' or ']':
                    i++;
                    while (expression.Length > i && expression[i] is ' ')
                        i++;

                    if (expression.Length > i && expression[i] == '^')
                    {
                        i++;
                        value = Math.Pow(value, EvaluateBasic(expression, context, provider, ref i, separator, isAbs));
                        return value;
                    }

                    return value;
                case >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    value = GetNumber(expression, provider, ref i, separator);
                    break;
                case ' ':
                    i++;
                    break;
                case '+':
                    i++;
                    value += EvaluateBasic(expression, context, provider, ref i, separator, isAbs);
                    break;
                case '-':
                    i++;
                    while (expression.Length > i && expression[i] is ' ')
                        i++;

                    //two negatives should combine to make a positive
                    if (expression.Length > i && expression[i] is '-')
                    {
                        i++;
                        value += EvaluateBasic(expression, context, provider, ref i, separator, isAbs);
                    }
                    else
                    {
                        value -= EvaluateBasic(expression, context, provider, ref i, separator, isAbs);
                    }

                    break;
                case '|' when isAbs:
                    i++;
                    return value;
                case '⌉':
                    i++;
                    return value;
                case '⌋':
                    i++;
                    return value;
                default:
                    value = EvaluateBasic(expression, context, provider, ref i, separator, isAbs, false, value);
                    break;
            }
        }

        return value;
    }

    private static double EvaluateBasic(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, bool isAbs, bool isEvaluatedFirst = false, double value = default)
    {
        var start = i;
        while (expression.Length > i)
        {
            if (separator.HasValue && expression[i] == separator.Value)
                return value;

            switch (expression[i])
            {
                case '(' or '[':
                    i++;
                    value = (value == 0 ? 1 : value) * EvaluateLowestBasic(expression, context, provider, ref i);
                    break;
                case ')' or ']':
                    return value;
                case >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    value = GetNumber(expression, provider, ref i, separator);
                    break;
                case ' ':
                    i++;
                    break;
                case '*':
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    if (expression.Length > i && expression[i] == '*')
                    {
                        i++;
                        value = Math.Pow(value,
                            EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true));
                    }
                    else
                    {
                        value *= EvaluateBasic(expression, context, provider, ref i, separator, isAbs);
                    }

                    break;
                case '\u00d7' or '·': //Addition
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value *= EvaluateBasic(expression, context, provider, ref i, separator, isAbs);
                    break;
                case '/' or '\u00f7':
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value /= EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true);
                    break;
                case '%':
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value %= EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true);
                    break;
                case '+':
                    if (start != i && !expression[start..i].IsWhiteSpace())
                        return value;
                    i++;
                    break;
                case '-':
                    if (start != i && !expression[start..i].IsWhiteSpace())
                        return value;
                    i++;
                    value = -EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true);
                    break;
                case '^':
                    i++;
                    value = Math.Pow(value,
                        EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true));
                    return value;
                case '|' when isAbs:
                    return value;
                case '|':
                    i++;
                    value = (value == 0 ? 1 : value) *
                            Math.Abs(EvaluateLowestBasic(expression, context, provider, ref i, null, true));
                    return value;
                case '⌈':
                    i++;
                    value = (value == 0 ? 1 : value) *
                            Math.Ceiling(EvaluateLowestBasic(expression, context, provider, ref i, null, true));
                    return value;
                case '⌉':
                    return value;
                case '⌊':
                    i++;
                    value = (value == 0 ? 1 : value) *
                            Math.Floor(EvaluateLowestBasic(expression, context, provider, ref i, null, true));
                    return value;
                case '⌋':
                    return value;
                case '\u221a': //square root symbol
                    if (isEvaluatedFirst && start != i)
                        return value;
                    i++;
                    value = (value == 0 ? 1 : value) *
                            Math.Sqrt(EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true));
                    return value;
                case '\u221b': //cube root symbol
                    if (isEvaluatedFirst && start != i)
                        return value;
                    i++;
                    value = (value == 0 ? 1 : value) *
                            Math.Pow(EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true),
                                1 / 3d);
                    return value;
                case '\u221c': //fourth root symbol
                    if (isEvaluatedFirst && start != i)
                        return value;
                    i++;
                    value = (value == 0 ? 1 : value) *
                            Math.Pow(EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true),
                                1 / 4d);
                    return value;
                default:
                    value = EvaluateFnOrConstant(expression, context, provider, ref i, separator, isAbs, value);
                    if (isEvaluatedFirst)
                        return value;
                    break;
            }
        }

        if (value == 0d && expression[start..i].IsWhiteSpace())
            return double.NaN;

        return value;
    }

    private static double EvaluateFnOrConstant(ReadOnlySpan<char> expression, IMathContext? context,
        IFormatProvider provider, ref int i, char? separator, bool isAbs, double value = default)
    {
        while (expression.Length > i)
            switch (expression[i])
            {
                case 'π':
                    i++;
                    value = (value == 0 ? 1 : value) *
                            EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true, Math.PI);
                    return value;
                case 'e': //the natural logarithmic base
                    i++;
                    value = (value == 0 ? 1 : value) *
                            EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true, Math.E);
                    return value;
                case 'τ':
                    i++;
                    value = (value == 0 ? 1 : value) *
                            EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true, Math.PI * 2);
                    return value;
                case '\u00b0': //degree symbol
                    i++;
                    return MathTrig.DegreesToRadians(value);
                case '\u221e': //infinity symbol
                    i++;
                    return double.PositiveInfinity;
                default:
                    return EvaluateFn(expression, context, provider, ref i, separator, isAbs, value);
            }

        return value;
    }

    private static double EvaluateFn(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, bool isAbs, double value)
    {
        var start = i;

        if (TryEvaluateModulus(expression, context, provider, ref i, separator, isAbs, ref value))
            return value;

        if (TryEvaluatePi(expression, ref i, ref value))
            return value;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        if (TryEvaluateFn(expression, context, provider, ref i, separator, isAbs, ref value))
            return value;

        if (context != null && context.TryEvaluateMathOperand(expression, provider, ref i, separator, isAbs, out var operandValue))
        {
            return (value == 0 ? 1 : value) * operandValue;
        }

        var bracketCharIndex = expression[start..].IndexOfAny('(', '[', ' ') + start;
        var unknownSubstring = bracketCharIndex > i ? expression[start..bracketCharIndex] : expression[start..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported");
    }

    private static double GetNumber(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        char? separator = null)
    {
        var start = i;
        i++;
        while (expression.Length > i)
            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '\u202f' or '\u00a0' or '٫' or '’' or '٬' or '⹁' &&
                (!separator.HasValue || expression[i] != separator.Value))
            {
                i++;
            }
            else
            {
                //an exponential notation number
                if (expression[i] is 'e' or 'E')
                {
                    i++;
                    if (expression.Length > i && expression[i] is '-' or '+')
                        i++;
                }
                else
                {
                    break;
                }
            }

        //if the last symbol is 'e' it's the natural logarithmic base constant
        if (expression[i - 1] is 'e')
            i--;

        return double.Parse(expression[start..i], NumberStyles.Number | NumberStyles.AllowExponent, provider);
    }

    #region private static Try Evaluate Methods

    private static bool TryEvaluateModulus(ReadOnlySpan<char> expression, IMathContext? context,
        IFormatProvider provider, ref int i, char? separator, bool isAbs, ref double value)
    {
        const string fn = "mod";
        if (!expression[i..].StartsWith(fn, StringComparison.InvariantCultureIgnoreCase))
            return false;

        i += 3;
        value %= EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true);
        return true;
    }

    private static bool TryEvaluatePi(ReadOnlySpan<char> expression, ref int i, ref double value)
    {
        if (expression.Length <= i + 1 || expression[i] is not ('p' or 'P') || expression[i + 1] is not ('i' or 'I'))
            return false;

        i += 2;
        if (expression.Length - 1 > i && expression[i] == '(' && expression[i + 1] == ')') //PI()
            i += 2;

        value = (value == 0 ? 1 : value) * Math.PI;
        return true;
    }

    private static bool TryEvaluateCurrencySymbol(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i)
    {
        var currencySymbol = NumberFormatInfo.GetInstance(provider).CurrencySymbol;
        if (!expression[i..].StartsWith(currencySymbol))
            return false;

        i += currencySymbol.Length;
        return true;
    }

    private static bool TryEvaluateFn(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, bool isAbs, ref double value)
    {
        if (!MathFnEvaluator.TryGetTrigonometricFn(expression, ref i, out var fn))
            if (MathFnEvaluator.TryGetLogarithmFn(expression, ref i, out var logFn))
                fn = logFn;
            else if (MathFnEvaluator.TryGetAbsFn(expression, ref i, out var absFn))
                fn = absFn;

        if (fn != null)
        {
            var a = EvaluateBasic(expression, context, provider, ref i, separator, isAbs, true);
            value = (value == 0 ? 1 : value) * fn(a);
            return true;
        }

        const string undefinedStr = "undefined";
        if (expression[i..].StartsWith(undefinedStr, StringComparison.InvariantCultureIgnoreCase))
        {
            i += undefinedStr.Length;
            value = double.NaN;
            return true;
        }

        return false;
    }

    #endregion
}