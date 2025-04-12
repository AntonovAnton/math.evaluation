using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using NCalc;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
#pragma warning disable CA1050 // Declare types in namespaces
public class EvaluationBenchmarks
#pragma warning restore CA1050 // Declare types in namespaces
{
    // ReSharper disable once InconsistentNaming
    private const double a = Math.PI / 6;
    // ReSharper disable once InconsistentNaming
    private const double b = Math.PI / 3;

    private readonly IMathContext _programmingMathContext = new ProgrammingMathContext();
    private readonly IMathContext _scientificContext = new ScientificMathContext();
    private readonly IMathContext _dotNetStandardMathContext = new DotNetStandardMathContext();

    private int _count;

    [Benchmark(Description = "MathEvaluator: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public double MathExpression_Evaluate()
        => "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".Evaluate();

    [Benchmark(Description = "NCalc: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public bool NCalc_Evaluate()
    {
        const string str = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6";
        var expression = new Expression(str, ExpressionOptions.NoCache);

        return Convert.ToBoolean(expression.Evaluate());
    }

    [Benchmark(Description = "MathEvaluator: \"Sin(pi/6) + Cos(pi/3)\"")]
    public double MathExpression_EvaluateSinCos()
        => "Sin(pi/6) + Cos(pi/3)".Evaluate(null, _scientificContext);

    [Benchmark(Description = "NCalc: \"Sin(pi/6) + Cos(pi/3)\"")]
    public double NCalc_EvaluateSinCos()
    {
        const string str = "Sin(pi/6) + Cos(pi/3)";
        var expression = new Expression(str, ExpressionOptions.NoCache)
        {
            Parameters =
            {
                ["pi"] = Math.PI
            }
        };

        return Convert.ToDouble(expression.Evaluate());
    }

    [Benchmark(Description = "MathEvaluator: \"Sin(a) + Cos(b)\"")]
    public double MathExpression_EvaluateSinCos_HasVariables()
    {
        var parameters = new MathParameters();
        parameters.BindVariable(a);
        parameters.BindVariable(b);

        return "Sin(a) + Cos(b)".Evaluate(parameters, _scientificContext);
    }

    [Benchmark(Description = "NCalc: \"Sin(a) + Cos(b)\"")]
    public double NCalc_EvaluateSinCos_HasVariables()
    {
        const string str = "Sin(a) + Cos(b)";
        var expression = new Expression(str, ExpressionOptions.NoCache)
        {
            Parameters =
            {
                ["a"] = a,
                ["b"] = b
            }
        };

        return Convert.ToDouble(expression.Evaluate());
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

    [Benchmark(Description = "NCalc: \"A or not B and (C or B)\"")]
    public bool NCalc_Evaluate_Boolean()
    {
        _count++;
        var value = _count % 2 == 0; //randomizing values

        var expression = new Expression("A or not B and (C or B)", ExpressionOptions.NoCache)
        {
            Parameters =
            {
                ["A"] = value,
                ["B"] = !value,
                ["C"] = value
            }
        };

        return Convert.ToBoolean(expression.Evaluate());
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

    [Benchmark(Description = "NCalc: \"A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01\"")]
    public bool NCalc_Evaluate_DotNet()
    {
        _count++;
        var value = _count % 2 == 0; //randomizing values

        const string str = "A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01";
        var expression = new Expression(str, ExpressionOptions.NoCache)
        {
            Parameters =
            {
                ["A"] = value,
                ["B"] = !value,
                ["C"] = value
            }
        };

        return Convert.ToBoolean(expression.Evaluate());
    }
}