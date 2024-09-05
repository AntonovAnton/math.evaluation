using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
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
    private readonly dynamic _fn;

    public Benchmarks()
    {
        _fn = "sin(a) + cos(b)".Compile(new { a, b }, _scientificContext);
    }

    [Benchmark(Description = "\"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\".Evaluate()")]
    public double MathEvaluator_Evaluate_ComplexExpression()
    {
        return "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".Evaluate();
    }

    [Benchmark(Description = "\"sin(pi/6) + cos(pi/3)\".Evaluate(_scientificContext)")]
    public double MathEvaluator_EvaluateSinCos_ComplexExpression()
    {
        return "sin(pi/6) + cos(pi/3)".Evaluate(_scientificContext);
    }

    [Benchmark(Description = "\"sin(a) + cos(b)\".Evaluate(_scientificContext, new { a, b })")]
    public double MathEvaluator_EvaluateSinCos_HasVariables_ComplexExpression()
    {
        return "sin(a) + cos(b)".Evaluate(_scientificContext, new { a, b });
    }

    [Benchmark(Description = "\"sin(a) + cos(b)\".Compile(new { a, b }, _scientificContext)")]
    public void MathExpression_CompileSinCos_HasVariables_ComplexExpression()
    {
        "sin(a) + cos(b)".Compile(new { a, b }, _scientificContext);
    }

    [Benchmark(Description = "compiled sin(a) + cos(b): fn(new { a, b })")]
    public double MathExpression_InvokeSinCos_HasVariables_ComplexExpression()
    {
        return _fn(new { a, b });
    }
}