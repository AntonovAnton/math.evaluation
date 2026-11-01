using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
public class ComplexNumbersBenchmarks
{
    private const string ExpressionString = "sin(a) * arctan(4i)/(1 - 6i)";

    private int _count;

    private readonly MathContext _mathContext = new ComplexScientificMathContext();
    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private MathExpression _mathExpressionForEvaluate;
    private MathExpression _mathExpressionForFastCompile;

    private Func<SinArg, Complex> _mathEvalCompiledFn;
    private Func<SinArg, Complex> _mathEvalFastCompiledFn;

    [GlobalSetup]
    public void Setup()
    {
        _mathExpressionForEvaluate = new MathExpression(ExpressionString, _mathContext, CultureInfo.InvariantCulture);
        _mathExpressionForFastCompile = new MathExpression(ExpressionString, _mathContext, CultureInfo.InvariantCulture, _fastCompiler);

        var dummyArg = new SinArg(Complex.Zero);

        _mathEvalCompiledFn = ExpressionString.CompileComplex(dummyArg, _mathContext, CultureInfo.InvariantCulture);
        _mathEvalFastCompiledFn = _mathExpressionForFastCompile.CompileComplex(dummyArg);
    }

    [Benchmark(Description = $"MathEvaluator evaluation: {ExpressionString}")]
    public Complex MathEvaluator_Evaluate()
    {
        Interlocked.Increment(ref _count);
        var r = _count % 3; //randomizing values
        var i = r - 1;

        var parameters = new MathParameters();
        parameters.BindVariable(new Complex(r, i), "a");

        return _mathExpressionForEvaluate.Evaluate<Complex>(parameters);
    }

    [Benchmark(Description = $"MathEvaluator compilation: {ExpressionString}")]
    public Func<SinArg, Complex> MathEvaluator_Compile()
    {
        return ExpressionString.CompileComplex(new SinArg(Complex.Zero), _mathContext, CultureInfo.InvariantCulture);
    }

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler compilation: {ExpressionString}")]
    public Func<SinArg, Complex> MathEvaluator_FastExpressionCompiler_Compile()
    {
        return _mathExpressionForFastCompile.CompileComplex(new SinArg(Complex.Zero));
    }

    [Benchmark(Description = "MathEvaluator invoke fn(a)")]
    public Complex MathEvaluator_InvokeCompiled()
    {
        Interlocked.Increment(ref _count);
        var r = _count % 3; //randomizing values
        var i = r - 1;

        return _mathEvalCompiledFn(new SinArg(new Complex(r, i)));
    }

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler invoke fn(a)")]
    public Complex MathEvaluator_FastExpressionCompiler_InvokeCompiled()
    {
        Interlocked.Increment(ref _count);
        var r = _count % 3; //randomizing values
        var i = r - 1;

        return _mathEvalFastCompiledFn(new SinArg(new Complex(r, i)));
    }

#pragma warning disable IDE1006 // Naming Styles
    // ReSharper disable once InconsistentNaming
    public record struct SinArg(Complex a);
#pragma warning restore IDE1006 // Naming Styles
}