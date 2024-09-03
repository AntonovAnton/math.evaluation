using System;
using System.Runtime.CompilerServices;
using MathEvaluation.Entities;

namespace MathEvaluation.Context;

/// <summary>
/// The base implementation of the <see cref="IMathContext"/>.
/// It uses a prefix tree, also known as a trie (pronounced "try"), 
/// for efficient searching of the constants, operators, and functions by their keys (names). 
/// Performance is improved by using <see cref="ReadOnlySpan{T}"/> for comparing strings. 
/// For more details, refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation"/>.
/// </summary>
/// <seealso cref="MathEvaluation.Context.IMathContext" />
public class MathContext : IMathContext
{
    private readonly MathEntitiesTrie _trie = new();

    /// <inheritdoc/>
    public IMathEntity? FirstMathEntity(ReadOnlySpan<char> expression)
        => _trie.FirstMathEntity(expression);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindConstant(double value, char key)
        => _trie.AddMathEntity(new MathConstant<double>(key.ToString(), value));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindConstant(double value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathConstant<double>(key, value));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), fn));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key, fn));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double, double> fn, char key)
        => _trie.AddMathEntity(new MathUnaryFunction<double>(key.ToString(), fn));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double, double> fn, char openingSymbol, char closingSymbol)
        => _trie.AddMathEntity(new MathUnaryFunction<double>(openingSymbol.ToString(), fn, null, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null, 
        char? openingSymbol = null, char? closingSymbol = null)
        => _trie.AddMathEntity(new MathUnaryFunction<double>(key, fn, openingSymbol, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => _trie.AddMathEntity(new MathFunction<double>(key, fn, openingSymbol, separator, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperandOperator(Func<double, double> fn, char key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<double>(key.ToString(), fn, isProcessingLeft));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperandOperator(Func<double, double> fn, string key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<double>(key, fn, isProcessingLeft));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperandsOperator(Func<double, double, double> fn, char key, int precedece)
        => _trie.AddMathEntity(new MathOperandsOperator<double>(key.ToString(), fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperandsOperator(Func<double, double, double> fn, string key, int precedece)
        => _trie.AddMathEntity(new MathOperandsOperator<double>(key, fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperator(Func<double, double, double> fn, char key, int precedece = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<double>(key.ToString(), fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperator(Func<double, double, double> fn, string key, int precedece = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<double>(key, fn, precedece));

    #region decimal

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindConstant(decimal value, char key)
        => _trie.AddMathEntity(new MathConstant<decimal>(key.ToString(), value));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindConstant(decimal value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathConstant<decimal>(key, value));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<decimal>(key.ToString(), fn));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<decimal>(key, fn));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal, decimal> fn, char key)
        => _trie.AddMathEntity(new MathUnaryFunction<decimal>(key.ToString(), fn));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal, decimal> fn, char openingSymbol, char closingSymbol)
        => _trie.AddMathEntity(new MathUnaryFunction<decimal>(openingSymbol.ToString(), fn, null, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        => _trie.AddMathEntity(new MathUnaryFunction<decimal>(key, fn, openingSymbol, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction((decimal[] args) => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction((decimal[] args) => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction((decimal[] args) => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal, decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction((decimal[] args) => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<decimal[], decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => _trie.AddMathEntity(new MathFunction<decimal>(key, fn, openingSymbol, separator, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperandOperator(Func<decimal, decimal> fn, char key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<decimal>(key.ToString(), fn, isProcessingLeft));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperandOperator(Func<decimal, decimal> fn, string key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<decimal>(key, fn, isProcessingLeft));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperandsOperator(Func<decimal, decimal, decimal> fn, char key, int precedece)
        => _trie.AddMathEntity(new MathOperandsOperator<decimal>(key.ToString(), fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperandsOperator(Func<decimal, decimal, decimal> fn, string key, int precedece)
        => _trie.AddMathEntity(new MathOperandsOperator<decimal>(key, fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperator(Func<decimal, decimal, decimal> fn, char key, int precedece = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<decimal>(key.ToString(), fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperator(Func<decimal, decimal, decimal> fn, string key, int precedece = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<decimal>(key, fn, precedece));

    #endregion

    #region boolean

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindConstant(bool value, char key)
        => _trie.AddMathEntity(new MathConstant<double>(key.ToString(), Convert.ToDouble(value)));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindConstant(bool value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathConstant<double>(key, Convert.ToDouble(value)));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<bool> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), () => Convert.ToDouble(fn())));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindFunction(Func<bool> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key, () => Convert.ToDouble(fn())));

    #endregion
}
