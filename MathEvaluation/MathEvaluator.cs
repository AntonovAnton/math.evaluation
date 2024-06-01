using System;
using System.Globalization;

namespace MathEvaluation;

public static class MathEvaluator
{
    public const char FnParamsSeparator = ',';

    public static double Evaluate(string expression, IFormatProvider? provider = null)
    {
        return Evaluate(expression.AsSpan(), provider);
    }

    public static double Evaluate(ReadOnlySpan<char> expression, IFormatProvider? provider)
    {
        try
        {
            var i = 0;
            return EvaluateLowestBasic(expression, provider ?? CultureInfo.CurrentCulture, ref i);
        }
        catch (Exception ex)
        {
            ex.Data[nameof(expression)] = expression.ToString();
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
                    value = (value == 0 ? 1 : value) *
                            EvaluateBasic(expression, provider, ref i, isFnParam, true, Math.PI);
                    return value;
                case '^':
                    i++;
                    value = Math.Pow(value, EvaluateBasic(expression, provider, ref i, isFnParam));
                    return value;
                case 'e': //the natural logarithmic base
                    i++;
                    value = (value == 0 ? 1 : value) *
                            EvaluateBasic(expression, provider, ref i, isFnParam, true, Math.E);
                    return value;
                case '\u00b0': //degree symbol
                    i++;
                    return MathFunctions.DegreesToRadians(value);
                default:
                    return EvaluateFn(expression, provider, ref i, isFnParam, value);
            }

        return value;
    }

    private static double EvaluateFn(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        bool isFnParam, double value)
    {
        var start = i;

        if (TryEvaluateModulus(expression, provider, ref i, isFnParam, ref value))
            return value;

        if (TryEvaluatePi(expression, ref i, ref value))
            return value;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        if (TryEvaluateFn(expression, provider, ref i, ref value))
            return value;

        var bracketCharIndex = expression[start..].IndexOf('(') + start;
        var unknownSubstring = bracketCharIndex > i ? expression[start..bracketCharIndex] : expression[start..];

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

    private static bool TryEvaluateModulus(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        bool isFnParam, ref double value)
    {
        const string fn = "mod";
        if (!expression[i..].StartsWith(fn, StringComparison.InvariantCultureIgnoreCase))
            return false;

        i += 3;
        value %= EvaluateBasic(expression, provider, ref i, isFnParam, true);
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

    private static bool TryEvaluateFn(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        ref double value)
    {
        if (MathFunctions.TryGetTrigonometricFn(expression, ref i, out var fn) && fn != null)
        {
            var a = EvaluateLowestBasic(expression, provider, ref i);
            value = (value == 0 ? 1 : value) * fn(a);
            return true;
        }

        return false;
    }

    #endregion
}