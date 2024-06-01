using System;

namespace MathEvaluation;

public static class StringExtensions
{
    public static double Evaluate(this string str, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(str, provider);

    public static double Evaluate(this ReadOnlySpan<char> span, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(span, provider);
}