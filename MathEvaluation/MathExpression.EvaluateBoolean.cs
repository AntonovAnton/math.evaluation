using MathEvaluation.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
    public bool EvaluateBoolean(object parameters)
        => EvaluateBoolean(new MathParameters(parameters));

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public bool EvaluateBoolean(IDictionary<string, bool> parameters)
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return EvaluateBoolean(mathParameters);
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public bool EvaluateBoolean(IDictionary<string, object> parameters)
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return EvaluateBoolean(mathParameters);
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public bool EvaluateBoolean(MathParameters? parameters = null)
        => Convert.ToBoolean(Evaluate(parameters));
}