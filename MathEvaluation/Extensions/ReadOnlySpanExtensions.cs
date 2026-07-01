using System;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Extensions;

internal static class ReadOnlySpanExtensions
{
    /// <param name="mathString">The math expression string.</param>
    extension(ReadOnlySpan<char> mathString)
    {
        /// <summary>Tries to parse a currency symbol.</summary>
        /// <param name="numberFormat">The number format info that defines the currency symbol.</param>
        /// <param name="i">The current char index.</param>
        /// <returns>
        ///     <c>true</c> if it's the currency symbol; otherwise, <c>false</c>.
        /// </returns>
        internal bool TryParseCurrency(NumberFormatInfo numberFormat, ref int i)
        {
            if (!mathString[i..].StartsWith(numberFormat.CurrencySymbol))
                return false;

            i += numberFormat.CurrencySymbol.Length;
            return true;
        }

        /// <summary>Parses a number.</summary>
        /// <param name="numberFormat">The number format.</param>
        /// <param name="i">The current char index.</param>
        /// <returns>The value.</returns>
        internal TResult ParseNumber<TResult>(NumberFormatInfo numberFormat, ref int i)
            where TResult : struct, INumberBase<TResult>
        {
            if (typeof(TResult) == typeof(Complex))
            {
                var value = mathString.ParseComplexNumber(numberFormat, ref i);
                return TResult.CreateChecked(value);
            }

            var numberStr = mathString.GetNumberString(numberFormat, ref i, out var isBinary, out var isOctal, out var isHex);
            if (isBinary)
            {
                var intValue = Convert.ToUInt64(numberStr[2..].ToString(), 2);
                return TResult.CreateChecked(intValue);
            }

            if (isOctal)
            {
                var intValue = Convert.ToUInt64(numberStr[2..].ToString(), 8);
                return TResult.CreateChecked(intValue);
            }

            // ReSharper disable once InvertIf
            if (isHex)
            {
                var intValue = Convert.ToUInt64(numberStr[2..].ToString(), 16);
                return TResult.CreateChecked(intValue);
            }

            return TResult.Parse(numberStr, NumberStyles.Number | NumberStyles.AllowExponent, numberFormat);
        }

        /// <summary>
        ///     Determines whether the string is a complex number part. Complex numbers are written in the form a ± bi, where `a` is
        ///     the real part and `bi` is the imaginary part.
        /// </summary>
        /// <param name="numberFormat">The number format.</param>
        /// <param name="isImaginaryPart">if set to <c>true</c> is the imaginary part of a complex number.</param>
        /// <returns>
        ///     <c>true</c> if it's a part of a complex number; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsComplexNumberPart(NumberFormatInfo numberFormat, out bool isImaginaryPart)
        {
            isImaginaryPart = false;

            if (mathString.IsEmpty)
                return false;

            var i = 0;
            mathString.SkipWhiteSpace(ref i);

            var numberStr = mathString.GetNumberString(numberFormat, ref i, out _, out _, out _);

            // GetNumberString sets the 'i' to the next index after the number part. 
            if (mathString.Length != i + 1 || mathString[i] != 'i')
                return !numberStr.IsEmpty && mathString.Length == i;

            isImaginaryPart = true;
            return true;
        }

        /// <summary>Skips white space (whitespace, tab, LF, and CR).</summary>
        /// <param name="i">The current char index.</param>
        internal void SkipWhiteSpace(ref int i)
        {
            while (mathString.Length > i && char.IsWhiteSpace(mathString[i]))
                i++;
        }

        /// <summary>
        ///     Determines whether the part of the math expression string is white space (has only whitespace, tab, LF, or CR).
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="end">The ending position.</param>
        /// <returns>
        ///     <c>true</c> if the specified string is white space; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsWhiteSpace(int start, int end)
            => mathString[start..end].IsWhiteSpace();

        /// <summary>Parses a complex number, format: a + bi.</summary>
        /// <param name="numberFormat">The number format.</param>
        /// <param name="i">The current char index.</param>
        /// <returns>The value.</returns>
        private Complex ParseComplexNumber(NumberFormatInfo numberFormat, ref int i)
        {
            if (mathString[i] == 'i')
            {
                i++;
                return Complex.ImaginaryOne;
            }

            var number = mathString.ParseNumber<double>(numberFormat, ref i);
            if (mathString.Length <= i || mathString[i] != 'i')
                return new Complex(number, 0d);

            i++;
            return new Complex(0d, number);
        }

        /// <summary>Gets the number string.</summary>
        /// <param name="numberFormat">The number format.</param>
        /// <param name="i">The current char index.</param>
        /// <param name="isBinary"></param>
        /// <param name="isOctal"></param>
        /// <param name="isHex"></param>
        /// <returns>The number string.</returns>
        private ReadOnlySpan<char> GetNumberString(NumberFormatInfo numberFormat, ref int i,
            out bool isBinary, out bool isOctal, out bool isHex)
        {
            var start = i;

            isBinary = false;
            isOctal = false;
            isHex = false;

            // Check for binary, octal, or hexadecimal prefixes
            if (mathString[start] is '0' && mathString.Length > start + 2)
            {
                if (mathString[start + 1] is 'b' or 'B' && mathString[start + 2] is '0' or '1')
                {
                    isBinary = true;
                    i += 3;
                    while (mathString.Length > i && mathString[i] is '0' or '1')
                        i++;

                    return mathString[start..i];
                }

                if (mathString[start + 1] is 'o' or 'O' && mathString[start + 2] is >= '0' and <= '7')
                {
                    isOctal = true;
                    i += 3;
                    while (mathString.Length > i && mathString[i] is >= '0' and <= '7')
                        i++;

                    return mathString[start..i];
                }

                if (mathString[start + 1] is 'x' or 'X' && mathString[start + 2] is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F')
                {
                    isHex = true;
                    i += 3;
                    while (mathString.Length > i && (mathString[i] is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F'))
                        i++;

                    return mathString[start..i];
                }
            }

            while (mathString.Length > i)
            {
                if (char.IsAsciiDigit(mathString[i]))
                {
                    i++;
                    continue;
                }

                if (tryParseNumberFormatSeparator(mathString, numberFormat.NumberDecimalSeparator, ref i) ||
                    tryParseNumberFormatSeparator(mathString, numberFormat.NumberGroupSeparator, ref i))
                {
                    continue;
                }

                //an exponential notation number
                if (mathString[i] is 'e' or 'E' && mathString.Length > i + 1 &&
                    (char.IsAsciiDigit(mathString[i + 1]) || mathString[i + 1] is '-' or '+'))
                {
                    i += 2;
                    continue;
                }

                break;
            }

            return mathString[start..i];

            static bool tryParseNumberFormatSeparator(ReadOnlySpan<char> str, string numberFormatSeparator, ref int i)
            {
                if (!str[i..].StartsWith(numberFormatSeparator) ||
                    (str.Length > i + numberFormatSeparator.Length &&
                     !char.IsAsciiDigit(str[i + numberFormatSeparator.Length])))
                {
                    return false;
                }

                i += numberFormatSeparator.Length;
                return true;
            }
        }
    }
}