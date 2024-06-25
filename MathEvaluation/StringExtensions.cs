using System;
using System.Runtime.CompilerServices;
using MathEvaluation.Context;

namespace MathEvaluation;

public static class StringExtensions
{
    public static MathEvaluator Bind(this string expression, object variables)
    {
        var mathContext = new MathContext();
        mathContext.Bind(variables);

        return new MathEvaluator(expression)
        {
            Context = mathContext
        };
    }

    public static MathEvaluator BindVariable(this string expression, double value, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        var mathContext = new MathContext();
        mathContext.BindVariable(value, name);

        return new MathEvaluator(expression)
        {
            Context = mathContext
        };
    }

    public static double Evaluate(this string expression, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(expression, provider);

    public static double Evaluate(this ReadOnlySpan<char> span, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(span, provider);
}