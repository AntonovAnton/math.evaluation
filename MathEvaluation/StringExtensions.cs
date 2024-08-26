using System;
using System.Runtime.CompilerServices;
using MathEvaluation.Context;

namespace MathEvaluation;

/// <summary>
/// Extends the string class to evaluate mathematical expressions.
/// </summary>
public static class StringExtensions
{
    /// <summary>Sets the math context for the math expression.</summary>
    /// <param name="expression">The math expression.</param>
    /// <param name="context">The math context.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator SetContext(this string expression, IMathContext context)
    {
        return new MathEvaluator(expression, context);
    }

    /// <summary>
    /// Binds custom variables and functions. Sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="expression">The math expression.</param>
    /// <param name="args">An object containing variables and functions.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator Bind(this string expression, object args)
    {
        var context = new MathContext();
        context.Bind(args);

        return new MathEvaluator(expression, context);
    }

    /// <summary>
    /// Binds the variable. Sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="expression">The math expression.</param>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this string expression, double value, char key)
        => BindVariable(expression, value, key.ToString());

    /// <inheritdoc cref="BindVariable(string, double, char)"/>
    public static MathEvaluator BindVariable(this string expression, double value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        var context = new MathContext();
        context.BindVariable(value, key);

        return new MathEvaluator(expression, context);
    }

    /// <summary>
    /// Binds the getting value function. Sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="expression">The math expression.</param>
    /// <param name="fn">The getting value function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this string expression, Func<double> fn, char key)
        => BindFunction(expression, fn, key.ToString());

    /// <inheritdoc cref="BindFunction(string, Func{double}, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        var context = new MathContext();
        context.BindFunction(fn, key);

        return new MathEvaluator(expression, context);
    }

    /// <summary>
    /// Binds the unary function. Sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="expression">The math expression.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this string expression, Func<double, double> fn, char key)
        => BindFunction(expression, fn, key.ToString());

    /// <inheritdoc cref="BindFunction(string, Func{double, double}, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        var context = new MathContext();
        context.BindFunction(fn, key);

        return new MathEvaluator(expression, context);
    }

    /// <summary>
    /// Binds the function. Sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="expression">The math expression.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this string expression, Func<double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double, double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double, double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<double, double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double, double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<double, double, double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double, double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<double[], double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    #region bind decimal methods

    /// <inheritdoc cref="BindVariable(string, double, char)"/>
    public static MathEvaluator BindVariable(this string expression, decimal value, char key)
        => BindVariable(expression, value, key.ToString());

    /// <inheritdoc cref="BindVariable(string, double, string?)"/>
    public static MathEvaluator BindVariable(this string expression, decimal value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        var context = new MathContext();
        context.BindVariable(value, key);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double}, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal> fn, char key)
        => BindFunction(expression, fn, key.ToString());

    /// <inheritdoc cref="BindFunction(string, Func{double}, string?)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        var context = new MathContext();
        context.BindFunction(fn, key);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double}, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal> fn, char key)
        => BindFunction(expression, fn, key.ToString());

    /// <inheritdoc cref="BindFunction(string, Func{double, double}, string?)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        var context = new MathContext();
        context.BindFunction(fn, key);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double, double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double, double, double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double, double, double, double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double, double, double, double, double, double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal, decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double[], double}, string?, char, char, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<decimal[], decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression, context);
    }

    #endregion

    #region bind boolean methods

    /// <inheritdoc cref="BindVariable(string, double, char)"/>
    public static MathEvaluator BindVariable(this string expression, bool value, char key)
        => BindVariable(expression, value, key.ToString());

    /// <inheritdoc cref="BindVariable(string, double, string?)"/>
    public static MathEvaluator BindVariable(this string expression, bool value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        var context = new MathContext();
        context.BindVariable(value, key);

        return new MathEvaluator(expression, context);
    }

    /// <inheritdoc cref="BindFunction(string, Func{double}, char)"/>
    public static MathEvaluator BindFunction(this string expression, Func<bool> fn, char key)
        => BindFunction(expression, fn, key.ToString());

    /// <inheritdoc cref="BindFunction(string, Func{double}, string?)"/>
    public static MathEvaluator BindFunction(this string expression, Func<bool> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        var context = new MathContext();
        context.BindFunction(fn, key);

        return new MathEvaluator(expression, context);
    }

    #endregion

    #region evaluate methods

    /// <inheritdoc cref="MathEvaluator.Evaluate(string, IFormatProvider?)"/>
    public static double Evaluate(this string expression, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(expression, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(string, IFormatProvider?)"/>
    public static double Evaluate(this string expression, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(expression, context, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(string, IMathContext?, IFormatProvider?)"/>
    public static double Evaluate(this ReadOnlySpan<char> span, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(span, provider);

    /// <inheritdoc cref="MathEvaluator.Evaluate(string, IMathContext?, IFormatProvider?)"/>
    public static double Evaluate(this ReadOnlySpan<char> span, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(span, context, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string expression, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(expression, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, IMathContext?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this string expression, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(expression, context, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this ReadOnlySpan<char> span, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(span, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, IMathContext?, IFormatProvider?)"/>
    public static decimal EvaluateDecimal(this ReadOnlySpan<char> span, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(span, context, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateDecimal(string, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string expression, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateBoolean(expression, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateBoolean(string, IMathContext?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this string expression, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateBoolean(expression, context, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateBoolean(string, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this ReadOnlySpan<char> span, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateBoolean(span, provider);

    /// <inheritdoc cref="MathEvaluator.EvaluateBoolean(string, IMathContext?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(this ReadOnlySpan<char> span, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateBoolean(span, context, provider);

    #endregion
}