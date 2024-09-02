using System;

namespace MathEvaluation.Entities;

/// <summary>
/// Base class for a math entity.
/// </summary>
public abstract class MathEntity : IMathEntity
{
    /// <inheritdoc/>
    public string Key { get; }

    /// <inheritdoc/>
    public abstract int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathEntity" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException"/>
    protected MathEntity(string? key)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
    }
}
