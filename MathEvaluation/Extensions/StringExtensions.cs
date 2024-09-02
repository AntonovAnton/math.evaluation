using System;
using System.Collections.Generic;
using MathEvaluation.Context;
using MathTrigonometric;

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

    #region evaluate methods

    /// <inheritdoc cref="MathEvaluator.Evaluate(string, IMathParameters?, IFormatProvider?)"/>
    public static double Evaluate(this string mathString,
        IMathParameters? parameters = null, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(mathString, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(string, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static double Evaluate(this string mathString, IMathContext? context,
        IMathParameters? parameters = null, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(mathString, context, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string mathString,
        IMathParameters? parameters = null, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(mathString, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string mathString,
        IMathContext? context, IMathParameters? parameters = null, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(mathString, context, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateBoolean(string, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string mathString,
        IMathParameters? parameters = null, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateBoolean(mathString, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateBoolean(string, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string mathString,
        IMathContext? context, IMathParameters? parameters = null, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateBoolean(mathString, context, parameters, provider);

    #endregion

    #region Evaluate(object parameters)

    /// <inheritdoc cref="MathEvaluator.Evaluate(string, object, IFormatProvider?)"/>
    public static double Evaluate(this string mathString,
        object parameters, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(mathString, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(string, IMathContext?, object, IFormatProvider?)"/>
    public static double Evaluate(this string mathString, IMathContext? context,
        object parameters, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(mathString, context, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, object, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string mathString,
        object parameters, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(mathString, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, IMathContext?, object, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string mathString,
        IMathContext? context, object parameters, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(mathString, context, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateBoolean(string, object, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string mathString,
        object parameters, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateBoolean(mathString, parameters, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateBoolean(string, IMathContext?, object, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string mathString,
        IMathContext? context, object parameters, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateBoolean(mathString, context, parameters, provider);

    #endregion
}