﻿using System.Globalization;
using Xunit.Abstractions;

namespace MathEvaluation.Tests;

public class MathEvaluatorTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData("2 / 5 / 2 * 5", 2d / 5 / 2 * 5)]
    [InlineData("2 + (5 - 1)", 2 + (5 - 1))]
    [InlineData("2(5 - 1)", 2 * (5 - 1))]
    [InlineData("(3 + 1) (5 - 1)", (3 + 1) * (5 - 1))]
    [InlineData("(3 + 1)(5 + 2(5 - 1))", (3 + 1) * (5 + 2 * (5 - 1)))]
    [InlineData("2 + (5 * 2 - 1)", 2 + (5 * 2 - 1))]
    [InlineData("4 * 0.1 - 2", 4 * 0.1 - 2)]
    [InlineData("6 + -( -4)", 6 + -(-4))]
    [InlineData("6 + - ( 4)", 6 + -(4))]
    [InlineData("(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6", (2.323 * 323 - 1 / (2 + 3.33) * 4) - 6)]
    [InlineData("2 - 5 * 10 / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("1 - -1", 1 - -1)]
    public void MathEvaluator_Evaluate_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

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
    public void MathEvaluator_Evaluate_HasNumbersInSpecificCulture_ExpectedValue(string expression, string? cultureName)
    {
        var expectedValue = 22888.32d * 30 / 323.34d / .5d - -1 / (2 + 22888.32d) * 4 - 6;
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var cultureInfo = cultureName == null ? null : new CultureInfo(cultureName);
        var value = MathEvaluator.Evaluate(expression, cultureInfo);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("π", System.Math.PI)]
    [InlineData("((5 - 1)π)", 4 * System.Math.PI)]
    [InlineData("1/2π", 1 / (2 * System.Math.PI))]
    [InlineData("1/(4 - 2)π", 1 / (2 * System.Math.PI))]
    [InlineData("(1/2)π", System.Math.PI / 2)]
    [InlineData("-1/π", -1 / System.Math.PI)]
    [InlineData("1 - -1/π", 1 + 1 / System.Math.PI)]
    [InlineData("2π * 2 / 2 + π", System.Math.PI * 3)]
    [InlineData("2π / π / 2 * π", System.Math.PI)]
    [InlineData("ππ", System.Math.PI * System.Math.PI)]
    [InlineData("pi", System.Math.PI)]
    [InlineData("((5 - 1)Pi)", 4 * System.Math.PI)]
    [InlineData("1/2PI", 1 / (2 * System.Math.PI))]
    public void MathEvaluator_Evaluate_HasPi_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Fact]
    public void MathEvaluator_Evaluate_HasNotSupportedFn_ThrowNotSupportedException()
    {
        var expression = "1 + ctng(3 + 4)";
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => MathEvaluator.Evaluate(expression));
        Assert.IsType<NotSupportedException>(ex);
        Assert.Contains("'ctng' isn't supported", ex.Message);
    }

    [Fact]
    public void MathEvaluator_Evaluate_HasIncorrectNumberFormat_ThrowFormatException()
    {
        var expression = "’888.32 CHF";
        var cultureInfo = new CultureInfo("de-CH");
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => MathEvaluator.Evaluate(expression, cultureInfo));
        Assert.IsType<FormatException>(ex);
        Assert.Contains("The input string '’888.32' was not in a correct format", ex.Message);
    }
}