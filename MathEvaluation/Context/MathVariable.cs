namespace MathEvaluation.Context;

/// <summary>
/// The math variable.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathVariable<T>(string? key, T value) : MathEntity(key)
    where T : struct
{
    /// <summary>Gets the value.</summary>
    /// <value>The value.</value>
    public T Value { get; } = value;
}
