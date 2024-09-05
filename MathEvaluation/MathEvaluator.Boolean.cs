using System;
using MathEvaluation.Context;
using MathEvaluation.Parameters;

namespace MathEvaluation;

public partial class MathEvaluator
{
    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    public bool EvaluateBoolean(IFormatProvider? provider = null)
        => EvaluateBoolean(MathString, Context, null, provider);

    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public bool EvaluateBoolean(object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(MathString, Context, new MathParameters(parameters), provider);

    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    public bool EvaluateBoolean(IMathParameters parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(MathString, Context, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString,
        IMathContext? context = null, IMathParameters? parameters = null, IFormatProvider? provider = null)
        => Evaluate(mathString, context, parameters, provider) != default;
}