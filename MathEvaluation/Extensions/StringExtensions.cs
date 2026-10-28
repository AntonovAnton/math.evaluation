using MathEvaluation.Context;
using MathEvaluation.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

// ReSharper disable UnusedMember.Global

namespace MathEvaluation.Extensions;

/// <summary>
///     Extends the string class to evaluate or compile mathematical expressions.
/// </summary>
public static class StringExtensions
{
    /// <param name="mathString">The math expression string.</param>
    extension(string mathString)
    {
        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public double Evaluate() => new MathExpression(mathString).Evaluate();

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public double Evaluate(MathContext? context, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate();

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        /// <exception cref="NotSupportedException">parameters</exception>
        [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
        public double Evaluate(object parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public double Evaluate(IDictionary<string, object> parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public double Evaluate(IDictionary<string, double> parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <summary>Evaluates the math expression string.</summary>
        /// <param name="parameters">The parameters of the math expression string.</param>
        /// <param name="context">The math context.</param>
        /// <param name="provider">The specified format provider.</param>
        /// <returns>Value of the math expression.</returns>
        /// <exception cref="MathExpressionException" />
        public double Evaluate(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <inheritdoc cref="extension(string).Evaluate{TResult}(MathParameters?, MathContext?, IFormatProvider?)" />
        public TResult Evaluate<TResult>()
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString).Evaluate<TResult>();

        /// <inheritdoc cref="extension(string).Evaluate{TResult}(MathParameters?, MathContext?, IFormatProvider?)" />
        public TResult Evaluate<TResult>(MathContext? context, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Evaluate<TResult>();

        /// <inheritdoc cref="extension(string).Evaluate{TResult}(MathParameters?, MathContext?, IFormatProvider?)" />
        /// <exception cref="NotSupportedException">parameters</exception>
        [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
        public TResult Evaluate<TResult>(object parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Evaluate<TResult>(parameters);

        /// <inheritdoc cref="extension(string).Evaluate{TResult}(MathParameters?, MathContext?, IFormatProvider?)" />
        public TResult Evaluate<TResult>(IDictionary<string, TResult> parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <inheritdoc cref="extension(string).Evaluate{TResult}(MathParameters?, MathContext?, IFormatProvider?)" />
        public TResult Evaluate<TResult>(IDictionary<string, object> parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Evaluate<TResult>(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        public TResult Evaluate<TResult>(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Evaluate<TResult>(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<decimal>() instead.")]
        public decimal EvaluateDecimal() => new MathExpression(mathString).Evaluate<decimal>();

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<decimal>(context, provider) instead.")]
        public decimal EvaluateDecimal(MathContext? context, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate<decimal>();

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        /// <exception cref="NotSupportedException">parameters</exception>
        [Obsolete("Use Evaluate<decimal>(parameters, context, provider) instead.")]
        [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
        public decimal EvaluateDecimal(object parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate<decimal>(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<decimal>(parameters, context, provider) instead.")]
        public decimal EvaluateDecimal(IDictionary<string, decimal> parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<decimal>(parameters, context, provider) instead.")]
        public decimal EvaluateDecimal(IDictionary<string, object> parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate<decimal>(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<decimal>(parameters, context, provider) instead.")]
        public decimal EvaluateDecimal(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate<decimal>(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public bool EvaluateBoolean() => new MathExpression(mathString).EvaluateBoolean();

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public bool EvaluateBoolean(MathContext? context, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateBoolean();

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        /// <exception cref="NotSupportedException">parameters</exception>
        [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
        public bool EvaluateBoolean(object parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateBoolean(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public bool EvaluateBoolean(IDictionary<string, bool> parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateBoolean(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public bool EvaluateBoolean(IDictionary<string, object> parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateBoolean(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        public bool EvaluateBoolean(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateBoolean(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<Complex>() instead.")]
        public Complex EvaluateComplex() => new MathExpression(mathString).Evaluate<Complex>();

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<Complex>(context, provider) instead.")]
        public Complex EvaluateComplex(MathContext? context, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate<Complex>();

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        /// <exception cref="NotSupportedException">parameters</exception>
        [Obsolete("Use Evaluate<Complex>(parameters, context, provider) instead.")]
        [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
        public Complex EvaluateComplex(object parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate<Complex>(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<Complex>(parameters, context, provider) instead.")]
        public Complex EvaluateComplex(IDictionary<string, Complex> parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<Complex>(parameters, context, provider) instead.")]
        public Complex EvaluateComplex(IDictionary<string, object> parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate<Complex>(parameters);

        /// <inheritdoc cref="extension(string).Evaluate(MathParameters?, MathContext?, IFormatProvider?)" />
        [Obsolete("Use Evaluate<Complex>(parameters, context, provider) instead.")]
        public Complex EvaluateComplex(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate<Complex>(parameters);

        /// <summary>Compiles the math expression string.</summary>
        /// <param name="parameters">The parameters of the math expression string.</param>
        /// <param name="context">The math context.</param>
        /// <param name="provider">The specified format provider.</param>
        /// <typeparam name="T">The type of the parameters.</typeparam>
        /// <returns>A delegate that represents the compiled expression.</returns>
        /// <exception cref="MathExpressionException" />
        /// <exception cref="ArgumentNullException">parameters</exception>
        /// <exception cref="NotSupportedException">parameters</exception>
        [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
        [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
        public Func<T, double> Compile<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Compile(parameters);

        /// <inheritdoc cref="extension(string).Compile{T}(T, MathContext?, IFormatProvider?)" />
        /// <typeparam name="T">The type of the parameters.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
        [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
        [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
        public Func<T, TResult> Compile<T, TResult>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Compile<T, TResult>(parameters);

        /// <inheritdoc cref="extension(string).Compile{T}(T, MathContext?, IFormatProvider?)" />
        [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
        [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
        public Func<T, decimal> CompileDecimal<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Compile<T, decimal>(parameters);

        /// <inheritdoc cref="extension(string).Compile{T}(T, MathContext?, IFormatProvider?)" />
        [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
        [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
        public Func<T, bool> CompileBoolean<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).CompileBoolean(parameters);

        /// <inheritdoc cref="extension(string).Compile{T}(T, MathContext?, IFormatProvider?)" />
        [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
        [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
        public Func<T, Complex> CompileComplex<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Compile<T, Complex>(parameters);

        #region internal static Methods

        /// <summary>Throws the exception if missing opening symbol.</summary>
        /// <param name="openingSymbol">The opening symbol.</param>
        /// <param name="invalidTokenPosition">The invalid token position.</param>
        /// <param name="i">The current char index.</param>
        /// <exception cref="MathExpressionException">It doesn't have the '{openingSymbol}' opening symbol.</exception>
        internal void ThrowExceptionIfNotOpened(char openingSymbol, int invalidTokenPosition, ref int i)
        {
            if (mathString.Length <= i || mathString[i] != openingSymbol)
                throw new MathExpressionException($"It doesn't have the '{openingSymbol}' opening symbol.", invalidTokenPosition);

            i++;
        }

        /// <summary>Throws the exception if missing closing symbol.</summary>
        /// <param name="closingSymbol">The closing symbol.</param>
        /// <param name="invalidTokenPosition">The invalid token position.</param>
        /// <param name="i">The current char index.</param>
        /// <exception cref="MathExpressionException">It doesn't have the '{closingSymbol}' closing symbol.</exception>
        internal void ThrowExceptionIfNotClosed(char closingSymbol, int invalidTokenPosition, ref int i)
        {
            if (mathString.Length <= i || mathString[i] != closingSymbol)
                throw new MathExpressionException($"It doesn't have the '{closingSymbol}' closing symbol.", invalidTokenPosition);

            i++;
        }

        /// <summary>Throws the exception if not evaluated.</summary>
        /// <param name="isOperand">if set to <c>true</c> [is operand].</param>
        /// <param name="invalidTokenPosition">The invalid token position.</param>
        /// <param name="i">The current char index.</param>
        /// <exception cref="MathExpressionException"></exception>
        internal void ThrowExceptionIfNotEvaluated(bool isOperand, int invalidTokenPosition, int i)
        {
            if (mathString.IsWhiteSpace(invalidTokenPosition, i))
                throw new MathExpressionException($"{(isOperand ? "The operand" : "It")} is not recognizable.", invalidTokenPosition);
        }

        /// <summary>Skips parenthesis ().</summary>
        /// <param name="i">The current char index.</param>
        internal void SkipParenthesis(ref int i)
        {
            if (mathString.Length <= i || mathString[i] != Constants.DefaultOpeningSymbol)
                return;

            var tokenPosition = i;
            i++;
            mathString.SkipWhiteSpace(ref i);
            mathString.ThrowExceptionIfNotClosed(Constants.DefaultClosingSymbol, tokenPosition, ref i);
        }

        #endregion
    }
}