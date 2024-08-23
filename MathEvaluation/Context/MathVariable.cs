namespace MathEvaluation.Context;

/// <summary>
/// The math variable.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathVariable<T> : MathEntity
    where T : struct
{
    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Variable;

    /// <summary>Gets the value.</summary>
    /// <value>The value.</value>
    public T Value { get; private set; }

    /// <summary>Initializes a new instance of the <see cref="MathVariable{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public MathVariable(string? key, T value)
        : base(key)
    {
        Value = value;
    }

    /// <summary>Sets the value.</summary>
    /// <param name="value">The value.</param>
    public void SetValue(T value)
        => Value = value;
}
