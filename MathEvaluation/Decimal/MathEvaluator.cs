using System;
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
                    i++;
                    var result = EvaluateDecimal(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    i++;
                    result = EvaluateConverterDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    break;
                case ' ':
                    i++;
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
                    while (expression.Length > i && expression[i] is ' ')
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
                case '*':
                    if (context is IProgrammingMathContext && expression.Length > i + 1 && expression[i + 1] == '*')
                    {
                        value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, value);
                        break;
                    }

                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when expression.Length <= i || expression[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '\n' or '\r': //LF or CR
                    i++;
                    break;
                case '^' when context is IScientificMathContext:
                    value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, value);
                    break;
                default:
                    var entity = context?.FirstMathEntity(expression[i..]);

                    //highest precedence is evaluating first
                    if (precedence != (int)EvalPrecedence.Unknown && precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateFnOrConstantDecimal(expression, context, provider, ref i, separator, closingSymbol, precedence, value, entity);
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
            power = EvaluateConverterDecimal(expression, context, provider, ref i, separator, closingSymbol, power);
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
                {
                    i++;
                    var value = EvaluateDecimal(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    i++;
                    return value;
                }
            case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                {
                    var value = GetDecimalNumber(expression, provider, ref i, separator, closingSymbol);
                    value = EvaluateVariableOrConverterDecimal(expression, context, provider, ref i, separator, closingSymbol, value);
                    return EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, value);
                }
            case '-':
                i++;
                return -EvaluateOperandDecimal(expression, context, provider, ref i, separator, closingSymbol);
            default:
                return EvaluateFnOrConstantDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.FuncOrVar, 0m);
        }
    }

    private static decimal EvaluateVariableOrConverterDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, decimal value, IMathEntity? entity = null)
    {
        entity = entity ?? context?.FirstMathEntity(expression[i..]);
        if (entity is MathVariable<decimal> mathVariable)
        {
            i += entity.Key.Length;
            var result = EvaluateConverterDecimal(expression, context, provider, ref i, separator, closingSymbol, mathVariable.Value);
            value = (value == 0 ? 1 : value) * result;
        }
        else if (entity is MathOperandConverter<decimal> mathConverter)
        {
            i += entity.Key.Length;
            var fn = mathConverter.Fn;
            var result = mathConverter.IsConvertingLeftOperand
                ? fn(value)
                : fn(EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic));
            value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
        }
        else if (entity is MathVariable<double> or MathOperandConverter<double>)
        {
            var doubleValue = EvaluateVariableOrConverter(expression, context, provider, ref i, separator, closingSymbol, (double)value, entity);
            return (decimal)doubleValue;
        }
        return value;
    }

    private static decimal EvaluateConverterDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, decimal value, IMathEntity? entity = null)
    {
        entity = entity ?? context?.FirstMathEntity(expression[i..]);
        if (entity is MathOperandConverter<decimal> mathConverter)
        {
            i += entity.Key.Length;
            var fn = mathConverter.Fn;
            var result = mathConverter.IsConvertingLeftOperand
                ? fn(value)
                : fn(EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic));
            value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
        }
        else if (entity is MathOperandConverter<double>)
        {
            var doubleValue = EvaluateConverter(expression, context, provider, ref i, separator, closingSymbol, (double)value, entity);
            return (decimal)doubleValue;
        }
        return value;
    }

    private static decimal EvaluateFnOrConstantDecimal(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, decimal value, IMathEntity? entity = null)
    {
        entity = entity ?? context?.FirstMathEntity(expression[i..]);
        if (entity != null && TryEvaluateContextDecimal(expression, context!, entity, provider, ref i, separator, closingSymbol, precedence, ref value))
            return value;

        var doubleValue = (double)value;
        if (entity != null && TryEvaluateContext(expression, context!, entity, provider, ref i, separator, closingSymbol, ref doubleValue))
            return (decimal)doubleValue;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        var end = expression[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static bool TryEvaluateContextDecimal(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, ref decimal value)
    {
        switch (entity)
        {
            case MathVariable<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateConverterDecimal(expression, context, provider, ref i, separator, closingSymbol, mathVariable.Value);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathVariableFunction<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateConverterDecimal(expression, context, provider, ref i, separator, closingSymbol, mathVariable.GetValue());
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandConverter<decimal> mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    var result = mathConverter.IsConvertingLeftOperand
                        ? fn(value)
                        : fn(EvaluateDecimal(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic));
                    value = EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
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
                        : fn(EvaluateOperandDecimal(expression, context, provider, ref i, separator, null));

                    if (mathFunction.ClosingSymbol.HasValue)
                        i++;

                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiationDecimal(expression, context, provider, ref i, separator, closingSymbol, result);
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