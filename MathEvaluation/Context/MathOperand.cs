using System;

namespace MathEvaluation.Context;

internal class MathOperand<T> : IMathOperand
{
    public string Name { get; }

    public MathOperand(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
    }
}
