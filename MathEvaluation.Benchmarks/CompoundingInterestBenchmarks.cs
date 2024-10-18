using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using NCalc;
using System.Diagnostics.CodeAnalysis;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net60)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
#pragma warning disable CA1050 // Declare types in namespaces
public class CompoundingInterestBenchmarks
#pragma warning restore CA1050 // Declare types in namespaces
{
    private int _count;

    private readonly IMathContext _mathContext = new ScientificMathContext();

    private readonly Func<CompoundInterestFormulaParams, double> _mathEvalCompiledFn;
    private readonly Func<CompoundInterestFormulaParams, double> _nCalcCompiledFn;

    public CompoundingInterestBenchmarks()
    {
        _mathEvalCompiledFn = MathEvaluator_Compile();
        _nCalcCompiledFn = NCalc_ToLambda();
    }

    [Benchmark(Description = "MathEvaluator evaluation")]
    public double MathEvaluator_Evaluate()
    {
        _count++;
        const int n = 365;
        var d = _count % n + 1; //randomizing values

        var parameters = new MathParameters(new { P = 10000, r = 0.05, n, d });

        return "P * (1 + r/n)^d".Evaluate(parameters, _mathContext);
    }

    [Benchmark(Description = "NCalc evaluation")]
    public double NCalc_Evaluate()
    {
        _count++;
        const int n = 365;
        var d = _count % n + 1; //randomizing values

        var expression = new Expression("P * Pow((1 + r/n), d)", ExpressionOptions.NoCache)
        {
            Parameters =
            {
                ["P"] = 10000,
                ["r"] = 0.05,
                ["n"] = n,
                ["d"] = d
            }
        };

        return Convert.ToDouble(expression.Evaluate());
    }

    [Benchmark(Description = "MathEvaluator compilation")]
    public Func<CompoundInterestFormulaParams, double> MathEvaluator_Compile()
        => "P * (1 + r/n)^d".Compile(new CompoundInterestFormulaParams(), _mathContext);

    [Benchmark(Description = "NCalc compilation")]
    public Func<CompoundInterestFormulaParams, double> NCalc_ToLambda()
    {
        var expression = new Expression("P * Pow((1 + r/n), d)", ExpressionOptions.NoCache);

        return expression.ToLambda<CompoundInterestFormulaParams, double>();
    }

    [Benchmark(Description = "MathEvaluator invoke fn(P, r, n, d)")]
    public double MathEvaluator_InvokeCompiled()
    {
        _count++;
        const int n = 365;
        var d = _count % n + 1; //randomizing values

        var parameters = new CompoundInterestFormulaParams(10000, 0.05, n, d);

        return _mathEvalCompiledFn(parameters);
    }

    [Benchmark(Description = "NCalc invoke fn(P, r, n, d)")]
    public double NCalc_InvokeCompiled()
    {
        _count++;
        const int n = 365;
        var d = _count % n + 1; //randomizing values

        var parameters = new CompoundInterestFormulaParams(10000, 0.05, n, d);

        return _nCalcCompiledFn(parameters);
    }

#pragma warning disable IDE1006 // Naming Styles
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public record CompoundInterestFormulaParams(double P = 0, double r = 0, int n = 0, int d = 0);
#pragma warning restore IDE1006 // Naming Styles
}