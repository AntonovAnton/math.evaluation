﻿using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Parameters;

/// <summary>
///     It allows for the search of custom variables and functions that are used as parameters within a mathematical expression.
///     It uses a prefix tree, also known as a trie (pronounced "try"),
///     for efficient searching of the parameters by their keys (names).
///     Performance is improved by using <see cref="ReadOnlySpan{T}" /> for comparing strings.
///     For more details, refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
public sealed class MathParameters
{
    private readonly MathEntitiesTrie _trie = new();

    /// <summary>Initializes a new instance of the <see cref="MathParameters" /> class.</summary>
    public MathParameters()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="MathParameters" /> class.</summary>
    /// <param name="parameters">An object containing variables and functions.</param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="NotSupportedException" />
    public MathParameters(object parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        if (parameters is IEnumerable dictionary)
        {
            Bind(dictionary);
            return;
        }

        Bind(parameters);
    }

    /// <summary>Binds custom variables.</summary>
    /// <param name="parameters">A dictionary containing variables.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException"></exception>
    public void Bind<TValue>(IDictionary<string, TValue> parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters), "The parameters argument cannot be null.");

        foreach (var pair in parameters)
        {
            var key = pair.Key;
            var value = pair.Value;
            BindKeyValue(true, key, value);
        }
    }

    /// <summary>Binds custom variables.</summary>
    /// <param name="parameters">An object containing variables and functions.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException"></exception>
    public void Bind(object parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        foreach (var propertyInfo in parameters.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!propertyInfo.CanRead)
                continue;

            var key = propertyInfo.Name;
            var value = propertyInfo.GetValue(parameters, null);
            BindKeyValue(false, key, value);
        }
    }

    /// <inheritdoc cref="BindVariable(double, char)" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="NotSupportedException" />
    public void BindVariable<T>(T value, char key)
        where T : struct
        => BindVariable(value, key.ToString());

    /// <inheritdoc cref="BindVariable(double, char)" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="NotSupportedException" />
    public void BindVariable<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null)
        where T : struct
    {
        var type = typeof(T);
        if (type.IsConvertibleToDouble())
            BindVariable(Convert.ToDouble(value), key);
        else
            throw new NotSupportedException($"{type} isn't supported for '{key}'.");
    }

    /// <summary>Binds the variable.</summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindVariable(double value, char key)
        => _trie.AddMathEntity(new MathVariable<double>(key.ToString(), value));

    /// <inheritdoc cref="BindVariable(double, char)" />
    public void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathVariable<double>(key, value));

    /// <summary>Binds the variable that is defined as an expression.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindExpressionVariable(string mathString, char key)
        => _trie.AddMathEntity(new MathExpressionVariable(key.ToString(), mathString));

    /// <inheritdoc cref="BindExpressionVariable(string, char)" />
    public void BindExpressionVariable(string mathString, [CallerArgumentExpression(nameof(mathString))] string? key = null)
        => _trie.AddMathEntity(new MathExpressionVariable(key, mathString));

    /// <summary>Binds the getting value function.</summary>
    /// <param name="fn">The getting value function.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), fn));

    /// <inheritdoc cref="BindFunction(Func{double}, char)" />
    public void BindFunction(Func<double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key, fn));

    /// <summary>Binds the unary function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        => _trie.AddMathEntity(new MathUnaryFunction<double>(key, fn, openingSymbol, closingSymbol));

    /// <summary>Binds the function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => _trie.AddMathEntity(new MathFunction<double>(key, fn, openingSymbol, separator, closingSymbol));

    #region decimal

    /// <inheritdoc cref="BindVariable(double, char)" />
    public void BindVariable(decimal value, char key)
        => _trie.AddMathEntity(new MathVariable<decimal>(key.ToString(), value));

    /// <inheritdoc cref="BindVariable(double, string?)" />
    public void BindVariable(decimal value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathVariable<decimal>(key, value));

    /// <inheritdoc cref="BindFunction(Func{double}, char)" />
    public void BindFunction(Func<decimal> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<decimal>(key.ToString(), fn));

    /// <inheritdoc cref="BindFunction(Func{double}, string?)" />
    public void BindFunction(Func<decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<decimal>(key, fn));

    /// <inheritdoc cref="BindFunction(Func{double, double}, string?, char?, char?)" />
    public void BindFunction(Func<decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        => _trie.AddMathEntity(new MathUnaryFunction<decimal>(key, fn, openingSymbol, closingSymbol));

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<decimal, decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double[], double}, string?, char, char, char)" />
    public void BindFunction(Func<decimal[], decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => _trie.AddMathEntity(new MathFunction<decimal>(key, fn, openingSymbol, separator, closingSymbol));

    #endregion

    #region boolean

    /// <inheritdoc cref="BindVariable(double, char)" />
    public void BindVariable(bool value, char key)
        => _trie.AddMathEntity(new MathVariable<double>(key.ToString(), Convert.ToDouble(value)));

    /// <inheritdoc cref="BindVariable(double, string?)" />
    public void BindVariable(bool value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathVariable<double>(key, Convert.ToDouble(value)));

    /// <inheritdoc cref="BindFunction(Func{double}, char)" />
    public void BindFunction(Func<bool> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), () => Convert.ToDouble(fn())));

    /// <inheritdoc cref="BindFunction(Func{double}, string?)" />
    public void BindFunction(Func<bool> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key, () => Convert.ToDouble(fn())));

    #endregion

    #region complex

    /// <inheritdoc cref="BindVariable(double, char)" />
    public void BindVariable(Complex value, char key)
        => _trie.AddMathEntity(new MathVariable<Complex>(key.ToString(), value));

    /// <inheritdoc cref="BindVariable(double, string?)" />
    public void BindVariable(Complex value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathVariable<Complex>(key, value));

    /// <inheritdoc cref="BindFunction(Func{double}, char)" />
    public void BindFunction(Func<Complex> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<Complex>(key.ToString(), fn));

    /// <inheritdoc cref="BindFunction(Func{double}, string?)" />
    public void BindFunction(Func<Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<Complex>(key, fn));

    /// <inheritdoc cref="BindFunction(Func{double, double}, string?, char?, char?)" />
    public void BindFunction(Func<Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        => _trie.AddMathEntity(new MathUnaryFunction<Complex>(key, fn, openingSymbol, closingSymbol));

    /// <inheritdoc cref="BindFunction(Func{double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<Complex, Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double, double, double, double, double, double}, string?, char, char, char)" />
    public void BindFunction(Func<Complex, Complex, Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction(Func{double[], double}, string?, char, char, char)" />
    public void BindFunction(Func<Complex[], Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => _trie.AddMathEntity(new MathFunction<Complex>(key, fn, openingSymbol, separator, closingSymbol));

    #endregion

    /// <summary>Returns the first contextually recognized mathematical entity in the expression string.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <returns><see cref="IMathEntity" /> instance or null.</returns>
    internal IMathEntity? FirstMathEntity(ReadOnlySpan<char> mathString)
        => _trie.FirstMathEntity(mathString);

    /// <summary>
    /// Binds variables and functions from a dictionary.
    /// </summary>
    /// <param name="parameters">A dictionary containing variables and functions.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException"></exception>
    private void Bind(IEnumerable parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters), "The parameters argument cannot be null.");

        var type = parameters.GetType();
        if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
        {
            foreach (DictionaryEntry pair in (IDictionary)parameters)
            {
                if (pair.Key is not string key)
                    throw new NotSupportedException("Only string keys are supported in the dictionary.");

                BindKeyValue(true, key, pair.Value);
            }

            return;
        }

        throw new NotSupportedException("The provided parameters object must implement IDictionary<TKey, TValue> with string keys.");
    }

    /// <summary>
    /// Handles the binding logic for a key-value pair.
    /// </summary>
    /// <param name="isDictionaryItem"></param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="NotSupportedException"></exception>
    private void BindKeyValue(bool isDictionaryItem, string key, object? value)
    {
        var propertyType = (value?.GetType()) ?? throw new NotSupportedException($"Null values are not supported for '{key}'.");
        if (propertyType.IsConvertibleToDouble())
        {
            if (propertyType.IsDecimal())
                BindVariable(isDictionaryItem, key, (decimal)value);
            else
                BindVariable(isDictionaryItem, key, Convert.ToDouble(value));

            return;
        }

        switch (value)
        {
            case string str:
                if (string.IsNullOrWhiteSpace(str))
                    throw new NotSupportedException($"Cannot bind a variable to an empty or whitespace-only expression string for '{key}'.");
                BindExpressionVariable(str, key);
                break;
            case Complex c:
                BindVariable(isDictionaryItem, key, c);
                break;
            case Func<double> fn1:
                BindFunction(fn1, key);
                break;
            case Func<double, double> fn2:
                BindFunction(fn2, key);
                break;
            case Func<double, double, double> fn3:
                BindFunction(fn3, key);
                break;
            case Func<double, double, double, double> fn4:
                BindFunction(fn4, key);
                break;
            case Func<double, double, double, double, double> fn5:
                BindFunction(fn5, key);
                break;
            case Func<double, double, double, double, double, double> fn6:
                BindFunction(fn6, key);
                break;
            case Func<double[], double> fns:
                BindFunction(fns, key);
                break;
            case Func<decimal> decimalFn1:
                BindFunction(decimalFn1, key);
                break;
            case Func<decimal, decimal> decimalFn2:
                BindFunction(decimalFn2, key);
                break;
            case Func<decimal, decimal, decimal> decimalFn3:
                BindFunction(decimalFn3, key);
                break;
            case Func<decimal, decimal, decimal, decimal> decimalFn4:
                BindFunction(decimalFn4, key);
                break;
            case Func<decimal, decimal, decimal, decimal, decimal> decimalFn5:
                BindFunction(decimalFn5, key);
                break;
            case Func<decimal, decimal, decimal, decimal, decimal, decimal> decimalFn6:
                BindFunction(decimalFn6, key);
                break;
            case Func<decimal[], decimal> decimalFns:
                BindFunction(decimalFns, key);
                break;
            case Func<bool> boolFn1:
                BindFunction(boolFn1, key);
                break;
            case Func<Complex> complexFn1:
                BindFunction(complexFn1, key);
                break;
            case Func<Complex, Complex> complexFn2:
                BindFunction(complexFn2, key);
                break;
            case Func<Complex, Complex, Complex> complexFn3:
                BindFunction(complexFn3, key);
                break;
            case Func<Complex, Complex, Complex, Complex> complexFn4:
                BindFunction(complexFn4, key);
                break;
            case Func<Complex, Complex, Complex, Complex, Complex> complexFn5:
                BindFunction(complexFn5, key);
                break;
            case Func<Complex, Complex, Complex, Complex, Complex, Complex> complexFn6:
                BindFunction(complexFn6, key);
                break;
            case Func<Complex[], Complex> complexFns:
                BindFunction(complexFns, key);
                break;
            default:
                {
                    if (propertyType.FullName?.StartsWith("System.Func") == true)
                        throw new NotSupportedException($"{propertyType} isn't supported for '{key}', you can use Func<T[], T> instead.");

                    throw new NotSupportedException($"{propertyType} isn't supported for '{key}'.");
                }
        }
    }

    private void BindVariable(bool isDictionaryItem, string key, double value)
        => _trie.AddMathEntity(new MathVariable<double>(key.ToString(), value, isDictionaryItem));

    private void BindVariable(bool isDictionaryItem, string key, decimal value)
        => _trie.AddMathEntity(new MathVariable<decimal>(key.ToString(), value, isDictionaryItem));

    private void BindVariable(bool isDictionaryItem, string key, Complex value)
        => _trie.AddMathEntity(new MathVariable<Complex>(key.ToString(), value, isDictionaryItem));
}