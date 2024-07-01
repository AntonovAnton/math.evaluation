using System;

namespace MathEvaluation.Context;

internal class MathOperator : IMathEntity
{
    public string Key { get; }

    public Func<double, double, double> Fn { get; }

    public MathOperator(string? key, Func<double, double, double> fn)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        Key = key;
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }
}
