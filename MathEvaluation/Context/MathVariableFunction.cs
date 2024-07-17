using System;

namespace MathEvaluation.Context;

/// <summary>
/// The getting math variable function.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathVariableFunction<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the get value function.</summary>
    /// <value>The get value function.</value>
    public Func<T> GetValue { get; }

    /// <summary>Initializes a new instance of the <see cref="MathVariableFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="getValue">The get value.</param>
    /// <exception cref="System.ArgumentNullException">getValue</exception>
    public MathVariableFunction(string? key, Func<T> getValue)
        : base(key)
    {
        GetValue = getValue ?? throw new ArgumentNullException(nameof(getValue));
    }
}
