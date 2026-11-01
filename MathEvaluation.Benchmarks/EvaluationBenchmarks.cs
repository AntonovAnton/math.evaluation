using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Context;
using MathEvaluation.Parameters;
using NCalc;
using System.Globalization;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.NativeAot80)]
[SimpleJob(RuntimeMoniker.NativeAot10_0)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
public class EvaluationBenchmarks
{
    private const string ExpressionString1 = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6";
    private const string ExpressionString2 = "Sin(pi/6) + Cos(pi/3)";
    private const string ExpressionString3 = "Sin(a) + Cos(b)";
    private const string ExpressionString4 = "A or not B and (C or B)";
    private const string ExpressionString5 = "A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01";
    private const string NCalcExpressionString5 = "A != B && !C ** -2.9 >= -12.9 + 0.1 / 0.01";


    // ReSharper disable once InconsistentNaming
    private const double a = Math.PI / 6;

    // ReSharper disable once InconsistentNaming
    private const double b = Math.PI / 3;

    private Expression NCalcExpression1 { get; set; }
    private Expression NCalcExpression2 { get; set; }
    private Expression NCalcExpression3 { get; set; }
    private Expression NCalcExpression4 { get; set; }
    private Expression NCalcExpression5 { get; set; }


    private MathExpression MathExpression1 { get; set; }
    private MathExpression MathExpression2 { get; set; }
    private MathExpression MathExpression3 { get; set; }
    private MathExpression MathExpression4 { get; set; }
    private MathExpression MathExpression5 { get; set; }

    private int _count;


    [GlobalSetup]
    public void Setup()
    {
        NCalcExpression1 = new Expression(ExpressionString1, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture); ;
        NCalcExpression2 = new Expression(ExpressionString2, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
        {
            Parameters = { ["pi"] = Math.PI }
        };
        NCalcExpression3 = new Expression(ExpressionString3, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);
        NCalcExpression4 = new Expression(ExpressionString4, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);
        NCalcExpression5 = new Expression(NCalcExpressionString5, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);

        MathExpression1 = new MathExpression(ExpressionString1, null, CultureInfo.InvariantCulture);
        MathExpression2 = new MathExpression(ExpressionString2, new ScientificMathContext(), CultureInfo.InvariantCulture);
        MathExpression3 = new MathExpression(ExpressionString3, new ScientificMathContext(), CultureInfo.InvariantCulture);
        MathExpression4 = new MathExpression(ExpressionString4, new ProgrammingMathContext(), CultureInfo.InvariantCulture);
        MathExpression5 = new MathExpression(ExpressionString5, new DotNetStandardMathContext(), CultureInfo.InvariantCulture);
    }

    [Benchmark(Description = $"MathEvaluator: {ExpressionString1}")]
    public double MathExpression_Evaluate()
        => MathExpression1.Evaluate();

    [Benchmark(Description = $"NCalc: {ExpressionString1}")]
    public double NCalc_Evaluate()
        => (double)NCalcExpression1.Evaluate();

    [Benchmark(Description = $"MathEvaluator: {ExpressionString2}")]
    public double MathExpression_EvaluateSinCos()
        => MathExpression2.Evaluate();

    [Benchmark(Description = $"NCalc: {ExpressionString2}")]
    public double NCalc_EvaluateSinCos()
        => (double)NCalcExpression2.Evaluate()!;

    [Benchmark(Description = $"MathEvaluator: {ExpressionString3}")]
    public double MathExpression_EvaluateSinCos_HasVariables()
    {
        var parameters = new MathParameters();
        parameters.BindVariable(a);
        parameters.BindVariable(b);

        return MathExpression3.Evaluate(parameters);
    }

    [Benchmark(Description = $"NCalc: {ExpressionString3}")]
    public double NCalc_EvaluateSinCos_HasVariables()
    {
        NCalcExpression3.Parameters["a"] = a;
        NCalcExpression3.Parameters["b"] = b;

        return (double)NCalcExpression3.Evaluate()!;
    }

    [Benchmark(Description = $"MathEvaluator: {ExpressionString4}")]
    public bool MathEvaluator_EvaluateBoolean()
    {
        Interlocked.Increment(ref _count);
        var value = _count % 2 == 0; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(value, "A");
        parameters.BindVariable(!value, "B");
        parameters.BindVariable(value, "C");

        return MathExpression4.EvaluateBoolean(parameters);
    }

    [Benchmark(Description = $"NCalc: {ExpressionString4}")]
    public bool NCalc_Evaluate_Boolean()
    {
        Interlocked.Increment(ref _count);
        var value = _count % 2 == 0; //randomizing values

        NCalcExpression4.Parameters["A"] = value;
        NCalcExpression4.Parameters["B"] = !value;
        NCalcExpression4.Parameters["C"] = value;

        return (bool)NCalcExpression4.Evaluate()!;
    }

    [Benchmark(Description = $"MathEvaluator: {ExpressionString5}")]
    public bool MathEvaluator_EvaluateBoolean_DotNetStandardMathContext()
    {
        Interlocked.Increment(ref _count);
        var value = _count % 2 == 0; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(value, "A");
        parameters.BindVariable(!value, "B");
        parameters.BindVariable(value, "C");

        return MathExpression5.EvaluateBoolean(parameters);
    }

    [Benchmark(Description = $"NCalc: {NCalcExpressionString5}")]
    public bool NCalc_Evaluate_DotNet()
    {
        Interlocked.Increment(ref _count);
        var value = _count % 2 == 0; //randomizing values

        NCalcExpression5.Parameters["A"] = value;
        NCalcExpression5.Parameters["B"] = !value;
        NCalcExpression5.Parameters["C"] = value;

        return (bool)NCalcExpression5.Evaluate()!;
    }
}