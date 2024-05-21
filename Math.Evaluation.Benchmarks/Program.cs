using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Math.Evaluation;
using Microsoft.CodeAnalysis.CSharp.Scripting;

BenchmarkRunner.Run<Benchmarks>();

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net80)]
[RPlotExporter]
[MemoryDiagnoser(true)]
public class Benchmarks
{
    private readonly MathEvaluator _mathEvaluator;

    public Benchmarks()
    {
        _mathEvaluator = new MathEvaluator();
    }

    [Benchmark(Description = "MathEvaluator.Evaluate(\"22888.32 * 30 / 323.34 / .5 - - 1 / (2 + 22888.32) * 4 - 6\")")]
    public double MathEvaluator_Evaluate_ComplexExpression()
        => _mathEvaluator.Evaluate("22888.32 * 30 / 323.34 / .5 - - 1 / (2 + 22888.32) * 4 - 6");


    [Benchmark(Description = "22888.32 * 30 / 323.34 / .5 - - 1 / (2 + 22888.32) * 4 - 6)")]
    public double CSharp_ComplexExpression()
        => _mathEvaluator.Evaluate("22888.32 * 30 / 323.34 / .5 - - 1 / (2 + 22888.32) * 4 - 6");


    [Benchmark(Description = "CSharpScript.EvaluateAsync<double>(\"22888.32 * 30 / 323.34 / .5 - - 1 / (2 + 22888.32) * 4 - 6\")")]
    public Task<double> RoslynEvaluator_Evaluate_ComplexExpression()
        => CSharpScript.EvaluateAsync<double>("22888.32 * 30 / 323.34 / .5 - - 1 / (2 + 22888.32) * 4 - 6");
}
