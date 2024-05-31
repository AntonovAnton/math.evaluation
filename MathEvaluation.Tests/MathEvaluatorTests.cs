using System.Globalization;
using Xunit.Abstractions;

namespace MathEvaluation.Tests;

public class MathEvaluatorTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData("-20.3", -20.3d)]
    [InlineData("2 / 5 / 2 * 5", 2d / 5 / 2 * 5)]
    [InlineData("2 + (5 - 1)", 2 + (5 - 1))]
    [InlineData("2(5 - 1)", 2 * (5 - 1))]
    [InlineData("(3 + 1) (5 - 1)", (3 + 1) * (5 - 1))]
    [InlineData("(3 + 1)(5 + 2(5 - 1))", (3 + 1) * (5 + 2 * (5 - 1)))]
    [InlineData("2 + (5 * 2 - 1)", 2 + (5 * 2 - 1))]
    [InlineData("4 * 0.1 - 2", 4 * 0.1 - 2)]
    [InlineData("6 + -( -4)", 6 + - -4)]
    [InlineData("6 + - ( 4)", 6 + -4)]
    [InlineData("(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6", 2.323 * 323 - 1 / (2 + 3.33) * 4 - 6)]
    [InlineData("2 - 5 * 10 / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("1 - -1", 1 - -1)]
    [InlineData("(3 · 2)\u00f7(5 \u00d7 2)", 3 * 2 / (5d * 2))]
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
    [InlineData("π", Math.PI)]
    [InlineData("((5 - 1)π)", 4 * Math.PI)]
    [InlineData("1/2π", 1 / (2 * Math.PI))]
    [InlineData("1/(4 - 2)π", 1 / (2 * Math.PI))]
    [InlineData("(1/2)π", Math.PI / 2)]
    [InlineData("-1/π", -1 / Math.PI)]
    [InlineData("1 - -1/π", 1 + 1 / Math.PI)]
    [InlineData("2π * 2 / 2 + π", Math.PI * 3)]
    [InlineData("2π / π / 2 * π", Math.PI)]
    [InlineData("ππ", Math.PI * Math.PI)]
    [InlineData("+ππ", Math.PI * Math.PI)]
    [InlineData("pi", Math.PI)]
    [InlineData("((5 - 1)Pi)", 4 * Math.PI)]
    [InlineData("Pi()((5 - 1))", 4 * Math.PI)]
    [InlineData("1/2PI", 1 / (2 * Math.PI))]
    public void MathEvaluator_Evaluate_HasPi_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("3 ** 4", 81)]
    [InlineData("-3 ** 4", -81)]
    [InlineData("3^4", 81)]
    [InlineData("2/3^4", 2 / 81d)]
    [InlineData("-3^4", -81)]
    [InlineData("(-3)^0.5", double.NaN)]
    [InlineData("3 + 2(2 + 3.5)^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    [InlineData("3 + 2(2 + 3.5)  ^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathEvaluator_Evaluate_HasPower_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("4 % 3", 1)]
    [InlineData("4 mod 3", 1)]
    [InlineData("4 mod 3 - 2", -1)]
    [InlineData("3 - 4.5 % 3.1 / 3 * 2 + 4", 3 - 4.5 % 3.1 / 3 * 2 + 4)]
    [InlineData("3 - 2 / 4.5 % 3.1 / 3 * 2 + 4", 3 - 2 / 4.5 % 3.1 / 3 * 2 + 4)]
    [InlineData("3 - 4.5 mod 3.1 / 3 * 2 + 4", 3 - 4.5 % 3.1 / 3 * 2 + 4)]
    public void MathEvaluator_Evaluate_HasModulus_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("0.4e3", 0.4e3)]
    [InlineData("0.4e-3", 0.4e-3)]
    [InlineData("0.4E3", 0.4E3)]
    [InlineData("0.4E-3", 0.4E-3)]
    [InlineData("6.022e23", 6.022e23)] //Avogadro's number
    [InlineData("6.626e-34", 6.626e-34)] //Planck's constant
    [InlineData(".2E3 - 2", .2E3 - 2)]
    [InlineData("3 - 0.4e3 / 3 * 2 + 4", 3 - 0.4e3 / 3 * 2 + 4)]
    [InlineData(".1 - 0.4e3 / .3E-2 * .1E+10 + 2e+3", .1 - 0.4e3 / .3E-2 * .1E+10 + 2e+3)]
    public void MathEvaluator_Evaluate_HasExpNotationNumbers_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("e", Math.E)]
    [InlineData("200e", 200 * Math.E)]
    [InlineData("200e^-0.15", 172.14159528501156d)]
    public void MathEvaluator_Evaluate_HasLogBase_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("sin(30\u00b0)", 0.49999999999999994d)]
    [InlineData("Sin(15° + 15°)", 0.49999999999999994d)]
    [InlineData("SIN((1/6)π)", 0.49999999999999994d)]
    [InlineData("sin(3.4)", -0.25554110202683122d)]
    [InlineData("sin( +3.4)", -0.25554110202683122d)]
    [InlineData("sin( -3 * 2)", 0.27941549819892586d)]
    [InlineData("sin(-3)", -0.14112000805986721d)]
    [InlineData("3 + 2sin(PI)", 3.0000000000000004d)]
    [InlineData("cos(60\u00b0)", 0.50000000000000011d)]
    [InlineData("Cos(30° + 30°)", 0.50000000000000011d)]
    [InlineData("COS((1/3)π)", 0.50000000000000011d)]
    [InlineData("sin(30°) + cos(60°)", 1d)]
    [InlineData("tan(0°)", 0)]
    [InlineData("Tan(45°)", 1d)]
    [InlineData("cot(0°)", double.NaN)]
    [InlineData("Cot(45°)", 1d)]
    [InlineData("sec(0°)", 1d)]
    [InlineData("Sec(60°)", 1.9999999999999996d)]
    [InlineData("csc(0°)", double.NaN)]
    [InlineData("Csc(30°)", 2.0000000000000004d)]
    [InlineData("CSC(90°)", 1d)]
    public void MathEvaluator_Evaluate_HasTrigonometricFn_ExpectedValue(string expression, double expectedValue)
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