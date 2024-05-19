using System.Diagnostics;
using System.Globalization;
using Xunit.Abstractions;

namespace Math.Evaluation.Tests;

public class MathEvaluatorTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData("2 / 5 / 2 * 5", 1d)]
    [InlineData("2 + (5 - 1)", 6d)]
    [InlineData("4 * 0.1 - 2", -1.6d)]
    [InlineData("6 + -( -4)", 10d)]
    [InlineData("6 + - ( 4)", 2d)]
    [InlineData("(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6", 743.578530956848d)]
    [InlineData("2 - (5 * 10 / 2) - 1", -24d)]
    [InlineData("1 - -1", 2d)]
    public void MathEvaluator_Evaluate_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var evaluator = new MathEvaluator();
        var stopwatch = Stopwatch.StartNew();
        var value = evaluator.Evaluate(expression);
        stopwatch.Stop();

        testOutputHelper.WriteLine(
            $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");
        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("22888.32 * 30 / 323.34 / .5 - - 1 / (2 + 22888.32) * 4 - 6", null, 4241.2297164052907d)]
    [InlineData("22,888.32 ¤ * 30 / 323.34 / .5 - - 1 / (2 + 22,888.32 ¤) * 4 - 6", "", 4241.2297164052907d)]
    [InlineData("$22,888.32 * 30 / 323.34 / .5 - - 1 / (2 + $22,888.32) * 4 - 6", "en-US", 4241.2297164052907d)]
    [InlineData("22 888,32 ¤ * 30 / 323,34 / ,5 - - 1 / (2 + 22 888,32 ¤) * 4 - 6", "af", 4241.2297164052907d)]
    [InlineData("22٬888٫32 ¤ * 30 / 323٫34 / ٫5 - - 1 / (2 + 22٬888٫32 ¤) * 4 - 6", "ar", 4241.2297164052907d)]
    [InlineData("22.888,32 د.ج.‏ * 30 / 323,34 / ,5 - - 1 / (2 + 22.888,32 د.ج.‏) * 4 - 6", "ar-DZ", 4241.2297164052907d)]
    [InlineData("22’888.32 CHF * 30 / 323.34 / .5 - - 1 / (2 + 22’888.32 CHF) * 4 - 6", "de-CH", 4241.2297164052907d)]
    [InlineData("22 888.32 ¤ * 30 / 323.34 / .5 - - 1 / (2 + 22 888.32 ¤) * 4 - 6", "dje", 4241.2297164052907d)]
    [InlineData("22⹁888.32 ¤ * 30 / 323.34 / .5 - - 1 / (2 + 22⹁888.32 ¤) * 4 - 6", "ff-Adlm", 4241.2297164052907d)]
    [InlineData("22 888,32 ¤ * 30 / 323,34 / ,5 - - 1 / (2 + 22 888,32 ¤) * 4 - 6", "fr", 4241.2297164052907d)]
    [InlineData("22’888,32 ¤ * 30 / 323,34 / ,5 - - 1 / (2 + 22’888,32 ¤) * 4 - 6", "wae", 4241.2297164052907d)]
    public void MathEvaluator_Evaluate_ExpressionHasNumbersInSpecificCulture_ExpectedValue(string expression, string? cultureName, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var cultureInfo = cultureName == null ? null : new CultureInfo(cultureName);

        var evaluator = new MathEvaluator();
        var stopwatch = Stopwatch.StartNew();
        var value = evaluator.Evaluate(expression, cultureInfo);
        stopwatch.Stop();
            
        testOutputHelper.WriteLine(
            $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");
        Assert.Equal(expectedValue, value, double.Epsilon);
    }
}