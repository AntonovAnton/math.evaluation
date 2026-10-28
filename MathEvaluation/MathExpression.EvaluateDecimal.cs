using MathEvaluation.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<decimal>() instead.")]
    public decimal EvaluateDecimal() => Evaluate<decimal>();

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<decimal>(parameters) instead.")]
    [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
    public decimal EvaluateDecimal(object parameters)
        => Evaluate<decimal>(parameters);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<decimal>(parameters) instead.")]
    public decimal EvaluateDecimal(IDictionary<string, decimal> parameters)
        => Evaluate(parameters);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<decimal>(parameters) instead.")]
    public decimal EvaluateDecimal(IDictionary<string, object> parameters)
        => Evaluate<decimal>(parameters);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<decimal>(parameters) instead.")]
    public decimal EvaluateDecimal(MathParameters? parameters)
        => Evaluate<decimal>(parameters);
}