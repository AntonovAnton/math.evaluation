using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MathEvaluation;
using MathEvaluation.Context;
using MathEvaluation.Extensions;

Console.OutputEncoding = Encoding.UTF8;

BenchmarkRunner.Run<Benchmarks>();

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class Benchmarks
{
    private const double a = Math.PI / 6;
    private const double b = Math.PI / 3;

    private readonly IMathContext _scientificContext = new ScientificMathContext();

    [Benchmark(Description = "\"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\".Evaluate()")]
    public double MathEvaluator_Evaluate_ComplexExpression()
    {
        return "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".Evaluate();
    }

    [Benchmark(Description = "\"sin(pi/6) + cos(pi/3)\".SetContext(_scientificContext).Evaluate()")]
    public double MathEvaluator_EvaluateSinCos_ComplexExpression()
    {
        return "sin(pi/6) + cos(pi/3)".SetContext(_scientificContext).Evaluate();
    }

    [Benchmark(Description = "\"sin(a) + cos(b)\".SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()")]
    public double MathEvaluator_EvaluateSinCos_HasVariables_ComplexExpression()
    {
        return "sin(a) + cos(b)".SetContext(_scientificContext).Evaluate(new MathParameters(new { a, b }));
    }
}