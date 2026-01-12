using MathEvaluation.Parameters;
using System.Numerics;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(object?)" />
    public Complex EvaluateComplex(object? parameters = null)
        => EvaluateComplex(parameters != null ? new MathParameters(parameters) : null);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public Complex EvaluateComplex(MathParameters? parameters)
        => Evaluate<Complex>(parameters);
}