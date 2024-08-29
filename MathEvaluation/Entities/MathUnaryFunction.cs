﻿using System;

namespace MathEvaluation.Entities;

/// <summary>
/// The function with one parameter, so closing symbol is optional.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathUnaryFunction<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T> Fn { get; }

    /// <summary>Gets the closing symbol.</summary>
    /// <value>The closing symbol.</value>
    public char? ClosingSymbol { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Initializes a new instance of the <see cref="MathUnaryFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException">fn</exception>
    public MathUnaryFunction(string? key, Func<T, T> fn, char? closingSymbol = null)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        ClosingSymbol = closingSymbol;
    }
}