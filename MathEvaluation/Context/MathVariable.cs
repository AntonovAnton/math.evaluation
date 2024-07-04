namespace MathEvaluation.Context;

internal class MathVariable<T>(string? key, T value) : MathEntity(key)
    where T : struct
{
    public T Value { get; } = value;
}
