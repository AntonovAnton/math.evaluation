using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using NCalc;
using NCalc.LambdaCompilation;
using System.Globalization;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
public class CompilationBenchmarks
{
    private const string ExpressionString1 = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6";
    private const string ExpressionString2 = "true or not false and (true or false)";
    private const string ExpressionString3 = "A or not B and (C or B)";
    private const string ExpressionString4 = "Sin(pi/6) + Cos(pi/3)";
    private const string ExpressionString5 = "Sin(a) + Cos(b)";

    private const double a = Math.PI / 6;
    private const double b = Math.PI / 3;

    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();
    private readonly MathContext _scientificContext = new ScientificMathContext();
    private readonly MathContext _programmingContext = new ProgrammingMathContext();

    private Func<BooleanVariables, bool> _mathEvalCompiledFn;
    private Func<BooleanVariables, bool> _mathEvalFastCompiledFn;
    private Func<BooleanVariables, bool> _nCalcCompiledFn;


    private Expression NCalcExpression1 { get; set; }
    private Expression NCalcExpression2 { get; set; }
    private Expression NCalcExpression3 { get; set; }
    private Expression NCalcExpression4 { get; set; }
    private Expression NCalcExpression5 { get; set; }


    private MathExpression MathExpression1 { get; set; }
    private MathExpression MathExpression1_Fast { get; set; }
    private MathExpression MathExpression2 { get; set; }
    private MathExpression MathExpression2_Fast { get; set; }
    private MathExpression MathExpression3 { get; set; }
    private MathExpression MathExpression3_Fast { get; set; }
    private MathExpression MathExpression4 { get; set; }
    private MathExpression MathExpression4_Fast { get; set; }
    private MathExpression MathExpression5 { get; set; }
    private MathExpression MathExpression5_Fast { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        NCalcExpression1 = new Expression(ExpressionString1, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);
        NCalcExpression2 = new Expression(ExpressionString2, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);
        NCalcExpression3 = new Expression(ExpressionString3, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);
        NCalcExpression4 = new Expression(ExpressionString4, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture) { Parameters = { ["pi"] = Math.PI } };
        NCalcExpression5 = new Expression(ExpressionString5, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture);

        MathExpression1 = new MathExpression(ExpressionString1, null, CultureInfo.InvariantCulture);
        MathExpression1_Fast = new MathExpression(ExpressionString1, null, CultureInfo.InvariantCulture, _fastCompiler);

        MathExpression2 = new MathExpression(ExpressionString2, _programmingContext, CultureInfo.InvariantCulture);
        MathExpression2_Fast = new MathExpression(ExpressionString2, _programmingContext, CultureInfo.InvariantCulture, _fastCompiler);

        MathExpression3 = new MathExpression(ExpressionString3, _programmingContext, CultureInfo.InvariantCulture);
        MathExpression3_Fast = new MathExpression(ExpressionString3, _programmingContext, CultureInfo.InvariantCulture, _fastCompiler);

        MathExpression4 = new MathExpression(ExpressionString4, _scientificContext, CultureInfo.InvariantCulture);
        MathExpression4_Fast = new MathExpression(ExpressionString4, _scientificContext, CultureInfo.InvariantCulture, _fastCompiler);

        MathExpression5 = new MathExpression(ExpressionString5, _scientificContext, CultureInfo.InvariantCulture);
        MathExpression5_Fast = new MathExpression(ExpressionString5, _scientificContext, CultureInfo.InvariantCulture, _fastCompiler);


        var dummyBoolArgs = new BooleanVariables { A = true, B = false, C = true };
        _mathEvalCompiledFn = MathExpression3.CompileBoolean(dummyBoolArgs);
        _mathEvalFastCompiledFn = MathExpression3_Fast.CompileBoolean(dummyBoolArgs);
        _nCalcCompiledFn = NCalcExpression3.ToLambda<BooleanVariables, bool>();
    }


    [Benchmark(Description = $"MathEvaluator: {ExpressionString1}")]
    public Func<double> MathExpression_Compile()
        => MathExpression1.Compile();

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler: {ExpressionString1}")]
    public Func<double> MathExpression_FastExpressionCompiler_Compile()
        => MathExpression1_Fast.Compile();

    [Benchmark(Description = $"NCalc: {ExpressionString1}")]
    public Func<double> NCalc_ToLambda()
        => NCalcExpression1.ToLambda<double>();


    [Benchmark(Description = $"MathEvaluator: {ExpressionString2}")]
    public Func<bool> MathExpression_CompileBoolean()
        => MathExpression2.CompileBoolean();

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler: {ExpressionString2}")]
    public Func<bool> MathExpression_FastExpressionCompiler_CompileBoolean()
        => MathExpression2_Fast.CompileBoolean();

    [Benchmark(Description = $"NCalc: {ExpressionString2}")]
    public Func<bool> NCalc_ToLambdaBoolean()
        => NCalcExpression2.ToLambda<bool>();


    [Benchmark(Description = $"MathEvaluator: {ExpressionString3}")]
    public Func<BooleanVariables, bool> MathExpression_CompileBoolean_HasVariables()
        => MathExpression3.CompileBoolean(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler: {ExpressionString3}")]
    public Func<BooleanVariables, bool> MathExpression_FastExpressionCompiler_CompileBoolean_HasVariables()
        => MathExpression3_Fast.CompileBoolean(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = $"NCalc: {ExpressionString3}")]
    public Func<BooleanVariables, bool> NCalc_ToLambdaBoolean_HasVariables()
        => NCalcExpression3.ToLambda<BooleanVariables, bool>();


    [Benchmark(Description = "MathEvaluator: fn(new BooleanVariables)")]
    public bool MathEvaluator_CompiledBoolean_HasVariables()
        => _mathEvalCompiledFn(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables)")]
    public bool MathEvaluator_FastExpressionCompiler_CompiledBoolean_HasVariables()
        => _mathEvalFastCompiledFn(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = "NCalc: fn(new BooleanVariables)")]
    public bool NCalc_CompiledBoolean_HasVariables()
        => _nCalcCompiledFn(new BooleanVariables { A = true, B = false, C = true });


    [Benchmark(Description = $"MathEvaluator: {ExpressionString4}")]
    public Func<double> MathExpression_CompileSinCos()
        => MathExpression4.Compile();

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler: {ExpressionString4}")]
    public Func<double> MathExpression_FastExpressionCompiler_CompileSinCos()
        => MathExpression4_Fast.Compile();

    [Benchmark(Description = $"NCalc: {ExpressionString4}")]
    public Func<double> NCalc_ToLambdaSinCos()
        => NCalcExpression4.ToLambda<double>();


    [Benchmark(Description = $"MathEvaluator: {ExpressionString5}")]
    public Func<Variables, double> MathExpression_CompileSinCos_HasVariables()
        => MathExpression5.Compile(new Variables { a = a, b = b });

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> MathExpression_FastExpressionCompiler_CompileSinCos_HasVariables()
        => MathExpression5_Fast.Compile(new Variables { a = a, b = b });

    [Benchmark(Description = $"NCalc: {ExpressionString5}")]
    public Func<Variables, double> NCalc_ToLambdaSinCos_HasVariables()
        => NCalcExpression5.ToLambda<Variables, double>();

    public record struct Variables(double a, double b);
    public record struct BooleanVariables(bool A, bool B, bool C);
}