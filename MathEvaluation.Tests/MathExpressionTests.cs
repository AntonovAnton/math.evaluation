using System.Globalization;
using Xunit.Abstractions;
using MathEvaluation.Context;

namespace MathEvaluation.Tests;

public class MathExpressionTests(ITestOutputHelper testOutputHelper)
{
    private readonly ScientificMathContext _scientificContext = new();
    private readonly ProgrammingMathContext _programmingContext = new();

    [Theory]
    //[InlineData(null, double.NaN)]
    //[InlineData("", double.NaN)]
    //[InlineData("0/0", double.NaN)]
    //[InlineData("   ", double.NaN)]
    //[InlineData("+", double.NaN)]
    //[InlineData("-", double.NaN)]
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
    [InlineData("2 - 5 * +10 / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / - -2 / - 2 - 1", 2 - 5 * -10d / 2 / -2 - 1)]
    [InlineData("2 - 5 * -10 / +2 / - 2 - 1", 2 - 5 * -10d / 2 / -2 - 1)]
    [InlineData("2 - 5 * -10 / -2 / - 2 - 1", 2 - 5 * -10d / -2 / -2 - 1)]
    [InlineData("1 - -1", 1 - -1)]
    [InlineData("2 + \n(5 - 1) - \r\n 3", 2 + (5 - 1) - 3)]
    public void MathExpression_CompileThenEvaluate_ExpectedValue(string? expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var mathExpression = new MathExpression(expression!);
        var func = mathExpression.Compile(new { });

        var value = func(new { });

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("3^4", 81d)]
    [InlineData("3^4^2", 81d * 81 * 81 * 81)]
    [InlineData("2/3^4", 2 / 81d)]
    [InlineData("0.5^2*3", 0.75d)]
    [InlineData("-3^4", -81d)]
    //[InlineData("2^3pi", 687.29133511454552d)]
    //[InlineData("-3^4sin(-PI/2)", 81d)]
    [InlineData("(-3)^0.5", double.NaN)]
    [InlineData("3 + 2(2 + 3.5)^ 2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    [InlineData("3 + 2(2 + 3.5)  ^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathExpression_CompileThenEvaluate_HasScientificPower_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var mathExpression = new MathExpression(expression, _scientificContext);
        var func = mathExpression.Compile(new { });

        var value = func(new { });

        Assert.Equal(expectedValue, value);
    }


    [Theory]
    //[InlineData("ln[1/x + √(1/x^2 + 1)]", "x", 0.5, 1.4436354751788103d)]
    [InlineData("3^x^2", 4, 81d * 81 * 81 * 81)]
    [InlineData("x", 0.5, 0.5d)]
    [InlineData("2x", -0.5, -1d)]
    public void MathEvaluator_Evaluate_HasVariable_ExpectedValue(string expression,
        double varValue, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"x = {varValue}");

        var mathExpression = new MathExpression(expression, _scientificContext);
        var func = mathExpression.Compile<ExpressionParameters>();

        var value = func(new ExpressionParameters { x = varValue });

        Assert.Equal(expectedValue, value);
    }
}

public class ExpressionParameters
{
    public double x { get; set; }
}