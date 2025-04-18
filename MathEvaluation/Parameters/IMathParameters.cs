using MathEvaluation.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Parameters;

/// <summary>
///     It allows for the search of custom variables that are used as parameters within a mathematical expression.
/// </summary>
public interface IMathParameters
{
    /// <summary>Binds custom variables.</summary>
    /// <param name="parameters">A dictionary containing variables.</param>
    /// <returns></returns>
    void Bind<TValue>(IDictionary<string, TValue> parameters);

    /// <summary>Binds custom variables.</summary>
    /// <param name="parameters">An object containing variables and functions.</param>
    /// <returns></returns>
    void Bind(object parameters);

    /// <inheritdoc cref="BindVariable(double, char)" />
    void BindVariable<T>(T value, char key)
        where T : struct;

    /// <inheritdoc cref="BindVariable(double, char)" />
    void BindVariable<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null)
        where T : struct;

    /// <summary>Binds the variable.</summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindVariable(double value, char key);

    /// <inheritdoc cref="BindVariable(double, char)" />
    void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <summary>Binds the getting value function.</summary>
    /// <param name="fn">The getting value function.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindFunction(Func<double> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, char)" />
    void BindFunction(Func<double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <summary>Binds the unary function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns></returns>
    void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null);

    /// <summary>Binds the function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns></returns>
    void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    #region decimal

    /// <inheritdoc cref="BindVariable(double, char)" />
    void BindVariable(decimal value, char key);

    /// <inheritdoc cref="BindVariable(double, string?)" />
    void BindVariable(decimal value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double}, char)" />
    void BindFunction(Func<decimal> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, string?)" />
    void BindFunction(Func<decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double, double}, string?, char?, char?)" />
    void BindFunction(Func<decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<decimal, decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double[], double}, string?, char, char, char)" />
    void BindFunction(Func<decimal[], decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    #endregion

    #region boolean

    /// <inheritdoc cref="BindVariable(double, char)" />
    void BindVariable(bool value, char key);

    /// <inheritdoc cref="BindVariable(double, string?)" />
    void BindVariable(bool value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double}, char)" />
    void BindFunction(Func<bool> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, string?)" />
    void BindFunction(Func<bool> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    #endregion

    #region complex

    /// <inheritdoc cref="BindVariable(double, char)" />
    void BindVariable(Complex value, char key);

    /// <inheritdoc cref="BindVariable(double, string?)" />
    void BindVariable(Complex value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double}, char)" />
    void BindFunction(Func<Complex> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, string?)" />
    void BindFunction(Func<Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double, double}, string?, char?, char?)" />
    void BindFunction(Func<Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<Complex, Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double, double}, string?, char, char, char)" />
    void BindFunction(Func<Complex, Complex, Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double[], double}, string?, char, char, char)" />
    void BindFunction(Func<Complex[], Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    #endregion

    /// <summary>Returns the first contextually recognized mathematical entity in the expression string.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <returns><see cref="IMathEntity" /> instance or null.</returns>
    internal IMathEntity? FirstMathEntity(ReadOnlySpan<char> mathString);
}