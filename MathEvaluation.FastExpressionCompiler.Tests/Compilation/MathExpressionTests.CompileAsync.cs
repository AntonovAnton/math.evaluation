using MathEvaluation.Context;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.FastExpressionCompiler.Tests.Compilation;

public partial class MathExpressionTests
{
    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasAsyncFunction_ExpectedValue()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>(async (x, _) =>
        {
            await Task.Delay(10);
            return x * 2;
        }, "f");

        using var expression = new FastMathExpression("f(x) + 1", context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc(new { x = 0d });
        var value = await fn(new { x = 3d }, CancellationToken.None);

        Assert.Equal(7d, value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasCompletedAsyncFunctions_ExpectedValue()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>((a, b, _) => ValueTask.FromResult(a + b), "add");
        context.BindAsyncFunction<double>((args, _) => ValueTask.FromResult(args.Max()), "max");

        using var expression = new FastMathExpression("add(1, 2) * max(4, 5, 2) - add(max(1, 3), 1)", context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc<double>();
        var value = await fn(CancellationToken.None);

        Assert.Equal(3d * 5 - 4, value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasNestedAsyncFunctions_ExpectedValue()
    {
        var context = new ScientificMathContext();
        context.BindAsyncFunction<double>(async (x, _) =>
        {
            await Task.Yield();
            return x + 1;
        }, "f");
        context.BindAsyncFunction<double>(async (x, _) =>
        {
            await Task.Yield();
            return x * 10;
        }, "g");

        using var expression = new FastMathExpression("f(g(x) + f(x)) + sin(f(0))", context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc(new { x = 0d });
        var value = await fn(new { x = 2d }, CancellationToken.None);

        Assert.Equal(20d + 3 + 1 + Math.Sin(1), value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasExpressionVariableWithAsyncFunction_FunctionInvokedOnce()
    {
        var calls = 0;
        var context = new MathContext();
        context.BindAsyncFunction<double>(async (x, _) =>
        {
            Interlocked.Increment(ref calls);
            await Task.Delay(1);
            return x * 2;
        }, "f");

        var y = "f(x1) + x2";
        using var expression = new FastMathExpression("y * y + x1", context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc(new { y, x1 = 0d, x2 = 0d });
        var value = await fn(new { y, x1 = 1.5d, x2 = 1d }, CancellationToken.None);

        Assert.Equal(4d * 4 + 1.5, value);
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasAsyncFunctionInParameters_ExpectedValue()
    {
        Func<double, CancellationToken, ValueTask<double>> f = (x, _) => ValueTask.FromResult(x * x);
        using var expression = new FastMathExpression("f(3) + 1 + 2", null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc(new { f });
        var value = await fn(new { f }, CancellationToken.None);

        Assert.Equal(12d, value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_ResultIsDecimalOrComplex_ExpectedValue()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>(async (x, _) =>
        {
            await Task.Yield();
            return x / 2;
        }, "f");

        using var decimalExpression = new FastMathExpression("f(5) + 2 + 0.1", context, CultureInfo.InvariantCulture);
        var decimalValue = await decimalExpression.CompileAsyncFunc<decimal>()(CancellationToken.None);
        Assert.Equal(4.6m, decimalValue);

        using var complexExpression = new FastMathExpression("f(5) + i", context, CultureInfo.InvariantCulture);
        var complexValue = await complexExpression.CompileAsyncFunc<Complex>()(CancellationToken.None);
        Assert.Equal(new Complex(2.5, 1), complexValue);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_NoAsyncFunctions_ExpectedValue()
    {
        var x = "x1 * 2";
        using var expression = new FastMathExpression("x + x1", null, CultureInfo.InvariantCulture);

        var fn = expression.CompileAsyncFunc(new { x, x1 = 0d });
        expression.Evaluating += SubscribeToEvaluating;
        var value = await fn(new { x, x1 = 2d }, CancellationToken.None);

        Assert.Equal(6d, value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_AsyncFunctionThrows_ThrowException()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>((double _, CancellationToken _) => throw new InvalidOperationException("sync"), "f");
        context.BindAsyncFunction<double>(async (double _, CancellationToken _) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("async");
        }, "g");

        using var syncExpression = new FastMathExpression("1 + f(1)", context, CultureInfo.InvariantCulture);
        var syncEx = await Record.ExceptionAsync(async () => await syncExpression.CompileAsyncFunc<double>()(CancellationToken.None));
        Assert.Equal("sync", Assert.IsType<InvalidOperationException>(syncEx).Message);

        using var asyncExpression = new FastMathExpression("1 + g(1)", context, CultureInfo.InvariantCulture);
        var asyncEx = await Record.ExceptionAsync(async () => await asyncExpression.CompileAsyncFunc<double>()(CancellationToken.None));
        Assert.Equal("async", Assert.IsType<InvalidOperationException>(asyncEx).Message);
    }

    [Fact]
    public void MathExpression_CompileOrEvaluate_HasAsyncFunction_ThrowMathExpressionException()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>((x, _) => ValueTask.FromResult(x), "f");

        using var expression = new FastMathExpression("f(1) + 1", context, CultureInfo.InvariantCulture);

        var compileEx = Record.Exception(() => expression.Compile());
        Assert.IsType<NotSupportedException>(Assert.IsType<MathExpressionException>(compileEx).InnerException);

        var evaluateEx = Record.Exception(() => expression.Evaluate());
        Assert.IsType<NotSupportedException>(Assert.IsType<MathExpressionException>(evaluateEx).InnerException);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasSameAsyncCallsAndVariable_ExpectedValue()
    {
        var calls = 0;
        var context = new MathContext();
        context.BindAsyncFunction<double>((x, _) =>
        {
            Interlocked.Increment(ref calls);
            return ValueTask.FromResult(x * 2);
        }, "f");
        var f_0 = "x1 + 1";

        using var expression = new FastMathExpression("f(1) + f(1) + f_0", context, CultureInfo.InvariantCulture);

        var fn = expression.CompileAsyncFunc(new { f_0, x1 = 0d });
        var value = await fn(new { f_0, x1 = 3d }, CancellationToken.None);

        Assert.Equal(2d + 2 + 4, value);
        Assert.Equal(2, calls);
    }

    [Fact]
    public async Task MathExpression_CompileTypedAsyncFuncThenInvoke_HasAsyncFunction_ExpectedValue()
    {
        var context = new ProgrammingMathContext();
        context.BindAsyncFunction<double>(async (x, _) =>
        {
            await Task.Yield();
            return x / 2;
        }, "f");
        context.BindAsyncFunction<decimal>((x, _) => ValueTask.FromResult(x * 1.5m), "g");

        using var booleanExpression = new FastMathExpression("f(x) > 2 and not f(0) > 0", context, CultureInfo.InvariantCulture);
        var booleanFn = booleanExpression.CompileBooleanAsyncFunc(new { x = 0d });
        Assert.True(await booleanFn(new { x = 5d }, CancellationToken.None));
        Assert.False(await booleanFn(new { x = 4d }, CancellationToken.None));
        Assert.True(await new FastMathExpression("f(6) = 3", context, CultureInfo.InvariantCulture).CompileBooleanAsyncFunc()(CancellationToken.None));

        using var decimalExpression = new FastMathExpression("g(x) + 0.1", context, CultureInfo.InvariantCulture);
        var decimalFn = decimalExpression.CompileDecimalAsyncFunc(new { x = 0m });
        Assert.Equal(3.1m, await decimalFn(new { x = 2m }, CancellationToken.None));
        Assert.Equal(1.6m, await new FastMathExpression("g(1) + 0.1", context, CultureInfo.InvariantCulture).CompileDecimalAsyncFunc()(CancellationToken.None));

        var complexContext = new ComplexScientificMathContext();
        complexContext.BindAsyncFunction<Complex>(async (z, _) =>
        {
            await Task.Yield();
            return z * Complex.ImaginaryOne;
        }, "rot");

        using var complexExpression = new FastMathExpression("rot(z) + 1", complexContext, CultureInfo.InvariantCulture);
        var complexFn = complexExpression.CompileComplexAsyncFunc(new { z = Complex.Zero });
        Assert.Equal(new Complex(-1, 2), await complexFn(new { z = new Complex(2, 2) }, CancellationToken.None));
        Assert.Equal(new Complex(1, 2), await new FastMathExpression("rot(2)+1", complexContext, CultureInfo.InvariantCulture).CompileComplexAsyncFunc()(CancellationToken.None));
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_PassesCancellationTokenToAsyncFunctions()
    {
        using var cts = new CancellationTokenSource();
        var tokens = new List<CancellationToken>();
        var context = new MathContext();
        context.BindAsyncFunction<double>(async (x, cancellationToken) =>
        {
            tokens.Add(cancellationToken);
            await Task.Yield();
            return x;
        }, "f");
        context.BindAsyncFunction<double>((a, b, cancellationToken) =>
        {
            tokens.Add(cancellationToken);
            return ValueTask.FromResult(a + b);
        }, "add");
        context.BindAsyncFunction<double>(cancellationToken =>
        {
            tokens.Add(cancellationToken);
            return ValueTask.FromResult(1d);
        }, "one");
        context.BindExpressionVariable("f(x) + one", "y");

        using var expression = new FastMathExpression("add(f(x), y)", context, CultureInfo.InvariantCulture);
        var fn = expression.CompileAsyncFunc(new { x = 0d });

        Assert.Equal(2d + 2 + 1, await fn(new { x = 2d }, cts.Token));

        // f(x), f(x) and one of the expression variable, add
        Assert.Equal(4, tokens.Count);
        Assert.All(tokens, token => Assert.Equal(cts.Token, token));
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_TokenIsCancelled_ThrowOperationCanceledExceptionAndFunctionIsNotInvoked()
    {
        var calls = 0;
        var context = new MathContext();
        context.BindAsyncFunction<double>((x, _) =>
        {
            Interlocked.Increment(ref calls);
            return ValueTask.FromResult(x);
        }, "f");

        using var expression = new FastMathExpression("f(1) + 1", context, CultureInfo.InvariantCulture);
        var fn = expression.CompileAsyncFunc();

        var ex = await Record.ExceptionAsync(async () => await fn(new CancellationToken(true)));

        Assert.IsAssignableFrom<OperationCanceledException>(ex);
        Assert.Equal(0, calls);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_TokenIsCancelledBetweenAwaits_NextFunctionIsNotInvoked()
    {
        var calls = new List<string>();
        CancellationTokenSource? cts = null;
        var context = new MathContext();

        // completes asynchronously, so the token is checked in the continuation after the await
        context.BindAsyncFunction<double>(async (x, _) =>
        {
            calls.Add($"f({x})");
            await Task.Yield();
            cts!.Cancel();
            return x;
        }, "f");

        // completes synchronously, so the token is checked in the continuation that is invoked immediately
        context.BindAsyncFunction<double>((x, _) =>
        {
            calls.Add($"g({x})");
            cts!.Cancel();
            return ValueTask.FromResult(x);
        }, "g");

        foreach (var (mathString, expectedCalls) in new[] { ("f(1) + f(2)", "f(1)"), ("g(1) + g(2)", "g(1)") })
        {
            calls.Clear();
            cts = new CancellationTokenSource();
            using var expression = new FastMathExpression(mathString, context, CultureInfo.InvariantCulture);
            var fn = expression.CompileAsyncFunc();

            var ex = await Record.ExceptionAsync(async () => await fn(cts.Token));

            Assert.IsAssignableFrom<OperationCanceledException>(ex);
            Assert.Equal([expectedCalls], calls);
            cts.Dispose();
        }
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_FunctionObservesCancellation_ThrowOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        var context = new MathContext();
        context.BindAsyncFunction<double>(async (x, cancellationToken) =>
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
            return x;
        }, "wait");

        using var expression = new FastMathExpression("wait(1) + 1", context, CultureInfo.InvariantCulture);
        var task = expression.CompileAsyncFunc()(cts.Token).AsTask();
        await cts.CancelAsync();

        var ex = await Record.ExceptionAsync(() => task);

        Assert.IsAssignableFrom<OperationCanceledException>(ex);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_NoAsyncFunctionsAndTokenIsCancelled_ThrowOperationCanceledException()
    {
        using var expression = new FastMathExpression("x + 1", null, CultureInfo.InvariantCulture);
        var fn = expression.CompileAsyncFunc(new { x = 0d });

        var ex = await Record.ExceptionAsync(async () => await fn(new { x = 1d }, new CancellationToken(true)));

        Assert.IsAssignableFrom<OperationCanceledException>(ex);
        Assert.Equal(2d, await fn(new { x = 1d }, CancellationToken.None));
    }
}
