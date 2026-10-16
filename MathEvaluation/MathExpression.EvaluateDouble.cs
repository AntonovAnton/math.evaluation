using MathEvaluation.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public double Evaluate() => Evaluate<double>();

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    /// <exception cref="NotSupportedException">parameters</exception>
    [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
    public double Evaluate(object parameters) => Evaluate<double>(new MathParameters(parameters));

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public double Evaluate(IDictionary<string, double> parameters)
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return Evaluate<double>(mathParameters);
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public double Evaluate(IDictionary<string, object> parameters)
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return Evaluate<double>(mathParameters);
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public double Evaluate(MathParameters? parameters) => Evaluate<double>(parameters);
}