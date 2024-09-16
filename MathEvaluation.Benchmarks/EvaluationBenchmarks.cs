using BenchmarkDotNet.Attributes;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using NCalc;

[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net80)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
#pragma warning disable CA1050 // Declare types in namespaces
public class EvaluationBenchmarks
#pragma warning restore CA1050 // Declare types in namespaces
{
    private const double a = Math.PI / 6;
    private const double b = Math.PI / 3;

    private readonly IMathContext _programmingMathContext = new ProgrammingMathContext();
    private readonly IMathContext _scientificContext = new ScientificMathContext();
    private readonly IMathContext _dotNetStandartMathContext = new DotNetStandartMathContext();

    private int _count;

    [Benchmark(Description = "MathEvaluator: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public double MathExpression_Evaluate()
    {
        return "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".Evaluate();
    }

    [Benchmark(Description = "NCalc: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public bool NCalc_Evaluate()
    {
        var str = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6";
        var expression = new Expression(str, ExpressionOptions.NoCache);

        return Convert.ToBoolean(expression.Evaluate());
    }

    [Benchmark(Description = "MathEvaluator: \"Sin(pi/6) + Cos(pi/3)\"")]
    public double MathExpression_EvaluateSinCos()
    {
        return "Sin(pi/6) + Cos(pi/3)".Evaluate(null, _scientificContext);
    }

    [Benchmark(Description = "NCalc: \"Sin(pi/6) + Cos(pi/3)\"")]
    public double NCalc_EvaluateSinCos()
    {
        var str = "Sin(pi/6) + Cos(pi/3)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        expression.Parameters["pi"] = Math.PI;

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
        var str = "Sin(a) + Cos(b)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        expression.Parameters["a"] = a;
        expression.Parameters["b"] = b;

        return Convert.ToDouble(expression.Evaluate());
    }

    [Benchmark(Description = "MathEvaluator: \"A or not B and (C or B)\"")]
    public bool MathEvaluator_EvaluateBoolean()
    {
        _count++;
        bool a = _count % 2 == 0; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(a, "A");
        parameters.BindVariable(!a, "B");
        parameters.BindVariable(a, "C");

        return "A or not B and (C or B)"
            .EvaluateBoolean(parameters, _programmingMathContext);
    }

    [Benchmark(Description = "NCalc: \"A or not B and (C or B)\"")]
    public bool NCalc_Evaluate_Boolean()
    {
        _count++;
        bool a = _count % 2 == 0; //randomizing values

        var expression = new Expression("A or not B and (C or B)", ExpressionOptions.NoCache);
        expression.Parameters["A"] = a;
        expression.Parameters["B"] = !a;
        expression.Parameters["C"] = a;

        return Convert.ToBoolean(expression.Evaluate());
    }

    [Benchmark(Description = "MathEvaluator: \"A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01\"")]
    public bool MathEvaluator_EvaluateBoolean_DotNetStandartMathContext()
    {
        _count++;
        bool a = _count % 2 == 0; //randomizing values

        var parameters = new MathParameters();
        parameters.BindVariable(a, "A");
        parameters.BindVariable(!a, "B");
        parameters.BindVariable(a, "C");

        return "A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01"
            .EvaluateBoolean(parameters, _dotNetStandartMathContext);
    }

    [Benchmark(Description = "NCalc: \"A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01\"")]
    public bool NCalc_Evaluate_DotNet()
    {
        _count++;
        bool a = _count % 2 == 0; //randomizing values

        var str = "A != B && !C ^ -2.9 >= -12.9 + 0.1 / 0.01";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        expression.Parameters["A"] = a;
        expression.Parameters["B"] = !a;
        expression.Parameters["C"] = a;

        return Convert.ToBoolean(expression.Evaluate());
    }
}
