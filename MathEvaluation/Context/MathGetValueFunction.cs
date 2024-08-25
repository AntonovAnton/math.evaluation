﻿using System;

namespace MathEvaluation.Context;

/// <summary>
/// The getting value function.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathGetValueFunction<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the getting value function.</summary>
    /// <value>The getting value function.</value>
    public Func<T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Variable;

    /// <summary>Initializes a new instance of the <see cref="MathGetValueFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The getting value function.</param>
    /// <exception cref="System.ArgumentNullException">fn</exception>
    public MathGetValueFunction(string? key, Func<T> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }
}
