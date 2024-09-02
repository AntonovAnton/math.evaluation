using System;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator that performs an action on one math operand. 
/// For example, degrees, factorial, decrement, increment, or negation.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperandOperator<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.OperandUnaryOperator;

    /// <summary>
    /// Gets a value indicating whether this instance is processing left operand.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is processing left operand; otherwise, <c>false</c>.
    /// </value>
    public bool IsProcessingLeft { get; } = false;

    /// <summary>
    /// Gets a value indicating whether this instance is processing right operand.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is processing right operand; otherwise, <c>false</c>.
    /// </value>
    public bool IsProcessingRight => !IsProcessingLeft;

    /// <summary>Initializes a new instance of the <see cref="MathOperandOperator{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="isProcessingLeft">
    ///   <c>true</c> if this instance is processing left operand; otherwise, <c>false</c>.
    /// </param>
    /// <exception cref="ArgumentNullException"/>
    public MathOperandOperator(string? key, Func<T, T> fn, bool isProcessingLeft = false)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        IsProcessingLeft = isProcessingLeft;
    }
}
