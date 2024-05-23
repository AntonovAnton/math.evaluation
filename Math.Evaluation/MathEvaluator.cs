using System;
using System.Globalization;

namespace Math.Evaluation;

public static class MathEvaluator
{
    public static double Evaluate(string expression, IFormatProvider? formatProvider = null)
    {
        try
        {
            var i = 0;
            var value = Calculate(expression.AsSpan(), formatProvider ?? CultureInfo.CurrentCulture, ref i);
            return value;
        }
        catch (Exception ex)
        {
            ex.Data[nameof(expression)] = expression;
            ex.Data[nameof(formatProvider)] = formatProvider;
            throw;
        }
    }

    private static double Calculate(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i
        , double value = default)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    var parenthesesI = 0;
                    value = (value == 0 ? 1 : value) *
                            Calculate(expression[i..], formatProvider, ref parenthesesI, value);
                    i += parenthesesI;
                    break;
                case ')':
                    i++;
                    return value;
                case >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    value = GetNumber(expression, formatProvider, ref i);
                    break;
                case '-':
                    i++;
                    while (expression.Length > i && expression[i] is ' ')
                    {
                        i++;
                    }

                    //two negatives should combine to make a positive
                    if (expression[i] is '-')
                    {
                        i++;
                        value += Evaluate(expression, formatProvider, ref i);
                    }
                    else
                        value -= Evaluate(expression, formatProvider, ref i);

                    break;
                case '+':
                    i++;
                    value += Evaluate(expression, formatProvider, ref i);
                    break;
                case ' ':
                    i++;
                    break;
                default:
                    value = Evaluate(expression, formatProvider, ref i, false, value);
                    break;
            }
        }

        return value;
    }

    private static double Evaluate(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i,
        bool isEvaluatedFirst = false, double value = default)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    var parenthesesI = 0;
                    value = (value == 0 ? 1 : value) *
                            Calculate(expression[i..], formatProvider, ref parenthesesI, value);
                    i += parenthesesI;
                    break;
                case ')':
                    return value;
                case >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    value = GetNumber(expression, formatProvider, ref i);
                    value = EvaluateFnOrConstant(expression, formatProvider, ref i, value);
                    if (isEvaluatedFirst)
                        return value;
                    break;
                case '*':
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value *= Evaluate(expression, formatProvider, ref i, true);
                    break;
                case '/':
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value /= Evaluate(expression, formatProvider, ref i, true);
                    break;
                case '-' or '+':
                    return value;
                case ' ':
                    i++;
                    break;
                default:
                    value = EvaluateFnOrConstant(expression, formatProvider, ref i, value);
                    break;
            }
        }

        return value;
    }

    private static double EvaluateFnOrConstant(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i,
        double value = default)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    var parenthesesI = 0;
                    value = (value == 0 ? 1 : value) *
                            Calculate(expression[i..], formatProvider, ref parenthesesI, value);
                    i += parenthesesI;
                    break;
                case ')':
                    return value;
                case 'π':
                    i++;
                    value = (value == 0 ? 1 : value) * System.Math.PI;
                    return value;
                case ' ' or '*' or '/' or '-' or '+' or >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    return value;
                default:

                    const string pi = "pi";
                    if (expression[i..].StartsWith(pi, StringComparison.InvariantCultureIgnoreCase))
                    {
                        i += 2;
                        value = (value == 0 ? 1 : value) * System.Math.PI;
                        return value;
                    }

                    var currencySymbol = NumberFormatInfo.GetInstance(formatProvider).CurrencySymbol;
                    if (expression[i..].StartsWith(currencySymbol))
                    {
                        i += currencySymbol.Length;
                        return value;
                    }

                    const string supportedChars = "() */-+0123456789.,\u202f\u00a0٫";
                    var supportedCharIndex = expression[i..].IndexOfAny(supportedChars) + i;
                    var unknownSubstring = supportedCharIndex > i ? expression[i..supportedCharIndex] : expression[i..];

                    throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported");
            }
        }

        return value;
    }

    private static double GetNumber(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i)
    {
        var start = i;
        i++;
        while (expression.Length > i)
        {
            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '\u202f' or '\u00a0' or '٫' or '’' or '٬' or '⹁')
                i++;
            else
                break;
        }

        var value = double.Parse(expression[start..i], NumberStyles.Number, formatProvider);
        return value;
    }
}