using System;

namespace MathEvaluation.Context;

internal class MathFunction : MathOperand
{
    public Func<double, double> Fn { get; }

    public char? Separator { get; }

    public MathFunction(string? key, Func<double, double> fn, char? separator = null)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Separator= separator;
    }
}
