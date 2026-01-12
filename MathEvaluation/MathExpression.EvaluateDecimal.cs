using MathEvaluation.Parameters;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(object?)" />
    public decimal EvaluateDecimal(object? parameters = null)
        => EvaluateDecimal(parameters != null ? new MathParameters(parameters) : null);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public decimal EvaluateDecimal(MathParameters? parameters)
        => Evaluate<decimal>(parameters);
}