using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
public class AsyncCompilationBenchmarks
{
    private const string MathString = "price(x) * qty + tax(price(x))";
    private const string SyncMathString = "priceSync(x) * qty + taxSync(priceSync(x))";

    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private readonly MathContext _context = new();
    private readonly Variables _variables = new() { x = 2, qty = 3 };

    private readonly Func<double, double> _price = x => x * 10;
    private readonly Func<double, double> _tax = x => x * 0.2;

    private readonly Func<Variables, double> _mathEvalCompiledFn;
    private readonly Func<Variables, ValueTask<double>> _mathEvalAsyncCompiledFn;
    private readonly Func<Variables, ValueTask<double>> _mathEvalFastAsyncCompiledFn;
    private readonly Func<Variables, ValueTask<double>> _mathEvalAsyncYieldingCompiledFn;

    public AsyncCompilationBenchmarks()
    {
        // completed synchronously, it measures the overhead of the async delegate
        _context.BindAsyncFunction<double>(x => ValueTask.FromResult(_price(x)), "price");
        _context.BindAsyncFunction<double>(x => ValueTask.FromResult(_tax(x)), "tax");

        // completed asynchronously, it measures the overhead of the continuations
        _context.BindAsyncFunction<double>(async x =>
        {
            await Task.Yield();
            return _price(x);
        }, "priceAsync");
        _context.BindAsyncFunction<double>(async x =>
        {
            await Task.Yield();
            return _tax(x);
        }, "taxAsync");

        _context.BindFunction(_price, "priceSync");
        _context.BindFunction(_tax, "taxSync");

        _mathEvalCompiledFn = MathExpression_Compile_SyncFunctions();
        _mathEvalAsyncCompiledFn = MathExpression_CompileAsyncFunc();
        _mathEvalFastAsyncCompiledFn = MathExpression_FastExpressionCompiler_CompileAsyncFunc();
        _mathEvalAsyncYieldingCompiledFn = new MathExpression("priceAsync(x) * qty + taxAsync(priceAsync(x))", _context)
            .CompileAsyncFunc(_variables);
    }


    [Benchmark(Description = "MathEvaluator: Compile \"" + SyncMathString + "\"")]
    public Func<Variables, double> MathExpression_Compile_SyncFunctions()
        => new MathExpression(SyncMathString, _context).Compile(_variables);

    [Benchmark(Description = "MathEvaluator: CompileAsyncFunc \"" + MathString + "\"")]
    public Func<Variables, ValueTask<double>> MathExpression_CompileAsyncFunc()
        => new MathExpression(MathString, _context).CompileAsyncFunc(_variables);

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: CompileAsyncFunc \"" + MathString + "\"")]
    public Func<Variables, ValueTask<double>> MathExpression_FastExpressionCompiler_CompileAsyncFunc()
        => new MathExpression(MathString, _context, null, _fastCompiler).CompileAsyncFunc(_variables);


    [Benchmark(Description = "MathEvaluator: fn(variables), sync functions")]
    public double MathEvaluator_Compiled_SyncFunctions()
        => _mathEvalCompiledFn(_variables);

    [Benchmark(Description = "MathEvaluator: await fn(variables), completed async functions")]
    public ValueTask<double> MathEvaluator_AsyncCompiled_CompletedFunctions()
        => _mathEvalAsyncCompiledFn(_variables);

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: await fn(variables), completed async functions")]
    public ValueTask<double> MathEvaluator_FastExpressionCompiler_AsyncCompiled_CompletedFunctions()
        => _mathEvalFastAsyncCompiledFn(_variables);

    [Benchmark(Description = "MathEvaluator: await fn(variables), yielding async functions")]
    public ValueTask<double> MathEvaluator_AsyncCompiled_YieldingFunctions()
        => _mathEvalAsyncYieldingCompiledFn(_variables);


    public record Variables
    {
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable once InconsistentNaming
        public double x { get; set; }

        // ReSharper disable once InconsistentNaming
        public double qty { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
