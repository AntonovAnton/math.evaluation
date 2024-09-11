using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;

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

    /// <summary>Initializes a new instance of the <see cref="MathContext" /> class.</summary>
    public MathContext()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="MathContext" /> class.</summary>
    /// <param name="context">An object containing constants and functions.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NotSupportedException"/>
    public MathContext(object context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        Bind(context);
    }

    /// <inheritdoc/>
    public IMathEntity? FirstMathEntity(ReadOnlySpan<char> expression)
        => _trie.FirstMathEntity(expression);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentNullException">parameters</exception>
    /// <exception cref="System.NotSupportedException"></exception>
    public void Bind(object context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        foreach (var propertyInfo in context.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!propertyInfo.CanRead)
                continue;

            var key = propertyInfo.Name;
            var value = propertyInfo.GetValue(context, null);
            var valueType = propertyInfo.PropertyType;
            if (valueType.IsConvertibleToDouble())
            {
                if (valueType.IsDecimal())
                    BindConstant(Convert.ToDecimal(value), key);
                else
                    BindConstant(Convert.ToDouble(value), key);
            }
            else if (value is Func<double> fn1)
                BindFunction(fn1, key);
            else if (value is Func<double, double> fn2)
                BindFunction(fn2, key);
            else if (value is Func<double, double, double> fn3)
                BindFunction(fn3, key);
            else if (value is Func<double, double, double, double> fn4)
                BindFunction(fn4, key);
            else if (value is Func<double, double, double, double, double> fn5)
                BindFunction(fn5, key);
            else if (value is Func<double, double, double, double, double, double> fn6)
                BindFunction(fn6, key);
            else if (value is Func<double[], double> fns)
                BindFunction(fns, key);
            else if (value is Func<decimal> decimalFn1)
                BindFunction(decimalFn1, key);
            else if (value is Func<decimal, decimal> decimalFn2)
                BindFunction(decimalFn2, key);
            else if (value is Func<decimal, decimal, decimal> decimalFn3)
                BindFunction(decimalFn3, key);
            else if (value is Func<decimal, decimal, decimal, decimal> decimalFn4)
                BindFunction(decimalFn4, key);
            else if (value is Func<decimal, decimal, decimal, decimal, decimal> decimalFn5)
                BindFunction(decimalFn5, key);
            else if (value is Func<decimal, decimal, decimal, decimal, decimal, decimal> decimalFn6)
                BindFunction(decimalFn6, key);
            else if (value is Func<decimal[], decimal> decimalFns)
                BindFunction(decimalFns, key);
            else if (value is Func<bool> boolFn1)
                BindFunction(boolFn1, key);
            else
            {
                if (propertyInfo.PropertyType.FullName.StartsWith("System.Func"))
                    throw new NotSupportedException($"{propertyInfo.PropertyType} isn't supported, you can use Func<double[], double> or Func<decimal[], decimal> istead.");

                throw new NotSupportedException($"{propertyInfo.PropertyType} isn't supported.");
            }
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NotSupportedException"/>
    public void BindConstant<T>(T value, char key)
        where T : struct
        => BindConstant(value, key.ToString());

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NotSupportedException"/>
    public void BindConstant<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null)
        where T : struct
    {
        var type = typeof(T);
        if (type.IsConvertibleToDouble())
            BindConstant(Convert.ToDouble(value), key);
        else
            throw new NotSupportedException($"{type} isn't supported.");
    }

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
    public void BindOperator(Func<double, double, double> fn, char key,
        int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null)
        => _trie.AddMathEntity(new MathOperator<double>(key.ToString(), fn, precedece, binaryOperatorType));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperator(Func<double, double, double> fn, string key,
        int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null)
        => _trie.AddMathEntity(new MathOperator<double>(key, fn, precedece, binaryOperatorType));

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
    public void BindOperator(Func<decimal, decimal, decimal> fn, char key,
        int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null)
        => _trie.AddMathEntity(new MathOperator<decimal>(key.ToString(), fn, precedece, binaryOperatorType));

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public void BindOperator(Func<decimal, decimal, decimal> fn, string key,
        int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null)
        => _trie.AddMathEntity(new MathOperator<decimal>(key, fn, precedece, binaryOperatorType));

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
