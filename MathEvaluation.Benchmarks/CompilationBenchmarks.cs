using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using NCalc;
using NCalc.LambdaCompilation;
using System.Globalization;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, iterationCount: 5)]
[MemoryDiagnoser]
public class CompilationBenchmarks
{
    private const string Expression1 = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6";
    private const string Expression2 = "true or not false and (true or false)";
    private const string Expression3 = "A or not B and (C or B)";
    private const string Expression4 = "Sin(pi/6) + Cos(pi/3)";
    private const string Expression5 = "Sin(a) + Cos(b)";

    // ReSharper disable once InconsistentNaming
    private const double a = Math.PI / 6;

    // ReSharper disable once InconsistentNaming
    private const double b = Math.PI / 3;

    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private readonly Func<BooleanVariables, bool> _mathEvalCompiledFn;
    private readonly Func<BooleanVariables, bool> _mathEvalFastCompiledFn;
    private readonly Func<BooleanVariables, bool> _nCalcCompiledFn;

    public CompilationBenchmarks()
    {
        _mathEvalCompiledFn = MathExpression_CompileBoolean_HasVariables();
        _mathEvalFastCompiledFn = MathExpression_FastExpressionCompiler_CompileBoolean_HasVariables();
        _nCalcCompiledFn = NCalc_ToLambdaBoolean_HasVariables();
    }


    [Benchmark(Description = $"MathEvaluator: {Expression1}")]
    public Func<double> MathExpression_Compile()
        => new MathExpression(Expression1, null, CultureInfo.InvariantCulture).Compile();

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler: {Expression1}")]
    public Func<double> MathExpression_FastExpressionCompiler_Compile()
        => new MathExpression(Expression1, null, CultureInfo.InvariantCulture, _fastCompiler).Compile();

    [Benchmark(Description = $"NCalc: {Expression1}")]
    public Func<double> NCalc_ToLambda()
        => new Expression(Expression1, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
            .ToLambda<double>();

    [Benchmark(Description = $"MathEvaluator: {Expression2}")]
    public Func<bool> MathExpression_CompileBoolean()
        => new MathExpression(Expression2, new ProgrammingMathContext(), CultureInfo.InvariantCulture).CompileBoolean();

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler: {Expression2}")]
    public Func<bool> MathExpression_FastExpressionCompiler_CompileBoolean()
        => new MathExpression(Expression2, new ProgrammingMathContext(), CultureInfo.InvariantCulture, _fastCompiler)
            .CompileBoolean();

    [Benchmark(Description = $"NCalc: {Expression2}")]
    public Func<bool> NCalc_ToLambdaBoolean()
     => new Expression(Expression2, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
        .ToLambda<bool>();


    [Benchmark(Description = $"MathEvaluator: {Expression3}")]
    public Func<BooleanVariables, bool> MathExpression_CompileBoolean_HasVariables()
        => new MathExpression(Expression3, new ProgrammingMathContext(), CultureInfo.InvariantCulture)
            .CompileBoolean(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler: {Expression3}")]
    public Func<BooleanVariables, bool> MathExpression_FastExpressionCompiler_CompileBoolean_HasVariables()
        => new MathExpression(Expression3, new ProgrammingMathContext(), CultureInfo.InvariantCulture, _fastCompiler)
            .CompileBoolean(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = $"NCalc: {Expression3}")]
    public Func<BooleanVariables, bool> NCalc_ToLambdaBoolean_HasVariables()
        => new Expression(Expression3, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
            .ToLambda<BooleanVariables, bool>();


    [Benchmark(Description = "MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })")]
    public bool MathEvaluator_CompiledBoolean_HasVariables()
        => _mathEvalCompiledFn(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })")]
    public bool MathEvaluator_FastExpressionCompiler_CompiledBoolean_HasVariables()
        => _mathEvalFastCompiledFn(new BooleanVariables { A = true, B = false, C = true });

    [Benchmark(Description = "NCalc: fn(new BooleanVariables { A = a, B = b, C = c })")]
    public bool NCalc_CompiledBoolean_HasVariables()
        => _nCalcCompiledFn(new BooleanVariables { A = true, B = false, C = true });


    [Benchmark(Description = $"MathEvaluator: {Expression4}")]
    public Func<double> MathExpression_CompileSinCos()
        => new MathExpression(Expression4, new ScientificMathContext(), CultureInfo.InvariantCulture).Compile();

    [Benchmark(Description = $"MathEvaluator.FastExpressionCompiler: {Expression4}")]
    public Func<double> MathExpression_FastExpressionCompiler_CompileSinCos()
        => new MathExpression(Expression4, new ScientificMathContext(), CultureInfo.InvariantCulture, _fastCompiler).Compile();

    [Benchmark(Description = $"NCalc: {Expression4}")]
    public Func<double> NCalc_ToLambdaSinCos()
        => new Expression(Expression4, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
        {
            Parameters =
            {
                ["pi"] = Math.PI
            }
        }.ToLambda<double>();


    [Benchmark(Description = $"MathEvaluator: {Expression5}")]
    public Func<Variables, double> MathExpression_CompileSinCos_HasVariables()
        => Expression5.Compile(new Variables { a = a, b = b }, new ScientificMathContext());

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> MathExpression_FastExpressionCompiler_CompileSinCos_HasVariables()
        => new MathExpression(Expression5, new ScientificMathContext(), CultureInfo.InvariantCulture, _fastCompiler)
            .Compile(new Variables { a = a, b = b });

    [Benchmark(Description = $"NCalc: {Expression5}")]
    public Func<Variables, double> NCalc_ToLambdaSinCos_HasVariables()
        => new Expression(Expression5, ExpressionOptions.NoCache, null, CultureInfo.InvariantCulture)
            .ToLambda<Variables, double>();


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