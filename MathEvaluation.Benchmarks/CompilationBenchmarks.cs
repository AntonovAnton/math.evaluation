using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using MathEvaluation.Extensions;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net10_0, warmupCount: 3, iterationCount: 10)] // short jobs: they spot regressions, the published results are in LibraryComparisonBenchmarks
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


    [Benchmark(Description = "MathEvaluator: \"A or not B and (C or B)\"")]
    public Func<BooleanVariables, bool> MathExpression_CompileBoolean_HasVariables()
        => "A or not B and (C or B)"
            .CompileBoolean(new BooleanVariables { A = true, B = false, C = true }, _programmingContext);

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"A or not B and (C or B)\"")]
    public Func<BooleanVariables, bool> MathExpression_FastExpressionCompiler_CompileBoolean_HasVariables()
        => new MathExpression("A or not B and (C or B)", _programmingContext, null, _fastCompiler)
            .CompileBoolean(new BooleanVariables { A = true, B = false, C = true });


    [Benchmark(Description = "MathEvaluator: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> MathExpression_CompileSinCos_HasVariables()
        => "Sin(a) + Cos(b)".Compile(new Variables { a = a, b = b }, _scientificContext);

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: \"Sin(a) + Cos(b)\"")]
    public Func<Variables, double> MathExpression_FastExpressionCompiler_CompileSinCos_HasVariables()
        => new MathExpression("Sin(a) + Cos(b)", _scientificContext, null, _fastCompiler).Compile(new Variables { a = a, b = b });


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