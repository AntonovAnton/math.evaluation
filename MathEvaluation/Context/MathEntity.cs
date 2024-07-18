using System;

namespace MathEvaluation.Context;

/// <summary>
/// Base class for a math entity.
/// </summary>
public abstract class MathEntity : IMathEntity
{
    /// <inheritdoc/>
    public string Key { get; }

    /// <summary>Initializes a new instance of the <see cref="MathEntity" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <exception cref="System.ArgumentNullException">key</exception>
    protected MathEntity(string? key)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
    }
}
