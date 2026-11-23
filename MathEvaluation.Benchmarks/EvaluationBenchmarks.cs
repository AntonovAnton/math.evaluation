using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;

namespace MathEvaluation.Benchmarks;

// short jobs: they spot regressions, the published results are in LibraryComparisonBenchmarks
[SimpleJob(RuntimeMoniker.Net10_0, warmupCount: 3, iterationCount: 10)]
[SimpleJob(RuntimeMoniker.NativeAot10_0, warmupCount: 3, iterationCount: 10)]
[SimpleJob(RuntimeMoniker.Net11_0, warmupCount: 3, iterationCount: 10)]
//[SimpleJob(RuntimeMoniker.NativeAot11_0, warmupCount: 3, iterationCount: 10)]
[MemoryDiagnoser]
public class EvaluationBenchmarks
{
    // ReSharper disable once InconsistentNaming
    private const double a = Math.PI / 6;

    // ReSharper disable once InconsistentNaming
    private const double b = Math.PI / 3;

    private readonly MathContext _programmingMathContext = new ProgrammingMathContext();
    private readonly MathContext _scientificContext = new ScientificMathContext();
    private readonly MathContext _dotNetStandardMathContext = new DotNetStandardMathContext();

    private int _count;

    [Benchmark(Description = "MathEvaluator: \"Sin(pi/6) + Cos(pi/3)\"")]
    public double MathExpression_EvaluateSinCos()
        => "Sin(pi/6) + Cos(pi/3)".Evaluate(_scientificContext);

    [Benchmark(Description = "MathEvaluator: \"Sin(a) + Cos(b)\"")]
    public double MathExpression_EvaluateSinCos_HasVariables()
    {
        var parameters = new MathParameters();
        parameters.BindVariable(a);
        parameters.BindVariable(b);

        return "Sin(a) + Cos(b)".Evaluate(parameters, _scientificContext);
    }

    [Benchmark(Description = "MathEvaluator: \"A or not B and (C or B)\"")]
    public bool MathEvaluator_EvaluateBoolean()
    {
        _count++;
        var value = _count % 2 == 0; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(value, "A");
        parameters.BindVariable(!value, "B");
        parameters.BindVariable(value, "C");

        return "A or not B and (C or B)"
            .EvaluateBoolean(parameters, _programmingMathContext);
    }

    [Benchmark(Description = "MathEvaluator: \"A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01\"")]
    public bool MathEvaluator_EvaluateBoolean_DotNetStandardMathContext()
    {
        _count++;
        var value = _count % 2 == 0; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(value, "A");
        parameters.BindVariable(!value, "B");
        parameters.BindVariable(value, "C");

        return "A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01"
            .EvaluateBoolean(parameters, _dotNetStandardMathContext);
    }

}