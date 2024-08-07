﻿using System;
using System.Runtime.CompilerServices;
using MathEvaluation.Context;

namespace MathEvaluation;

/// <summary>
/// Extends the <see cref="MathEvaluator"/> class to bind custom variables and functions.
/// </summary>
public static class MathEvaluatorExtensions
{
    /// <summary>
    /// Binds custom variables and functions. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="args">An object containing variables and functions.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator Bind(this MathEvaluator evaluator, object args)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.Bind(args);
        return evaluator;
    }

    /// <summary>
    /// Binds the variable. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this MathEvaluator evaluator, double value, char key)
        => BindVariable(evaluator, value, key.ToString());

    /// <summary>
    /// Binds the variable. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this MathEvaluator evaluator, double value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(value, key);
        return evaluator;
    }

    /// <summary>
    /// Binds the variable. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="getValue">The get value function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this MathEvaluator evaluator, Func<double> getValue, char key)
        => BindVariable(evaluator, getValue, key.ToString());

    /// <summary>
    /// Binds the variable. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="getValue">The get value function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this MathEvaluator evaluator, Func<double> getValue,
        [CallerArgumentExpression(nameof(getValue))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(getValue, key);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double> fn, char key)
        => BindFunction(evaluator, fn, key.ToString());

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double, double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double[], double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the variable. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this MathEvaluator evaluator, decimal value, char key)
        => BindVariable(evaluator, value, key.ToString());

    /// <summary>
    /// Binds the variable. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this MathEvaluator evaluator, decimal value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(value, key);
        return evaluator;
    }

    /// <summary>
    /// Binds the variable. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="getValue">The get value function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this MathEvaluator evaluator, Func<decimal> getValue, char key)
        => BindVariable(evaluator, getValue, key.ToString());

    /// <summary>
    /// Binds the variable. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="getValue">The get value function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindVariable(this MathEvaluator evaluator, Func<decimal> getValue,
        [CallerArgumentExpression(nameof(getValue))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(getValue, key);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal> fn, char key)
        => BindFunction(evaluator, fn, key.ToString());

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal, decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    /// <summary>
    /// Binds the function. If <see cref="MathEvaluator.Context"/> is null sets the base <see cref="MathContext"/>.
    /// </summary>
    /// <param name="evaluator">The math evaluator.</param>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns><see cref="MathEvaluator"/> instance</returns>
    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal[], decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }
}