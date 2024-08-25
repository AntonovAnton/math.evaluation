﻿using System;

namespace MathEvaluation.Context;

/// <summary>
/// The math operator processes the left and right math operands.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperandsOperator<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperandsOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="fn">The function.</param>
    /// <param name="precedece">The operator precedence.</param>
    /// <exception cref="System.ArgumentNullException">fn</exception>
    public MathOperandsOperator(string? key, Func<T, T, T> fn, int precedece)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Precedence = precedece;
    }
}