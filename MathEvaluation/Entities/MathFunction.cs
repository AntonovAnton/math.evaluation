﻿using MathEvaluation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The function with multiple parameters, so opening, separator, and closing symbol are required.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathFunction<T> : MathEntity
    where T : struct, IConvertible
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T[], T> Fn { get; }

    /// <summary>Gets the opening symbol.</summary>
    /// <value>The opening symbol.</value>
    public char OpeningSymbol { get; }

    /// <summary>Gets the parameters separator.</summary>
    /// <value>The parameter separator.</value>
    public char Separator { get; }

    /// <summary>Gets the closing symbol.</summary>
    /// <value>The closing symbol.</value>
    public char ClosingSymbol { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Initializes a new instance of the <see cref="MathFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathFunction(string? key, Func<T[], T> fn, char openingSymbol, char separator, char closingSymbol)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Separator = separator;
        OpeningSymbol = openingSymbol;
        ClosingSymbol = closingSymbol;
    }

    /// <inheritdoc/>
    public override double Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, ref i, separator, closingSymbol, (decimal)value);

        var start = i;
        i += Key.Length;
        mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol, start, ref i);

        var args = new List<T>();
        while (mathExpression.MathString.Length > i)
        {
            var arg = mathExpression.Evaluate(ref i, Separator, ClosingSymbol);
            args.Add(arg is T a ? a : (T)Convert.ChangeType(arg, typeof(T)));

            if (mathExpression.MathString[i] == Separator)
            {
                i++; //other param
                continue;
            }
            break;
        }
        mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol, start, ref i);

        var result = Convert.ToDouble(Fn([.. args]));
        result = mathExpression.EvaluateExponentiation(ref i, separator, closingSymbol, result);
        return value == default ? result : value * result;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, ref i, separator, closingSymbol, (double)value);

        var start = i;
        i += Key.Length;
        mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol, start, ref i);

        var args = new List<T>();
        while (mathExpression.MathString.Length > i)
        {
            var arg = mathExpression.EvaluateDecimal(ref i, Separator, ClosingSymbol);
            args.Add(arg is T a ? a : (T)Convert.ChangeType(arg, typeof(T)));

            if (mathExpression.MathString[i] == Separator)
            {
                i++; //other param
                continue;
            }
            break;
        }
        mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol, start, ref i);

        var result = Convert.ToDecimal(Fn([.. args]));
        result = mathExpression.EvaluateExponentiationDecimal(ref i, separator, closingSymbol, result);
        return value == default ? result : value * result;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var start = i;
        i += Key.Length;
        mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol, start, ref i);

        var args = new List<Expression>();
        while (mathExpression.MathString.Length > i)
        {
            var arg = mathExpression.Build<T>(ref i, Separator, ClosingSymbol);
            args.Add(arg);

            if (mathExpression.MathString[i] == Separator)
            {
                i++; //other param
                continue;
            }
            break;
        }
        mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol, start, ref i);

        Expression right = Expression.Invoke(Expression.Constant(Fn), Expression.NewArrayInit(typeof(T), args));
        right = right.Type != typeof(TResult) ? Expression.Convert(right, typeof(TResult)) : right;
        right = mathExpression.BuildExponentiation<TResult>(ref i, separator, closingSymbol, right);
        return left.IsZero() ? right : Expression.Multiply(left, right).Reduce();
    }
}
