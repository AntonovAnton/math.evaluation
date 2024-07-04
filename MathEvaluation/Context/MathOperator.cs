using System;

namespace MathEvaluation.Context;

internal class MathOperator<T> : MathEntity
    where T : struct
{
    public Func<T, T, T> Fn { get; }

    public MathOperator(string? key, Func<T, T, T> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }
}
