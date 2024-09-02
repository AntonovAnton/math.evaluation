using System;
using System.Collections.Generic;
using MathEvaluation.Context;

namespace MathEvaluation;

public partial class MathEvaluator
{
    /// <inheritdoc cref="Evaluate(IMathParameters?, IFormatProvider?)"/>
    public bool EvaluateBoolean(IMathParameters? parameters = null, IFormatProvider? provider = null)
        => EvaluateBoolean(MathString.AsSpan(), Context, parameters, provider);

    /// <inheritdoc cref="Evaluate(string, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(string mathString,
        IMathParameters? parameters = null, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString.AsSpan(), null, parameters, provider);

    /// <inheritdoc cref="Evaluate(string, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString,
        IMathParameters? parameters = null, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, null, parameters, provider);

    /// <inheritdoc cref="Evaluate(string, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(string mathString,
        IMathContext? context, IMathParameters? parameters = null, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString.AsSpan(), context, parameters, provider);

    /// <inheritdoc cref="Evaluate(string, IMathContext?, IMathParameters?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString,
        IMathContext? context, IMathParameters? parameters = null, IFormatProvider? provider = null)
        => Evaluate(mathString, context, parameters, provider) != default;

    #region Evaluate(object parameters)

    /// <inheritdoc cref="Evaluate(object, IFormatProvider?)"/>
    public bool EvaluateBoolean(object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(MathString.AsSpan(), Context, new MathParameters(parameters), provider);

    /// <inheritdoc cref="Evaluate(string, object, IFormatProvider?)"/>
    public bool EvaluateBoolean(string mathString, object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString.AsSpan(), null, new MathParameters(parameters), provider);

    /// <inheritdoc cref="Evaluate(string, object, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> mathString, object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, null, new MathParameters(parameters), provider);

    /// <inheritdoc cref="Evaluate(string, IMathContext?, object, IFormatProvider?)"/>
    public static bool EvaluateBoolean(string mathString,
        IMathContext? context, object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString.AsSpan(), context, new MathParameters(parameters), provider);

    /// <inheritdoc cref="Evaluate(string, IMathContext?, object, IFormatProvider?)"/>
    public static bool EvaluateBoolean(IReadOnlyList<char> mathString,
        IMathContext? context, object parameters, IFormatProvider? provider = null)
        => EvaluateBoolean(mathString, context, new MathParameters(parameters), provider);

    #endregion
}