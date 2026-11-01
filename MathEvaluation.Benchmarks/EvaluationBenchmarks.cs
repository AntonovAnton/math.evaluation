using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
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
    private const string Expression1 = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6";
    private const string Expression2 = "Sin(pi/6) + Cos(pi/3)";
    private const string Expression3 = "Sin(a) + Cos(b)";
    private const string Expression4 = "A or not B and (C or B)";
    private const string Expression5 = "A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01";
    private const string NCalcExpression5 = "A != B && !C ** -2.9 >= -12.9 + 0.1 / 0.01";

    // ReSharper disable once InconsistentNaming
    private const double a = Math.PI / 6;

    // ReSharper disable once InconsistentNaming
    private const double b = Math.PI / 3;


    private int _count;

    [Benchmark(Description = $"MathEvaluator: {Expression1}")]
    public double MathExpression_Evaluate()
        => Expression1.Evaluate(null, CultureInfo.InvariantCulture);

    [Benchmark(Description = $"NCalc: {Expression1}")]
    public double NCalc_Evaluate()
    {
        var expression = new Expression(Expression1, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);

        return (double)expression.Evaluate()!;
    }

    [Benchmark(Description = $"MathEvaluator: {Expression2}")]
    public double MathExpression_EvaluateSinCos()
        => Expression2.Evaluate(new ScientificMathContext(), CultureInfo.InvariantCulture);

    [Benchmark(Description = $"NCalc: {Expression2}")]
    public double NCalc_EvaluateSinCos()
    {
        var expression = new Expression(Expression2, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
        {
            Parameters =
            {
                ["pi"] = Math.PI
            }
        };

        return (double)expression.Evaluate()!;
    }

    [Benchmark(Description = $"MathEvaluator: ${Expression3}")]
    public double MathExpression_EvaluateSinCos_HasVariables()
    {
        var parameters = new MathParameters();
        parameters.BindVariable(a);
        parameters.BindVariable(b);

        return Expression3.Evaluate(parameters, new ScientificMathContext(), CultureInfo.InvariantCulture);
    }

    [Benchmark(Description = $"NCalc: {Expression3}")]
    public double NCalc_EvaluateSinCos_HasVariables()
    {
        var expression = new Expression(Expression3, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
        {
            Parameters =
            {
                ["a"] = a,
                ["b"] = b
            }
        };

        return (double)expression.Evaluate()!;
    }

    [Benchmark(Description = $"MathEvaluator: {Expression4}")]
    public bool MathEvaluator_EvaluateBoolean()
    {
        _count++;
        var value = _count % 2 == 0; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(value, "A");
        parameters.BindVariable(!value, "B");
        parameters.BindVariable(value, "C");

        return Expression4.EvaluateBoolean(parameters, new ProgrammingMathContext(), CultureInfo.InvariantCulture);
    }

    [Benchmark(Description = $"NCalc: {Expression4}")]
    public bool NCalc_Evaluate_Boolean()
    {
        _count++;
        var value = _count % 2 == 0; //randomizing values

        var expression = new Expression(Expression4, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
        {
            Parameters =
            {
                ["A"] = value,
                ["B"] = !value,
                ["C"] = value
            }
        };

        return (bool)expression.Evaluate()!;
    }

    [Benchmark(Description = $"MathEvaluator: {Expression5}")]
    public bool MathEvaluator_EvaluateBoolean_DotNetStandardMathContext()
    {
        _count++;
        var value = _count % 2 == 0; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(value, "A");
        parameters.BindVariable(!value, "B");
        parameters.BindVariable(value, "C");

        return Expression5.EvaluateBoolean(parameters, new DotNetStandardMathContext(), CultureInfo.InvariantCulture);
    }

    [Benchmark(Description = $"NCalc: {NCalcExpression5}")]
    public bool NCalc_Evaluate_DotNet()
    {
        _count++;
        var value = _count % 2 == 0; //randomizing values

        var expression = new Expression(NCalcExpression5, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
        {
            Parameters =
            {
                ["A"] = value,
                ["B"] = !value,
                ["C"] = value
            }
        };

        return (bool)expression.Evaluate()!;
    }
}