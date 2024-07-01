using System;
using System.Globalization;
using MathEvaluation.Context;

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

    private static double EvaluateLowestBasic(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
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

    private static double EvaluateBasic(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
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
                    if (isEvaluatedFirst)
                        return value;

                    if (expression.Length > i + 1 && expression[i + 1] == '/')
                    {
                        i += 2;
                        value = Math.Floor(value / EvaluateOperand(expression, context, provider, ref i, separator));
                        break;
                    }

                    i++;
                    value /= EvaluateBasic(expression, context, provider, ref i, separator, true);
                    break;
                case '*':
                    if (expression.Length > i + 1 && expression[i + 1] == '*')
                    {
                        value = EvaluateExponentiation(expression, context, provider, ref i, separator, value);
                        break;
                    }

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
                case '%' when context is not IScientificMathContext:
                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value %= EvaluateBasic(expression, context, provider, ref i, separator, true);
                    break;
                case '^' when context is IScientificMathContext:
                    value = EvaluateExponentiation(expression, context, provider, ref i, separator, value);
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

    private static double EvaluateExponentiation(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator = null, double value = default)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        if ((context is IScientificMathContext && expression.Length > i && expression[i] == '^') ||
            (context is not IScientificMathContext && expression.Length > i + 1 && expression[i] == '*' && expression[i + 1] == '*'))
        {
            i++;
            if (expression[i] == '*')
                i++;

            var power = EvaluateOperand(expression, context, provider, ref i, separator);
            return Math.Pow(value, power);
        }

        return value;
    }

    private static double EvaluateOperand(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        switch (expression[i])
        {
            case '(':
                i++;
                var value = EvaluateLowestBasic(expression, context, provider, ref i, ')');
                return EvaluateExponentiation(expression, context, provider, ref i, separator, value);
            case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                value = GetNumber(expression, provider, ref i, separator);
                var entity = context?.FindMathEntity(expression[i..]);
                if (entity is MathVariable mathVariable)
                {
                    i += entity.Key.Length;
                    value = (value == 0 ? 1 : value) * mathVariable.Value;
                }
                if (entity is MathConstant mathConstant)
                {
                    i += entity.Key.Length;
                    value = (value == 0 ? 1 : value) * mathConstant.Value;
                }
                if (entity is MathOperandConverter mathConverter)
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    value = fn(value);
                }
                return EvaluateExponentiation(expression, context, provider, ref i, separator, value);
            case '-':
                i++;
                return -EvaluateOperand(expression, context, provider, ref i, separator);
            default:
                value = EvaluateFnOrConstant(expression, context, provider, ref i, separator, true, 0d);
                return EvaluateExponentiation(expression, context, provider, ref i, separator, value);
        }
    }

    private static double EvaluateFnOrConstant(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, bool isEvaluatedFirst, double value)
    {
        if (context != null && TryEvaluateContext(expression, context, provider, ref i, separator, isEvaluatedFirst, ref value))
            return value;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        var end = expression[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported");
    }

    private static bool TryEvaluateContext(ReadOnlySpan<char> expression, IMathContext context, IFormatProvider provider,
        ref int i, char? separator, bool isEvaluatedFirst, ref double value)
    {
        var entity = context.FindMathEntity(expression[i..]);
        switch (entity)
        {
            case MathVariable mathVariable:
                i += entity.Key.Length;
                value = (value == 0 ? 1 : value) *
                    EvaluateExponentiation(expression, context, provider, ref i, separator, mathVariable.Value);
                return true;
            case MathConstant mathConstant:
                i += entity.Key.Length;
                value = (value == 0 ? 1 : value) *
                    EvaluateExponentiation(expression, context, provider, ref i, separator, mathConstant.Value);
                return true;
            case MathOperandConverter mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    var result = fn(value);
                    value = EvaluateExponentiation(expression, context, provider, ref i, separator, result);
                    return true;
                }
            case MathFunction mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.Separator.HasValue
                        ? fn(EvaluateLowestBasic(expression, context, provider, ref i, mathFunction.Separator))
                        : fn(EvaluateOperand(expression, context, provider, ref i, separator));
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, result);
                    return true;
                }
            case MathOperator mathOperator:
                {
                    if (isEvaluatedFirst)
                        return true;

                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var rightOperand = EvaluateOperand(expression, context, provider, ref i, separator);
                    EvaluateExponentiation(expression, context, provider, ref i, separator, rightOperand);
                    value = fn(value, rightOperand);
                    return true;
                }
            default:
                return false;
        }
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