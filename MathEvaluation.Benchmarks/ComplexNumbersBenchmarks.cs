using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System.Numerics;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
#pragma warning disable CA1050 // Declare types in namespaces
public class ComplexNumbersBenchmarks
#pragma warning restore CA1050 // Declare types in namespaces
{
    private int _count;

    private readonly IMathContext _mathContext = new ComplexScientificMathContext();
    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private readonly Func<SinArg, Complex> _mathEvalCompiledFn;
    private readonly Func<SinArg, Complex> _mathEvalFastCompiledFn;

    public ComplexNumbersBenchmarks()
    {
        _mathEvalCompiledFn = MathEvaluator_Compile();
        _mathEvalFastCompiledFn = MathEvaluator_FastExpressionCompiler_Compile();
    }

    [Benchmark(Description = "MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)")]
    public Complex MathEvaluator_Evaluate()
    {
        _count++;
        var r = _count % 3; //randomizing values
        var i = r - 1;

        var parameters = new MathParameters();
        parameters.BindVariable(new Complex(r, i), "a");

        return "sin(a) * arctan(4i)/(1 - 6i)".EvaluateComplex(parameters, _mathContext);
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

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler invoke fn(a)")]
    public Complex MathEvaluator_FastExpressionCompiler_InvokeCompiled()
    {
        _count++;
        var r = _count % 3; //randomizing values
        var i = r - 1;

        return _mathEvalFastCompiledFn(new SinArg(new Complex(r, i)));
    }

#pragma warning disable IDE1006 // Naming Styles
    // ReSharper disable once InconsistentNaming
    public record SinArg(Complex a);
#pragma warning restore IDE1006 // Naming Styles
}