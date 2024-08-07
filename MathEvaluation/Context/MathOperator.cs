﻿using System;

namespace MathEvaluation.Context;

/// <summary>
/// The math operator processes the left and right math operands.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperator<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T, T> Fn { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="fn">The function.</param>
    /// <exception cref="System.ArgumentNullException">fn</exception>
    public MathOperator(string? key, Func<T, T, T> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }
}
