using System;

namespace MathEvaluation.Context;

internal abstract class MathOperand : IMathEntity
{
    public string Key { get; }

    protected MathOperand(string? key)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
    }
}
