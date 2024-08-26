namespace MathEvaluation.Entities;

/// <summary>
/// Math entity interface.
/// </summary>
public interface IMathEntity
{
    /// <summary>Gets the key.</summary>
    /// <value>The key (name, notation, or symbol).</value>
    string Key { get; }

    /// <summary>
    /// Gets the evaluation precedence.
    /// </summary>
    /// <value>
    /// The evaluation precedence.
    /// </value>
    public int Precedence { get; }
}
