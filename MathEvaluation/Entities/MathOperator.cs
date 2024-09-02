using System;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator processes the left and right expressions.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperator<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="fn">The function.</param>
    /// <param name="precedece">The operator precedence.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathOperator(string? key, Func<T, T, T> fn, int precedece = (int)EvalPrecedence.Basic)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Precedence = precedece;
    }
}
