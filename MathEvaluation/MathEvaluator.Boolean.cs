using System;
using MathEvaluation.Context;

namespace MathEvaluation;

public partial class MathEvaluator
{
    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    public bool EvaluateBoolean(IFormatProvider? provider = null)
        => EvaluateBoolean(MathString, Context, null, provider);

    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    public bool EvaluateBoolean(IMathParameters parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(MathString, Context, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, null, null, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString, IMathParameters parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, null, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString, IMathContext context, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, context, null, provider);

    #region object parameters

    /// <inheritdoc cref="Evaluate(IMathParameters, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public bool EvaluateBoolean(object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(MathString, Context, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString, object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, null, parameters, provider);

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString, IMathContext? context, object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, context, new MathParameters(parameters), provider);

    #endregion

    /// <inheritdoc cref="Evaluate(ReadOnlySpan{char}, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters, IFormatProvider? provider = null)
        => Evaluate(mathString, context, parameters, provider) != default;
}