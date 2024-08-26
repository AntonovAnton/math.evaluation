using System;
using System.Runtime.CompilerServices;
using MathEvaluation.Entities;

namespace MathEvaluation.Context;

/// <summary>
/// The math context allows for the search of custom variables, operators, and functions.
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

    /// <summary>Returns the first contextually recognized mathematical entity in the expression.</summary>
    /// <param name="expression">The math expression.</param>
    /// <returns><see cref="IMathEntity" /> instance or null.</returns>
    IMathEntity? FirstMathEntity(ReadOnlySpan<char> expression);

    /// <summary>Binds custom variables and functions.</summary>
    /// <param name="args">An object containing variables and functions.</param>
    /// <returns></returns>
    void Bind(object args);

    /// <summary>Binds the variable.</summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindVariable(double value, char key);

    /// <inheritdoc cref="BindVariable(double, char)"/>
    void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <summary>Binds the getting value function.</summary>
    /// <param name="fn">The getting value function.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindFunction(Func<double> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, char)"/>
    void BindFunction(Func<double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <summary>Binds the unary function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindFunction(Func<double, double> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double, double}, char)"/>
    void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <summary>Binds the unary function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    void BindFunction(Func<double, double> fn, char openingSymbol, char closingSymbol);

    /// <summary>Binds the function.</summary>
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

    /// <summary>Binds the operator that performs an action on one math operand.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="isProcessingLeft">
    ///   <c>true</c> if this instance is processing left operand; otherwise, <c>false</c>.
    /// </param>
    void BindOperandOperator(Func<double, double> fn, char key, bool isProcessingLeft = false);

    /// <inheritdoc cref="BindOperandOperator(Func{double, double}, char, bool)"/>
    void BindOperandOperator(Func<double, double> fn, string key, bool isProcessingLeft = false);

    /// <summary>Binds the math operator that can process the left and right math operands.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="precedece">The operator precedence.</param>
    /// <param name="key">The key.</param>
    void BindOperandsOperator(Func<double, double, double> fn, char key, int precedece);

    /// <inheritdoc cref="BindOperandsOperator(Func{double, double, double}, char, int)"/>
    void BindOperandsOperator(Func<double, double, double> fn, string key, int precedece);

    /// <summary>Binds the math operator that processes the left and right expressions.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="precedece">The operator precedence.</param>
    void BindOperator(Func<double, double, double> fn, char key, int precedece = (int)EvalPrecedence.Basic);

    /// <inheritdoc cref="BindOperator(Func{double, double, double}, char, int)"/>
    void BindOperator(Func<double, double, double> fn, string key, int precedece = (int)EvalPrecedence.Basic);

    #region decimal

    /// <inheritdoc cref="BindVariable(double, char)"/>
    void BindVariable(decimal value, char key);

    /// <inheritdoc cref="BindVariable(double, string?)"/>
    void BindVariable(decimal value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double}, char)"/>
    void BindFunction(Func<decimal> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, string?)"/>
    void BindFunction(Func<decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double, double}, char)"/>
    void BindFunction(Func<decimal, decimal> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double, double}, string?)"/>
    void BindFunction(Func<decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double, double}, char, char)"/>
    void BindFunction(Func<decimal, decimal> fn, char openingSymbol, char closingSymbol);

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

    /// <inheritdoc cref="BindOperandOperator(Func{double, double}, char, bool)"/>
    void BindOperandOperator(Func<decimal, decimal> fn, char key, bool isProcessingLeft = false);

    /// <inheritdoc cref="BindOperandOperator(Func{double, double}, string, bool)"/>
    void BindOperandOperator(Func<decimal, decimal> fn, string key, bool isProcessingLeft = false);

    /// <inheritdoc cref="BindOperandsOperator(Func{double, double, double}, char, int)"/>
    void BindOperandsOperator(Func<decimal, decimal, decimal> fn, char key, int precedece);

    /// <inheritdoc cref="BindOperandsOperator(Func{double, double, double}, string, int)"/>
    void BindOperandsOperator(Func<decimal, decimal, decimal> fn, string key, int precedece);

    /// <inheritdoc cref="BindOperator(Func{double, double, double}, char, int)"/>
    void BindOperator(Func<decimal, decimal, decimal> fn, char key, int precedece = (int)EvalPrecedence.Basic);

    /// <inheritdoc cref="BindOperator(Func{double, double, double}, string, int)"/>
    void BindOperator(Func<decimal, decimal, decimal> fn, string key, int precedece = (int)EvalPrecedence.Basic);

    #endregion

    #region boolean

    /// <inheritdoc cref="BindVariable(double, char)"/>
    void BindVariable(bool value, char key);

    /// <inheritdoc cref="BindVariable(double, string?)"/>
    void BindVariable(bool value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double}, char)"/>
    void BindFunction(Func<bool> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, string?)"/>
    void BindFunction(Func<bool> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    #endregion
}
