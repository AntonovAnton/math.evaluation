using MathEvaluation.Context;
using MathEvaluation.Parameters;
using System;
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
        /// <inheritdoc cref="MathExpression.Evaluate(MathParameters?)" />
        public double Evaluate(MathContext? context, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate();

        /// <inheritdoc cref="MathExpression.Evaluate(object?)" />
        public double Evaluate(object? parameters = null, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <inheritdoc cref="MathExpression.Evaluate(MathParameters?)" />
        public double Evaluate(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Evaluate(parameters);

        /// <inheritdoc cref="MathExpression.Evaluate{TResult}(MathParameters?)" />
        public TResult Evaluate<TResult>(MathContext? context, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Evaluate<TResult>();

        /// <inheritdoc cref="MathExpression.Evaluate{TResult}(object?)" />
        public TResult Evaluate<TResult>(object? parameters = null, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Evaluate<TResult>(parameters);

        /// <inheritdoc cref="MathExpression.Evaluate{TResult}(MathParameters?)" />
        public TResult Evaluate<TResult>(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Evaluate<TResult>(parameters);

        /// <inheritdoc cref="MathExpression.EvaluateDecimal(MathParameters?)" />
        public decimal EvaluateDecimal(MathContext? context, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateDecimal();

        /// <inheritdoc cref="MathExpression.EvaluateDecimal(object?)" />
        public decimal EvaluateDecimal(object? parameters = null, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateDecimal(parameters);

        /// <inheritdoc cref="MathExpression.EvaluateDecimal(MathParameters?)" />
        public decimal EvaluateDecimal(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateDecimal(parameters);

        /// <inheritdoc cref="MathExpression.EvaluateBoolean(MathParameters?)" />
        public bool EvaluateBoolean(MathContext? context, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateBoolean();

        /// <inheritdoc cref="MathExpression.EvaluateBoolean(object?)" />
        public bool EvaluateBoolean(object? parameters = null, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateBoolean(parameters);

        /// <inheritdoc cref="MathExpression.EvaluateBoolean(MathParameters?)" />
        public bool EvaluateBoolean(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateBoolean(parameters);

        /// <inheritdoc cref="MathExpression.EvaluateComplex(MathParameters?)" />
        public Complex EvaluateComplex(MathContext? context, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateComplex();

        /// <inheritdoc cref="MathExpression.EvaluateComplex(object?)" />
        public Complex EvaluateComplex(object? parameters = null, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateComplex(parameters);

        /// <inheritdoc cref="MathExpression.EvaluateComplex(MathParameters?)" />
        public Complex EvaluateComplex(MathParameters? parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).EvaluateComplex(parameters);

        /// <inheritdoc cref="MathExpression.Compile{T}(T)" />
        public Func<T, double> Compile<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).Compile(parameters);

        /// <inheritdoc cref="MathExpression.Compile{T, TResult}(T)" />
        public Func<T, TResult> Compile<T, TResult>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            where TResult : struct, INumberBase<TResult>
            => new MathExpression(mathString, context, provider).Compile<T, TResult>(parameters);

        /// <inheritdoc cref="MathExpression.CompileDecimal{T}(T)" />
        public Func<T, decimal> CompileDecimal<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).CompileDecimal(parameters);

        /// <inheritdoc cref="MathExpression.CompileBoolean{T}(T)" />
        public Func<T, bool> CompileBoolean<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).CompileBoolean(parameters);

        /// <inheritdoc cref="MathExpression.CompileComplex{T}(T)" />
        public Func<T, Complex> CompileComplex<T>(T parameters, MathContext? context = null, IFormatProvider? provider = null)
            => new MathExpression(mathString, context, provider).CompileComplex(parameters);

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