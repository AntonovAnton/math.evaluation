using MathEvaluation.Entities;
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
        ArgumentNullException.ThrowIfNull(parameters);

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
            var propertyType = value?.GetType();

            BindKeyValue(propertyType, key, value, true);
        }
    }

    /// <summary>Binds custom variables.</summary>
    /// <param name="parameters">An object containing variables and functions.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException"></exception>
    public void Bind(object parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        foreach (var propertyInfo in parameters.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead))
        {
            var getter = propertyInfo.GetGetMethod();
            if (getter == null)
                continue;

            var key = propertyInfo.Name;
            var value = getter.Invoke(parameters, null);
            var propertyType = propertyInfo.PropertyType;

            BindKeyValue(propertyType, key, value, false);
        }
    }

    /// <summary>Binds the variable that is defined as an expression.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindExpressionVariable(string mathString, char key)
        => _trie.AddMathEntity(new MathExpressionVariable(key.ToString(), mathString));

    /// <inheritdoc cref="BindExpressionVariable(string, char)" />
    public void BindExpressionVariable(string mathString, [CallerArgumentExpression(nameof(mathString))] string? key = null)
        => _trie.AddMathEntity(new MathExpressionVariable(key, mathString));

    #region INumberBase

    /// <summary>Binds the variable.</summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindVariable<T>(T value, char key)
        where T : struct, INumberBase<T>
        => BindVariable(value, key.ToString());

    /// <inheritdoc cref="BindVariable{T}(T, char)" />
    public void BindVariable<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathVariable<T>(key, value));

    /// <summary>Binds the getting value function.</summary>
    /// <param name="fn">The getting value function.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction<T>(Func<T> fn, char key)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathGetValueFunction<T>(key.ToString(), fn));

    /// <inheritdoc cref="BindFunction{T}(Func{T}, char)" />
    public void BindFunction<T>(Func<T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathGetValueFunction<T>(key, fn));

    /// <summary>Binds the unary function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction<T>(Func<T, T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathUnaryFunction<T>(key, fn, openingSymbol, closingSymbol));

    /// <summary>Binds the function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction<T>(Func<T, T, T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => BindFunction<T>(args => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction{T}(Func{T, T, T}, string?, char, char, char)" />
    public void BindFunction<T>(Func<T, T, T, T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => BindFunction<T>(args => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction{T}(Func{T, T, T}, string?, char, char, char)" />
    public void BindFunction<T>(Func<T, T, T, T, T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => BindFunction<T>(args => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction{T}(Func{T, T, T}, string?, char, char, char)" />
    public void BindFunction<T>(Func<T, T, T, T, T, T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => BindFunction<T>(args => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc cref="BindFunction{T}(Func{T, T, T}, string?, char, char, char)" />
    public void BindFunction<T>(Func<T[], T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathFunction<T>(key, fn, openingSymbol, separator, closingSymbol));

    #endregion

    #region boolean

    /// <inheritdoc cref="BindVariable{T}(T, char)" />
    public void BindVariable(bool value, char key)
        => _trie.AddMathEntity(new MathVariable<double>(key.ToString(), Convert.ToDouble(value)));

    /// <inheritdoc cref="BindVariable{T}(T, string?)" />
    public void BindVariable(bool value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathVariable<double>(key, Convert.ToDouble(value)));

    /// <inheritdoc cref="BindFunction{T}(Func{T}, char)" />
    public void BindFunction(Func<bool> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), () => Convert.ToDouble(fn())));

    /// <inheritdoc cref="BindFunction{T}(Func{T}, string?)" />
    public void BindFunction(Func<bool> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key, () => Convert.ToDouble(fn())));

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
            foreach (var pair in parameters)
            {
                switch (pair)
                {
                    case DictionaryEntry dictEntry:
                        {
                            if (dictEntry.Key is not string key)
                                throw new NotSupportedException("Only string keys are supported in the dictionary.");

                            var propertyType = dictEntry.Value?.GetType();
                            BindKeyValue(propertyType, key, dictEntry.Value, true);
                            break;
                        }
                    case KeyValuePair<string, object?> kvp:
                        {
                            var propertyType = kvp.Value?.GetType();
                            BindKeyValue(propertyType, kvp.Key, kvp.Value, true);
                            break;
                        }
                    case var genericPair:
                        {
                            var pairType = genericPair.GetType();
                            var keyProperty = pairType.GetProperty("Key");
                            var valueProperty = pairType.GetProperty("Value");
                            if (keyProperty == null || valueProperty == null)
                                throw new NotSupportedException("The provided parameters object must implement IDictionary<TKey, TValue> with string keys.");
                            var keyObj = keyProperty.GetValue(genericPair);
                            var valueObj = valueProperty.GetValue(genericPair);
                            if (keyObj is not string key)
                                throw new NotSupportedException("Only string keys are supported in the dictionary.");
                            BindKeyValue(valueProperty.PropertyType, key, valueObj, true);
                            break;
                        }
                }
            }

            return;
        }

        throw new NotSupportedException("The provided parameters object must implement IDictionary<TKey, TValue> with string keys.");
    }

    /// <summary>
    /// Handles the binding logic for a key-value pair.
    /// </summary>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="isDictionaryItem"></param>
    /// <exception cref="NotSupportedException"></exception>
    private void BindKeyValue(Type? propertyType, string key, object? value, bool isDictionaryItem)
    {
        if (value == null)
            throw new NotSupportedException($"Null values are not supported for '{key}'.");

        propertyType ??= value.GetType();

        if (propertyType.IsBooleanType())
        {
            BindVariable(key, Convert.ToDouble((bool)value), isDictionaryItem);
            return;
        }

        if (TryBind<double>(key, value, isDictionaryItem))
            return;
        if (TryBind<decimal>(key, value, isDictionaryItem))
            return;
        if (TryBind<Complex>(key, value, isDictionaryItem))
            return;
        if (TryBind<float>(key, value, isDictionaryItem))
            return;
        if (TryBind<int>(key, value, isDictionaryItem))
            return;
        if (TryBind<Half>(key, value, isDictionaryItem))
            return;
        if (TryBind<BigInteger>(key, value, isDictionaryItem))
            return;
        if (TryBind<char>(key, value, isDictionaryItem))
            return;
        if (TryBind<byte>(key, value, isDictionaryItem))
            return;
        if (TryBind<sbyte>(key, value, isDictionaryItem))
            return;
        if (TryBind<short>(key, value, isDictionaryItem))
            return;
        if (TryBind<ushort>(key, value, isDictionaryItem))
            return;
        if (TryBind<uint>(key, value, isDictionaryItem))
            return;
        if (TryBind<long>(key, value, isDictionaryItem))
            return;
        if (TryBind<ulong>(key, value, isDictionaryItem))
            return;
        if (TryBind<nint>(key, value, isDictionaryItem))
            return;
        if (TryBind<nuint>(key, value, isDictionaryItem))
            return;
        if (TryBind<Int128>(key, value, isDictionaryItem))
            return;
        if (TryBind<UInt128>(key, value, isDictionaryItem))
            return;

        if (propertyType.IsNumberBaseType())
        {
            BindVariable(key, (dynamic)value, isDictionaryItem);
            return;
        }

        switch (value)
        {
            case string str:
                if (string.IsNullOrWhiteSpace(str))
                    throw new NotSupportedException($"Cannot bind a variable to an empty or whitespace-only expression string for '{key}'.");
                BindExpressionVariable(str, key);
                break; ;
            case Func<bool> boolFn1:
                BindFunction(boolFn1, key);
                break;
            default:
                {
                    if (propertyType.FullName?.StartsWith("System.Func") == true)
                        throw new NotSupportedException($"{propertyType} isn't supported for '{key}', you can use Func<T[], T> instead.");

                    throw new NotSupportedException($"{propertyType} isn't supported for '{key}'.");
                }
        }
    }

    private void BindVariable<T>(string key, T value, bool isDictionaryItem)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathVariable<T>(key.ToString(), value, isDictionaryItem));

    private bool TryBind<T>(string key, object value, bool isDictionaryItem)
        where T : struct, INumberBase<T>
    {
        switch (value)
        {
            case T variableValue:
                BindVariable(key, variableValue, isDictionaryItem);
                return true;
            case Func<T> fn1:
                BindFunction(fn1, key);
                return true;
            case Func<T, T> fn2:
                BindFunction(fn2, key);
                return true;
            case Func<T, T, T> fn3:
                BindFunction(fn3, key);
                return true;
            case Func<T, T, T, T> fn4:
                BindFunction(fn4, key);
                return true;
            case Func<T, T, T, T, T> fn5:
                BindFunction(fn5, key);
                return true;
            case Func<T, T, T, T, T, T> fn6:
                BindFunction(fn6, key);
                return true;
            case Func<T[], T> fns:
                BindFunction(fns, key);
                return true;
            default:
                return false;
        }
    }
}