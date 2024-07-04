using System;

namespace MathEvaluation.Context;

internal class MathVariableFunction<T> : MathEntity
    where T : struct
{
    public Func<T> GetValue { get; }

    public MathVariableFunction(string? key, Func<T> getValue)
        : base(key)
    {
        GetValue = getValue ?? throw new ArgumentNullException(nameof(getValue));
    }
}
