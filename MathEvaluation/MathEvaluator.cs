using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;
using MathEvaluation.Entities;

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
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));

        if (expression.IsWhiteSpace())
            throw new ArgumentException("Expression is empty or white space.", nameof(expression));

        var i = 0;
        try
        {
            var numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;
            return Evaluate(expression, context, numberFormat, ref i, null, null, (int)EvalPrecedence.Unknown);
        }
        catch (Exception ex)
        {
            ex.Data[nameof(expression)] = expression.ToString();
            ex.Data[nameof(context)] = context;
            ex.Data[nameof(provider)] = provider;
            ex.Data["position"] = i;
            throw;
        }
    }

    private static double Evaluate(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand = false, double value = default)
    {
        var decimalSeparator = numberFormat?.NumberDecimalSeparator?.Length > 0 ? numberFormat.NumberDecimalSeparator[0] : '.';

        var start = i;
        while (expression.Length > i)
        {
            if (separator.HasValue && expression[i] == separator.Value &&
                (numberFormat == null || decimalSeparator != separator.Value || IsNotMeaningless(expression[start..i])))
                return value;

            if (closingSymbol.HasValue && expression[i] == closingSymbol.Value)
                return value;

            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '٫' || expression[i] == decimalSeparator)
            {
                if (isOperand)
                    return Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = GetNumber(expression, numberFormat, ref i, separator, closingSymbol);
                continue;
            }

            switch (expression[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    i++;
                    var result = Evaluate(expression, context, numberFormat, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    i++;
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    break;
                case '+' when expression.Length == i + 1 || expression[i + 1] != '+':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && IsNotMeaningless(expression[start..i]))
                        return value;

                    i++;
                    value += Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '-' when expression.Length == i + 1 || expression[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && IsNotMeaningless(expression[start..i]))
                        return value;

                    i++;
                    value -= Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol,
                        precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, isOperand);

                    if (isOperand)
                        return value;
                    break;
                case '*' when expression.Length == i + 1 || expression[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when expression.Length == i + 1 || expression[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = context?.FirstMathEntity(expression[i..]);
                    if (entity == null && numberFormat != null && TryParseCurrencySymbol(expression, numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateMathEntity(expression, context, numberFormat, ref i, separator, closingSymbol, precedence, value, entity);
                    if (isOperand)
                        return value;
                    break;
            }
        }

        if (value == default && !IsNotMeaningless(expression[start..i]))
            if (isOperand)
                throw new ArgumentException("Expression cannot be evaluated. The operand is not recognizable.", nameof(expression));
            else
                throw new ArgumentException("Expression cannot be evaluated. It is not recognizable.", nameof(expression));

        return value;
    }

    private static double EvaluateOperand(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol)
        => Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);

    private static double EvaluateMathEntity(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, int precedence, double value, IMathEntity? entity, bool throwOnUnsupported = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryEvaluateContext(expression, context!, entity, numberFormat, ref i, separator, closingSymbol, ref value))
                return value;

            var decimalValue = (decimal)value;
            if (TryEvaluateContextDecimal(expression, context!, entity, numberFormat, ref i, separator, closingSymbol, ref decimalValue))
                return (double)decimalValue;
        }

        if (!throwOnUnsupported)
            return value;

        var end = expression[i..].IndexOfAny("(0123456789.,٫+-*/ \t\n\r") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static double EvaluateExponentiation(ReadOnlySpan<char> expression, IMathContext? context, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, double value)
    {
        SkipMeaninglessChars(expression, ref i);
        if (expression.Length <= i)
            return value;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = context?.FirstMathEntity(expression[i..]);
        return EvaluateMathEntity(expression, context, numberFormat, ref i, separator, closingSymbol, precedence, value, entity, false);
    }

    private static bool TryEvaluateContext(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol, ref double value)
    {
        switch (entity)
        {
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, mathVariable.Value);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var result = mathOperator.IsProcessingLeft
                        ? fn(value)
                        : fn(EvaluateOperand(expression, context, numberFormat, ref i, separator, closingSymbol));
                    value = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateOperand(expression, context, numberFormat, ref i, separator, closingSymbol);
                    right = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, right);
                    value = fn(value, right);
                    return true;
                }
            case MathOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = Evaluate(expression, context, numberFormat, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, right);
                    return true;
                }
            case MathGetValueFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    SkipParenthesisChars(expression, ref i);
                    var result = mathFunction.Fn();
                    result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathUnaryFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    var fn = mathFunction.Fn;
                    var result = mathFunction.ClosingSymbol.HasValue
                        ? fn(Evaluate(expression, context, numberFormat, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown))
                        : fn(EvaluateOperand(expression, context, numberFormat, ref i, separator, closingSymbol));

                    if (mathFunction.ClosingSymbol.HasValue)
                        i++;

                    result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
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
                        var arg = Evaluate(expression, context, numberFormat, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
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
                    result = EvaluateExponentiation(expression, context, numberFormat, ref i, separator, closingSymbol, result);
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            default:
                return false;
        }
    }

    private static bool TryParseCurrencySymbol(ReadOnlySpan<char> expression, NumberFormatInfo numberFormat, ref int i)
    {
        if (!expression[i..].StartsWith(numberFormat.CurrencySymbol))
            return false;

        i += numberFormat.CurrencySymbol.Length;
        return true;
    }

    private static double GetNumber(ReadOnlySpan<char> expression, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol)
    {
        var str = GetNumberString(expression, numberFormat, ref i, separator, closingSymbol);
        return double.Parse(str, NumberStyles.Number | NumberStyles.AllowExponent, numberFormat);
    }

    private static ReadOnlySpan<char> GetNumberString(ReadOnlySpan<char> expression, NumberFormatInfo? numberFormat,
        ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        while (expression.Length > i)
        {
            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '\u202f' or '\u00a0' or '٫' or '’' or '٬' or '⹁' &&
                (!separator.HasValue || expression[i] != separator.Value) &&
                (!closingSymbol.HasValue || expression[i] != closingSymbol.Value))
            {
                i++;
            }
            else if (expression[i] is 'e' or 'E') //an exponential notation number
            {
                i++;
                if (expression.Length > i && expression[i] is '-' or '+')
                    i++;
            }
            else if (IsNumberFormatSeparator(expression, numberFormat?.NumberGroupSeparator, ref i) ||
                IsNumberFormatSeparator(expression, numberFormat?.NumberDecimalSeparator, ref i))
                continue;
            else
                break;
        }

        //if the last symbol is 'e' it's the natural logarithmic base constant
        if (expression[i - 1] is 'e')
            i--;

        return expression[start..i];

        static bool IsNumberFormatSeparator(ReadOnlySpan<char> expression, string? numberFormatSeparator, ref int i)
        {
            if (string.IsNullOrEmpty(numberFormatSeparator) ||
                !expression[i..].StartsWith(numberFormatSeparator) ||
                (expression.Length > i + numberFormatSeparator.Length &&
                expression[i + numberFormatSeparator.Length] is not >= '0' and <= '9'))
                return false;

            i += numberFormatSeparator.Length;
            return true;
        }
    }

    /// <summary>Skips whitespace, tab, LF, and CR.</summary>
    /// <param name="expression">The string math expression.</param>
    /// <param name="i">The current char index.</param>
    private static void SkipMeaninglessChars(ReadOnlySpan<char> expression, ref int i)
    {
        while (expression.Length > i && IsMeaningless(expression[i]))
            i++;
    }

    /// <summary>Skips parenthesis ().</summary>
    /// <param name="expression">The string math expression.</param>
    /// <param name="i">The current char index.</param>
    private static void SkipParenthesisChars(ReadOnlySpan<char> expression, ref int i)
    {
        if (expression.Length > i && expression[i] == '(')
        {
            i++;
            SkipMeaninglessChars(expression, ref i);
            if (expression.Length > i && expression[i] == ')')
                i++;
        }
    }

    /// <summary>
    /// Determines whether the specified string is meaningless (has only whitespace, tab, LF, or CR).
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns>
    ///   <c>true</c> if the specified string is meaningless; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsNotMeaningless(ReadOnlySpan<char> str)
    {
        foreach (char c in str)
            if (!IsMeaningless(c))
                return true;

        return false;
    }

    /// <summary>
    /// Determines whether the specified char is meaningless (is whitespace, tab, LF, or CR).
    /// </summary>
    /// <param name="c">The char.</param>
    /// <returns>
    ///   <c>true</c> if the specified char is meaningless; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsMeaningless(char c)
        => c is ' ' or '\t' or '\n' or '\r';
}