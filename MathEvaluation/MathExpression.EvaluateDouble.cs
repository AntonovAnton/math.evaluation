using MathEvaluation.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MathEvaluation;

public partial class MathExpression
{
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

    /// <summary>Evaluates the <see cref="MathString">math expression string</see>.</summary>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="MathExpressionException" />
    public double Evaluate(MathParameters? parameters = null)
        => Evaluate<double>(parameters);
}