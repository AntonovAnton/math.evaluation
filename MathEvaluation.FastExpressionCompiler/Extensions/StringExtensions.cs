using MathEvaluation.Context;
using System;
using System.Numerics;

namespace MathEvaluation.Extensions;

/// <summary>
///     Extends the string class to evaluate or compile mathematical expressions.
/// </summary>
public static class StringExtensions
{
    extension(string mathString)
    {
        /// <inheritdoc cref="MathExpression.Compile{T, TResult}(T)" />
        public Func<T, TResult> CompileFast<T, TResult>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Compile<T, TResult>(parameters);

        /// <inheritdoc cref="MathExpression.Compile{T}(T)" />
        public Func<T, double> CompileFast<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new FastMathExpression(mathString, context, provider).Compile(parameters);

        /// <inheritdoc cref="MathExpression.CompileDecimal{T}(T)" />
        public Func<T, decimal> CompileDecimalFast<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new FastMathExpression(mathString, context, provider).CompileDecimal(parameters);

        /// <inheritdoc cref="MathExpression.CompileBoolean{T}(T)" />
        public Func<T, bool> CompileBooleanFast<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new FastMathExpression(mathString, context, provider).CompileBoolean(parameters);

        /// <inheritdoc cref="MathExpression.CompileComplex{T}(T)" />
        public Func<T, Complex> CompileComplexFast<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new FastMathExpression(mathString, context, provider).CompileComplex(parameters);
    }
}