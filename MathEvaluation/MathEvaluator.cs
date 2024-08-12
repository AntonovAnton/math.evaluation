using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;

namespace MathEvaluation;

/// <summary>
/// Fast and comprehensive evaluator for dynamically evaluating mathematical expressions from strings.
/// </summary>
public partial class MathEvaluator(string expression, IMathContext? context = null)
{
    /// <summary>Gets the math expression.</summary>
    /// <value>The math expression.</value>
    public string Expression { get; } = expression;

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext" /> interface.</value>
    public IMathContext? Context { get; internal set; } = context;

    /// <summary>Evaluates the <see cref="Expression">math expression</see>.</summary>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public double Evaluate(IFormatProvider? provider = null)
    {
        return Evaluate(Expression.AsSpan(), Context, provider);
    }

    /// <summary>Evaluates the math <paramref name="expression" />.</summary>
    /// <param name="expression">The math expression.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static double Evaluate(string expression, IFormatProvider? provider = null)
    {
        return Evaluate(expression.AsSpan(), null, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IFormatProvider?)"/>
    public static double Evaluate(ReadOnlySpan<char> expression, IFormatProvider? provider = null)
    {
        return Evaluate(expression, null, provider);
    }

    /// <summary>Evaluates the math <paramref name="expression" />.</summary>
    /// <param name="expression">The math expression.</param>
    /// <param name="context">The specified math context.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static double Evaluate(string expression, IMathContext? context, IFormatProvider? provider = null)
    {
        return Evaluate(expression.AsSpan(), context, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IMathContext?, IFormatProvider?)"/>
    public static double Evaluate(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider? provider = null)
    {
        try
        {
            if (expression == null || expression.IsWhiteSpace())
                return double.NaN;

            var i = 0;
            return Evaluate(expression, context, provider ?? CultureInfo.CurrentCulture, ref i, null, null, (int)EvalPrecedence.Unknown);
        }
        catch (Exception ex)
        {
            ex.Data[nameof(expression)] = expression.ToString();
            ex.Data[nameof(provider)] = provider;
            throw;
        }
    }

    private static double Evaluate(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, double value = default)
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
                    var result = Evaluate(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    i++;
                    result = EvaluateConverter(expression, context, provider, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    break;
                case ' ':
                    i++;
                    break;
                case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                    value = GetNumber(expression, provider, ref i, separator, closingSymbol);
                    break;
                case '+':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !expression[start..i].IsWhiteSpace())
                        return value;

                    i++;
                    value += Evaluate(expression, context, provider, ref i, separator, closingSymbol,
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
                        value += Evaluate(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic);
                    }
                    else
                    {
                        value -= Evaluate(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic);
                    }

                    break;
                case '*':
                    if (context is IProgrammingMathContext && expression.Length > i + 1 && expression[i + 1] == '*')
                    {
                        value = EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, value);
                        break;
                    }

                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= Evaluate(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when expression.Length <= i || expression[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= Evaluate(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '\n' or '\r': //LF or CR
                    i++;
                    break;
                case '^' when context is IScientificMathContext:
                    value = EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, value);
                    break;
                default:
                    var entity = context?.FirstMathEntity(expression[i..]);

                    //highest precedence is evaluating first
                    if (precedence != (int)EvalPrecedence.Unknown && precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateFnOrConstant(expression, context, provider, ref i, separator, closingSymbol, precedence, value, entity);
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
            power = EvaluateConverter(expression, context, provider, ref i, separator, closingSymbol, power);
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
                {
                    i++;
                    var value = Evaluate(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    i++;
                    return value;
                }
            case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                {
                    var value = GetNumber(expression, provider, ref i, separator, closingSymbol);
                    value = EvaluateVariableOrConverter(expression, context, provider, ref i, separator, closingSymbol, value);
                    return EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, value);
                }
            case '-':
                i++;
                return -EvaluateOperand(expression, context, provider, ref i, separator, closingSymbol);
            default:
                return EvaluateFnOrConstant(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.FuncOrVar, 0d);
        }
    }

    private static double EvaluateVariableOrConverter(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, double value, IMathEntity? entity = null)
    {
        entity = entity ?? context?.FirstMathEntity(expression[i..]);
        if (entity is MathVariable<double> mathVariable)
        {
            i += entity.Key.Length;
            var result = EvaluateConverter(expression, context, provider, ref i, separator, closingSymbol, mathVariable.Value);
            value = (value == 0 ? 1 : value) * result;
        }
        else if (entity is MathOperandConverter<double> mathConverter)
        {
            i += entity.Key.Length;
            var fn = mathConverter.Fn;
            var result = mathConverter.IsConvertingLeftOperand
                ? fn(value)
                : fn(Evaluate(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic));
            value = EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
        }
        else if (entity is MathVariable<decimal> or MathOperandConverter<decimal>)
        {
            var decimalValue = EvaluateVariableOrConverterDecimal(expression, context, provider, ref i, separator, closingSymbol, (decimal)value, entity);
            return (double)decimalValue;
        }
        return value;
    }

    private static double EvaluateConverter(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, double value, IMathEntity? entity = null)
    {
        entity = entity ?? context?.FirstMathEntity(expression[i..]);
        if (entity is MathOperandConverter<double> mathConverter)
        {
            i += entity.Key.Length;
            var fn = mathConverter.Fn;
            var result = mathConverter.IsConvertingLeftOperand
                ? fn(value)
                : fn(Evaluate(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic));
            value = EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
        }
        else if (entity is MathOperandConverter<decimal>)
        {
            var decimalValue = EvaluateConverterDecimal(expression, context, provider, ref i, separator, closingSymbol, (decimal)value, entity);
            return (double)decimalValue;
        }
        return value;
    }

    private static double EvaluateFnOrConstant(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, double value, IMathEntity? entity = null)
    {
        entity = entity ?? context?.FirstMathEntity(expression[i..]);
        if (entity != null && TryEvaluateContext(expression, context!, entity, provider, ref i, separator, closingSymbol, ref value))
            return value;

        var decimalValue = (decimal)value;
        if (entity != null && TryEvaluateContextDecimal(expression, context!, entity, provider, ref i, separator, closingSymbol, precedence, ref decimalValue))
            return (double)decimalValue;

        if (TryEvaluateCurrencySymbol(expression, provider, ref i))
            return value;

        var end = expression[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static bool TryEvaluateContext(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, ref double value)
    {
        switch (entity)
        {
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateConverter(expression, context, provider, ref i, separator, closingSymbol, mathVariable.Value);
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathVariableFunction<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateConverter(expression, context, provider, ref i, separator, closingSymbol, mathVariable.GetValue());
                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandConverter<double> mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    var result = mathConverter.IsConvertingLeftOperand
                        ? fn(value)
                        : fn(Evaluate(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic));
                    value = EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var rightOperand = Evaluate(expression, context, provider, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, rightOperand);
                    return true;
                }
            case BasicMathFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.ClosingSymbol.HasValue
                        ? fn(Evaluate(expression, context, provider, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown))
                        : fn(EvaluateOperand(expression, context, provider, ref i, separator, null));

                    if (mathFunction.ClosingSymbol.HasValue)
                        i++;

                    value = (value == 0 ? 1 : value) *
                        EvaluateExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    if (expression.Length <= i || expression[i] != mathFunction.OpenningSymbol)
                        return false;

                    i++; //openning
                    var args = new List<double>();
                    while (expression.Length > i)
                    {
                        var arg = Evaluate(expression, context, provider, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
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