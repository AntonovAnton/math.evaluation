using System;
using MathEvaluation.Context;
using MathEvaluation.Parameters;

namespace MathEvaluation.Extensions;

/// <summary>
/// Extends the string class to evaluate mathematical expressions.
/// </summary>
public static class StringExtensions
{
    /// <summary>Sets the math context for the math expression string.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="context">The math context.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator SetContext(this string mathString, IMathContext context)
    {
        return new MathEvaluator(mathString, context);
    }

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static double Evaluate(this string mathString, IFormatProvider? provider = null)
        => Evaluate(mathString, null, null, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static double Evaluate(this string mathString, IMathParameters parameters, IFormatProvider? provider = null)
        => Evaluate(mathString, null, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static double Evaluate(this string mathString, IMathContext context, IFormatProvider? provider = null)
        => Evaluate(mathString, context, null, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static double Evaluate(this string mathString, object parameters, IFormatProvider? provider = null)
        => Evaluate(mathString, null, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static double Evaluate(this string mathString, IMathContext? context, object parameters, IFormatProvider? provider = null)
        => Evaluate(mathString, context, new MathParameters(parameters), provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static double Evaluate(this string mathString,
        IMathContext? context, IMathParameters? parameters, IFormatProvider? provider = null)
        => MathEvaluator.Evaluate(mathString, context, parameters, provider);

    public static Func<double> Compile(this string mathString, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).Compile();

    public static Func<T, double> Compile<T>(this string mathString, T parameters, IMathContext? context = null, IFormatProvider? provider = null)
        => new MathExpression(mathString, context, provider).Compile(parameters);

    #region decimal

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string mathString, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, null, null, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string mathString, IMathParameters parameters, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, null, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string mathString, IMathContext context, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, context, null, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static decimal EvaluateDecimal(this string mathString, object parameters, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, null, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static decimal EvaluateDecimal(this string mathString, IMathContext? context, object parameters, IFormatProvider? provider = null)
        => EvaluateDecimal(mathString, context, new MathParameters(parameters), provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string mathString,
        IMathContext? context, IMathParameters? parameters, IFormatProvider? provider = null)
        => MathEvaluator.EvaluateDecimal(mathString, context, parameters, provider);

    #endregion

    #region boolean

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string mathString, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, null, null, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string mathString, IMathParameters parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, null, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string mathString, IMathContext context, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, context, null, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static bool EvaluateBoolean(this string mathString, object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, null, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static bool EvaluateBoolean(this string mathString, IMathContext? context, object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, context, new MathParameters(parameters), provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string mathString,
        IMathContext? context, IMathParameters? parameters, IFormatProvider? provider = null)
        => MathEvaluator.EvaluateBoolean(mathString, context, parameters, provider);

    #endregion
}