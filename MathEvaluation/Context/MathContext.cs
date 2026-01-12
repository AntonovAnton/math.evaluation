using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Context;

/// <summary>
///     The base math context allows for the search of constants, operators, and functions.
///     It uses a prefix tree, also known as a trie (pronounced "try"),
///     for efficient searching of the math entities by their keys (names).
///     Performance is improved by using <see cref="ReadOnlySpan{T}" /> for comparing strings.
///     For more details, refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
public class MathContext
{
    private readonly MathEntitiesTrie _trie = new();

    /// <summary>Initializes a new instance of the <see cref="MathContext" /> class.</summary>
    public MathContext()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="MathContext" /> class.</summary>
    /// <param name="context">An object containing constants and functions.</param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="NotSupportedException" />
    public MathContext(object context)
    {
        ArgumentNullException.ThrowIfNull(context);

        Bind(context);
    }

    /// <summary>Binds constants and functions.</summary>
    /// <param name="context">An object containing constants and functions.</param>
    /// <exception cref="System.ArgumentNullException">parameters</exception>
    /// <exception cref="System.NotSupportedException"></exception>
    public void Bind(object context)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (var propertyInfo in context.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead))
        {
            var getter = propertyInfo.GetGetMethod();
            if (getter == null)
                continue;

            var key = propertyInfo.Name;
            var value = getter.Invoke(context, null);
            var propertyType = propertyInfo.PropertyType;

            BindKeyValue(propertyType, key, value);
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

    /// <summary>Binds the compatible math operator that matches a C# operator (it allows to improve performance).</summary>
    /// <param name="key">The key.</param>
    /// <param name="operatorType">The operator type.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(char key, OperatorType operatorType)
        => _trie.AddMathEntity(new MathCompatibleOperator(key.ToString(), operatorType));

    /// <inheritdoc cref="BindOperator(char, OperatorType)" />
    public void BindOperator(string key, OperatorType operatorType)
        => _trie.AddMathEntity(new MathCompatibleOperator(key, operatorType));

    #region INumberBase

    /// <summary>Binds the constant.</summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindConstant<T>(T value, char key)
        where T : struct, INumberBase<T>
        => BindConstant(value, key.ToString());

    /// <inheritdoc cref="BindConstant{T}(T, char)" />
    public void BindConstant<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathConstant<T>(key, value));

    /// <summary>Binds the getting value function.</summary>
    /// <param name="fn">The getting value function.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction<T>(Func<T> fn, char key)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathGetValueFunction<T>(key.ToString(), fn));

    /// <summary>Binds the unary function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction<T>(Func<T> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathGetValueFunction<T>(key, fn));

    /// <inheritdoc cref="BindFunction{T}(Func{T, T}, char)" />
    public void BindFunction<T>(Func<T, T> fn, char key)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathUnaryFunction<T>(key.ToString(), fn));

    /// <summary>Binds the unary function.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindFunction<T>(Func<T, T> fn, char openingSymbol, char closingSymbol)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathUnaryFunction<T>(openingSymbol.ToString(), fn, null, closingSymbol));

    /// <summary>Binds the function.</summary>
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

    /// <summary>Binds the operator that performs an action on one math operand.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="isProcessingLeft">
    ///     <c>true</c> if this instance is processing left operand; otherwise, <c>false</c>.
    /// </param>
    /// <param name="precedence">The operator precedence.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindOperandOperator<T>(Func<T, T> fn, char key, bool isProcessingLeft = false, int precedence = (int)EvalPrecedence.OperandUnaryOperator)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathOperandOperator<T>(key.ToString(), fn, isProcessingLeft, precedence));

    /// <inheritdoc cref="BindOperandOperator{T}(Func{T, T}, string, bool, int)" />
    public void BindOperandOperator<T>(Func<T, T> fn, string key, bool isProcessingLeft = false, int precedence = (int)EvalPrecedence.OperandUnaryOperator)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathOperandOperator<T>(key, fn, isProcessingLeft, precedence));

    /// <summary>Binds the math operator that can process the left and right math operands.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="precedence">The operator precedence.</param>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindOperandsOperator<T>(Func<T, T, T> fn, char key, int precedence)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathOperandsOperator<T>(key.ToString(), fn, precedence));

    /// <inheritdoc cref="BindOperandsOperator{T}(Func{T, T, T}, string, int)" />
    public void BindOperandsOperator<T>(Func<T, T, T> fn, string key, int precedence)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathOperandsOperator<T>(key, fn, precedence));

    /// <summary>Binds the math operator that processes the left and right expressions.</summary>
    /// <param name="fn">The function.</param>
    /// <param name="key">The key.</param>
    /// <param name="precedence">The operator precedence.</param>
    /// <exception cref="ArgumentNullException" />
    public void BindOperator<T>(Func<T, T, T> fn, char key, int precedence = (int)EvalPrecedence.Basic)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathOperator<T>(key.ToString(), fn, precedence));

    /// <inheritdoc cref="BindOperator{T}(Func{T, T, T}, char, int)" />
    public void BindOperator<T>(Func<T, T, T> fn, string key, int precedence = (int)EvalPrecedence.Basic)
        where T : struct, INumberBase<T>
        => _trie.AddMathEntity(new MathOperator<T>(key, fn, precedence));

    #endregion

    #region boolean

    /// <inheritdoc cref="BindConstant{T}(T, char)" />
    public void BindConstant(bool value, char key)
        => _trie.AddMathEntity(new MathConstant<double>(key.ToString(), Convert.ToDouble(value)));

    /// <inheritdoc cref="BindConstant{T}(T, string?)" />
    public void BindConstant(bool value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathConstant<double>(key, Convert.ToDouble(value)));

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
    /// Handles the binding logic for a key-value pair.
    /// </summary>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="NotSupportedException"></exception>
    private void BindKeyValue(Type propertyType, string key, object? value)
    {
        if (value == null)
            throw new NotSupportedException($"Cannot bind a variable to a null value for '{key}'.");

        if (propertyType.IsBooleanType())
        {
            BindConstant(Convert.ToDouble((bool)value), key);
            return;
        }

        if (TryBind<double>(key, value))
            return;
        if (TryBind<decimal>(key, value))
            return;
        if (TryBind<Complex>(key, value))
            return;
        if (TryBind<float>(key, value))
            return;
        if (TryBind<int>(key, value))
            return;
        if (TryBind<Half>(key, value))
            return;
        if (TryBind<BigInteger>(key, value))
            return;
        if (TryBind<char>(key, value))
            return;
        if (TryBind<byte>(key, value))
            return;
        if (TryBind<sbyte>(key, value))
            return;
        if (TryBind<short>(key, value))
            return;
        if (TryBind<ushort>(key, value))
            return;
        if (TryBind<uint>(key, value))
            return;
        if (TryBind<long>(key, value))
            return;
        if (TryBind<ulong>(key, value))
            return;
        if (TryBind<nint>(key, value))
            return;
        if (TryBind<nuint>(key, value))
            return;
        if (TryBind<Int128>(key, value))
            return;
        if (TryBind<UInt128>(key, value))
            return;

        if (propertyType.IsNumberBaseType())
        {
            BindConstant((dynamic)value, key);
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

    private bool TryBind<T>(string key, object value)
        where T : struct, INumberBase<T>
    {
        switch (value)
        {
            case T variableValue:
                BindConstant(variableValue, key);
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