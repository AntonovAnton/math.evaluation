using System;

namespace MathEvaluation.Context;

/// <summary>
/// The math function converts the value of the math operand. 
/// For example, degrees or factorial.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperandConverter<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Convertation;

    /// <summary>
    /// Gets a value indicating whether this instance is converting left operand.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is converting left operand; otherwise, <c>false</c>.
    /// </value>
    public bool IsConvertingLeftOperand { get; } = false;

    /// <summary>
    /// Gets a value indicating whether this instance is converting right operand.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is converting right operand; otherwise, <c>false</c>.
    /// </value>
    public bool IsConvertingRightOperand => !IsConvertingLeftOperand;

    /// <summary>Initializes a new instance of the <see cref="MathOperandConverter{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="isConvertingLeftOperand">
    ///   <c>true</c> if this instance is converting left operand; otherwise, <c>false</c>.
    /// </param>
    /// <exception cref="System.ArgumentNullException">fn</exception>
    public MathOperandConverter(string? key, Func<T, T> fn, bool isConvertingLeftOperand = false)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        IsConvertingLeftOperand  = isConvertingLeftOperand;
    }
}
