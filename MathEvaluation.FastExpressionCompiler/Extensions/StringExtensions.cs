using MathEvaluation.Context;
using System;
using System.Numerics;

namespace MathEvaluation.Extensions;

/// <summary>
///     Extends the string class to evaluate or compile mathematical expressions.
/// </summary>
public static class StringExtensions
{
    /// <inheritdoc cref="MathExpression.Compile{T}(T)" />
    public static Func<T, double> CompileFast<T>(this string mathString, T parameters, MathContext? context = null, IFormatProvider? provider = null)
        => new FastMathExpression(mathString, context, provider).Compile(parameters);

    /// <inheritdoc cref="MathExpression.CompileDecimal{T}(T)" />
    public static Func<T, decimal> CompileDecimalFast<T>(this string mathString, T parameters, MathContext? context = null, IFormatProvider? provider = null)
        => new FastMathExpression(mathString, context, provider).CompileDecimal(parameters);

    /// <inheritdoc cref="MathExpression.CompileBoolean{T}(T)" />
    public static Func<T, bool> CompileBooleanFast<T>(this string mathString, T parameters, MathContext? context = null, IFormatProvider? provider = null)
        => new FastMathExpression(mathString, context, provider).CompileBoolean(parameters);

    /// <inheritdoc cref="MathExpression.CompileComplex{T}(T)" />
    public static Func<T, Complex> CompileComplexFast<T>(this string mathString, T parameters, MathContext? context = null, IFormatProvider? provider = null)
        => new FastMathExpression(mathString, context, provider).CompileComplex(parameters);
}