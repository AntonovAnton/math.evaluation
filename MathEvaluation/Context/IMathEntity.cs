namespace MathEvaluation.Context;

/// <summary>
/// Math entity interface.
/// </summary>
public interface IMathEntity
{
    /// <summary>Gets the key.</summary>
    /// <value>The key (name, notation, or symbol).</value>
    string Key { get; }
}
