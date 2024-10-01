using System;
using System.Linq.Expressions;

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

    /// <inheritdoc/>
    public abstract double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value);

    /// <inheritdoc/>
    public abstract decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value);

    /// <inheritdoc/>
    public abstract Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
        where TResult : struct, IConvertible;

    /// <summary> Converts to string. </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
        => $"{nameof(Key)}: \"{Key}\", {nameof(Precedence)}: {Precedence}";
}
