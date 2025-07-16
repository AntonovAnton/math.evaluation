using MathEvaluation.Parameters;
using System;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(object?)" />
    public bool EvaluateBoolean(object? parameters = null)
        => EvaluateBoolean(parameters != null ? new MathParameters(parameters) : null);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public bool EvaluateBoolean(MathParameters? parameters)
        => Convert.ToBoolean(Evaluate(parameters));
}