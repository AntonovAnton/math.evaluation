using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;

namespace MathEvaluation;

public partial class MathEvaluator
{
    public decimal EvaluateDecimal(IFormatProvider? provider = null)
    {
        return EvaluateDecimal(Expression.AsSpan(), Context, provider);
    }

    public static decimal EvaluateDecimal(string expression, IFormatProvider? provider = null)
    {
        return EvaluateDecimal(expression.AsSpan(), null, provider);
    }

    public static decimal EvaluateDecimal(ReadOnlySpan<char> expression, IFormatProvider? provider = null)
    {
        return EvaluateDecimal(expression, null, provider);
    }

    public static decimal EvaluateDecimal(string expression, IMathContext? context, IFormatProvider? provider = null)
    {
        return EvaluateDecimal(expression.AsSpan(), context, provider);
    }

    public static decimal EvaluateDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider? provider = null)
    {
        try
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.IsWhiteSpace())
                throw new ArgumentException("Expression is empty or white space.", nameof(expression));

            var i = 0;
            return EvaluateLowestBasicDecimal(expression, context, provider ?? CultureInfo.CurrentCulture, ref i);
        }
        catch (Exception ex)
        {
            ex.Data[nameof(expression)] = expression.ToString();
            ex.Data[nameof(provider)] = provider;
            throw;
        }
    }

    private static decimal EvaluateLowestBasicDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator = null, char? closingSymbol = null, decimal value = default)
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
                    value += EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol);
                    break;
                case '-':
                    i++;
                    while (expression.Length > i && expression[i] is ' ')
                        i++;

                    //two negatives should combine to make a positive
                    if (expression.Length > i && expression[i] is '-')
                    {
                        i++;
                        value += EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol);
                    }
                    else
                    {
                        value -= EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol);
                    }

                    break;
                default:
                    value = EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol);
                    break;
            }
        }

        return value;
    }

    private static decimal EvaluateBasicDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, bool isEvaluatedFirst = false, decimal value = default)
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
                    var result = EvaluateLowestBasicDecimal(expression, context, provider, ref i, null, ')');
                    result = EvaluateConverterDecimal(expression, context, ref i, result);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
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
                        value = Math.Floor(value / EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol, true));
                        break;
                    }

                    i++;
                    value /= EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol, true);
                    break;
                case '*':
                    if (expression.Length > i + 1 && expression[i + 1] == '*')
                    {
                        value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, value);
                        break;
                    }

                    if (isEvaluatedFirst)
                        return value;
                    i++;
                    value *= EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol);
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
                    value = -EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol, true);
                    break;
                case > (char)43 and < (char)58 or '٫': // ,-./0123456789٫
                    value = GetDecimalNumber(expression, provider, ref i, separator, closingSymbol);
                    break;
                case '^':
                    value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, value);
                    break;
                default:
                    value = EvaluateFnOrConstantDecimal(expression, context, provider, ref i, separator, closingSymbol, isEvaluatedFirst, value);
                    if (isEvaluatedFirst)
                        return value;
                    break;
            }
        }

        if (value == 0m && expression[start..i].IsWhiteSpace())
            throw new ArgumentException("Expression cannot be evaluated.", nameof(expression));

        return value;
    }

    private static decimal EvaluateExponentiationDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, decimal value)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        if ((context is IScientificMathContext && expression.Length > i && expression[i] == '^') ||
            (context is IProgrammingMathContext && expression.Length > i + 1 && expression[i] == '*' && expression[i + 1] == '*'))
        {
            i++;
            if (expression[i] == '*')
                i++;

            var power = EvaluateOperandDecimal(expression, context, provider, ref i, separator, closingSymbol);
            power = EvaluateConverterDecimal(expression, context, ref i, power);
            return (decimal)Math.Pow((double)value, (double)power);
        }

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
                i++;
                return EvaluateLowestBasicDecimal(expression, context, provider, ref i, null, ')');
            case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                {
                    var value = GetDecimalNumber(expression, provider, ref i, separator, closingSymbol);
                    value = EvaluateVariableOrConverterDecimal(expression, context, ref i, value);
                    return EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, value);
                }
            case '-':
                i++;
                return -EvaluateOperandDecimal(expression, context, provider, ref i, separator, closingSymbol);
            default:
                return EvaluateFnOrConstantDecimal(expression, context, provider, ref i, separator, closingSymbol, true, 0m);
        }
    }

    private static decimal EvaluateVariableOrConverterDecimal(ReadOnlySpan<char> expression, IMathContext? context, ref int i, decimal value,
        IMathEntity? entity = null)
    {
        entity = entity ?? context?.FindMathEntity(expression[i..]);
        if (entity is MathVariable<decimal> mathVariable)
        {
            i += entity.Key.Length;
            var result = EvaluateConverterDecimal(expression, context, ref i, mathVariable.Value);
            value = (value == 0 ? 1 : value) * result;
        }
        else if (entity is MathOperandConverter<decimal> mathConverter)
        {
            i += entity.Key.Length;
            var fn = mathConverter.Fn;
            value = fn(value);
        }
        else if (entity is MathVariable<double> or MathOperandConverter<double>)
        {
            var doubleValue = EvaluateVariableOrConverter(expression, context, ref i, (double)value, entity);
            return (decimal)doubleValue;
        }
        return value;
    }

    private static decimal EvaluateConverterDecimal(ReadOnlySpan<char> expression, IMathContext? context, ref int i, decimal value,
        IMathEntity? entity = null)
    {
        entity = entity ?? context?.FindMathEntity(expression[i..]);
        if (entity is MathOperandConverter<decimal> mathConverter)
        {
            i += entity.Key.Length;
            var fn = mathConverter.Fn;
            value = fn(value);
        }
        else if (entity is MathOperandConverter<double>)
        {
            var doubleValue = EvaluateConverter(expression, context, ref i, (double)value, entity);
            return (decimal)doubleValue;
        }
        return value;
    }

    private static decimal EvaluateFnOrConstantDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, bool isEvaluatedFirst, decimal value)
    {
        if (context != null && TryEvaluateContextDecimal(expression, context, provider, ref i, separator, closingSymbol, isEvaluatedFirst, ref value))
            return value;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        var doubleValue = (double)value;
        if (context != null && TryEvaluateContext(expression, context, provider, ref i, separator, closingSymbol, isEvaluatedFirst, ref doubleValue))
        {
            return (decimal)doubleValue;
        }

        var end = expression[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static bool TryEvaluateContextDecimal(ReadOnlySpan<char> expression, IMathContext context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, bool isEvaluatedFirst, ref decimal value)
    {
        var entity = context.FindMathEntity(expression[i..]);
        switch (entity)
        {
            case MathVariable<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateConverterDecimal(expression, context, ref i, mathVariable.Value);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathVariableFunction<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateConverterDecimal(expression, context, ref i, mathVariable.GetValue());
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandConverter<decimal> mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    var result = fn(value);
                    value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperator<decimal> mathOperator:
                {
                    if (isEvaluatedFirst)
                        return true;

                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var rightOperand = EvaluateBasicDecimal(expression, context, provider, ref i, separator, closingSymbol, true);
                    value = fn(value, rightOperand);
                    return true;
                }
            case BasicMathFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.ClosingSymbol.HasValue
                        ? fn(EvaluateLowestBasicDecimal(expression, context, provider, ref i, null, mathFunction.ClosingSymbol))
                        : fn(EvaluateOperandDecimal(expression, context, provider, ref i, separator, null));
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    if (expression.Length <= i || expression[i] != mathFunction.OpenningSymbol)
                        return false;

                    i++;
                    var args = new List<decimal>();
                    while (expression.Length > i)
                    {
                        var arg = EvaluateLowestBasicDecimal(expression, context, provider, ref i, mathFunction.Separator, mathFunction.ClosingSymbol);
                        args.Add(arg);
                        if (expression.Length <= i || expression[i - 1] != mathFunction.Separator)
                        {
                            break;
                        }
                    }
                    var result = mathFunction.Fn([.. args]);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
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