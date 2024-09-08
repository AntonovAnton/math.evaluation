using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using MathEvaluation.Entities;

namespace MathEvaluation.Context;

/// <summary>
/// The math context allows for the search of constants, operators, and functions.
/// </summary>
public interface IMathContext
{
    /// <summary>Returns the first contextually recognized mathematical entity in the expression string.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <returns><see cref="IMathEntity" /> instance or null.</returns>
    IMathEntity? FirstMathEntity(ReadOnlySpan<char> mathString);

    /// <summary>Binds constants and functions.</summary>
    /// <param name="context">An object containing constants and functions.</param>
    /// <returns></returns>
    void Bind(object context);

    /// <summary>Binds the constant.</summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    void BindConstant(double value, char key);

    /// <inheritdoc cref="BindConstant(double, char)"/>
    void BindConstant(double value, [CallerArgumentExpression(nameof(value))] string? key = null);

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

    /// <summary>Binds the unary function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    void BindFunction(Func<double, double> fn, char openingSymbol, char closingSymbol);

    /// <summary>Binds the function.</summary>
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
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns></returns>
    void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

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
    /// <param name="binaryOperatorType">The specified expression type of the operator allows improve performance if it matches a C# binary operator.</param>
    void BindOperator(Func<double, double, double> fn, char key,
        int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null);

    /// <inheritdoc cref="BindOperator(Func{double, double, double}, char, int, ExpressionType?)"/>
    void BindOperator(Func<double, double, double> fn, string key,
        int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null);

    #region decimal

    /// <inheritdoc cref="BindConstant(double, char)"/>
    void BindConstant(decimal value, char key);

    /// <inheritdoc cref="BindConstant(double, string?)"/>
    void BindConstant(decimal value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double}, char)"/>
    void BindFunction(Func<decimal> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, string?)"/>
    void BindFunction(Func<decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double, double}, char)"/>
    void BindFunction(Func<decimal, decimal> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double, double}, char, char)"/>
    void BindFunction(Func<decimal, decimal> fn, char openingSymbol, char closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double}, string?, char?, char?)"/>
    void BindFunction(Func<decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double, double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal, decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double[], double}, string?, char, char, char)"/>
    void BindFunction(Func<decimal[], decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol);

    /// <inheritdoc cref="BindOperandOperator(Func{double, double}, char, bool)"/>
    void BindOperandOperator(Func<decimal, decimal> fn, char key, bool isProcessingLeft = false);

    /// <inheritdoc cref="BindOperandOperator(Func{double, double}, string, bool)"/>
    void BindOperandOperator(Func<decimal, decimal> fn, string key, bool isProcessingLeft = false);

    /// <inheritdoc cref="BindOperandsOperator(Func{double, double, double}, char, int)"/>
    void BindOperandsOperator(Func<decimal, decimal, decimal> fn, char key, int precedece);

    /// <inheritdoc cref="BindOperandsOperator(Func{double, double, double}, string, int)"/>
    void BindOperandsOperator(Func<decimal, decimal, decimal> fn, string key, int precedece);

    /// <inheritdoc cref="BindOperator(Func{double, double, double}, char, int, ExpressionType?)"/>
    void BindOperator(Func<decimal, decimal, decimal> fn, char key,
        int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null);

    /// <inheritdoc cref="BindOperator(Func{double, double, double}, string, int, ExpressionType?)"/>
    void BindOperator(Func<decimal, decimal, decimal> fn, string key,
        int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null);

    #endregion

    #region boolean

    /// <inheritdoc cref="BindConstant(double, char)"/>
    void BindConstant(bool value, char key);

    /// <inheritdoc cref="BindConstant(double, string?)"/>
    void BindConstant(bool value, [CallerArgumentExpression(nameof(value))] string? key = null);

    /// <inheritdoc cref="BindFunction(Func{double}, char)"/>
    void BindFunction(Func<bool> fn, char key);

    /// <inheritdoc cref="BindFunction(Func{double}, string?)"/>
    void BindFunction(Func<bool> fn, [CallerArgumentExpression(nameof(fn))] string? key = null);

    #endregion
}
