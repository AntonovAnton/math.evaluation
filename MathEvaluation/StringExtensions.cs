using System;
using System.Runtime.CompilerServices;
using MathEvaluation.Context;

namespace MathEvaluation;

public static class StringExtensions
{
    public static MathEvaluator SetContext(this string expression, IMathContext context)
    {
        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator Bind(this string expression, object variables)
    {
        var context = new MathContext();
        context.Bind(variables);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindVariable(this string expression, double value, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        var context = new MathContext();
        context.BindVariable(value, name);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static double Evaluate(this string expression, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(expression, provider);

    public static double Evaluate(this ReadOnlySpan<char> span, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(span, provider);
}