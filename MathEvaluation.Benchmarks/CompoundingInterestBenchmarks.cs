using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using MathEvaluation.Parameters;
using NCalc;
using NCalc.LambdaCompilation;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
public class CompoundingInterestBenchmarks
{
    private const string MathEvaluatorExpressionString = "P * (1 + r/n)^d";
    private const string NCalcExpressionString = "P * (1 + r/n)**d";
    private const int n = 365;

    private Expression NCalcExpression1 { get; set; }
    private Expression NCalcExpression2 { get; set; }

    private MathExpression MathExpression1 { get; set; }
    private MathExpression MathExpression2 { get; set; }
    private MathExpression MathExpression3 { get; set; }

    private int _count;

    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private Func<CompoundInterestFormulaParams, double> _mathEvalCompiledFn;
    private Func<CompoundInterestFormulaParams, double> _mathEvalFastCompiledFn;
    private Func<CompoundInterestFormulaParams, double> _nCalcCompiledFn;


    [GlobalSetup]
    public void Setup()
    {
        NCalcExpression1 = new Expression(NCalcExpressionString, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);
        NCalcExpression2 = new Expression(NCalcExpressionString, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);

        MathExpression1 = new MathExpression(MathEvaluatorExpressionString, new ScientificMathContext(), CultureInfo.InvariantCulture);
        MathExpression2 = new MathExpression(MathEvaluatorExpressionString, new ScientificMathContext(), CultureInfo.InvariantCulture);
        MathExpression3 = new MathExpression(MathEvaluatorExpressionString, new ScientificMathContext(), CultureInfo.InvariantCulture, _fastCompiler);

        var dummyParams = new CompoundInterestFormulaParams();
        _mathEvalCompiledFn = MathExpression2.Compile(dummyParams);
        _mathEvalFastCompiledFn = MathExpression3.Compile(dummyParams);
        _nCalcCompiledFn = NCalcExpression2.ToLambda<CompoundInterestFormulaParams, double>();

    }

    [Benchmark(Description = "MathEvaluator evaluation")]
    public double MathEvaluator_Evaluate()
    {
        Interlocked.Increment(ref _count);
        var d = _count % n + 1; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(10000, "P");
        parameters.BindVariable(0.05, "r");
        parameters.BindVariable(n, "n");
        parameters.BindVariable(d, "d");

        return MathExpression1.Evaluate(parameters);
    }

    [Benchmark(Description = "NCalc evaluation")]
    public double NCalc_Evaluate()
    {
        Interlocked.Increment(ref _count);
        var d = _count % n + 1; //randomizing values

        NCalcExpression1.Parameters["P"] = 10000;
        NCalcExpression1.Parameters["r"] = 0.05;
        NCalcExpression1.Parameters["n"] = n;
        NCalcExpression1.Parameters["d"] = d;

        return (double)NCalcExpression1.Evaluate()!;
    }

    [Benchmark(Description = "MathEvaluator compilation")]
    public Func<CompoundInterestFormulaParams, double> MathEvaluator_Compile()
        => MathExpression2.Compile(new CompoundInterestFormulaParams());

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler compilation")]
    public Func<CompoundInterestFormulaParams, double> MathEvaluator_FastExpressionCompiler_Compile()
        => MathExpression3.Compile(new CompoundInterestFormulaParams());

    [Benchmark(Description = "NCalc compilation")]
    public Func<CompoundInterestFormulaParams, double> NCalc_ToLambda()
        => NCalcExpression2.ToLambda<CompoundInterestFormulaParams, double>();

    [Benchmark(Description = "MathEvaluator invoke fn(P, r, n, d)")]
    public double MathEvaluator_InvokeCompiled()
    {
        Interlocked.Increment(ref _count);
        var d = _count % n + 1; //randomizing values

        var parameters = new CompoundInterestFormulaParams(10000, 0.05, n, d);

        return _mathEvalCompiledFn(parameters);
    }

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)")]
    public double MathEvaluator_FastExpressionCompiler_InvokeCompiled()
    {
        Interlocked.Increment(ref _count);
        var d = _count % n + 1; //randomizing values

        var parameters = new CompoundInterestFormulaParams(10000, 0.05, n, d);

        return _mathEvalFastCompiledFn(parameters);
    }

    [Benchmark(Description = "NCalc invoke fn(P, r, n, d)")]
    public double NCalc_InvokeCompiled()
    {
        Interlocked.Increment(ref _count);
        var d = _count % n + 1; //randomizing values

        var parameters = new CompoundInterestFormulaParams(10000, 0.05, n, d);

        return _nCalcCompiledFn(parameters);
    }

#pragma warning disable IDE1006 // Naming Styles
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public record struct CompoundInterestFormulaParams(double P = 0, double r = 0, int n = 0, int d = 0);
#pragma warning restore IDE1006 // Naming Styles
}