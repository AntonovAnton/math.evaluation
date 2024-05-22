using System.Diagnostics;
using System.Globalization;
using Xunit.Abstractions;

namespace Math.Evaluation.Tests;

public class MathEvaluatorTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MathEvaluator _mathEvaluator;

    public MathEvaluatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _mathEvaluator = new MathEvaluator();
    }

    [Theory]
    [InlineData("2 / 5 / 2 * 5", 2d / 5 / 2 * 5)]
    [InlineData("2 + (5 - 1)", 2 + (5 - 1))]
    [InlineData("2(5 - 1)", 2 * (5 - 1))]
    [InlineData("(3 + 1) (5 - 1)", (3 + 1) * (5 - 1))]
    [InlineData("2 + (5 * 2 - 1)", 2 + (5 * 2 - 1))]
    [InlineData("4 * 0.1 - 2", 4 * 0.1 - 2)]
    [InlineData("6 + -( -4)", 6 + -(-4))]
    [InlineData("6 + - ( 4)", 6 + -(4))]
    [InlineData("(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6", (2.323 * 323 - 1 / (2 + 3.33) * 4) - 6)]
    [InlineData("2 - 5 * 10 / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("1 - -1", 1 - -1)]
    public void MathEvaluator_Evaluate_ExpectedValue(string expression, double expectedValue)
    {
        _testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var stopwatch = Stopwatch.StartNew();
        var value = _mathEvaluator.Evaluate(expression);
        stopwatch.Stop();

        _testOutputHelper.WriteLine(
            $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");
        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("22888.32 * 30 / 323.34 / .5 - - 1 / (2 + 22888.32) * 4 - 6", null)]
    [InlineData("22,888.32 ¤ * 30 / 323.34 / .5 - - 1 / (2 + 22,888.32 ¤) * 4 - 6", "")]
    [InlineData("$22,888.32 * 30 / 323.34 / .5 - - 1 / (2 + $22,888.32) * 4 - 6", "en-US")]
    [InlineData("22 888,32 ¤ * 30 / 323,34 / ,5 - - 1 / (2 + 22 888,32 ¤) * 4 - 6", "af")]
    [InlineData("22٬888٫32 ¤ * 30 / 323٫34 / ٫5 - - 1 / (2 + 22٬888٫32 ¤) * 4 - 6", "ar")]
    [InlineData("22.888,32 د.ج.‏ * 30 / 323,34 / ,5 - - 1 / (2 + 22.888,32 د.ج.‏) * 4 - 6", "ar-DZ")]
    [InlineData("22’888.32 CHF * 30 / 323.34 / .5 - - 1 / (2 + 22’888.32 CHF) * 4 - 6", "de-CH")]
    [InlineData("22 888.32 ¤ * 30 / 323.34 / .5 - - 1 / (2 + 22 888.32 ¤) * 4 - 6", "dje")]
    [InlineData("22⹁888.32 ¤ * 30 / 323.34 / .5 - - 1 / (2 + 22⹁888.32 ¤) * 4 - 6", "ff-Adlm")]
    [InlineData("22 888,32 ¤ * 30 / 323,34 / ,5 - - 1 / (2 + 22 888,32 ¤) * 4 - 6", "fr")]
    [InlineData("22’888,32 ¤ * 30 / 323,34 / ,5 - - 1 / (2 + 22’888,32 ¤) * 4 - 6", "wae")]
    public void MathEvaluator_Evaluate_ExpressionHasNumbersInSpecificCulture_ExpectedValue(string expression, string? cultureName)
    {
        var expectedValue = 22888.32d * 30 / 323.34d / .5d - -1 / (2 + 22888.32d) * 4 - 6;
        _testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var cultureInfo = cultureName == null ? null : new CultureInfo(cultureName);

        var stopwatch = Stopwatch.StartNew();
        var value = _mathEvaluator.Evaluate(expression, cultureInfo);
        stopwatch.Stop();

        _testOutputHelper.WriteLine(
            $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("π", System.Math.PI)]
    [InlineData("(1/2)π", System.Math.PI / 2)]
    [InlineData("-1/π", -1 / System.Math.PI)]
    [InlineData("1 - -1/π", 1 + 1 / System.Math.PI)]
    [InlineData("2π * 2 / 2 + π", System.Math.PI * 3)]
    [InlineData("2π / π / 2 * π", System.Math.PI)]
    [InlineData("ππ", System.Math.PI * System.Math.PI)]
    public void MathEvaluator_EvaluateHasPi_ExpectedValue(string expression, double expectedValue)
    {
        _testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var stopwatch = Stopwatch.StartNew();
        var value = _mathEvaluator.Evaluate(expression);
        stopwatch.Stop();

        _testOutputHelper.WriteLine(
            $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");
        Assert.Equal(expectedValue, value, double.Epsilon);
    }
}