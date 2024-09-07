using BenchmarkDotNet.Attributes;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using NCalc;

[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net80)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
public class CompilationBenchmarks
{
    private const double a = Math.PI / 6;
    private const double b = Math.PI / 3;

    private readonly IMathContext _scientificContext = new ScientificMathContext();
    private readonly IMathContext _programmingContext = new ProgrammingMathContext();

    private readonly Func<BooleanVariables, bool> _mathEvalCompiledFn;
    private readonly Func<BooleanVariables, bool> _nCalcCompiledFn;

    public CompilationBenchmarks()
    {
        _mathEvalCompiledFn = MathExpression_CompileBoolean_HasVariables();
        _nCalcCompiledFn = NCalc_ToLambdaBoolean_HasVariables();
    }

    [Benchmark(Description = "MathEvaluator: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public Func<double> MathExpression_Compile()
    {
        return "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".Compile();
    }

    [Benchmark(Description = "NCalc: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public Func<double> NCalc_ToLambda()
    {
        var str = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6";
        var expression = new Expression(str, ExpressionOptions.NoCache);

        return expression.ToLambda<double>();
    }

    [Benchmark(Description = "MathEvaluator: \"true or not false and (true or false)\"")]
    public Func<bool> MathExpression_CompileBoolean()
    {
        return "true or not false and (true or false)"
            .CompileBoolean(_programmingContext);
    }

    [Benchmark(Description = "NCalc: \"true or not false and (true or false)\"")]
    public Func<bool> NCalc_ToLambdaBoolean()
    {
        var str = "true or not false and (true or false)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        return expression.ToLambda<bool>();
    }

    [Benchmark(Description = "MathEvaluator: \"A or not B and (C or B)\"")]
    public Func<BooleanVariables, bool> MathExpression_CompileBoolean_HasVariables()
    {
        return "A or not B and (C or B)"
            .CompileBoolean(new BooleanVariables { A = true, B = false, C = true }, _programmingContext);
    }

    [Benchmark(Description = "NCalc: \"A or not B and (C or B)\"")]
    public Func<BooleanVariables, bool> NCalc_ToLambdaBoolean_HasVariables()
    {
        var str = "A or not B and (C or B)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        return expression.ToLambda<BooleanVariables, bool>();
    }

    [Benchmark(Description = "MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })")]
    public bool MathEvaluator_CompiledBoolean_HasVariables()
    {
        return _mathEvalCompiledFn(new BooleanVariables { A = true, B = false, C = true });
    }

    [Benchmark(Description = "NCalc: fn(new BooleanVariables { A = a, B = b, C = c })")]
    public bool NCalc_CompiledBoolean_HasVariables()
    {
        return _nCalcCompiledFn(new BooleanVariables { A = true, B = false, C = true });
    }

    [Benchmark(Description = "MathEvaluator: \"Sin(pi/6) + Cos(pi/3)\"")]
    public Func<double> MathExpression_CompileSinCos()
    {
        return "Sin(pi/6) + Cos(pi/3)".Compile(_scientificContext);
    }

    [Benchmark(Description = "NCalc: \"Sin(pi/6) + Cos(pi/3)\"")]
    public Func<double> NCalc_ToLambdaSinCos()
    {
        var str = "Sin(pi/6) + Cos(pi/3)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        expression.Parameters["pi"] = Math.PI;

        return expression.ToLambda<double>();
    }

    [Benchmark(Description = "MathEvaluator: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> MathExpression_CompileSinCos_HasVariables()
    {
        return "Sin(a) + Cos(b)".Compile(new Variables { a = a, b = b }, _scientificContext);
    }

    [Benchmark(Description = "NCalc: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> NCalc_ToLambdaSinCos_HasVariables()
    {
        var str = "Sin(a) + Cos(b)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        return expression.ToLambda<Variables, double>();
    }

    public record Variables
    {
        public double a { get; set; }
        public double b { get; set; }
    }

    public record BooleanVariables
    {
        public bool A { get; set; }
        public bool B { get; set; }
        public bool C { get; set; }
    }
}