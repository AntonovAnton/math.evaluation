using MathEvaluation.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<Complex>(parameters) instead.")]
    [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
    public Complex EvaluateComplex(object parameters)
        => Evaluate<Complex>(parameters);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<Complex>(parameters) instead.")]
    public Complex EvaluateComplex(IDictionary<string, Complex> parameters)
        => Evaluate(parameters);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<Complex>(parameters) instead.")]
    public Complex EvaluateComplex(IDictionary<string, object> parameters)
        => Evaluate<Complex>(parameters);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    [Obsolete("Use Evaluate<Complex>(parameters) instead.")]
    public Complex EvaluateComplex(MathParameters? parameters = null)
        => Evaluate<Complex>(parameters);
}