using MathEvaluation.Parameters;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public decimal EvaluateDecimal() => EvaluateDecimal((MathParameters?)null);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
    public decimal EvaluateDecimal(object parameters)
        => EvaluateDecimal(new MathParameters(parameters));

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public decimal EvaluateDecimal(IDictionary<string, decimal> parameters)
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return Evaluate<decimal>(mathParameters);
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public decimal EvaluateDecimal(IDictionary<string, object> parameters)
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return Evaluate<decimal>(mathParameters);
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public decimal EvaluateDecimal(MathParameters? parameters)
        => Evaluate<decimal>(parameters);
}