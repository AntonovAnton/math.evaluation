using System;

namespace MathEvaluation.Context;

internal abstract class MathEntity : IMathEntity
{
    public string Key { get; }

    protected MathEntity(string? key)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
    }
}
