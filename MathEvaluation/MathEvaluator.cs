using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;

namespace MathEvaluation;

public partial class MathEvaluator(string expression)
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
        ref int i, char? separator = null, char? closingSymbol = null, double value = default)
    {
        while (expression.Length > i)
        {
            if ((closingSymbol.HasValue && expression[i] == closingSymbol.Value) ||
                (separator.HasValue && expression[i] == separator.Value))
            {
                i++;
                return value;
            }

            switch (expression[i])
            {
                case '+':
                    i++;
                    value += EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol);
                    break;
                case '-':
                    i++;
                    while (expression.Length > i && expression[i] is ' ')
                        i++;

                    //two negatives should combine to make a positive
                    if (expression.Length > i && expression[i] is '-')
                    {
                        i++;
                        value += EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol);
                    }
                    else
                    {
                        value -= EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol);
                    }

                    break;
                default:
                    value = EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol);
                    break;
            }
        }

        return value;
    }

    private static double EvaluateBasic(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, bool isEvaluatedFirst = false, double value = default)
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
                    i++;
                    var result = EvaluateLowestBasic(expression, context, provider, ref i, null, ')');
                    result = EvaluateConverter(expression, context, ref i, result);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    break;
                case ' ':
                    i++;
                    break;
                case '/':
                    if (isEvaluatedFirst)
                        return value;

                    if (context is IProgrammingMathContext or IScientificMathContext &&
                        expression.Length > i + 1 && expression[i + 1] == '/')
                    {
                        i += 2;
                        value = Math.Floor(value / EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol, true));
                        break;
                    }

                    i++;
                    value /= EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol, true);
                    break;
                case '*':
                    if (expression.Length > i + 1 && expression[i + 1] == '*')
                    {
                        value = EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, value);
                        break;
                    }

                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value *= EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol);
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
                    value = -EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol, true);
                    break;
                case > (char)43 and < (char)58 or '٫': // ,-./0123456789٫
                    value = GetNumber(expression, provider, ref i, separator, closingSymbol);
                    break;
                case '^':
                    value = EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, value);
                    break;
                default:
                    value = EvaluateFnOrConstant(expression, context, provider, ref i, separator, closingSymbol, isEvaluatedFirst, value);
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
        ref int i, char? separator, char? closingSymbol, double value)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        if ((context is IScientificMathContext && expression.Length > i && expression[i] == '^') ||
            (context is IProgrammingMathContext && expression.Length > i + 1 && expression[i] == '*' && expression[i + 1] == '*'))
        {
            i++;
            if (expression[i] == '*')
                i++;

            var power = EvaluateOperand(expression, context, provider, ref i, separator, closingSymbol);
            power = EvaluateConverter(expression, context, ref i, power);
            return Math.Pow(value, power);
        }

        return value;
    }

    private static double EvaluateOperand(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        switch (expression[i])
        {
            case '(':
                i++;
                return EvaluateLowestBasic(expression, context, provider, ref i, null, ')');
            case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                {
                    var value = GetNumber(expression, provider, ref i, separator, closingSymbol);
                    value = EvaluateVariableOrConverter(expression, context, ref i, value);
                    return EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, value);
                }
            case '-':
                i++;
                return -EvaluateOperand(expression, context, provider, ref i, separator, closingSymbol);
            default:
                return EvaluateFnOrConstant(expression, context, provider, ref i, separator, closingSymbol, true, 0d);
        }
    }

    private static double EvaluateVariableOrConverter(ReadOnlySpan<char> expression, IMathContext? context, ref int i, double value,
        IMathEntity? entity = null)
    {
        entity = entity ?? context?.FindMathEntity(expression[i..]);
        if (entity is MathVariable<double> mathVariable)
        {
            i += entity.Key.Length;
            var result = EvaluateConverter(expression, context, ref i, mathVariable.Value);
            value = (value == 0 ? 1 : value) * result;
        }
        else if (entity is MathOperandConverter<double> mathConverter)
        {
            i += entity.Key.Length;
            var fn = mathConverter.Fn;
            value = fn(value);
        }
        else if (entity is MathVariable<decimal> or MathOperandConverter<decimal>)
        {
            var decimalValue = EvaluateVariableOrConverterDecimal(expression, context, ref i, (decimal)value, entity);
            return (double)decimalValue;
        }
        return value;
    }

    private static double EvaluateConverter(ReadOnlySpan<char> expression, IMathContext? context, ref int i, double value,
        IMathEntity? entity = null)
    {
        entity = entity ?? context?.FindMathEntity(expression[i..]);
        if (entity is MathOperandConverter<double> mathConverter)
        {
            i += entity.Key.Length;
            var fn = mathConverter.Fn;
            value = fn(value);
        }
        else if (entity is MathOperandConverter<decimal>)
        {
            var decimalValue = EvaluateConverterDecimal(expression, context, ref i, (decimal)value, entity);
            return (double)decimalValue;
        }
        return value;
    }

    private static double EvaluateFnOrConstant(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, bool isEvaluatedFirst, double value)
    {
        if (context != null && TryEvaluateContext(expression, context, provider, ref i, separator, closingSymbol, isEvaluatedFirst, ref value))
            return value;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        var decimalValue = (decimal)value;
        if (context != null && TryEvaluateContextDecimal(expression, context, provider, ref i, separator, closingSymbol, isEvaluatedFirst, ref decimalValue))
            return (double)decimalValue;

        var end = expression[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static bool TryEvaluateContext(ReadOnlySpan<char> expression, IMathContext context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, bool isEvaluatedFirst, ref double value)
    {
        var entity = context.FindMathEntity(expression[i..]);
        switch (entity)
        {
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateConverter(expression, context, ref i, mathVariable.Value);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathVariableFunction<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateConverter(expression, context, ref i, mathVariable.GetValue());
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandConverter<double> mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    var result = fn(value);
                    value = EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperator<double> mathOperator:
                {
                    if (isEvaluatedFirst)
                        return true;

                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var rightOperand = EvaluateBasic(expression, context, provider, ref i, separator, closingSymbol, true);
                    value = fn(value, rightOperand);
                    return true;
                }
            case BasicMathFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.ClosingSymbol.HasValue
                        ? fn(EvaluateLowestBasic(expression, context, provider, ref i, null, mathFunction.ClosingSymbol))
                        : fn(EvaluateOperand(expression, context, provider, ref i, separator, null));
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    if (expression.Length <= i || expression[i] != mathFunction.OpenningSymbol)
                        return false;

                    i++;
                    var args = new List<double>();
                    while (expression.Length > i)
                    {
                        var arg = EvaluateLowestBasic(expression, context, provider, ref i, mathFunction.Separator, mathFunction.ClosingSymbol);
                        args.Add(arg);
                        if (expression.Length <= i || expression[i - 1] != mathFunction.Separator)
                        {
                            break;
                        }
                    }
                    var result = mathFunction.Fn([.. args]);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
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
        ref int i, char? separator, char? closingSymbol)
    {
        var str = GetNumberString(expression, ref i, separator, closingSymbol);
        return double.Parse(str, NumberStyles.Number | NumberStyles.AllowExponent, provider);
    }

    private static ReadOnlySpan<char> GetNumberString(ReadOnlySpan<char> expression, ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        i++;
        while (expression.Length > i)
            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '\u202f' or '\u00a0' or '٫' or '’' or '٬' or '⹁' &&
                (!separator.HasValue || expression[i] != separator.Value) &&
                (!closingSymbol.HasValue || expression[i] != closingSymbol.Value))
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

        return expression[start..i];
    }
}