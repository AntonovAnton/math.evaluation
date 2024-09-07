using System;
using MathEvaluation.Context;
using MathEvaluation.Parameters;

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
}