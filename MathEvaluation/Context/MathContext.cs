﻿using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Context;

/// <summary>
///     The base implementation of the <see cref="IMathContext" />.
///     It uses a prefix tree, also known as a trie (pronounced "try"),
///     for efficient searching of the constants, operators, and functions by their keys (names).
///     Performance is improved by using <see cref="ReadOnlySpan{T}" /> for comparing strings.
///     For more details, refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation" />.
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
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="NotSupportedException" />
    public MathContext(object context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        Bind(context);
    }

    /// <inheritdoc />
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
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsConvertibleToDouble())
            {
                if (propertyType.IsDecimal())
                    BindConstant((decimal)value, key);
                else
                    BindConstant(Convert.ToDouble(value), key);
            }
            else switch (value)
                {
                    case Complex c:
                        BindConstant(c, key);
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
                                throw new NotSupportedException($"{propertyType} isn't supported, you can use Func<T[], T> instead.");

                            throw new NotSupportedException($"{propertyType} isn't supported.");
                        }
                }
        }
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="NotSupportedException" />
    public void BindConstant<T>(T value, char key)
        where T : struct
        => BindConstant(value, key.ToString());

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="NotSupportedException" />
    public void BindConstant<T>(T value, [CallerArgumentExpression(nameof(value))] string? key = null)
        where T : struct
    {
        var type = typeof(T);
        if (type.IsConvertibleToDouble())
            BindConstant(Convert.ToDouble(value), key);
        else
            throw new NotSupportedException($"{type} isn't supported.");
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindConstant(double value, char key)
        => _trie.AddMathEntity(new MathConstant<double>(key.ToString(), value));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindConstant(double value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathConstant<double>(key, value));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key, fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double> fn, char key)
        => _trie.AddMathEntity(new MathUnaryFunction<double>(key.ToString(), fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double> fn, char openingSymbol, char closingSymbol)
        => _trie.AddMathEntity(new MathUnaryFunction<double>(openingSymbol.ToString(), fn, null, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        => _trie.AddMathEntity(new MathUnaryFunction<double>(key, fn, openingSymbol, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => _trie.AddMathEntity(new MathFunction<double>(key, fn, openingSymbol, separator, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandOperator(Func<double, double> fn, char key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<double>(key.ToString(), fn, isProcessingLeft));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandOperator(Func<double, double> fn, string key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<double>(key, fn, isProcessingLeft));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandsOperator(Func<double, double, double> fn, char key, int precedence)
        => _trie.AddMathEntity(new MathOperandsOperator<double>(key.ToString(), fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandsOperator(Func<double, double, double> fn, string key, int precedence)
        => _trie.AddMathEntity(new MathOperandsOperator<double>(key, fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(Func<double, double, double> fn, char key, int precedence = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<double>(key.ToString(), fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(Func<double, double, double> fn, string key, int precedence = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<double>(key, fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(char key, OperatorType operatorType)
        => _trie.AddMathEntity(new MathCompatibleOperator(key.ToString(), operatorType));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(string key, OperatorType operatorType)
        => _trie.AddMathEntity(new MathCompatibleOperator(key, operatorType));

    #region decimal

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindConstant(decimal value, char key)
        => _trie.AddMathEntity(new MathConstant<decimal>(key.ToString(), value));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindConstant(decimal value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathConstant<decimal>(key, value));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<decimal>(key.ToString(), fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<decimal>(key, fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal, decimal> fn, char key)
        => _trie.AddMathEntity(new MathUnaryFunction<decimal>(key.ToString(), fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal, decimal> fn, char openingSymbol, char closingSymbol)
        => _trie.AddMathEntity(new MathUnaryFunction<decimal>(openingSymbol.ToString(), fn, null, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        => _trie.AddMathEntity(new MathUnaryFunction<decimal>(key, fn, openingSymbol, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal, decimal, decimal, decimal, decimal, decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<decimal[], decimal> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => _trie.AddMathEntity(new MathFunction<decimal>(key, fn, openingSymbol, separator, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandOperator(Func<decimal, decimal> fn, char key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<decimal>(key.ToString(), fn, isProcessingLeft));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandOperator(Func<decimal, decimal> fn, string key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<decimal>(key, fn, isProcessingLeft));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandsOperator(Func<decimal, decimal, decimal> fn, char key, int precedence)
        => _trie.AddMathEntity(new MathOperandsOperator<decimal>(key.ToString(), fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandsOperator(Func<decimal, decimal, decimal> fn, string key, int precedence)
        => _trie.AddMathEntity(new MathOperandsOperator<decimal>(key, fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(Func<decimal, decimal, decimal> fn, char key, int precedence = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<decimal>(key.ToString(), fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(Func<decimal, decimal, decimal> fn, string key, int precedence = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<decimal>(key, fn, precedence));

    #endregion

    #region boolean

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindConstant(bool value, char key)
        => _trie.AddMathEntity(new MathConstant<double>(key.ToString(), Convert.ToDouble(value)));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindConstant(bool value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathConstant<double>(key, Convert.ToDouble(value)));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<bool> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key.ToString(), () => Convert.ToDouble(fn())));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<bool> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<double>(key, () => Convert.ToDouble(fn())));

    #endregion

    #region complex

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindConstant(Complex value, char key)
        => _trie.AddMathEntity(new MathConstant<Complex>(key.ToString(), value));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindConstant(Complex value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _trie.AddMathEntity(new MathConstant<Complex>(key, value));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex> fn, char key)
        => _trie.AddMathEntity(new MathGetValueFunction<Complex>(key.ToString(), fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _trie.AddMathEntity(new MathGetValueFunction<Complex>(key, fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex, Complex> fn, char key)
        => _trie.AddMathEntity(new MathUnaryFunction<Complex>(key.ToString(), fn));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex, Complex> fn, char openingSymbol, char closingSymbol)
        => _trie.AddMathEntity(new MathUnaryFunction<Complex>(openingSymbol.ToString(), fn, null, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char? openingSymbol = null, char? closingSymbol = null)
        => _trie.AddMathEntity(new MathUnaryFunction<Complex>(key, fn, openingSymbol, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex, Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex, Complex, Complex, Complex, Complex, Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => BindFunction(args => fn(args[0], args[1], args[2], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindFunction(Func<Complex[], Complex> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = Constants.DefaultOpeningSymbol, char separator = Constants.DefaultParamsSeparator,
        char closingSymbol = Constants.DefaultClosingSymbol)
        => _trie.AddMathEntity(new MathFunction<Complex>(key, fn, openingSymbol, separator, closingSymbol));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandOperator(Func<Complex, Complex> fn, char key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<Complex>(key.ToString(), fn, isProcessingLeft));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandOperator(Func<Complex, Complex> fn, string key, bool isProcessingLeft = false)
        => _trie.AddMathEntity(new MathOperandOperator<Complex>(key, fn, isProcessingLeft));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandsOperator(Func<Complex, Complex, Complex> fn, char key, int precedence)
        => _trie.AddMathEntity(new MathOperandsOperator<Complex>(key.ToString(), fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperandsOperator(Func<Complex, Complex, Complex> fn, string key, int precedence)
        => _trie.AddMathEntity(new MathOperandsOperator<Complex>(key, fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(Func<Complex, Complex, Complex> fn, char key, int precedence = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<Complex>(key.ToString(), fn, precedence));

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException" />
    public void BindOperator(Func<Complex, Complex, Complex> fn, string key, int precedence = (int)EvalPrecedence.Basic)
        => _trie.AddMathEntity(new MathOperator<Complex>(key, fn, precedence));

    #endregion

    /// <inheritdoc />
    IMathEntity? IMathContext.FirstMathEntity(ReadOnlySpan<char> expression)
        => _trie.FirstMathEntity(expression);
}