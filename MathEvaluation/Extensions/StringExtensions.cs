using MathEvaluation.Context;
using MathEvaluation.Parameters;
using System;

namespace MathEvaluation.Extensions;

/// <summary>
/// Extends the string class to evaluate or compile mathematical expressions.
/// </summary>
public static class StringExtensions
{
    /// <inheritdoc cref="MathExpression.Evaluate(IMathParameters?)"/>
    public static double Evaluate(this string mathString, IMathContext? context, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).Evaluate();

    /// <inheritdoc cref="MathExpression.EvaluateDecimal(IMathParameters?)"/>
    public static decimal EvaluateDecimal(this string mathString, IMathContext? context, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).EvaluateDecimal();

    /// <inheritdoc cref="MathExpression.EvaluateBoolean(IMathParameters?)"/>
    public static bool EvaluateBoolean(this string mathString, IMathContext? context, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).EvaluateBoolean();

    /// <inheritdoc cref="MathExpression.Evaluate(object?)"/>
    public static double Evaluate(this string mathString,
        object? parameters = null, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).Evaluate(parameters);

    /// <inheritdoc cref="MathExpression.Evaluate(IMathParameters?)"/>
    public static double Evaluate(this string mathString,
        IMathParameters? parameters, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).Evaluate(parameters);

    /// <inheritdoc cref="MathExpression.EvaluateDecimal(object?)"/>
    public static decimal EvaluateDecimal(this string mathString,
        object? parameters = null, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).EvaluateDecimal(parameters);

    /// <inheritdoc cref="MathExpression.EvaluateDecimal(IMathParameters?)"/>
    public static decimal EvaluateDecimal(this string mathString,
        IMathParameters? parameters, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).EvaluateDecimal(parameters);

    /// <inheritdoc cref="MathExpression.EvaluateBoolean(object?)"/>
    public static bool EvaluateBoolean(this string mathString,
        object? parameters = null, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).EvaluateBoolean(parameters);

    /// <inheritdoc cref="MathExpression.EvaluateBoolean(IMathParameters?)"/>
    public static bool EvaluateBoolean(this string mathString,
        IMathParameters? parameters, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).EvaluateBoolean(parameters);

    /// <inheritdoc cref="MathExpression.Compile()"/>
    public static Func<double> Compile(this string mathString, IMathContext context)
        => new MathExpression(mathString, context).Compile();

    /// <inheritdoc cref="MathExpression.Compile()"/>
    public static Func<double> Compile(this string mathString, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).Compile();

    /// <inheritdoc cref="MathExpression.Compile{T}(T)"/>
    public static Func<T, double> Compile<T>(this string mathString, T parameters, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).Compile(parameters);

    /// <inheritdoc cref="MathExpression.CompileDecimal()"/>
    public static Func<decimal> CompileDecimal(this string mathString, IMathContext context)
        => new MathExpression(mathString, context).CompileDecimal();

    /// <inheritdoc cref="MathExpression.CompileDecimal()"/>
    public static Func<decimal> CompileDecimal(this string mathString, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).CompileDecimal();

    /// <inheritdoc cref="MathExpression.CompileDecimal{T}(T)"/>
    public static Func<T, decimal> CompileDecimal<T>(this string mathString, T parameters, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).CompileDecimal(parameters);

    /// <inheritdoc cref="MathExpression.CompileBoolean()"/>
    public static Func<bool> CompileBoolean(this string mathString, IMathContext context)
        => new MathExpression(mathString, context).CompileBoolean();

    /// <inheritdoc cref="MathExpression.CompileBoolean()"/>
    public static Func<bool> CompileBoolean(this string mathString, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).CompileBoolean();

    /// <inheritdoc cref="MathExpression.CompileBoolean{T}(T)"/>
    public static Func<T, bool> CompileBoolean<T>(this string mathString, T parameters, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).CompileBoolean(parameters);

    #region internal static Methods

    /// <summary>Throws the exception if missing opening symbol.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    /// <param name="i">The current char index.</param>
    /// <exception cref="MathExpressionException">It doesn't have the '{openingSymbol}' opening symbol.</exception>
    internal static void ThrowExceptionIfNotOpened(this string str, char openingSymbol, int invalidTokenPosition, ref int i)
    {
        if (str.Length <= i || str[i] != openingSymbol)
            throw new MathExpressionException($"It doesn't have the '{openingSymbol}' opening symbol.", invalidTokenPosition);

        i++;
    }

    /// <summary>Throws the exception if missing closing symbol.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    /// <param name="i">The current char index.</param>
    /// <exception cref="MathExpressionException">It doesn't have the '{closingSymbol}' closing symbol.</exception>
    internal static void ThrowExceptionIfNotClosed(this string str, char closingSymbol, int invalidTokenPosition, ref int i)
    {
        if (str.Length <= i || str[i] != closingSymbol)
            throw new MathExpressionException($"It doesn't have the '{closingSymbol}' closing symbol.", invalidTokenPosition);

        i++;
    }

    /// <summary>Throws the exception if not evaluated.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="isOperand">if set to <c>true</c> [is operand].</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    /// <param name="i">The current char index.</param>
    /// <exception cref="MathExpressionException"></exception>
    internal static void ThrowExceptionIfNotEvaluated(this string str, bool isOperand, int invalidTokenPosition, int i)
    {
        if (str.IsMeaningless(invalidTokenPosition, i))
            throw new MathExpressionException($"{(isOperand ? "The operand" : "It")} is not recognizable.", invalidTokenPosition);
    }

    /// <summary>Throws the exception about invalid token.</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    /// <exception cref="MathEvaluation.MathExpressionException">'{unknownSubstring.ToString()}' is not recognizable.</exception>
    internal static void ThrowExceptionInvalidToken(this string str, int invalidTokenPosition)
    {
        var i = invalidTokenPosition;
        var end = str.AsSpan(i).IndexOfAny("(0123456789.,٫+-*/ \t\n\r") + i;
        var unknownSubstring = end > i ? str[i..end] : str[i..];

        throw new MathExpressionException($"'{unknownSubstring}' is not recognizable.", i);
    }

    /// <summary>Skips meaningless chars (whitespace, tab, LF, and CR).</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="i">The current char index.</param>
    internal static void SkipMeaningless(this string str, ref int i)
    {
        while (str.Length > i && IsMeaningless(str[i]))
            i++;
    }

    /// <summary>Skips parenthesis ().</summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="i">The current char index.</param>
    internal static void SkipParenthesis(this string str, ref int i)
    {
        if (str.Length > i && str[i] == '(')
        {
            var tokenPosition = i;
            i++;
            str.SkipMeaningless(ref i);
            str.ThrowExceptionIfNotClosed(')', tokenPosition, ref i);
        }
    }

    /// <summary>
    /// Determines whether the part of the math expression string is meaningless (has only whitespace, tab, LF, or CR).
    /// </summary>
    /// <param name="str">The math expression string.</param>
    /// <param name="start">The starting position.</param>
    /// <param name="end">The ending position.</param>
    /// <returns>
    ///   <c>true</c> if the specified string is meaningless; otherwise, <c>false</c>.
    /// </returns>
    internal static bool IsMeaningless(this string str, int start, int end)
    {
        while (start < end && IsMeaningless(str[start]))
            start++;

        return start == end;
    }

    #endregion

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