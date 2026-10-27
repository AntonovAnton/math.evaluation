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
        /// <inheritdoc cref="extension(string).CompileFast{T}(T, MathContext?, IFormatProvider?)" />
        /// <typeparam name="T">The type of the parameters.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
        public Func<T, TResult> CompileFast<T, TResult>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new FastMathExpression(mathString, context, provider).Compile<T, TResult>(parameters);

        /// <summary>Compiles the math expression string.</summary>
        /// <param name="parameters">The parameters of the math expression string.</param>
        /// <param name="context">The math context.</param>
        /// <param name="provider">The specified format provider.</param>
        /// <typeparam name="T">The type of the parameters.</typeparam>
        /// <returns>A delegate that represents the compiled expression.</returns>
        /// <exception cref="MathExpressionException" />
        /// <exception cref="ArgumentNullException">parameters</exception>
        /// <exception cref="NotSupportedException">parameters</exception>
        public Func<T, double> CompileFast<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new FastMathExpression(mathString, context, provider).Compile(parameters);

        /// <inheritdoc cref="extension(string).CompileFast{T}(T, MathContext?, IFormatProvider?)" />
        public Func<T, decimal> CompileDecimalFast<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new FastMathExpression(mathString, context, provider).CompileDecimal(parameters);

        /// <inheritdoc cref="extension(string).CompileFast{T}(T, MathContext?, IFormatProvider?)" />
        public Func<T, bool> CompileBooleanFast<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new FastMathExpression(mathString, context, provider).CompileBoolean(parameters);

        /// <inheritdoc cref="extension(string).CompileFast{T}(T, MathContext?, IFormatProvider?)" />
        public Func<T, Complex> CompileComplexFast<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new FastMathExpression(mathString, context, provider).CompileComplex(parameters);
    }
}