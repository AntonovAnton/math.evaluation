using System;
using System.Globalization;

namespace MathEvaluation;

public static class MathEvaluator
{
    public const char FnParamsSeparator = ',';

    public static double Evaluate(string expression, IFormatProvider? provider = null)
    {
        try
        {
            var i = 0;
            return EvaluateLowestBasic(expression.AsSpan(), provider ?? CultureInfo.CurrentCulture, ref i);
        }
        catch (Exception ex)
        {
            ex.Data[nameof(expression)] = expression;
            ex.Data[nameof(provider)] = provider;
            throw;
        }
    }

    private static double EvaluateLowestBasic(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        bool isFnParam = false, double value = default)
    {
        while (expression.Length > i)
            switch (expression[i])
            {
                case '(':
                    i++;
                    value = (value == 0 ? 1 : value) * EvaluateLowestBasic(expression, provider, ref i);
                    break;
                case ')':
                    i++;
                    while (expression.Length > i && expression[i] is ' ')
                        i++;

                    if (expression.Length > i && expression[i] == '^')
                    {
                        i++;
                        value = Math.Pow(value, EvaluateBasic(expression, provider, ref i, isFnParam));
                        return value;
                    }

                    return value;
                case FnParamsSeparator when isFnParam:
                    i++;
                    return value;
                case >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    value = GetNumber(expression, provider, ref i, isFnParam);
                    break;
                case ' ':
                    i++;
                    break;
                case '+':
                    i++;
                    value += EvaluateBasic(expression, provider, ref i, isFnParam);
                    break;
                case '-':
                    i++;
                    while (expression.Length > i && expression[i] is ' ')
                        i++;

                    //two negatives should combine to make a positive
                    if (expression[i] is '-')
                    {
                        i++;
                        value += EvaluateBasic(expression, provider, ref i, isFnParam);
                    }
                    else
                    {
                        value -= EvaluateBasic(expression, provider, ref i, isFnParam);
                    }

                    break;
                default:
                    value = EvaluateBasic(expression, provider, ref i, isFnParam, false, value);
                    break;
            }

        return value;
    }

    private static double EvaluateBasic(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        bool isFnParam, bool isEvaluatedFirst = false, double value = default)
    {
        var start = i;
        while (expression.Length > i)
            switch (expression[i])
            {
                case '(':
                    i++;
                    value = (value == 0 ? 1 : value) * EvaluateLowestBasic(expression, provider, ref i);
                    break;
                case ')':
                    return value;
                case FnParamsSeparator when isFnParam:
                    return value;
                case >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    value = GetNumber(expression, provider, ref i, isFnParam);
                    break;
                case ' ':
                    i++;
                    break;
                case '*':
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    if (i < expression.Length && expression[i] == '*')
                    {
                        i++;
                        value = Math.Pow(value, EvaluateBasic(expression, provider, ref i, isFnParam));
                    }
                    else
                    {
                        value *= EvaluateBasic(expression, provider, ref i, isFnParam);
                    }

                    break;
                case '\u00d7' or '·': //Addition
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value *= EvaluateBasic(expression, provider, ref i, isFnParam);
                    break;
                case '/' or '\u00f7':
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value /= EvaluateBasic(expression, provider, ref i, isFnParam, true);
                    break;
                case '%':
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value %= EvaluateBasic(expression, provider, ref i, isFnParam, true);
                    break;
                case '+':
                    if (start == i || expression[start..i].IsWhiteSpace())
                    {
                        i++;
                        break;
                    }

                    return value;
                case '-':
                    if (start == i || expression[start..i].IsWhiteSpace())
                    {
                        i++;
                        value = -EvaluateBasic(expression, provider, ref i, isFnParam);
                        break;
                    }

                    return value;
                default:
                    value = EvaluateFnOrConstant(expression, provider, ref i, isFnParam, value);
                    if (isEvaluatedFirst)
                        return value;
                    break;
            }

        return value;
    }

    private static double EvaluateFnOrConstant(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        bool isFnParam, double value = default)
    {
        while (expression.Length > i)
            switch (expression[i])
            {
                case 'π':
                    i++;
                    value = (value == 0 ? 1 : value) * Math.PI;
                    return value;
                case '^':
                    i++;
                    value = Math.Pow(value, EvaluateBasic(expression, provider, ref i, isFnParam));
                    return value;
                default:
                    return EvaluateFn(expression, provider, ref i, isFnParam, value);
            }

        return value;
    }

    private static double EvaluateFn(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        bool isFnParam, double value)
    {
        //x mod y
        if (expression[i..].StartsWith("mod", StringComparison.InvariantCultureIgnoreCase))
        {
            i += 3;
            value %= EvaluateBasic(expression, provider, ref i, isFnParam, true);
            return value;
        }

        const string pi = "pi";
        if (expression[i..].StartsWith(pi, StringComparison.InvariantCultureIgnoreCase))
        {
            i += 2;
            value = (value == 0 ? 1 : value) * Math.PI;
            if (i < expression.Length - 1 && expression[i] == '(' && expression[i + 1] == ')') //PI()
                i += 2;

            return value;
        }

        var currencySymbol = NumberFormatInfo.GetInstance(provider).CurrencySymbol;
        if (expression[i..].StartsWith(currencySymbol))
        {
            i += currencySymbol.Length;
            return value;
        }

        //pow(x, y) or power(x, y)
        if (expression[i..].StartsWith("pow", StringComparison.InvariantCultureIgnoreCase))
        {
            var start = i;
            i += 3;
            if (expression[i..].StartsWith("er", StringComparison.InvariantCultureIgnoreCase))
                i += 2;

            if (i < expression.Length - 1 && expression[i] == '(')
                i++;
            else
                throw new NotSupportedException($"'{expression[start..i].ToString()}' isn't supported");

            var x = EvaluateLowestBasic(expression, provider, ref i, true);
            var y = EvaluateLowestBasic(expression, provider, ref i, true);
            value = (value == 0 ? 1 : value) * Math.Pow(x, y);
            return value;
        }
        
        var bracketCharIndex = expression[i..].IndexOf('(') + i;
        var unknownSubstring = bracketCharIndex > i ? expression[i..bracketCharIndex] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported");
    }

    private static double GetNumber(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        bool isFnParam = false)
    {
        var start = i;
        i++;
        while (expression.Length > i)
            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '\u202f' or '\u00a0' or '٫' or '’' or '٬' or '⹁' &&
                (isFnParam == false || expression[i] != FnParamsSeparator))
                i++;
            else
            {
                if (expression[i] is 'e' or 'E')
                {
                    i++;
                    if (expression.Length > i && expression[i] is '-' or '+')
                        i++;
                }
                else
                    break;
            }

        return double.Parse(expression[start..i], NumberStyles.Number | NumberStyles.AllowExponent, provider);
    }
}