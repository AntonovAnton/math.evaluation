using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System.Numerics;

namespace MathEvaluation.Benchmarks;

// short jobs: they spot regressions, the published results are in LibraryComparisonBenchmarks
[SimpleJob(RuntimeMoniker.Net10_0, warmupCount: 3, iterationCount: 10)]
[SimpleJob(RuntimeMoniker.Net11_0, warmupCount: 3, iterationCount: 10)]
[MemoryDiagnoser]
public class ComplexNumbersBenchmarks
{
    private int _count;

    private readonly MathContext _mathContext = new ComplexScientificMathContext();
    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private readonly Func<SinArg, Complex> _mathEvalCompiledFn;

    public ComplexNumbersBenchmarks()
    {
        _mathEvalCompiledFn = MathEvaluator_Compile();
    }

    [Benchmark(Description = "MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)")]
    public Complex MathEvaluator_Evaluate()
    {
        _count++;
        var r = _count % 3; //randomizing values
        var i = r - 1;

        var parameters = new MathParameters();
        parameters.BindVariable(new Complex(r, i), "a");

        return "sin(a) * arctan(4i)/(1 - 6i)".Evaluate<Complex>(parameters, _mathContext);
    }

    [Benchmark(Description = "MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)")]
    public Func<SinArg, Complex> MathEvaluator_Compile()
    {
        return "sin(a) * arctan(4i)/(1 - 6i)".CompileComplex(new SinArg(Complex.Zero), _mathContext);
    }

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)")]
    public Func<SinArg, Complex> MathEvaluator_FastExpressionCompiler_Compile()
    {
        return new MathExpression("sin(a) * arctan(4i)/(1 - 6i)", _mathContext, null, _fastCompiler)
            .CompileComplex(new SinArg(Complex.Zero));
    }

    [Benchmark(Description = "MathEvaluator invoke fn(a)")]
    public Complex MathEvaluator_InvokeCompiled()
    {
        _count++;
        var r = _count % 3; //randomizing values
        var i = r - 1;

        return _mathEvalCompiledFn(new SinArg(new Complex(r, i)));
    }


#pragma warning disable IDE1006 // Naming Styles
    // ReSharper disable once InconsistentNaming
    public record SinArg(Complex a);
#pragma warning restore IDE1006 // Naming Styles
}