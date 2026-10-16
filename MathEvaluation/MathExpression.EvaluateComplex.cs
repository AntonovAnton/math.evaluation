using MathEvaluation.Parameters;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public Complex EvaluateComplex() => EvaluateComplex((MathParameters?)null);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
    public Complex EvaluateComplex(object parameters)
        => EvaluateComplex(new MathParameters(parameters));

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public Complex EvaluateComplex(IDictionary<string, Complex> parameters)
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return Evaluate<Complex>(mathParameters);
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public Complex EvaluateComplex(IDictionary<string, object> parameters)
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return Evaluate<Complex>(mathParameters);
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public Complex EvaluateComplex(MathParameters? parameters)
        => Evaluate<Complex>(parameters);
}