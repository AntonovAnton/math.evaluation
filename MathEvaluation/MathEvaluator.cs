using System;
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

    internal static double EvaluateLowestBasic(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator = null, double value = default)
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
                case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                    value = GetNumber(expression, provider, ref i, separator);
                    break;
                case ' ':
                    i++;
                    break;
                case '+':
                    i++;
                    value += EvaluateBasic(expression, context, provider, ref i, separator);
                    break;
                case '-':
                    i++;
                    while (expression.Length > i && expression[i] is ' ')
                        i++;

                    //two negatives should combine to make a positive
                    if (expression.Length > i && expression[i] is '-')
                    {
                        i++;
                        value += EvaluateBasic(expression, context, provider, ref i, separator);
                    }
                    else
                    {
                        value -= EvaluateBasic(expression, context, provider, ref i, separator);
                    }

                    break;
                default:
                    value = EvaluateBasic(expression, context, provider, ref i, separator, false, value);
                    break;
            }
        }

        return value;
    }

    internal static double EvaluateBasic(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, bool isEvaluatedFirst = false, double value = default)
    {
        var start = i;
        while (expression.Length > i)
        {
            if (separator.HasValue && expression[i] == separator.Value)
                return value;

            switch (expression[i])
            {
                case '(':
                    i++;
                    var result = EvaluateLowestBasic(expression, context, provider, ref i, ')');
                    value = (value == 0 ? 1 : value) * EvaluateExponentiation(expression, context, provider, ref i, separator, result);
                    break;
                case ' ':
                    i++;
                    break;
                case '/':
                case '÷' when context is IScientificMathContext:
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value /= EvaluateBasic(expression, context, provider, ref i, separator, true);
                    break;
                case '*':
                    if (expression.Length > i + 1 && expression[i + 1] == '*')
                        return EvaluateExponentiation(expression, context, provider, ref i, separator, value);

                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value *= EvaluateBasic(expression, context, provider, ref i, separator);
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
                    value = -EvaluateBasic(expression, context, provider, ref i, separator, true);
                    break;
                case > (char)43 and < (char)58 or '٫': // ,-./0123456789٫
                    value = GetNumber(expression, provider, ref i, separator);
                    break;
                case '×' or '·' when context is IScientificMathContext: //Addition
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value *= EvaluateBasic(expression, context, provider, ref i, separator);
                    break;
                case '%' when context is not IScientificMathContext:
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value %= EvaluateBasic(expression, context, provider, ref i, separator, true);
                    break;
                case '^' when context is IScientificMathContext:
                    return EvaluateExponentiation(expression, context, provider, ref i, separator, value);
                case '\u221a' when context is IScientificMathContext: //square root symbol
                    if (isEvaluatedFirst && start != i)
                        return value;
                    i++;
                    result = Math.Sqrt(EvaluateBasic(expression, context, provider, ref i, separator, true));
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, result);
                    break;
                case '\u221b' when context is IScientificMathContext: //cube root symbol
                    if (isEvaluatedFirst && start != i)
                        return value;
                    i++;
                    result = Math.Pow(EvaluateBasic(expression, context, provider, ref i, separator, true), 1 / 3d);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, result);
                    break;
                case '\u221c' when context is IScientificMathContext: //fourth root symbol
                    if (isEvaluatedFirst && start != i)
                        return value;
                    i++;
                    result = Math.Pow(EvaluateBasic(expression, context, provider, ref i, separator, true), 0.25d);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, result);
                    break;
                default:
                    value = EvaluateFnOrConstant(expression, context, provider, ref i, separator, isEvaluatedFirst, value);
                    if (isEvaluatedFirst)
                        return value;
                    break;
            }
        }

        if (value == 0d && expression[start..i].IsWhiteSpace())
            return double.NaN;

        return value;
    }

    internal static double EvaluateExponentiation(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator = null, double value = default)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        //exponentiation
        if ((context is IScientificMathContext && expression.Length > i && expression[i] == '^') ||
            (context is not IScientificMathContext && expression.Length > i + 1 && expression[i] == '*' && expression[i + 1] == '*'))
        {
            i++;
            if (expression[i] == '*')
                i++;


            return Math.Pow(value, EvaluateBasic(expression, context, provider, ref i, separator, true));
        }

        return value;
    }

    private static double EvaluateFnOrConstant(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, bool isEvaluatedFirst, double value)
    {
        var start = i;

        if (context?.TryEvaluate(expression, provider, ref i, separator, isEvaluatedFirst, ref value) == true)
            return value;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        var end = expression[start..].IndexOfAny("([ |") + start;
        var unknownSubstring = end > start ? expression[start..end] : expression[start..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported");
    }

    private static bool TryEvaluateCurrencySymbol(ReadOnlySpan<char> expression, IFormatProvider provider,
        ref int i)
    {
        var currencySymbol = NumberFormatInfo.GetInstance(provider).CurrencySymbol;
        if (!expression[i..].StartsWith(currencySymbol))
            return false;

        i += currencySymbol.Length;
        return true;
    }

    private static double GetNumber(ReadOnlySpan<char> expression, IFormatProvider provider,
        ref int i, char? separator)
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
}