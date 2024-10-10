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
    ///   <c>true</c> if it's the currency symbol; otherwise, <c>false</c>.
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
        var numberStr = str.GetNumberString(numberFormat, ref i);
        return double.Parse(numberStr, NumberStyles.Number | NumberStyles.AllowExponent, numberFormat);
    }

    /// <summary>Parses a number.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="numberFormat">The number format.</param>
    /// <param name="i">The current char index.</param>
    /// <returns>The value.</returns>
    internal static decimal ParseDecimalNumber(this ReadOnlySpan<char> str, NumberFormatInfo? numberFormat, ref int i)
    {
        var numberStr = str.GetNumberString(numberFormat, ref i);
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
        if (str.Length > i && str[i] == 'i')
        {
            i++;
            return new Complex(0d, number);
        }

        return new Complex(number, 0d);
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
