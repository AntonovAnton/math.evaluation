using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using NCalc;
using NCalc.LambdaCompilation;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
public class CompilationBenchmarks
{
    // ReSharper disable once InconsistentNaming
    private const double a = Math.PI / 6;
    // ReSharper disable once InconsistentNaming
    private const double b = Math.PI / 3;

    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private readonly MathContext _scientificContext = new ScientificMathContext();
    private readonly MathContext _programmingContext = new ProgrammingMathContext();

    private readonly Func<BooleanVariables, bool> _mathEvalCompiledFn;
    private readonly Func<BooleanVariables, bool> _mathEvalFastCompiledFn;
    private readonly Func<BooleanVariables, bool> _nCalcCompiledFn;

    public CompilationBenchmarks()
    {
        _mathEvalCompiledFn = MathExpression_CompileBoolean_HasVariables();
        _mathEvalFastCompiledFn = MathExpression_FastExpressionCompiler_CompileBoolean_HasVariables();
        _nCalcCompiledFn = NCalc_ToLambdaBoolean_HasVariables();
    }


    [Benchmark(Description = "MathEvaluator: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public Func<double> MathExpression_Compile()
        => new MathExpression("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6").Compile();

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public Func<double> MathExpression_FastExpressionCompiler_Compile()
        => new MathExpression("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6", null, null, _fastCompiler).Compile();

    [Benchmark(Description = "NCalc: \"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\"")]
    public Func<double> NCalc_ToLambda()
    {
        const string str = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6";
        var expression = new Expression(str, ExpressionOptions.NoCache);

        return expression.ToLambda<double>();
    }



    [Benchmark(Description = "MathEvaluator: \"true or not false and (true or false)\"")]
    public Func<bool> MathExpression_CompileBoolean()
        => new MathExpression("true or not false and (true or false)", _programmingContext)
            .CompileBoolean();

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"true or not false and (true or false)\"")]
    public Func<bool> MathExpression_FastExpressionCompiler_CompileBoolean()
        => new MathExpression("true or not false and (true or false)", _programmingContext, null, _fastCompiler)
            .CompileBoolean();

    [Benchmark(Description = "NCalc: \"true or not false and (true or false)\"")]
    public Func<bool> NCalc_ToLambdaBoolean()
    {
        const string str = "true or not false and (true or false)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        return expression.ToLambda<bool>();
    }



    [Benchmark(Description = "MathEvaluator: \"A or not B and (C or B)\"")]
    public Func<BooleanVariables, bool> MathExpression_CompileBoolean_HasVariables()
        => "A or not B and (C or B)"
            .CompileBoolean(new BooleanVariables { A = true, B = false, C = true }, _programmingContext);

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"A or not B and (C or B)\"")]
    public Func<BooleanVariables, bool> MathExpression_FastExpressionCompiler_CompileBoolean_HasVariables()
        => new MathExpression("A or not B and (C or B)", _programmingContext, null, _fastCompiler)
            .CompileBoolean(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = "NCalc: \"A or not B and (C or B)\"")]
    public Func<BooleanVariables, bool> NCalc_ToLambdaBoolean_HasVariables()
    {
        const string str = "A or not B and (C or B)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        return expression.ToLambda<BooleanVariables, bool>();
    }



    [Benchmark(Description = "MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })")]
    public bool MathEvaluator_CompiledBoolean_HasVariables()
        => _mathEvalCompiledFn(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })")]
    public bool MathEvaluator_FastExpressionCompiler_CompiledBoolean_HasVariables()
        => _mathEvalFastCompiledFn(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = "NCalc: fn(new BooleanVariables { A = a, B = b, C = c })")]
    public bool NCalc_CompiledBoolean_HasVariables()
        => _nCalcCompiledFn(new BooleanVariables { A = true, B = false, C = true });



    [Benchmark(Description = "MathEvaluator: \"Sin(pi/6) + Cos(pi/3)\"")]
    public Func<double> MathExpression_CompileSinCos()
        => new MathExpression("Sin(pi/6) + Cos(pi/3)", _scientificContext).Compile();

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"Sin(pi/6) + Cos(pi/3)\"")]
    public Func<double> MathExpression_FastExpressionCompiler_CompileSinCos()
        => new MathExpression("Sin(pi/6) + Cos(pi/3)", _scientificContext, null, _fastCompiler).Compile();

    [Benchmark(Description = "NCalc: \"Sin(pi/6) + Cos(pi/3)\"")]
    public Func<double> NCalc_ToLambdaSinCos()
    {
        const string str = "Sin(pi/6) + Cos(pi/3)";
        var expression = new Expression(str, ExpressionOptions.NoCache)
        {
            Parameters =
            {
                ["pi"] = Math.PI
            }
        };

        return expression.ToLambda<double>();
    }



    [Benchmark(Description = "MathEvaluator: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> MathExpression_CompileSinCos_HasVariables()
        => "Sin(a) + Cos(b)".Compile(new Variables { a = a, b = b }, _scientificContext);

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> MathExpression_FastExpressionCompiler_CompileSinCos_HasVariables()
        => new MathExpression("Sin(a) + Cos(b)", _scientificContext, null, _fastCompiler).Compile(new Variables { a = a, b = b });

    [Benchmark(Description = "NCalc: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> NCalc_ToLambdaSinCos_HasVariables()
    {
        const string str = "Sin(a) + Cos(b)";
        var expression = new Expression(str, ExpressionOptions.NoCache);
        return expression.ToLambda<Variables, double>();
    }



    public record Variables
    {
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once MemberHidesStaticFromOuterClass
        public double a { get; set; }
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once MemberHidesStaticFromOuterClass
        public double b { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public record BooleanVariables
    {
        public bool A { get; set; }
        public bool B { get; set; }
        public bool C { get; set; }
    }
}
