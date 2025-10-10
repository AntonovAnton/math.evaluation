using System;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Extensions;

internal static class ReadOnlySpanExtensions
{
    /// <summary>Tries to parse a currency symbol.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format info that defines the currency symbol.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>
    ///     <c>true</c> if it's the currency symbol; otherwise, <c>false</c>.
    /// </returns>
    internal static bool TryParseCurrency(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
    {
        if (numberFormat == null || !str[i..].StartsWith(numberFormat.CurrencySymbol))
            return false;

        i += numberFormat.CurrencySymbol.Length;
        return true;
    }

    /// <summary>Parses a number.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>The value.</returns>
    internal static double ParseNumber(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
    {
        var numberStr = str.GetNumberString(numberFormat, ref i, out var isBinary, out var isOctal, out var isHex);
        if (isBinary)
            return Convert.ToInt64(numberStr[2..].ToString(), 2);
        if (isOctal)
            return Convert.ToInt64(numberStr[2..].ToString(), 8);
        if (isHex)
            return Convert.ToInt64(numberStr[2..].ToString(), 16);

        return double.Parse(numberStr, NumberStyles.Number | NumberStyles.AllowExponent, numberFormat);
    }

    /// <summary>Parses a number.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>The value.</returns>
    internal static decimal ParseDecimalNumber(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
    {
        var numberStr = str.GetNumberString(numberFormat, ref i, out var isBinary, out var isOctal, out var isHex);
        if (isBinary)
            return Convert.ToInt64(numberStr[2..].ToString(), 2);
        if (isOctal)
            return Convert.ToInt64(numberStr[2..].ToString(), 8);
        if (isHex)
            return Convert.ToInt64(numberStr[2..].ToString(), 16);

        return decimal.Parse(numberStr, NumberStyles.Number | NumberStyles.AllowExponent, numberFormat);
    }

    /// <summary>Parses a complex number, format: a + bi.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>The value.</returns>
    internal static Complex ParseComplexNumber(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
    {
        str.SkipMeaningless(ref i);
        if (str[i] == 'i')
        {
            i++;
            return Complex.ImaginaryOne;
        }

        var number = str.ParseNumber(numberFormat, ref i);
        if (str.Length <= i || str[i] != 'i')
            return new Complex(number, 0d);

        i++;
        return new Complex(0d, number);
    }

    /// <summary>Parses a number.</summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>The value.</returns>
    internal static TResult ParseNumber<TResult>(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
        where TResult : struct
    {
        if (typeof(TResult) == typeof(Complex))
        {
            var value = ParseComplexNumber(str, numberFormat, ref i);
            return value is TResult result ? result : (TResult)Convert.ChangeType(value, typeof(TResult));
        }

        if (typeof(TResult) == typeof(decimal))
        {
            var value = ParseDecimalNumber(str, numberFormat, ref i);
            return value is TResult result ? result : (TResult)Convert.ChangeType(value, typeof(TResult));
        }
        else
        {
            var value = ParseNumber(str, numberFormat, ref i);
            return value is TResult result ? result : (TResult)Convert.ChangeType(value, typeof(TResult));
        }
    }

    /// <summary>
    ///     Determines whether the string is a complex number part. Complex numbers are written in the form a ± bi, where a is
    ///     the real part and bi is the imaginary part.
    /// </summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="isImaginaryPart">if set to <c>true</c> is the imaginary part of a complex number.</param>
    /// <returns>
    ///     <c>true</c> if it's a part of a complex number; otherwise, <c>false</c>.
    /// </returns>
    internal static bool IsComplexNumberPart(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, out bool isImaginaryPart)
    {
        isImaginaryPart = false;

        if (str.IsEmpty)
            return false;

        var i = 0;
        str.SkipMeaningless(ref i);

        var numberStr = str.GetNumberString(numberFormat, ref i, out var _, out var _, out var _);

        // GetNumberString sets the 'i' to the next index after the number part. 
        if (str.Length != i + 1 || str[i] != 'i')
            return !numberStr.IsEmpty && str.Length == i;

        isImaginaryPart = true;
        return true;
    }

    /// <summary>Gets the number string.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <param name="isBinary"></param>
    /// <param name="isOctal"></param>
    /// <param name="isHex"></param>
    /// <returns>The number string.</returns>
    private static ReadOnlySpan<char> GetNumberString(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i,
        out bool isBinary, out bool isOctal, out bool isHex)
    {
        var start = i;

        isBinary = false;
        isOctal = false;
        isHex = false;

        if (str[start] is '0' && str.Length > start + 2)
        {
            if (str[start + 1] is 'b' or 'B' && str[start + 2] is '0' or '1')
            {
                isBinary = true;
                i += 3;
                while (str.Length > i && str[i] is '0' or '1')
                    i++;

                return str[start..i];
            }
            else if (str[start + 1] is 'o' or 'O' && str[start + 2] is >= '0' and <= '7')
            {
                isOctal = true;
                i += 3;
                while (str.Length > i && str[i] is >= '0' and <= '7')
                    i++;

                return str[start..i];
            }
            else if (str[start + 1] is 'x' or 'X' && str[start + 2] is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F')
            {
                isHex = true;
                i += 3;
                while (str.Length > i && (str[i] is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F'))
                    i++;

                return str[start..i];
            }
        }

        while (str.Length > i)
        {
            if (str[i] is >= '0' and <= '9' ||
                (numberFormat?.NumberDecimalSeparator == null && str[i] is '.'))
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

            if (tryParseNumberFormatSeparator(str, numberFormat?.NumberDecimalSeparator, ref i) ||
                tryParseNumberFormatSeparator(str, numberFormat?.NumberGroupSeparator, ref i))
                continue;

            break;
        }

        return str[start..i];

        static bool tryParseNumberFormatSeparator(ReadOnlySpan<char> str, string? numberFormatSeparator, ref int i)
        {
            if (string.IsNullOrEmpty(numberFormatSeparator) ||
                !str[i..].StartsWith(numberFormatSeparator) ||
                (str.Length > i + numberFormatSeparator.Length &&
                 str[i + numberFormatSeparator.Length] is < '0' or > '9'))
                return false;

            i += numberFormatSeparator.Length;
            return true;
        }
    }
}