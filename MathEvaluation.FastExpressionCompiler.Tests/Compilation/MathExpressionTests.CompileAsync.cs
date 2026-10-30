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
        context.BindAsyncFunction<double>(async x =>
        {
            await Task.Delay(10);
            return x * 2;
        }, "f");

        using var expression = new FastMathExpression("f(x) + 1", context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc(new { x = 0d });
        var value = await fn(new { x = 3d });

        Assert.Equal(7d, value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasCompletedAsyncFunctions_ExpectedValue()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>((a, b) => ValueTask.FromResult(a + b), "add");
        context.BindAsyncFunction<double>(args => ValueTask.FromResult(args.Max()), "max");

        using var expression = new FastMathExpression("add(1, 2) * max(4, 5, 2) - add(max(1, 3), 1)", context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc<double>();
        var value = await fn();

        Assert.Equal(3d * 5 - 4, value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasNestedAsyncFunctions_ExpectedValue()
    {
        var context = new ScientificMathContext();
        context.BindAsyncFunction<double>(async x =>
        {
            await Task.Yield();
            return x + 1;
        }, "f");
        context.BindAsyncFunction<double>(async x =>
        {
            await Task.Yield();
            return x * 10;
        }, "g");

        using var expression = new FastMathExpression("f(g(x) + f(x)) + sin(f(0))", context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc(new { x = 0d });
        var value = await fn(new { x = 2d });

        Assert.Equal(20d + 3 + 1 + Math.Sin(1), value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasExpressionVariableWithAsyncFunction_FunctionInvokedOnce()
    {
        var calls = 0;
        var context = new MathContext();
        context.BindAsyncFunction<double>(async x =>
        {
            Interlocked.Increment(ref calls);
            await Task.Delay(1);
            return x * 2;
        }, "f");

        var y = "f(x1) + x2";
        using var expression = new FastMathExpression("y * y + x1", context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc(new { y, x1 = 0d, x2 = 0d });
        var value = await fn(new { y, x1 = 1.5d, x2 = 1d });

        Assert.Equal(4d * 4 + 1.5, value);
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasAsyncFunctionInParameters_ExpectedValue()
    {
        Func<double, ValueTask<double>> f = x => ValueTask.FromResult(x * x);
        using var expression = new FastMathExpression("f(3) + 1 + 2", null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileAsyncFunc(new { f });
        var value = await fn(new { f });

        Assert.Equal(12d, value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_ResultIsDecimalOrComplex_ExpectedValue()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>(async x =>
        {
            await Task.Yield();
            return x / 2;
        }, "f");

        using var decimalExpression = new FastMathExpression("f(5) + 2 + 0.1", context, CultureInfo.InvariantCulture);
        var decimalValue = await decimalExpression.CompileAsyncFunc<decimal>()();
        Assert.Equal(4.6m, decimalValue);

        using var complexExpression = new FastMathExpression("f(5) + i", context, CultureInfo.InvariantCulture);
        var complexValue = await complexExpression.CompileAsyncFunc<Complex>()();
        Assert.Equal(new Complex(2.5, 1), complexValue);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_NoAsyncFunctions_ExpectedValue()
    {
        var x = "x1 * 2";
        using var expression = new FastMathExpression("x + x1", null, CultureInfo.InvariantCulture);

        var fn = expression.CompileAsyncFunc(new { x, x1 = 0d });
        expression.Evaluating += SubscribeToEvaluating;
        var value = await fn(new { x, x1 = 2d });

        Assert.Equal(6d, value);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_AsyncFunctionThrows_ThrowException()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>((double _) => throw new InvalidOperationException("sync"), "f");
        context.BindAsyncFunction<double>(async (double _) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("async");
        }, "g");

        using var syncExpression = new FastMathExpression("1 + f(1)", context, CultureInfo.InvariantCulture);
        var syncEx = await Record.ExceptionAsync(async () => await syncExpression.CompileAsyncFunc<double>()());
        Assert.Equal("sync", Assert.IsType<InvalidOperationException>(syncEx).Message);

        using var asyncExpression = new FastMathExpression("1 + g(1)", context, CultureInfo.InvariantCulture);
        var asyncEx = await Record.ExceptionAsync(async () => await asyncExpression.CompileAsyncFunc<double>()());
        Assert.Equal("async", Assert.IsType<InvalidOperationException>(asyncEx).Message);
    }

    [Fact]
    public void MathExpression_CompileOrEvaluate_HasAsyncFunction_ThrowMathExpressionException()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>(x => ValueTask.FromResult(x), "f");

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
        context.BindAsyncFunction<double>(x =>
        {
            Interlocked.Increment(ref calls);
            return ValueTask.FromResult(x * 2);
        }, "f");
        var f_0 = "x1 + 1";

        using var expression = new FastMathExpression("f(1) + f(1) + f_0", context, CultureInfo.InvariantCulture);

        var fn = expression.CompileAsyncFunc(new { f_0, x1 = 0d });
        var value = await fn(new { f_0, x1 = 3d });

        Assert.Equal(2d + 2 + 4, value);
        Assert.Equal(2, calls);
    }

    [Fact]
    public async Task MathExpression_CompileTypedAsyncFuncThenInvoke_HasAsyncFunction_ExpectedValue()
    {
        var context = new ProgrammingMathContext();
        context.BindAsyncFunction<double>(async x =>
        {
            await Task.Yield();
            return x / 2;
        }, "f");
        context.BindAsyncFunction<decimal>(x => ValueTask.FromResult(x * 1.5m), "g");

        using var booleanExpression = new FastMathExpression("f(x) > 2 and not f(0) > 0", context, CultureInfo.InvariantCulture);
        var booleanFn = booleanExpression.CompileBooleanAsyncFunc(new { x = 0d });
        Assert.True(await booleanFn(new { x = 5d }));
        Assert.False(await booleanFn(new { x = 4d }));
        Assert.True(await new FastMathExpression("f(6) = 3", context, CultureInfo.InvariantCulture).CompileBooleanAsyncFunc()());

        using var decimalExpression = new FastMathExpression("g(x) + 0.1", context, CultureInfo.InvariantCulture);
        var decimalFn = decimalExpression.CompileDecimalAsyncFunc(new { x = 0m });
        Assert.Equal(3.1m, await decimalFn(new { x = 2m }));
        Assert.Equal(1.6m, await new FastMathExpression("g(1) + 0.1", context, CultureInfo.InvariantCulture).CompileDecimalAsyncFunc()());

        var complexContext = new ComplexScientificMathContext();
        complexContext.BindAsyncFunction<Complex>(async z =>
        {
            await Task.Yield();
            return z * Complex.ImaginaryOne;
        }, "rot");

        using var complexExpression = new FastMathExpression("rot(z) + 1", complexContext, CultureInfo.InvariantCulture);
        var complexFn = complexExpression.CompileComplexAsyncFunc(new { z = Complex.Zero });
        Assert.Equal(new Complex(-1, 2), await complexFn(new { z = new Complex(2, 2) }));
        Assert.Equal(new Complex(1, 2), await new FastMathExpression("rot(2)+1", complexContext, CultureInfo.InvariantCulture).CompileComplexAsyncFunc()());
    }
}
