using MathEvaluation.Context;
using System.Globalization;

namespace MathEvaluation.Tests.Compilation;

public partial class MathExpressionTests
{
    private static MathContext CreateBooleanAsyncContext(MathContext context)
    {
        // completes asynchronously, so the continuations are tested as well
        context.BindAsyncFunction<double>(async x =>
        {
            await Task.Yield();
            return x;
        }, "f");
        context.BindAsyncFunction<double>((a, b) => ValueTask.FromResult(Math.Max(a, b)), "max");
        return context;
    }

    [Theory]
    [InlineData("f(0) = f(1)", false)]
    [InlineData("not(f(0))", true)]
    [InlineData("f(0) <> True", true)]
    [InlineData("f(0) or TRUE", true)]
    [InlineData("f(1) xor f(1)", false)]
    [InlineData("f(200) >= 2.4", 200 >= 2.4)]
    [InlineData("200 <= f(2.4)", 200 <= 2.4)]
    [InlineData("f(1.0) >= 0.1 and 5.4 <= f(5.4)", (1.0 >= 0.1) & (5.4 <= 5.4))]
    [InlineData("1.0 - f(1.95) >= 0.1", 1.0 - 1.95 >= 0.1)]
    [InlineData("f(2) ** 3 = 8", true)]
    [InlineData("max(3, f(2)) % 2 <> 1.1", true)]
    [InlineData("4 <> f(4) OR 5.4 = max(5.4, f(1)) AND NOT true", false)]
    [InlineData("4 <> f(4) OR 5.4 = f(5.4) AND NOT 0 < f(1) XOR 1.0 - 1.95 * f(2) >= -12.9 + 0.1 / 0.01", true)]
    public async Task MathExpression_CompileBooleanAsyncFuncThenInvoke_HasProgrammingBooleanLogic_ExpectedValue(string mathString, bool expectedValue)
    {
        var context = CreateBooleanAsyncContext(new ProgrammingMathContext());
        using var expression = new MathExpression(mathString, context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileBooleanAsyncFunc();
        var value = await fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("¬f(0)", true)]
    [InlineData("f(0) ≠ ⊤", true)]
    [InlineData("f(200) ≥ 2.4 ∧ f(1) ⪯ 0.1", false)]
    [InlineData("f(1) ∨ f(0) ⇒ f(0)", false)]
    [InlineData("f(1) ↔ max(f(0), 1)", true)]
    [InlineData("√f(16) = 4 ∧ sin(f(0)) = 0", true)]
    public async Task MathExpression_CompileBooleanAsyncFuncThenInvoke_HasScientificBooleanLogic_ExpectedValue(string mathString, bool expectedValue)
    {
        var context = CreateBooleanAsyncContext(new ScientificMathContext());
        using var expression = new MathExpression(mathString, context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileBooleanAsyncFunc();
        var value = await fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("f(A) or not f(B) and (C or B)", true, false, true, true)]
    [InlineData("f(A) and f(B) xor f(C)", true, true, true, false)]
    [InlineData("not (f(A) or f(B)) and C", false, false, true, true)]
    public async Task MathExpression_CompileBooleanAsyncFuncThenInvoke_HasVariables_ExpectedValue(string mathString, bool a, bool b, bool c, bool expectedValue)
    {
        var context = CreateBooleanAsyncContext(new ProgrammingMathContext());
        using var expression = new MathExpression(mathString, context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileBooleanAsyncFunc(new { A = false, B = false, C = false });
        var value = await fn(new { A = a, B = b, C = c });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("iif(f(a) > 5, f(a), 0) = a", 7d, true)]
    [InlineData("iif(f(a) > 5, f(a), 0) = a", 3d, false)]
    [InlineData("iif(max(a, 2) = 2, true, false)", 1d, true)]
    public async Task MathExpression_CompileBooleanAsyncFuncThenInvoke_HasIif_ExpectedValue(string mathString, double a, bool expectedValue)
    {
        var context = CreateBooleanAsyncContext(new ProgrammingMathContext());
        using var expression = new MathExpression(mathString, context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileBooleanAsyncFunc(new { a = 0d });
        var value = await fn(new { a });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public async Task MathExpression_CompileBooleanAsyncFuncThenInvoke_HasExpressionVariable_ExpectedValue()
    {
        var x = "f(x1) + x2";
        var mathString = "x = 0.7 and x > f(x2)";
        var context = CreateBooleanAsyncContext(new ScientificMathContext());
        using var expression = new MathExpression(mathString, context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileBooleanAsyncFunc(new { x, x1 = 0d, x2 = 0d });
        var value = await fn(new { x, x1 = 0.5d, x2 = 0.2d });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.True(value);
    }

    [Fact]
    public async Task MathExpression_CompileBooleanAsyncFuncThenInvoke_AsyncFunctionThrows_ThrowException()
    {
        var context = new ProgrammingMathContext();
        context.BindAsyncFunction<double>(async (double _) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("async");
        }, "g");

        using var expression = new MathExpression("g(1) > 0 or true", context, CultureInfo.InvariantCulture);
        var fn = expression.CompileBooleanAsyncFunc();

        var ex = await Record.ExceptionAsync(async () => await fn());
        Assert.Equal("async", Assert.IsType<InvalidOperationException>(ex).Message);
    }

    [Fact]
    public void MathExpression_CompileBoolean_HasAsyncFunction_ThrowMathExpressionException()
    {
        var context = CreateBooleanAsyncContext(new ProgrammingMathContext());
        using var expression = new MathExpression("f(1) > 0", context, CultureInfo.InvariantCulture);

        var compileEx = Record.Exception(() => expression.CompileBoolean());
        Assert.IsType<NotSupportedException>(Assert.IsType<MathExpressionException>(compileEx).InnerException);

        var evaluateEx = Record.Exception(() => expression.EvaluateBoolean());
        Assert.IsType<NotSupportedException>(Assert.IsType<MathExpressionException>(evaluateEx).InnerException);
    }
}
