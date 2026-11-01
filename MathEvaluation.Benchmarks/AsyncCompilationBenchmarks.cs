using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MathEvaluation.Compilation;
using MathEvaluation.Context;
using System.Globalization;

namespace MathEvaluation.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
public class AsyncCompilationBenchmarks
{
    private const string MathString = "price(x) * qty + tax(price(x))";
    private const string SyncMathString = "priceSync(x) * qty + taxSync(priceSync(x))";
    private const string YieldingMathString = "priceAsync(x) * qty + taxAsync(priceAsync(x))";

    private readonly IExpressionCompiler _fastCompiler = new FastMathExpressionCompiler();

    private readonly MathContext _context = new();
    private readonly Variables _variables = new(2, 3);

    private readonly Func<double, double> _price = x => x * 10;
    private readonly Func<double, double> _tax = x => x * 0.2;

    private Func<Variables, double> _mathEvalCompiledFn;
    private Func<Variables, CancellationToken, ValueTask<double>> _mathEvalAsyncCompiledFn;
    private Func<Variables, CancellationToken, ValueTask<double>> _mathEvalFastAsyncCompiledFn;
    private Func<Variables, CancellationToken, ValueTask<double>> _mathEvalAsyncYieldingCompiledFn;

    private MathExpression _exprSync;
    private MathExpression _exprAsync;
    private MathExpression _exprAsyncFast;

    [GlobalSetup]
    public void Setup()
    {
        _context.BindAsyncFunction<double>((x, _) => ValueTask.FromResult(_price(x)), "price");
        _context.BindAsyncFunction<double>((x, _) => ValueTask.FromResult(_tax(x)), "tax");

        _context.BindAsyncFunction<double>(async (x, _) =>
        {
            await Task.Yield();
            return _price(x);
        }, "priceAsync");
        _context.BindAsyncFunction<double>(async (x, _) =>
        {
            await Task.Yield();
            return _tax(x);
        }, "taxAsync");

        _context.BindFunction(_price, "priceSync");
        _context.BindFunction(_tax, "taxSync");

        _exprSync = new MathExpression(SyncMathString, _context, CultureInfo.InvariantCulture);
        _exprAsync = new MathExpression(MathString, _context, CultureInfo.InvariantCulture);
        _exprAsyncFast = new MathExpression(MathString, _context, CultureInfo.InvariantCulture, _fastCompiler);
        var exprYielding = new MathExpression(YieldingMathString, _context, CultureInfo.InvariantCulture);

        _mathEvalCompiledFn = _exprSync.Compile(_variables);
        _mathEvalAsyncCompiledFn = _exprAsync.CompileAsyncFunc(_variables);
        _mathEvalFastAsyncCompiledFn = _exprAsyncFast.CompileAsyncFunc(_variables);
        _mathEvalAsyncYieldingCompiledFn = exprYielding.CompileAsyncFunc(_variables);
    }


    [Benchmark(Description = "MathEvaluator: Compile \"" + SyncMathString + "\"")]
    public Func<Variables, double> MathExpression_Compile_SyncFunctions()
        => _exprSync.Compile(_variables);

    [Benchmark(Description = "MathEvaluator: CompileAsyncFunc \"" + MathString + "\"")]
    public Func<Variables, CancellationToken, ValueTask<double>> MathExpression_CompileAsyncFunc()
        => _exprAsync.CompileAsyncFunc(_variables);

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: CompileAsyncFunc \"" + MathString + "\"")]
    public Func<Variables, CancellationToken, ValueTask<double>> MathExpression_FastExpressionCompiler_CompileAsyncFunc()
        => _exprAsyncFast.CompileAsyncFunc(_variables);


    [Benchmark(Description = "MathEvaluator: fn(variables), sync functions")]
    public double MathEvaluator_Compiled_SyncFunctions()
        => _mathEvalCompiledFn(_variables);

    [Benchmark(Description = "MathEvaluator: await fn(variables), completed async functions")]
    public ValueTask<double> MathEvaluator_AsyncCompiled_CompletedFunctions()
        => _mathEvalAsyncCompiledFn(_variables, CancellationToken.None);

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler: await fn(variables), completed async functions")]
    public ValueTask<double> MathEvaluator_FastExpressionCompiler_AsyncCompiled_CompletedFunctions()
        => _mathEvalFastAsyncCompiledFn(_variables, CancellationToken.None);

    [Benchmark(Description = "MathEvaluator: await fn(variables), yielding async functions")]
    public async Task<double> MathEvaluator_AsyncCompiled_YieldingFunctions()
        => await _mathEvalAsyncYieldingCompiledFn(_variables, CancellationToken.None);

#pragma warning disable IDE1006
    public record struct Variables(double x, double qty);
#pragma warning restore IDE1006
}
