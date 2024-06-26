using System.Runtime.CompilerServices;
using MathEvaluation.Context;

namespace MathEvaluation;

public static class MathEvaluatorExtensions
{
    public static MathEvaluator Bind(this MathEvaluator evaluator, object variables)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.Bind(variables);
        return evaluator;
    }

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, double value,
        [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(value, name);
        return evaluator;
    }
}