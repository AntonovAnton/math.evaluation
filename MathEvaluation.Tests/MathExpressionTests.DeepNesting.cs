using MathEvaluation.Context;
using System.Globalization;
using System.Runtime.ExceptionServices;

namespace MathEvaluation.Tests;

/// <summary>
///     Tests of deeply nested math expression strings.
///     The parser uses recursive descent, so the nesting depth is limited by the call stack.
///     The tests of the guard run on a dedicated thread that has a small call stack, so the guard trips
///     at a small nesting depth. It keeps them fast and independent of the call stack of the test host.
/// </summary>
public class MathExpressionDeepNestingTests(ITestOutputHelper testOutputHelper)
{
    /// <summary>The nesting depth that has to be evaluated on the default call stack of a test thread (1 MB).</summary>
    private const int SupportedNestingDepth = 200;

    /// <summary>
    ///     A small call stack size, the guard trips on it at the nesting depth of 337-1169,
    ///     depending on what is nested, so the tests of the guard need no huge math expression string.
    /// </summary>
    private const int SmallStackSize = 512 * 1024;

    /// <summary>
    ///     A call stack size that is 8 times bigger, the guard trips on it at the nesting depth of 3489-12081,
    ///     so it evaluates the <see cref="DeepNestingDepth" /> that the <see cref="SmallStackSize" /> rejects.
    /// </summary>
    private const int BigStackSize = 4 * 1024 * 1024;

    /// <summary>
    ///     The nesting depth that doesn't fit into the <see cref="SmallStackSize" /> call stack,
    ///     but is evaluated correctly on the <see cref="BigStackSize" /> one.
    /// </summary>
    private const int DeepNestingDepth = 1500;

    /// <summary>The nesting depth that doesn't fit into the <see cref="SmallStackSize" /> call stack in any case.</summary>
    private const int ExcessiveNestingDepth = 5000;

    #region nesting depth that is supported on the default call stack

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(SupportedNestingDepth)]
    public void MathExpression_Evaluate_NestedParentheses_ExpectedValue(int depth)
    {
        var mathString = NestInParentheses("2*3+1", depth);
        testOutputHelper.WriteLine($"Nesting depth: {depth}");

        var value = new MathExpression(mathString, null, CultureInfo.InvariantCulture).Evaluate();

        Assert.Equal(7d, value);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(SupportedNestingDepth)]
    public void MathExpression_EvaluateDecimal_NestedParentheses_ExpectedValue(int depth)
    {
        var mathString = NestInParentheses("2*3+1", depth);
        testOutputHelper.WriteLine($"Nesting depth: {depth}");

        var value = new MathExpression(mathString, null, CultureInfo.InvariantCulture).EvaluateDecimal();

        Assert.Equal(7m, value);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(SupportedNestingDepth)]
    public void MathExpression_Compile_NestedParentheses_ExpectedValue(int depth)
    {
        var mathString = NestInParentheses("2*3+1", depth);
        testOutputHelper.WriteLine($"Nesting depth: {depth}");

        var fn = new MathExpression(mathString, null, CultureInfo.InvariantCulture).Compile();

        Assert.Equal(7d, fn());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(SupportedNestingDepth)]
    public void MathExpression_Evaluate_NestedFunctions_ExpectedValue(int depth)
    {
        var mathString = NestInFunction("abs", "-7", depth);
        testOutputHelper.WriteLine($"Nesting depth: {depth}");

        var value = new MathExpression(mathString, new ScientificMathContext(), CultureInfo.InvariantCulture).Evaluate();

        Assert.Equal(7d, value);
    }

    /// <summary>
    ///     A long chain of binary operators is not a nesting, so it must not consume the call stack.
    /// </summary>
    [Theory]
    [InlineData(1000)]
    [InlineData(100000)]
    public void MathExpression_Evaluate_LongChainOfOperators_ExpectedValue(int count)
    {
        var mathString = string.Join(" + ", Enumerable.Repeat("1", count));
        testOutputHelper.WriteLine($"Operators count: {count - 1}");

        var value = new MathExpression(mathString, null, CultureInfo.InvariantCulture).Evaluate();

        Assert.Equal(count, value);
    }

    #endregion

    #region the limit is the call stack size, not a hardcoded depth

    /// <summary>
    ///     The same math expression string is rejected on a small call stack and evaluated correctly on a bigger one,
    ///     so the guard adapts to the call stack size instead of applying a hardcoded limit.
    /// </summary>
    [Fact]
    public void MathExpression_Evaluate_DeeplyNestedParentheses_SmallStack_ThrowMathExpressionException()
    {
        var mathString = NestInParentheses("2*3+1", DeepNestingDepth);
        testOutputHelper.WriteLine($"Nesting depth: {DeepNestingDepth}, stack size: {SmallStackSize} bytes");

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, null, CultureInfo.InvariantCulture).Evaluate(),
            SmallStackSize));

        Assert.IsType<MathExpressionException>(ex);
    }

    /// <inheritdoc cref="MathExpression_Evaluate_DeeplyNestedParentheses_SmallStack_ThrowMathExpressionException" />
    [Fact]
    public void MathExpression_Evaluate_DeeplyNestedParentheses_BigStack_ExpectedValue()
    {
        var mathString = NestInParentheses("2*3+1", DeepNestingDepth);
        testOutputHelper.WriteLine($"Nesting depth: {DeepNestingDepth}, stack size: {BigStackSize} bytes");

        var value = RunOnThread(
            () => new MathExpression(mathString, null, CultureInfo.InvariantCulture).Evaluate(),
            BigStackSize);

        Assert.Equal(7d, value);
    }

    #endregion

    #region nesting depth that overflows the call stack

    /// <summary>
    ///     Reproduces the issue #117: a nesting depth that doesn't fit into the call stack has to be reported as
    ///     a <see cref="MathExpressionException" /> instead of a <see cref="StackOverflowException" />.
    ///     A <see cref="StackOverflowException" /> cannot be caught, it terminates the process,
    ///     so before the parser guarded the call stack this test killed the test host with the exit code 139.
    /// </summary>
    [Fact]
    public void MathExpression_Evaluate_NestedParenthesesOfExcessiveDepth_ThrowMathExpressionException()
    {
        var mathString = NestInParentheses("2*3+1", ExcessiveNestingDepth);

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, null, CultureInfo.InvariantCulture).Evaluate(),
            SmallStackSize));

        var mathEx = Assert.IsType<MathExpressionException>(ex);
        testOutputHelper.WriteLine(mathEx.Message);

        Assert.True(mathEx.NestingDepth > SupportedNestingDepth, $"Nesting depth {mathEx.NestingDepth} is expected to be reported.");
        Assert.Equal(mathEx.NestingDepth, mathEx.Data["nestingDepth"]);
        Assert.Contains("nested too deeply", mathEx.Message);
    }

    /// <summary>The context of the failed evaluating has to be reported as usual.</summary>
    [Fact]
    public void MathExpression_Evaluate_NestedParenthesesOfExcessiveDepth_ExceptionDataIsFilled()
    {
        var mathString = NestInParentheses("2*3+1", ExcessiveNestingDepth);
        var parameters = new { a = 1d };

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, null, CultureInfo.InvariantCulture).Evaluate(parameters),
            SmallStackSize));

        var mathEx = Assert.IsType<MathExpressionException>(ex);

        Assert.Equal(mathString, mathEx.Data["mathString"]);
        Assert.Null(mathEx.Data["context"]);
        Assert.Equal(CultureInfo.InvariantCulture.NumberFormat, mathEx.Data["provider"]);
        Assert.Null(mathEx.Data["compiler"]);
        Assert.NotNull(mathEx.Data["parameters"]);
        Assert.Equal(mathEx.NestingDepth, mathEx.Data["nestingDepth"]);
    }

    [Fact]
    public void MathExpression_EvaluateDecimal_NestedParenthesesOfExcessiveDepth_ThrowMathExpressionException()
    {
        var mathString = NestInParentheses("2*3+1", ExcessiveNestingDepth);

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, null, CultureInfo.InvariantCulture).EvaluateDecimal(),
            SmallStackSize));

        Assert.IsType<MathExpressionException>(ex);
    }

    [Fact]
    public void MathExpression_EvaluateComplex_NestedParenthesesOfExcessiveDepth_ThrowMathExpressionException()
    {
        var mathString = NestInParentheses("2*3+1", ExcessiveNestingDepth);

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, null, CultureInfo.InvariantCulture).EvaluateComplex(),
            SmallStackSize));

        Assert.IsType<MathExpressionException>(ex);
    }

    /// <summary>The expression tree builder is as recursive as the evaluating, so it has to be guarded as well.</summary>
    [Fact]
    public void MathExpression_Compile_NestedParenthesesOfExcessiveDepth_ThrowMathExpressionException()
    {
        var mathString = NestInParentheses("2*3+1", ExcessiveNestingDepth);

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, null, CultureInfo.InvariantCulture).Compile(),
            SmallStackSize));

        Assert.IsType<MathExpressionException>(ex);
    }

    [Fact]
    public void MathExpression_Evaluate_NestedFunctionsOfExcessiveDepth_ThrowMathExpressionException()
    {
        var mathString = NestInFunction("abs", "-7", ExcessiveNestingDepth);

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, new ScientificMathContext(), CultureInfo.InvariantCulture).Evaluate(),
            SmallStackSize));

        Assert.IsType<MathExpressionException>(ex);
    }

    #endregion

    #region chain of operand operators that recurses without calling Evaluate

    /// <summary>
    ///     An operand operator that processes the left operand, such as the factorial '!' or the degree symbol,
    ///     recurses through EvaluateExponentiation and never calls Evaluate back,
    ///     so that cycle has to guard the call stack on its own.
    /// </summary>
    [Theory]
    [InlineData('!')] //factorial
    [InlineData('°')] //degree symbol
    public void MathExpression_Evaluate_ExcessiveChainOfOperandOperators_ThrowMathExpressionException(char op)
    {
        var mathString = "2" + new string(op, ExcessiveNestingDepth);

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, new ScientificMathContext(), CultureInfo.InvariantCulture).Evaluate(),
            SmallStackSize));

        var mathEx = Assert.IsType<MathExpressionException>(ex);
        testOutputHelper.WriteLine(mathEx.Message);

        Assert.True(mathEx.NestingDepth > SupportedNestingDepth, $"Nesting depth {mathEx.NestingDepth} is expected to be reported.");
    }

    /// <inheritdoc cref="MathExpression_Evaluate_ExcessiveChainOfOperandOperators_ThrowMathExpressionException" />
    [Theory]
    [InlineData('!')] //factorial
    [InlineData('°')] //degree symbol
    public void MathExpression_Compile_ExcessiveChainOfOperandOperators_ThrowMathExpressionException(char op)
    {
        var mathString = "2" + new string(op, ExcessiveNestingDepth);

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, new ScientificMathContext(), CultureInfo.InvariantCulture).Compile(),
            SmallStackSize));

        Assert.IsType<MathExpressionException>(ex);
    }

    /// <summary>The same cycle is reachable by a postfix operator that is longer than one char.</summary>
    [Fact]
    public void MathExpression_Evaluate_ExcessiveChainOfPostfixIncrements_ThrowMathExpressionException()
    {
        var mathString = "2" + string.Concat(Enumerable.Repeat("++ ", ExcessiveNestingDepth));

        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression(mathString, new DotNetStandardMathContext(), CultureInfo.InvariantCulture).Evaluate(),
            SmallStackSize));

        Assert.IsType<MathExpressionException>(ex);
    }

    #endregion

    #region expression variable that references itself

    /// <summary>
    ///     A variable that is defined as an expression string is evaluated by a nested MathExpression instance,
    ///     so a variable that references itself recurses through the nested instances until the call stack runs out.
    /// </summary>
    [Fact]
    public void MathExpression_Evaluate_SelfReferencingExpressionVariable_ThrowMathExpressionException()
    {
        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression("x", null, CultureInfo.InvariantCulture).Evaluate(new { x = "x + 1" }),
            SmallStackSize));

        var mathEx = Assert.IsType<MathExpressionException>(ex);
        testOutputHelper.WriteLine(mathEx.Message);

        //the depth is continued by the nested instances, so it counts the whole call stack, not the last nested instance
        Assert.True(mathEx.NestingDepth > SupportedNestingDepth, $"Nesting depth {mathEx.NestingDepth} is expected to be reported.");
    }

    /// <inheritdoc cref="MathExpression_Evaluate_SelfReferencingExpressionVariable_ThrowMathExpressionException" />
    [Fact]
    public void MathExpression_Compile_SelfReferencingExpressionVariable_ThrowMathExpressionException()
    {
        var ex = Record.Exception(() => RunOnThread(
            () => new MathExpression("x", null, CultureInfo.InvariantCulture).Compile(new { x = "x + 1" }),
            SmallStackSize));

        Assert.IsType<MathExpressionException>(ex);
    }

    #endregion

    private static string NestInParentheses(string mathString, int depth)
        => string.Concat(new string('(', depth), mathString, new string(')', depth));

    private static string NestInFunction(string fnName, string mathString, int depth)
        => string.Concat(string.Concat(Enumerable.Repeat($"{fnName}(", depth)), mathString, new string(')', depth));

    /// <summary>Runs the <paramref name="fn" /> on a dedicated thread that has the specified call stack size.</summary>
    private static T RunOnThread<T>(Func<T> fn, int maxStackSize)
    {
        var value = default(T)!;
        Exception? exception = null;

        var thread = new Thread(() =>
        {
            try
            {
                value = fn();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }, maxStackSize);

        thread.Start();
        thread.Join();

        if (exception != null)
            ExceptionDispatchInfo.Capture(exception).Throw();

        return value;
    }
}