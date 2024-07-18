using System;

namespace MathEvaluation.Context;

/// <summary>
/// The math function converts the value of the math operand on the left. 
/// For example, degrees or factorial.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperandConverter<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T> Fn { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperandConverter{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <exception cref="System.ArgumentNullException">fn</exception>
    public MathOperandConverter(string? key, Func<T, T> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }
}
