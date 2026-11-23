using System.Collections;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

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
    [RequiresUnreferencedCode("Binding parameters requires reflection to access their properties which may be trimmed")]
    public MathParameters(object parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        if (parameters is IEnumerable enumerable)
        {
            if (TryBindDictionary(enumerable))
                return;

            Bind(enumerable);
            return;
        }

        Bind(parameters);
    }

    /// <summary>Binds custom variables.</summary>
    /// <param name="parameters">A dictionary containing variables.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException"></exception>
    public void Bind(IDictionary<string, object> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        foreach (var (key, value) in parameters)
        {
            BindProperty(key, value, true);
        }
    }

    /// <inheritdoc cref="Bind(IDictionary{string, object?})"/>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public void Bind<TValue>(IDictionary<string, TValue> parameters)
        where TValue : struct, INumberBase<TValue>
    {
        ArgumentNullException.ThrowIfNull(parameters);

        foreach (var (key, value) in parameters)
        {
            BindVariable(key, value, true);
        }
    }

    /// <inheritdoc cref="Bind(IDictionary{string, object?})"/>
    public void Bind(IDictionary<string, bool> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        foreach (var (key, value) in parameters)
        {
            BindBoolVariable(key, value, true);
        }
    }

    /// <summary>Binds custom variables.</summary>
    /// <param name="parameters">An object containing variables and functions.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException"></exception>
    [RequiresUnreferencedCode("Binding parameters requires reflection to access their properties which may be trimmed")]
    public void Bind(object parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        if (!RuntimeFeature.IsDynamicCodeSupported)
        {
            throw new PlatformNotSupportedException(
                "Parameter reflection is not supported on NativeAOT. Define parameters explicitly instead.");
        }

        foreach (var propertyInfo in parameters.GetType().GetReadableProperties())
        {
            var getter = propertyInfo.GetGetMethod();
            if (getter == null)
                continue;

            var key = propertyInfo.Name;
            var value = getter.Invoke(parameters, null);
            var propertyType = propertyInfo.PropertyType;

            BindProperty(propertyType, key, value, false);
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
        => _trie.AddMathEntity(new MathFunction<T>(key, args => fn(args[0], args[1]), openingSymbol, separator, closingSymbol));

    /// <inheritdoc cref="BindFunction{T}(Func{T, T, T}, string?, char, char, char)" />
    public void BindFunction<T>(Func<T, T, T, T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathFunction<T>(key, args => fn(args[0], args[1], args[2]), openingSymbol, separator, closingSymbol));

    /// <inheritdoc cref="BindFunction{T}(Func{T, T, T}, string?, char, char, char)" />
    public void BindFunction<T>(Func<T, T, T, T, T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathFunction<T>(key, args => fn(args[0], args[1], args[2], args[3]), openingSymbol, separator, closingSymbol));

    /// <inheritdoc cref="BindFunction{T}(Func{T, T, T}, string?, char, char, char)" />
    public void BindFunction<T>(Func<T, T, T, T, T, T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathFunction<T>(key, args => fn(args[0], args[1], args[2], args[3], args[4]), openingSymbol, separator, closingSymbol));

    /// <inheritdoc cref="BindFunction{T}(Func{T, T, T}, string?, char, char, char)" />
    public void BindFunction<T>(Func<T[], T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathFunction<T>(key, fn, openingSymbol, separator, closingSymbol));

    /// <summary>Binds the async getting value function, it is supported only by the CompileAsyncFunc method.</summary>
    /// <param name="fn">The async getting value function, its parameter is the cancellation token of the compiled delegate.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindAsyncFunction<T>(Func<CancellationToken, ValueTask<T>> fn, char key)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathAsyncGetValueFunction<T>(key.ToString(), fn));

    /// <inheritdoc cref="BindAsyncFunction{T}(Func{CancellationToken, ValueTask{T}}, char)" />
    public void BindAsyncFunction<T>(Func<CancellationToken, ValueTask<T>> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathAsyncGetValueFunction<T>(key, fn));

    /// <summary>Binds the async unary function, it is supported only by the CompileAsyncFunc method.</summary>
    /// <param name="fn">The async function, its last parameter is the cancellation token of the compiled delegate.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindAsyncFunction<T>(Func<T, CancellationToken, ValueTask<T>> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathAsyncUnaryFunction<T>(key, fn, openingSymbol, closingSymbol));

    /// <summary>Binds the async function, it is supported only by the CompileAsyncFunc method.</summary>
    /// <param name="fn">The async function, its last parameter is the cancellation token of the compiled delegate.</param>
    /// <param name="key">The key.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindAsyncFunction<T>(Func<T, T, CancellationToken, ValueTask<T>> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathAsyncFunction<T>(key, (args, cancellationToken) => fn(args[0], args[1], cancellationToken), openingSymbol, separator,
            closingSymbol));

    /// <inheritdoc cref="BindAsyncFunction{T}(Func{T, T, CancellationToken, ValueTask{T}}, string?, char, char, char)" />
    public void BindAsyncFunction<T>(Func<T, T, T, CancellationToken, ValueTask<T>> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathAsyncFunction<T>(key, (args, cancellationToken) => fn(args[0], args[1], args[2], cancellationToken), openingSymbol,
            separator, closingSymbol));

    /// <inheritdoc cref="BindAsyncFunction{T}(Func{T, T, CancellationToken, ValueTask{T}}, string?, char, char, char)" />
    public void BindAsyncFunction<T>(Func<T, T, T, T, CancellationToken, ValueTask<T>> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathAsyncFunction<T>(key, (args, cancellationToken) => fn(args[0], args[1], args[2], args[3], cancellationToken),
            openingSymbol, separator, closingSymbol));

    /// <inheritdoc cref="BindAsyncFunction{T}(Func{T, T, CancellationToken, ValueTask{T}}, string?, char, char, char)" />
    public void BindAsyncFunction<T>(Func<T, T, T, T, T, CancellationToken, ValueTask<T>> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathAsyncFunction<T>(key, (args, cancellationToken) => fn(args[0], args[1], args[2], args[3], args[4], cancellationToken),
            openingSymbol, separator, closingSymbol));

    /// <inheritdoc cref="BindAsyncFunction{T}(Func{T, T, CancellationToken, ValueTask{T}}, string?, char, char, char)" />
    public void BindAsyncFunction<T>(Func<T[], CancellationToken, ValueTask<T>> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathAsyncFunction<T>(key, fn, openingSymbol, separator, closingSymbol));

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

    /// <inheritdoc cref="BindAsyncFunction{T}(Func{CancellationToken, ValueTask{T}}, char)" />
    public void BindAsyncFunction(Func<CancellationToken, ValueTask<bool>> fn, char key)
        => _trie.AddMathEntity(new MathAsyncGetValueFunction<double>(key.ToString(), cancellationToken => AsyncContinuation.ToDouble(fn(cancellationToken))));

    /// <inheritdoc cref="BindAsyncFunction{T}(Func{CancellationToken, ValueTask{T}}, string?)" />
    public void BindAsyncFunction(Func<CancellationToken, ValueTask<bool>> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathAsyncGetValueFunction<double>(key, cancellationToken => AsyncContinuation.ToDouble(fn(cancellationToken))));

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
    [RequiresUnreferencedCode("Binding parameters requires reflection to access their properties which may be trimmed")]
    private void Bind(IEnumerable parameters)
    {
        if (!RuntimeFeature.IsDynamicCodeSupported)
        {
            throw new PlatformNotSupportedException(
                "Parameter reflection is not supported on NativeAOT. Define parameters explicitly instead.");
        }

        var type = parameters.GetType();
        if (!type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
        {
            throw new NotSupportedException("The provided parameters object must implement IDictionary<TKey, TValue> with string keys.");
        }

        foreach (var pair in parameters)
        {
            switch (pair)
            {
                case DictionaryEntry dictEntry:
                    {
                        if (dictEntry.Key is not string key)
                            throw new NotSupportedException("Only string keys are supported in the dictionary.");

                        var propertyType = dictEntry.Value?.GetType();
                        BindProperty(propertyType, key, dictEntry.Value, true);
                        break;
                    }
                default:
                    {
                        var pairType = pair.GetType();
                        var keyProperty = pairType.GetProperty("Key");
                        var valueProperty = pairType.GetProperty("Value");
                        if (keyProperty == null || valueProperty == null)
                            throw new NotSupportedException("The provided parameters object must implement IDictionary<TKey, TValue> with string keys.");

                        var keyObj = keyProperty.GetValue(pair);
                        var valueObj = valueProperty.GetValue(pair);
                        if (keyObj is not string key)
                            throw new NotSupportedException("Only string keys are supported in the dictionary.");

                        BindProperty(valueProperty.PropertyType, key, valueObj, true);
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// Binds the property based on its type and value. It supports various numeric types, boolean values, and expression strings.
    /// </summary>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="isDictionaryItem"></param>
    /// <exception cref="NotSupportedException"></exception>
    [RequiresUnreferencedCode("Binding property requires reflection which may access trimmed members")]
    private void BindProperty(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.Interfaces)]
        Type? propertyType,
        string key, object? value, bool isDictionaryItem)
    {
        if (value == null)
            throw new NotSupportedException($"Null values are not supported for '{key}'.");

        if (value is bool boolValue)
        {
            BindBoolVariable(key, boolValue, isDictionaryItem);
            return;
        }

        if (TryBindNumber(key, value, isDictionaryItem))
            return;

        switch (value)
        {
            case string str:
                if (string.IsNullOrWhiteSpace(str))
                    throw new NotSupportedException($"Cannot bind an empty or whitespace-only expression string for '{key}'.");

                BindExpressionVariable(str, key);
                break;
            case Func<bool> boolFn:
                BindFunction(boolFn, key);
                break;
            case Func<CancellationToken, ValueTask<bool>> boolAsyncFn:
                BindAsyncFunction(boolAsyncFn, key);
                break;
            default:
                {
                    propertyType ??= value.GetType();
                    if (RuntimeFeature.IsDynamicCodeSupported && propertyType.IsNumberBaseType())
                    {
                        BindVariable(key, (dynamic)value, isDictionaryItem);
                        return;
                    }

                    throw propertyType.CreateNotSupportedException(key);
                }
        }
    }

    ///<inheritdoc cref="BindProperty(Type?, string, object?, bool)" />
    private void BindProperty(string key, object? value, bool isDictionaryItem)
    {
        if (value == null)
            throw new NotSupportedException($"Null values are not supported for '{key}'.");

        if (value is bool boolValue)
        {
            BindBoolVariable(key, boolValue, isDictionaryItem);
            return;
        }

        if (TryBindNumber(key, value, isDictionaryItem))
            return;

        switch (value)
        {
            case string str:
                if (string.IsNullOrWhiteSpace(str))
                    throw new NotSupportedException($"Cannot bind an empty or whitespace-only expression string for '{key}'.");

                BindExpressionVariable(str, key);
                break;
            case Func<bool> boolFn:
                BindFunction(boolFn, key);
                break;
            case Func<CancellationToken, ValueTask<bool>> boolAsyncFn:
                BindAsyncFunction(boolAsyncFn, key);
                break;
            default:
                {
                    var propertyType = value.GetType();
                    throw propertyType.CreateNotSupportedException(key);
                }
        }
    }

    private void BindVariable<T>(string key, T value, bool isDictionaryItem)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathVariable<T>(key, value, isDictionaryItem));

    private void BindBoolVariable(string key, bool value, bool isDictionaryItem)
        => _trie.AddMathEntity(new MathVariable<double>(key, Convert.ToDouble(value), isDictionaryItem));

    private bool TryBindDictionary(object parameters)
    {
        if (TryBindNumberDictionary(parameters))
            return true;

        if (parameters is IDictionary<string, bool> boolValuesDict)
        {
            Bind(boolValuesDict);
            return true;
        }

        if (parameters is IDictionary<string, object> dictionary)
        {
            Bind(dictionary);
            return true;
        }

        return false;
    }

    /// <summary>Tries to bind the number, or the function that returns the number.</summary>
    private bool TryBindNumber(string key, object value, bool isDictionaryItem)
        => TryBind<double>(key, value, isDictionaryItem) ||
           TryBind<decimal>(key, value, isDictionaryItem) ||
           TryBind<Complex>(key, value, isDictionaryItem) ||
           TryBind<float>(key, value, isDictionaryItem) ||
           TryBind<int>(key, value, isDictionaryItem) ||
           TryBind<Half>(key, value, isDictionaryItem) ||
           TryBind<BigInteger>(key, value, isDictionaryItem) ||
           TryBind<char>(key, value, isDictionaryItem) ||
           TryBind<byte>(key, value, isDictionaryItem) ||
           TryBind<sbyte>(key, value, isDictionaryItem) ||
           TryBind<short>(key, value, isDictionaryItem) ||
           TryBind<ushort>(key, value, isDictionaryItem) ||
           TryBind<uint>(key, value, isDictionaryItem) ||
           TryBind<long>(key, value, isDictionaryItem) ||
           TryBind<ulong>(key, value, isDictionaryItem) ||
           TryBind<nint>(key, value, isDictionaryItem) ||
           TryBind<nuint>(key, value, isDictionaryItem) ||
           TryBind<Int128>(key, value, isDictionaryItem) ||
           TryBind<UInt128>(key, value, isDictionaryItem)
#if NET11_0_OR_GREATER
           || TryBindNet11Types(key, value, isDictionaryItem)
#endif
    ;

    /// <summary>Tries to bind the dictionary of the numbers.</summary>
    private bool TryBindNumberDictionary(object parameters)
        => TryBindDictionary<double>(parameters) ||
           TryBindDictionary<decimal>(parameters) ||
           TryBindDictionary<Complex>(parameters) ||
           TryBindDictionary<float>(parameters) ||
           TryBindDictionary<int>(parameters) ||
           TryBindDictionary<Half>(parameters) ||
           TryBindDictionary<BigInteger>(parameters) ||
           TryBindDictionary<char>(parameters) ||
           TryBindDictionary<byte>(parameters) ||
           TryBindDictionary<sbyte>(parameters) ||
           TryBindDictionary<short>(parameters) ||
           TryBindDictionary<ushort>(parameters) ||
           TryBindDictionary<uint>(parameters) ||
           TryBindDictionary<long>(parameters) ||
           TryBindDictionary<ulong>(parameters) ||
           TryBindDictionary<nint>(parameters) ||
           TryBindDictionary<nuint>(parameters) ||
           TryBindDictionary<Int128>(parameters) ||
           TryBindDictionary<UInt128>(parameters)
#if NET11_0_OR_GREATER
           || TryBindNet11Dictionary(parameters)
#endif
    ;

#if NET11_0_OR_GREATER
    /// <summary>Tries to bind the .NET 11+ numeric types: Decimal32/64/128, BFloat16, and Complex&lt;T&gt;.</summary>
    private bool TryBindNet11Types(string key, object value, bool isDictionaryItem)
        => TryBind<Decimal64>(key, value, isDictionaryItem) ||
           TryBind<Decimal128>(key, value, isDictionaryItem) ||
           TryBind<Decimal32>(key, value, isDictionaryItem) ||
           TryBind<BFloat16>(key, value, isDictionaryItem) ||
           TryBind<Complex<double>>(key, value, isDictionaryItem) ||
           TryBind<Complex<float>>(key, value, isDictionaryItem) ||
           TryBind<Complex<Half>>(key, value, isDictionaryItem) ||
           TryBind<Complex<BFloat16>>(key, value, isDictionaryItem) ||
           TryBind<Complex<Decimal32>>(key, value, isDictionaryItem) ||
           TryBind<Complex<Decimal64>>(key, value, isDictionaryItem) ||
           TryBind<Complex<Decimal128>>(key, value, isDictionaryItem);

    /// <summary>Tries to bind the dictionary of the .NET 11+ numeric types: Decimal32/64/128, BFloat16, and Complex&lt;T&gt;.</summary>
    private bool TryBindNet11Dictionary(object parameters)
        => TryBindDictionary<Decimal64>(parameters) ||
           TryBindDictionary<Decimal128>(parameters) ||
           TryBindDictionary<Decimal32>(parameters) ||
           TryBindDictionary<BFloat16>(parameters) ||
           TryBindDictionary<Complex<double>>(parameters) ||
           TryBindDictionary<Complex<float>>(parameters) ||
           TryBindDictionary<Complex<Half>>(parameters) ||
           TryBindDictionary<Complex<BFloat16>>(parameters) ||
           TryBindDictionary<Complex<Decimal32>>(parameters) ||
           TryBindDictionary<Complex<Decimal64>>(parameters) ||
           TryBindDictionary<Complex<Decimal128>>(parameters);
#endif

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
            case Func<CancellationToken, ValueTask<T>> afn0:
                BindAsyncFunction(afn0, key);
                return true;
            case Func<T, CancellationToken, ValueTask<T>> afn1:
                BindAsyncFunction(afn1, key);
                return true;
            case Func<T, T, CancellationToken, ValueTask<T>> afn2:
                BindAsyncFunction(afn2, key);
                return true;
            case Func<T, T, T, CancellationToken, ValueTask<T>> afn3:
                BindAsyncFunction(afn3, key);
                return true;
            case Func<T, T, T, T, CancellationToken, ValueTask<T>> afn4:
                BindAsyncFunction(afn4, key);
                return true;
            case Func<T, T, T, T, T, CancellationToken, ValueTask<T>> afn5:
                BindAsyncFunction(afn5, key);
                return true;
            // ReSharper disable once IdentifierTypo
            case Func<T[], CancellationToken, ValueTask<T>> afns:
                BindAsyncFunction(afns, key);
                return true;
            default:
                return false;
        }
    }

    private bool TryBindDictionary<TValue>(object parameters)
        where TValue : struct, INumberBase<TValue>
    {
        if (parameters is IDictionary<string, TValue> dictionary)
        {
            Bind(dictionary);
            return true;
        }

        return false;
    }
}