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
    private const string Expression = "sin(a) * arctan(4i)/(1 - 6i)";

    private int _count;

    private readonly MathContext _mathContext = new ComplexScientificMathContext();
    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private readonly Func<SinArg, Complex> _mathEvalCompiledFn;
    private readonly Func<SinArg, Complex> _mathEvalFastCompiledFn;

    public ComplexNumbersBenchmarks()
    {
        _mathEvalCompiledFn = MathEvaluator_Compile();
        _mathEvalFastCompiledFn = MathEvaluator_FastExpressionCompiler_Compile();
    }

    [Benchmark(Description = $"MathEvaluator evaluation: {Expression}")]
    public Complex MathEvaluator_Evaluate()
    {
        _count++;
        var r = _count % 3; //randomizing values
        var i = r - 1;

        var parameters = new MathParameters();
        parameters.BindVariable(new Complex(r, i), "a");

        return Expression.Evaluate<Complex>(parameters, _mathContext);
    }

    [Benchmark(Description = $"MathEvaluator compilation: {Expression}")]
    public Func<SinArg, Complex> MathEvaluator_Compile()
    {
        return Expression.CompileComplex(new SinArg(Complex.Zero), _mathContext, CultureInfo.InvariantCulture);
    }

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler compilation: {Expression}")]
    public Func<SinArg, Complex> MathEvaluator_FastExpressionCompiler_Compile()
    {
        return new MathExpression(Expression, _mathContext, CultureInfo.InvariantCulture, _fastCompiler)
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