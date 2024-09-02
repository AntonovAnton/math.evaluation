using System;
using System.Globalization;

namespace MathEvaluation.Extensions;

internal static class ReadOnlySpanExtensions
{
    /// <summary>Tries to parse the currency symbol.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format info that defines the currency symbol.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>
    ///   <c>true</c> if it's the currency symbol; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryParseCurrencySymbol(this ReadOnlySpan<char> str, NumberFormatInfo numberFormat, ref int i)
    {
        if (!str[i..].StartsWith(numberFormat.CurrencySymbol))
            return false;

        i += numberFormat.CurrencySymbol.Length;
        return true;
    }

    /// <summary>Parses the number.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>The value.</returns>
    public static double ParseNumber(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
    {
        var numberStr = str.GetNumberString(numberFormat, ref i);
        return double.Parse(numberStr, NumberStyles.Number | NumberStyles.AllowExponent, numberFormat);
    }

    /// <summary>Parses the number.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>The value.</returns>
    public static decimal ParseDecimalNumber(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
    {
        var numberStr = str.GetNumberString(numberFormat, ref i);
        return decimal.Parse(numberStr, NumberStyles.Number | NumberStyles.AllowExponent, numberFormat);
    }

    /// <summary>Skips meaningless chars (whitespace, tab, LF, and CR).</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="i">The current char index.</param>
    public static void SkipMeaninglessChars(this ReadOnlySpan<char> str, ref int i)
    {
        while (str.Length > i && IsMeaningless(str[i]))
            i++;
    }

    /// <summary>Skips parenthesis ().</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="i">The current char index.</param>
    public static void SkipParenthesisChars(this ReadOnlySpan<char> str, ref int i)
    {
        if (str.Length > i && str[i] == '(')
        {
            i++;
            str.SkipMeaninglessChars(ref i);
            str.ThrowExceptionIfNotClosed(')', i, ref i);
        }
    }

    /// <summary>
    /// Determines whether the specified string is meaningless (has only whitespace, tab, LF, or CR).
    /// </summary>
    /// <param name="str">The math expression string.</param>
    /// <returns>
    ///   <c>true</c> if the specified string is meaningless; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNotMeaningless(this ReadOnlySpan<char> str)
    {
        foreach (char c in str)
            if (!IsMeaningless(c))
                return true;

        return false;
    }

    /// <summary>Throws the exception if not evaluated.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="value">The value.</param>
    /// <param name="isOperand">if set to <c>true</c> [is operand].</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    /// <param name="i">The current char index.</param>
    /// <exception cref="MathEvaluationException"></exception>
    public static void ThrowExceptionIfNotEvaluated(this ReadOnlySpan<char> str,
        double value, bool isOperand, int invalidTokenPosition, int i)
    {
        if (value == default && !str[invalidTokenPosition..i].IsNotMeaningless())
            throw new MathEvaluationException($"{(isOperand ? "The operand" : "It")} is not recognizable.", invalidTokenPosition);
    }

    /// <summary>Throws the exception if not evaluated.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="value">The value.</param>
    /// <param name="isOperand">if set to <c>true</c> [is operand].</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    /// <param name="i">The current char index.</param>
    /// <exception cref="MathEvaluationException"></exception>
    public static void ThrowExceptionIfNotEvaluated(this ReadOnlySpan<char> str,
        decimal value, bool isOperand, int invalidTokenPosition, int i)
    {
        if (value == default && !str[invalidTokenPosition..i].IsNotMeaningless())
            throw new MathEvaluationException($"{(isOperand ? "The operand" : "It")} is not recognizable.", invalidTokenPosition);
    }

    /// <summary>Throws the exception if missing opening symbol.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    /// <param name="i">The current char index.</param>
    /// <exception cref="MathEvaluationException">It doesn't have the '{openingSymbol}' opening symbol.</exception>
    public static void ThrowExceptionIfNotOpened(this ReadOnlySpan<char> str,
        char openingSymbol, int invalidTokenPosition, ref int i)
    {
        if (str.Length <= i || str[i] != openingSymbol)
            throw new MathEvaluationException($"It doesn't have the '{openingSymbol}' opening symbol.", invalidTokenPosition);

        i++;
    }

    /// <summary>Throws the exception if missing closing symbol.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    /// <param name="i">The current char index.</param>
    /// <exception cref="MathEvaluationException">It doesn't have the '{closingSymbol}' closing symbol.</exception>
    public static void ThrowExceptionIfNotClosed(this ReadOnlySpan<char> str,
        char closingSymbol, int invalidTokenPosition, ref int i)
    {
        if (str.Length <= i || str[i] != closingSymbol)
            throw new MathEvaluationException($"It doesn't have the '{closingSymbol}' closing symbol.", invalidTokenPosition);

        i++;
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

    /// <summary>Gets the number string.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>The number string.</returns>
    private static ReadOnlySpan<char> GetNumberString(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
    {
        var start = i;
        while (str.Length > i)
        {
            if (str[i] is >= '0' and <= '9' ||
                numberFormat?.NumberDecimalSeparator == null && str[i] is '.')
            {
                i++;
                continue;
            }

            //an exponential notation number
            if (str[i] is 'e' or 'E' && str.Length > i + 1 &&
                str[i + 1] is >= '0' and <= '9' or '-' or '+')
            {
                i += 2;
                continue;
            }

            if (TryParseNumberFormatSeparator(str, numberFormat?.NumberDecimalSeparator, ref i) ||
                TryParseNumberFormatSeparator(str, numberFormat?.NumberGroupSeparator, ref i))
                continue;
            break;
        }

        return str[start..i];

        static bool TryParseNumberFormatSeparator(ReadOnlySpan<char> str, string? numberFormatSeparator, ref int i)
        {
            if (string.IsNullOrEmpty(numberFormatSeparator) ||
                !str[i..].StartsWith(numberFormatSeparator) ||
                str.Length > i + numberFormatSeparator.Length &&
                str[i + numberFormatSeparator.Length] is not >= '0' and <= '9')
                return false;

            i += numberFormatSeparator.Length;
            return true;
        }
    }
}
