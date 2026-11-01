using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Tests.Compilation;

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

        using var expression = new MathExpression("f(x) + 1", context, CultureInfo.InvariantCulture);
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

        using var expression = new MathExpression("add(1, 2) * max(4, 5, 2) - add(max(1, 3), 1)", context, CultureInfo.InvariantCulture);

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

        using var expression = new MathExpression("f(g(x) + f(x)) + sin(f(0))", context, CultureInfo.InvariantCulture);

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
        using var expression = new MathExpression("y * y + x1", context, CultureInfo.InvariantCulture);

        var fn = expression.CompileAsyncFunc(new { y, x1 = 0d, x2 = 0d });
        var value = await fn(new { y, x1 = 1.5d, x2 = 1d });

        Assert.Equal(4d * 4 + 1.5, value);
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasAsyncFunctionInParameters_ExpectedValue()
    {
        Func<double, ValueTask<double>> f = x => ValueTask.FromResult(x * x);
        using var expression = new MathExpression("f(3) + 1", null, CultureInfo.InvariantCulture);

        var fn = expression.CompileAsyncFunc(new { f });
        var value = await fn(new { f });

        Assert.Equal(10d, value);
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

        using var decimalExpression = new MathExpression("f(5) + 0.1", context, CultureInfo.InvariantCulture);
        var decimalValue = await decimalExpression.CompileAsyncFunc<decimal>()();
        Assert.Equal(2.6m, decimalValue);

        using var complexExpression = new MathExpression("f(5) + i", context, CultureInfo.InvariantCulture);
        var complexValue = await complexExpression.CompileAsyncFunc<Complex>()();
        Assert.Equal(new Complex(2.5, 1), complexValue);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_NoAsyncFunctions_ExpectedValue()
    {
        var x = "x1 * 2";
        using var expression = new MathExpression("x + x1", null, CultureInfo.InvariantCulture);

        var fn = expression.CompileAsyncFunc(new { x, x1 = 0d });
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

        using var syncExpression = new MathExpression("1 + f(1)", context, CultureInfo.InvariantCulture);
        var syncEx = await Record.ExceptionAsync(async () => await syncExpression.CompileAsyncFunc<double>()());
        Assert.Equal("sync", Assert.IsType<InvalidOperationException>(syncEx).Message);

        using var asyncExpression = new MathExpression("1 + g(1)", context, CultureInfo.InvariantCulture);
        var asyncEx = await Record.ExceptionAsync(async () => await asyncExpression.CompileAsyncFunc<double>()());
        Assert.Equal("async", Assert.IsType<InvalidOperationException>(asyncEx).Message);
    }

    [Fact]
    public void MathExpression_CompileOrEvaluate_HasAsyncFunction_ThrowMathExpressionException()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>(x => ValueTask.FromResult(x), "f");

        using var expression = new MathExpression("f(1) + 1", context, CultureInfo.InvariantCulture);

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

        using var expression = new MathExpression("f(1) + f(1) + f_0", context, CultureInfo.InvariantCulture);

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

        using var booleanExpression = new MathExpression("f(x) > 2 and not f(0) > 0", context, CultureInfo.InvariantCulture);
        var booleanFn = booleanExpression.CompileBooleanAsyncFunc(new { x = 0d });
        Assert.True(await booleanFn(new { x = 5d }));
        Assert.False(await booleanFn(new { x = 4d }));
        Assert.True(await new MathExpression("f(6) = 3", context, CultureInfo.InvariantCulture).CompileBooleanAsyncFunc()());

        using var decimalExpression = new MathExpression("g(x) + 0.1", context, CultureInfo.InvariantCulture);
        var decimalFn = decimalExpression.CompileDecimalAsyncFunc(new { x = 0m });
        Assert.Equal(3.1m, await decimalFn(new { x = 2m }));
        Assert.Equal(1.6m, await new MathExpression("g(1) + 0.1", context, CultureInfo.InvariantCulture).CompileDecimalAsyncFunc()());

        var complexContext = new ComplexScientificMathContext();
        complexContext.BindAsyncFunction<Complex>(async z =>
        {
            await Task.Yield();
            return z * Complex.ImaginaryOne;
        }, "rot");

        using var complexExpression = new MathExpression("rot(z) + 1", complexContext, CultureInfo.InvariantCulture);
        var complexFn = complexExpression.CompileComplexAsyncFunc(new { z = Complex.Zero });
        Assert.Equal(new Complex(-1, 2), await complexFn(new { z = new Complex(2, 2) }));
        Assert.Equal(new Complex(1, 2), await new MathExpression("rot(2)+1", complexContext, CultureInfo.InvariantCulture).CompileComplexAsyncFunc()());
    }

    [Fact]
    public async Task StringExtensions_CompileAsyncFunc_HasAsyncFunction_ExpectedValue()
    {
        var context = new ProgrammingMathContext();
        context.BindAsyncFunction<double>(async x =>
        {
            await Task.Yield();
            return x / 2;
        }, "f");
        context.BindAsyncFunction<decimal>(x => ValueTask.FromResult(x * 1.5m), "g");
        context.BindAsyncFunction<Complex>(async z =>
        {
            await Task.Yield();
            return z * Complex.ImaginaryOne;
        }, "rot");

        var doubleFn = "f(x) + 1".CompileAsyncFunc(new { x = 0d }, context);
        Assert.Equal(3.5d, await doubleFn(new { x = 5d }));

        var numberFn = "g(x) + 0.1".CompileAsyncFunc<DecimalVariables, decimal>(new DecimalVariables { x = 0m }, context);
        Assert.Equal(3.1m, await numberFn(new DecimalVariables { x = 2m }));

        var decimalFn = "g(x) + 0.1".CompileDecimalAsyncFunc(new { x = 0m }, context);
        Assert.Equal(3.1m, await decimalFn(new { x = 2m }));

        var booleanFn = "f(x) > 2 and not f(0) > 0".CompileBooleanAsyncFunc(new { x = 0d }, context);
        Assert.True(await booleanFn(new { x = 5d }));
        Assert.False(await booleanFn(new { x = 4d }));

        var complexFn = "rot(z) + 1".CompileComplexAsyncFunc(new { z = Complex.Zero }, context);
        Assert.Equal(new Complex(-1, 2), await complexFn(new { z = new Complex(2, 2) }));
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasAsyncGetValueFunction_ExpectedValue()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>(async () =>
        {
            await Task.Yield();
            return 5d;
        }, "five");
        context.BindAsyncFunction<double>(() => ValueTask.FromResult(7d), 's');

        using var expression = new MathExpression("five * 2 + five() + 2s", context, CultureInfo.InvariantCulture);
        var fn = expression.CompileAsyncFunc();

        Assert.Equal(5d * 2 + 5 + 2 * 7, await fn());
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasAsyncUnaryFunction_ExpectedValue()
    {
        var context = new MathContext();
        context.BindAsyncFunction<double>(async v =>
        {
            await Task.Yield();
            return v * 2;
        }, "twice");
        context.BindAsyncFunction<double>(async v =>
        {
            await Task.Yield();
            return Math.Abs(v);
        }, '[', ']');
        context.BindAsyncFunction<double>(async v =>
        {
            await Task.Yield();
            return v / 2;
        }, "half", '<', '>');
        context.BindAsyncFunction<double>(v => ValueTask.FromResult(v * v), 'q');

        // without brackets, with brackets, nested without brackets, custom symbols, custom symbols with a key, a char key
        foreach (var (mathString, expectedValue) in new[]
                 {
                     ("twice 3 + 1", 7d),
                     ("twice(3) + 1", 7d),
                     ("twice twice 2", 8d),
                     ("twice(1 + 2) * twice 2", 24d),
                     ("[3 - 5] * 2", 4d),
                     ("half<8> + 1", 5d),
                     ("q 3 + 1", 10d)
                 })
        {
            using var expression = new MathExpression(mathString, context, CultureInfo.InvariantCulture);
            var fn = expression.CompileAsyncFunc();

            Assert.Equal(expectedValue, await fn());
        }
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasAsyncBooleanFunction_ExpectedValue()
    {
        var context = new ProgrammingMathContext();
        context.BindAsyncFunction(async () =>
        {
            await Task.Yield();
            return true;
        }, "ready");
        context.BindAsyncFunction(() => ValueTask.FromResult(false), "stopped");
        context.BindAsyncFunction(() => ValueTask.FromResult(true), 'z');

        using var readyExpression = new MathExpression("ready and not stopped and z", context, CultureInfo.InvariantCulture);
        Assert.True(await readyExpression.CompileBooleanAsyncFunc()());

        using var stoppedExpression = new MathExpression("ready and stopped", context, CultureInfo.InvariantCulture);
        Assert.False(await stoppedExpression.CompileBooleanAsyncFunc()());

        // a boolean is a number in a math expression
        using var numberExpression = new MathExpression("ready + stopped + 1", context, CultureInfo.InvariantCulture);
        Assert.Equal(2d, await numberExpression.CompileAsyncFunc()());
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasAsyncFunctionsInObjects_ExpectedValue()
    {
        var five = (Func<ValueTask<double>>)(() => ValueTask.FromResult(5d));
        var ready = (Func<ValueTask<bool>>)(() => ValueTask.FromResult(true));

        // in parameters
        using var parametersExpression = new MathExpression("five * 2 + ready", null, CultureInfo.InvariantCulture);
        var parametersFn = parametersExpression.CompileAsyncFunc(new { five, ready });
        Assert.Equal(11d, await parametersFn(new { five, ready }));

        // in context
        var context = new MathContext(new { five, ready });
        using var contextExpression = new MathExpression("five * x + ready", context, CultureInfo.InvariantCulture);
        var contextFn = contextExpression.CompileAsyncFunc(new { x = 0d });
        Assert.Equal(16d, await contextFn(new { x = 3d }));
    }

    private record DecimalVariables
    {
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable once InconsistentNaming
        public decimal x { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    [Fact]
    public void MathExpression_CompileAsyncFunc_HasTaskFunctionInParameters_ThrowExceptionWithHowToWrap()
    {
        Func<double, Task<double>> f = x => Task.FromResult(x);
        using var expression = new MathExpression("f(1)", null, CultureInfo.InvariantCulture);

        var ex = Record.Exception(() => expression.CompileAsyncFunc(new { f }));

        var inner = Assert.IsType<NotSupportedException>(Assert.IsType<MathExpressionException>(ex).InnerException);
        Assert.Contains("'f'", inner.Message);
        Assert.Contains("ValueTask<T>", inner.Message);
        Assert.Contains("new ValueTask<T>(f(x))", inner.Message);
        Assert.DoesNotContain("Func<T[], T>", inner.Message);
    }

    [Fact]
    public void MathContext_BindObjectWithAsyncFunctionsOfWrongTypes_ThrowExceptionWithHint()
    {
        Func<double, Task<double>> taskFn = x => Task.FromResult(x);
        Func<int, ValueTask<double>> differentTypesFn = x => ValueTask.FromResult((double)x);
        Func<int, string> notNumberFn = x => x.ToString();

        var taskEx = Assert.Throws<NotSupportedException>(() => new MathContext(new { taskFn }));
        Assert.Contains("new ValueTask<T>(f(x))", taskEx.Message);

        var typesEx = Assert.Throws<NotSupportedException>(() => new MathContext(new { differentTypesFn }));
        Assert.Contains("Func<T[], ValueTask<T>>", typesEx.Message);

        var otherEx = Assert.Throws<NotSupportedException>(() => new MathContext(new { notNumberFn }));
        Assert.Contains("Func<T[], T>", otherEx.Message);

        var dictionary = new Dictionary<string, object> { ["f"] = taskFn };
        var dictionaryEx = Record.Exception(() => new MathExpression("f(1)").CompileAsyncFunc(dictionary));
        Assert.Contains("new ValueTask<T>(f(x))", dictionaryEx?.Message);
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_FunctionIsPassedToDelegate_CompileTimeFunctionIsUsed()
    {
        Func<double, ValueTask<double>> a = x => ValueTask.FromResult(x + 100);
        Func<double, ValueTask<double>> b = x => ValueTask.FromResult(x + 200);
        using var expression = new MathExpression("f(x)", null, CultureInfo.InvariantCulture);

        var fn = expression.CompileAsyncFunc(new { f = a, x = 0d });

        // the function is taken from the object passed at compile time, only the values of variables are taken from the argument
        Assert.Equal(101d, await fn(new { f = b, x = 1d }));
    }

    [Fact]
    public async Task MathExpression_CompileAsyncFuncThenInvoke_HasExpressionVariableWithAsyncFunctionInContext_FunctionInvokedOnce()
    {
        var calls = 0;
        var context = new MathContext();
        context.BindAsyncFunction<double>(async id =>
        {
            Interlocked.Increment(ref calls);
            await Task.Yield();
            return id * 10;
        }, "price");
        context.BindExpressionVariable("price(id) * qty", "subtotal");

        using var expression = new MathExpression("subtotal + subtotal * 0.2", context, CultureInfo.InvariantCulture);
        var fn = expression.CompileAsyncFunc(new { id = 0, qty = 0d });

        Assert.Equal(42d * 10 * 3 * 1.2, await fn(new { id = 42, qty = 3d }), precision: 5);
        Assert.Equal(1, calls);
    }
}
