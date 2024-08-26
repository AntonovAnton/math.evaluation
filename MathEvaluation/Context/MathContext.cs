﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using MathEvaluation.Entities;

namespace MathEvaluation.Context;

/// <summary>
/// The base implementation of the <see cref="IMathContext"/> allows for the search of custom variables, operators, and functions.
/// It uses a prefix tree, also known as a trie (pronounced "try"), 
/// for efficient searching of the variables, operators, and functions by their keys (names). 
/// Performance is improved by using <see cref="ReadOnlySpan{T}"/> for comparing strings. 
/// For more details, refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation"/>.
/// </summary>
/// <seealso cref="MathEvaluation.Context.IMathContext" />
public class MathContext : IMathContext
{
    private readonly MathContextTrie _mathContextTrie = new();

    /// <inheritdoc/>
    public IMathEntity? FirstMathEntity(ReadOnlySpan<char> expression)
        => _mathContextTrie.FirstMathEntity(expression);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentNullException">args</exception>
    /// <exception cref="System.NotSupportedException"></exception>
    public void Bind(object args)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(args));

        foreach (var propertyInfo in args
                     .GetType()
                     .GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!propertyInfo.CanRead)
                continue;

            var key = propertyInfo.Name;
            var value = propertyInfo.GetValue(args, null);
            if (IsConvertibleToDoubleType(propertyInfo.PropertyType))
            {
                if (IsDecimalType(propertyInfo.PropertyType))
                    BindVariable(Convert.ToDecimal(value), key);
                else
                    BindVariable(Convert.ToDouble(value), key);
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
                    throw new NotSupportedException($"{propertyInfo.PropertyType} isn't supported, you can use Func<T[], T> istead.");

                throw new NotSupportedException($"{propertyInfo.PropertyType} isn't supported.");
            }
        }
    }

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindVariable(double value, char key)
        => _mathContextTrie.AddMathEntity(new MathVariable<double>(key.ToString(), value));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathVariable<double>(key, value));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double> fn, char key)
        => _mathContextTrie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), fn));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathGetValueFunction<double>(key, fn));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double, double> fn, char key)
        => _mathContextTrie.AddMathEntity(new MathUnaryFunction<double>(key.ToString(), fn));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathUnaryFunction<double>(key, fn));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double, double> fn, char openingSymbol, char closingSymbol)
        => _mathContextTrie.AddMathEntity(new MathUnaryFunction<double>(openingSymbol.ToString(), fn, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => _mathContextTrie.AddMathEntity(new MathFunction<double>(key, fn, openingSymbol, separator, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperandOperator(Func<double, double> fn, char key, bool isProcessingLeft = false)
        => _mathContextTrie.AddMathEntity(new MathOperandOperator<double>(key.ToString(), fn, isProcessingLeft));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperandOperator(Func<double, double> fn, string key, bool isProcessingLeft = false)
        => _mathContextTrie.AddMathEntity(new MathOperandOperator<double>(key, fn, isProcessingLeft));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperandsOperator(Func<double, double, double> fn, char key, int precedece)
        => _mathContextTrie.AddMathEntity(new MathOperandsOperator<double>(key.ToString(), fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperandsOperator(Func<double, double, double> fn, string key, int precedece)
        => _mathContextTrie.AddMathEntity(new MathOperandsOperator<double>(key, fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperator(Func<double, double, double> fn, char key, int precedece = (int)EvalPrecedence.Basic)
        => _mathContextTrie.AddMathEntity(new MathOperator<double>(key.ToString(), fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperator(Func<double, double, double> fn, string key, int precedece = (int)EvalPrecedence.Basic)
        => _mathContextTrie.AddMathEntity(new MathOperator<double>(key, fn, precedece));

    #region decimal

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindVariable(decimal value, char key)
        => _mathContextTrie.AddMathEntity(new MathVariable<decimal>(key.ToString(), value));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindVariable(decimal value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathVariable<decimal>(key, value));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal> fn, char key)
        => _mathContextTrie.AddMathEntity(new MathGetValueFunction<decimal>(key.ToString(), fn));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathGetValueFunction<decimal>(key, fn));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal, decimal> fn, char key)
        => _mathContextTrie.AddMathEntity(new MathUnaryFunction<decimal>(key.ToString(), fn));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathUnaryFunction<decimal>(key, fn));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal, decimal> fn, char openingSymbol, char closingSymbol)
        => _mathContextTrie.AddMathEntity(new MathUnaryFunction<decimal>(openingSymbol.ToString(), fn, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((decimal[] args) => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((decimal[] args) => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((decimal[] args) => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal, decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((decimal[] args) => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<decimal[], decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => _mathContextTrie.AddMathEntity(new MathFunction<decimal>(key, fn, openingSymbol, separator, closingSymbol));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperandOperator(Func<decimal, decimal> fn, char key, bool isProcessingLeft = false)
        => _mathContextTrie.AddMathEntity(new MathOperandOperator<decimal>(key.ToString(), fn, isProcessingLeft));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperandOperator(Func<decimal, decimal> fn, string key, bool isProcessingLeft = false)
        => _mathContextTrie.AddMathEntity(new MathOperandOperator<decimal>(key, fn, isProcessingLeft));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperandsOperator(Func<decimal, decimal, decimal> fn, char key, int precedece)
        => _mathContextTrie.AddMathEntity(new MathOperandsOperator<decimal>(key.ToString(), fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperandsOperator(Func<decimal, decimal, decimal> fn, string key, int precedece)
        => _mathContextTrie.AddMathEntity(new MathOperandsOperator<decimal>(key, fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperator(Func<decimal, decimal, decimal> fn, char key, int precedece = (int)EvalPrecedence.Basic)
        => _mathContextTrie.AddMathEntity(new MathOperator<decimal>(key.ToString(), fn, precedece));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindOperator(Func<decimal, decimal, decimal> fn, string key, int precedece = (int)EvalPrecedence.Basic)
        => _mathContextTrie.AddMathEntity(new MathOperator<decimal>(key, fn, precedece));

    #endregion

    #region boolean

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindVariable(bool value, char key)
        => _mathContextTrie.AddMathEntity(new MathVariable<double>(key.ToString(), Convert.ToDouble(value)));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindVariable(bool value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathVariable<double>(key, Convert.ToDouble(value)));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<bool> fn, char key)
        => _mathContextTrie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), () => Convert.ToDouble(fn())));

    /// <inheritdoc/>
    /// <exception cref="System.ArgumentException">key</exception>
    public void BindFunction(Func<bool> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathGetValueFunction<double>(key, () => Convert.ToDouble(fn())));

    #endregion

    private static bool IsConvertibleToDoubleType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.SByte or TypeCode.Byte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32
            or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single or TypeCode.Double
            or TypeCode.Decimal or TypeCode.Boolean => true,
        _ => false
    };

    private static bool IsDecimalType(Type type) => Type.GetTypeCode(type) == TypeCode.Decimal;
}
