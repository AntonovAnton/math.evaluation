using System;
using MathEvaluation.Context;

namespace MathEvaluation;

public partial class MathEvaluator
{
    /// <inheritdoc cref="Evaluate(IFormatProvider?)"/>
    public bool EvaluateBoolean(IFormatProvider? provider = null)
    {
        return EvaluateBoolean(Expression.AsSpan(), Context, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IFormatProvider?)"/>
    public static bool EvaluateBoolean(string expression, IFormatProvider? provider = null)
    {
        return EvaluateBoolean(expression.AsSpan(), null, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> expression, IFormatProvider? provider = null)
    {
        return EvaluateBoolean(expression, null, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IMathContext?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(string expression, IMathContext? context, IFormatProvider? provider = null)
    {
        return EvaluateBoolean(expression.AsSpan(), context, provider);
    }

    /// <inheritdoc cref="Evaluate(string, IMathContext?, IFormatProvider?)"/>
    public static bool EvaluateBoolean(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider? provider = null)
    {
        return Evaluate(expression, context, provider) != default;
    }
}