using System;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Context;

/// <summary>
/// The math context allows for the use of custom variables and functions.
/// </summary>
public interface IMathContext
{
    /// <summary>
    /// The default function parameters separator.
    /// </summary>
    public const char DefaultParamsSeparator = ',';

    /// <summary>
    /// The default function opening symbol.
    /// </summary>
    public const char DefaultOpeningSymbol = '(';

    /// <summary>
    /// The default function closing symbol.
    /// </summary>
    public const char DefaultClosingSymbol = ')';

    /// <summary>
    /// Returns the first contextually recognized mathematical entity in the expression.
    /// </summary>
    /// <param name="expression">The math expression.</param>
    /// <returns><see cref="IMathEntity" /> instance or null.</returns>
    IMathEntity? FirstMathEntity(ReadOnlySpan<char> expression);

    /// <summary>
    /// Binds custom variables and functions.
    /// </summary>
    /// <param name="args">An object containing variables and functions.</param>
    /// <returns></returns>
    void Bind(object args);

    /// <summary>
    /// Binds the variable.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindVariable(double value, char key);

    /// <inheritdoc cref="BindVariable(double, char)"/>
    void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <summary>
    /// Binds the variable.
    /// </summary>
    /// <param name="getValue">The get value function.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindVariable(Func<double> getValue, char key);

    /// <inheritdoc cref="BindVariable(Func{double}, char)"/>
    void BindVariable(Func<double> getValue, [CallerArgumentExpression(nameof(getValue))] string? key = null);

    /// <summary>
    /// Binds the function.
    /// </summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindFunction(Func<double, double> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double, double}, char)"/>
    void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <summary>
    /// Binds the function.
    /// </summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns></returns>
    void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindVariable(double, char)"/>
    void BindVariable(decimal value, char key);

    /// <inheritdoc cref="BindVariable(double, string?)"/>
    void BindVariable(decimal value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindVariable(Func{double}, char)"/>
    void BindVariable(Func<decimal> getValue, char key);

    /// <inheritdoc cref="BindVariable(Func{double}, string?)"/>
    void BindVariable(Func<decimal> getValue, [CallerArgumentExpression(nameof(getValue))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double, double}, char)"/>
    void BindFunction(Func<decimal, decimal> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double, double}, string?)"/>
    void BindFunction(Func<decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal, decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double[], double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal[], decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    /// <inheritdoc cref="BindVariable(double, char)"/>
    void BindVariable(bool value, char key);

    /// <inheritdoc cref="BindVariable(double, string?)"/>
    void BindVariable(bool value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindVariable(Func{double}, char)"/>
    void BindVariable(Func<bool> getValue, char key);

    /// <inheritdoc cref="BindVariable(Func{double}, string?)"/>
    void BindVariable(Func<bool> getValue, [CallerArgumentExpression(nameof(getValue))] string? key = null);
}
