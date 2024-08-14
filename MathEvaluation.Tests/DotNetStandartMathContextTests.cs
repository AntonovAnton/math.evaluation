using Xunit.Abstractions;
using MathEvaluation.Context;

namespace MathEvaluation.Tests;

public class DotNetStandartMathContextTests(ITestOutputHelper testOutputHelper)
{
    private readonly DotNetStandartMathContext _context = new();

    [Theory]
    [InlineData("double.NaN", double.NaN)]
    [InlineData("double.PositiveInfinity", double.PositiveInfinity)]
    [InlineData("double.NegativeInfinity", double.NegativeInfinity)]
    [InlineData("double.Epsilon", double.Epsilon)]
    [InlineData("double.MinValue", double.MinValue)]
    [InlineData("double.MaxValue", double.MaxValue)]
    [InlineData("Double.NaN", double.NaN)]
    [InlineData("Double.PositiveInfinity", double.PositiveInfinity)]
    [InlineData("Double.NegativeInfinity", double.NegativeInfinity)]
    [InlineData("Double.Epsilon", double.Epsilon)]
    [InlineData("Double.MinValue", double.MinValue)]
    [InlineData("Double.MaxValue", double.MaxValue)]
    [InlineData("float.NaN", double.NaN)]
    [InlineData("float.PositiveInfinity", double.PositiveInfinity)]
    [InlineData("float.NegativeInfinity", double.NegativeInfinity)]
    [InlineData("float.Epsilon", float.Epsilon)]
    [InlineData("float.MinValue", float.MinValue)]
    [InlineData("float.MaxValue", float.MaxValue)]
    [InlineData("Single.NaN", float.NaN)]
    [InlineData("Single.PositiveInfinity", float.PositiveInfinity)]
    [InlineData("Single.NegativeInfinity", float.NegativeInfinity)]
    [InlineData("Single.Epsilon", float.Epsilon)]
    [InlineData("Single.MinValue", float.MinValue)]
    [InlineData("Single.MaxValue", float.MaxValue)]
    [InlineData("-20.3d", -20.3d)]
    [InlineData("2D / 5m / 2f * 5F", 2d / 5 / 2 * 5)]
    [InlineData("2ul / 5d / 2UL * 5Ul", 2d / 5 / 2 * 5)]
    [InlineData("2u / 5d / 2U * 5lU", 2d / 5 / 2 * 5)]
    [InlineData("2M + (5l - 1L)", 2 + (5 - 1))]
    [InlineData("2lu + (5Lu - 1LU)", 2 + (5 - 1))]
    public void MathEvaluator_Evaluate_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("false == Math.E", false)]
    [InlineData("!false", true)]
    [InlineData("false != Math.E", true)]
    [InlineData("false || Math.E", true)]
    [InlineData("true ^ true", false)]
    [InlineData("200 >= 2.4", 200 >= 2.4)]
    [InlineData("200 <= 2.4", 200 <= 2.4)]
    [InlineData("1.0 >= 0.1 & 5.4 <= 5.4", 1.0 >= 0.1 & 5.4 <= 5.4)]
    [InlineData("1 > -0 && 2 < 3 || 2 > 1", 1 > -0 && 2 < 1 || 2 > 1)]
    [InlineData("5.4 < 5.4", 5.4 < 5.4)]
    [InlineData("1.0 > 1.0 + -0.7 && 5.4 < 5.5", 1.0 > 1.0 + -0.7 && 5.4 < 5.5)]
    [InlineData("1.0 - 1.95 >= 0.1", 1.0 - 1.95 >= 0.1)]
    [InlineData("Math.Sin(0) == Math.Tan(0)", true)]
    [InlineData("Math.Sin(0) != Math.Cos(1)", true)]
    [InlineData("4 != 4 | 5.4 == 5.4 & !true ^ 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", true)]
    [InlineData("4 != 4 || 5.4 == 5.4 && !true ^ 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", true)]
    public void MathEvaluator_Evaluate_HasBooleanLogic_ExpectedValue(string expression, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value == 1.0);
    }

    [Theory]
    [InlineData("2 ^ 3", 1)]
    [InlineData("2 & 3", 2)]
    [InlineData("2 | 3", 3)]
    [InlineData("2 & 3 ^ 2 | 3", 3)]
    [InlineData("2 & 3 ^ (2 | 3)", 1)]
    [InlineData("2 & ~1 ^ 3 | 4", 5)]
    [InlineData("2345345345345345344L ^ 3", 2345345345345345344L ^ 3)]
    [InlineData("2345345345345345344UL ^ 3", 2345345345345345344UL ^ 3)]
    public void MathEvaluator_Evaluate_HasBitwiseBooleanLogic_ExpectedValue(string expression, long expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.E", Math.E)]
    [InlineData("200 * Math.E", 200 * Math.E)]
    [InlineData("200 * Math.Pow(Math.E, -0.15)", 172.14159528501156d)]
    public void MathEvaluator_Evaluate_HasLnBase_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.PI", Math.PI)]
    [InlineData("((5 - 1)*Math.PI)", 4 * Math.PI)]
    [InlineData("Math.PI*((5 - 1))", 4 * Math.PI)]
    [InlineData("1/(2*Math.PI)", 1 / (2 * Math.PI))]
    public void MathEvaluator_Evaluate_HasMathPI_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Sin(Math.PI / 6)", 0.49999999999999994d)]
    [InlineData("Math.Sin(0.5 * Math.PI)", 1d)]
    [InlineData("Math.Sin(0.5)/2", 0.2397127693021015d)]
    [InlineData("Math.Cos(1)", 0.54030230586813977d)]
    [InlineData("Math.Cos(1)*(1 + 2)", 0.54030230586813977d * 3)]
    [InlineData("(Math.Cos(1)*(1 + 2)) % Math.Cos(1)+0.5", 0.5d)]
    [InlineData("Math.Sin(-3)/Math.Cos(1)", -0.14112000805986721d / 0.54030230586813977d)]
    [InlineData("Math.Sin(-3)*Math.Cos(1)", -0.14112000805986721d * 0.54030230586813977d)]
    [InlineData("Math.Cos(Math.Pow(1,4))", 0.54030230586813977d)]
    [InlineData("Math.Cos(Math.Pow(1,4))/2", 0.54030230586813977d / 2)]
    [InlineData("Math.Sin(Math.PI/12 + Math.PI/12)", 0.49999999999999994d)]
    [InlineData("Math.Sin((1/6)*Math.PI)", 0.49999999999999994d)]
    [InlineData("Math.Sin(1 + 2.4)", -0.25554110202683122d)]
    [InlineData("Math.Sin(3.4)", -0.25554110202683122d)]
    [InlineData("Math.Sin( +3.4)", -0.25554110202683122d)]
    [InlineData("Math.Sin( -3 * 2)", 0.27941549819892586d)]
    [InlineData("Math.Sin(-3)", -0.14112000805986721d)]
    [InlineData("3 + 2 * Math.Sin(Math.PI)", 3.0000000000000004d)]
    [InlineData("Math.Cos(Math.PI/3)", 0.50000000000000011d)]
    [InlineData("Math.Cos(Math.PI/6 + Math.PI/6)", 0.50000000000000011d)]
    [InlineData("Math.Cos((1/3)*Math.PI)", 0.50000000000000011d)]
    [InlineData("Math.Sin(Math.PI/6) + Math.Cos(Math.PI/3)", 1d)]
    [InlineData("Math.Tan(0)", 0)]
    [InlineData("Math.Tan(Math.PI/4)", 0.99999999999999989d)]
    [InlineData("Math.Sin(0) + 3", 3d)]
    [InlineData("Math.Cos(1) * 2 + 3", 0.54030230586813977d * 2 + 3d)]
    public void MathEvaluator_Evaluate_HasTrigonometricFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Sinh(0)", 0d)]
    [InlineData("Math.Sinh(0.88137358701954305)", 1d)]
    [InlineData("Math.Sinh(double.PositiveInfinity)", double.PositiveInfinity)]
    [InlineData("Math.Sinh( -0.48121182505960347)", -0.5d)]
    [InlineData("Math.Sinh(-0.88137358701954305)", -1d)]
    [InlineData("Math.Sinh(double.NegativeInfinity)", double.NegativeInfinity)]
    [InlineData("Math.Cosh(0)", 1d)]
    [InlineData("Math.Cosh(1.3169578969248166)", 1.9999999999999998d)]
    [InlineData("Math.Cosh(double.PositiveInfinity)", double.PositiveInfinity)]
    [InlineData("Math.Tanh(0)", 0)]
    [InlineData("Math.Tanh( -0.54930614433405489)", -0.5d)]
    [InlineData("Math.Tanh(double.NegativeInfinity)", -1d)]
    public void MathEvaluator_Evaluate_HasHyperbolicTrigonometricFn_ExpectedValue(string expression,
        double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Asin(double.NegativeInfinity)", double.NaN)]
    [InlineData("Math.Asin(double.PositiveInfinity)", double.NaN)]
    [InlineData("Math.Asin(-2)", double.NaN)]
    [InlineData("Math.Asin(-1)", -Math.PI / 2)]
    [InlineData("Math.Acos(-2)", double.NaN)]
    [InlineData("Math.Acos(-1)", Math.PI)]
    [InlineData("Math.Atan(-double.PositiveInfinity)", -Math.PI / 2)]
    [InlineData("Math.Atan(-2)", -1.1071487177940904d)]
    public void MathEvaluator_Evaluate_HasInverseTrigonometricFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Asinh(0)", 0)]
    [InlineData("Math.Asinh(0.5)", 0.48121182505960347d)]
    [InlineData("Math.Asinh(2)", 1.4436354751788103d)]
    [InlineData("Math.Asinh(double.PositiveInfinity)", double.PositiveInfinity)]
    [InlineData("Math.Asinh(-2)", -1.4436354751788103d)]
    [InlineData("Math.Asinh(double.NegativeInfinity)", double.NegativeInfinity)]
    [InlineData("Math.Acosh(0)", double.NaN)]
    [InlineData("Math.Acosh(0.5)", double.NaN)]
    [InlineData("Math.Acosh(1)", 0)]
    [InlineData("Math.Acosh(2)", 1.3169578969248166d)]
    [InlineData("Math.Acosh(-2)", double.NaN)]
    [InlineData("Math.Acosh(double.NegativeInfinity)", double.NaN)]
    [InlineData("Math.Atanh(0)", 0)]
    [InlineData("Math.Atanh(0.5)", 0.54930614433405489d)]
    [InlineData("Math.Atanh(1)", double.PositiveInfinity)]
    [InlineData("Math.Atanh(2)", double.NaN)]
    [InlineData("Math.Atanh(double.PositiveInfinity)", double.NaN)]
    [InlineData("Math.Atanh(-2)", double.NaN)]
    public void MathEvaluator_Evaluate_HasInverseHyperbolicFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("3 * Math.Abs-5", 15d)]
    [InlineData("3 * Math.Abs(  -5)", 15d)]
    [InlineData("3 / Math.Abs(  -(9/3))", 1d)]
    [InlineData("Math.Abs(Math.Sin(-3))", 0.14112000805986721d)]
    [InlineData("3 + 2* Math.Pow(Math.Abs(-2 + -3.5), 2)", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathEvaluator_Evaluate_HasAbs_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Sqrt(25)", 5d)]
    [InlineData("Math.Sqrt(0)", 0d)]
    [InlineData("Math.Sqrt(-25)", double.NaN)]
    [InlineData("Math.Sqrt(9*9)", 9d)]
    [InlineData("Math.Sqrt(9)*Math.Sqrt(9)", 9d)]
    [InlineData("Math.Sqrt(9)*(1 + 2)", 9d)]
    [InlineData("Math.Sqrt(9)/Math.Sqrt(9)", 1d)]
    [InlineData("Math.Sqrt(1)", 1d)]
    [InlineData("1/Math.Sqrt(9)", 1 / 3d)]
    [InlineData("Math.Pow(8, 1/3)", 2)]
    [InlineData("Math.Pow(-8, 1/3)", double.NaN)]
    [InlineData("Math.Pow(Math.Pow(8, 1/3), 2)", 4d)]
    [InlineData("Math.Sqrt(9) * Math.Pow(8, 1/3)", 6d)]
    [InlineData("Math.Pow(16, 0.25)", 2d)]
    [InlineData("1/Math.Pow(Math.Sqrt(9), 2)", 1 / 9d)]
    public void MathEvaluator_Evaluate_HasRoot_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Log10(0)", double.NegativeInfinity)]
    [InlineData("Math.Log10(1)", 0d)]
    [InlineData("Math.Log10(10)", 1d)]
    [InlineData("Math.Log10(Math.E)", 0.43429448190325182d)]
    [InlineData("Math.Log10(100)", 2d)]
    [InlineData("Math.Log10(-100)", double.NaN)]
    [InlineData("Math.Log10(double.PositiveInfinity)", double.PositiveInfinity)]
    [InlineData("Math.Log(0)", double.NegativeInfinity)]
    [InlineData("Math.Log(1)", 0d)]
    [InlineData("Math.Log(10)", 2.3025850929940459d)]
    [InlineData("Math.Log(10, Math.E)", 2.3025850929940459d)]
    [InlineData("Math.Log(Math.E)", 1d)]
    [InlineData("Math.Log(100)", 4.6051701859880918d)]
    [InlineData("Math.Log(-100)", double.NaN)]
    [InlineData("Math.Log(double.PositiveInfinity)", double.PositiveInfinity)]
    [InlineData("-2*Math.Log(1/0.5 + Math.Sqrt((1/(0.5*0.5) + 1))", -2 * 1.4436354751788103d)]
    public void MathEvaluator_Evaluate_HasLogarithmFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Floor(-20.3)", -21d)]
    [InlineData("-Math.Floor(20.3)", -20d)]
    [InlineData("-Math.Floor(0)", 0d)]
    [InlineData("Math.Floor(-0.1)", -1d)]
    [InlineData("3*Math.Floor(-5)", -15d)]
    [InlineData("2 / Math.Floor(-5) / 2 * 5", 2d / -5 / 2 * 5)]
    [InlineData("Math.Floor(2 + (5 - 1))", 2 + (5 - 1))]
    [InlineData("2*(5 - Math.Floor(-1))", 2 * (5 + 1))]
    [InlineData("Math.Floor(-(5 - 1))(3 + 1)", -(3 + 1) * (5 - 1))]
    [InlineData("(3 + 1)*Math.Floor(-(5 - 1))", (3 + 1) * -(5 - 1))]
    [InlineData("6 + Math.Floor(( -4))", 6 - 4)]
    [InlineData("6 + - Math.Floor(4)", 6 - 4)]
    [InlineData("2 - 5 * Math.Floor(-10) / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("Math.Floor(Math.Sin(3))", 0d)]
    [InlineData("Math.Floor(Math.Sin(-3))", -1d)]
    [InlineData("3 + 2*Math.Pow(Math.Floor(2 + 3.5) , 2)", 3 + 2 * 25d)]
    public void MathEvaluator_Evaluate_HasFloor_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Ceiling(-20.3)", -20d)]
    [InlineData("-Math.Ceiling(20.3)", -21d)]
    [InlineData("-Math.Ceiling(0)", 0d)]
    [InlineData("Math.Ceiling(-0.1)", 0d)]
    [InlineData("3*Math.Ceiling(-5)", -15d)]
    [InlineData("2 / Math.Ceiling(-5) / 2 * 5", 2d / -5 / 2 * 5)]
    [InlineData("Math.Ceiling(2 + (5 - 1))", 2 + (5 - 1))]
    [InlineData("2*(5 - Math.Ceiling(-1))", 2 * (5 + 1))]
    [InlineData("Math.Ceiling(-(5 - 1))*(3 + 1)", -(3 + 1) * (5 - 1))]
    [InlineData("(3 + 1)*Math.Ceiling(-(5 - 1))", (3 + 1) * -(5 - 1))]
    [InlineData("6 + Math.Ceiling(( -4))", 6 - 4)]
    [InlineData("6 + - Math.Ceiling(4)", 6 - 4)]
    [InlineData("2 - 5 * Math.Ceiling(-10) / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("Math.Ceiling(Math.Sin(3))", 1d)]
    [InlineData("Math.Ceiling(Math.Sin(-3))", 0d)]
    [InlineData("Math.Ceiling(2 + 3.5)", 6d)]
    [InlineData("3 + 2*Math.Ceiling(2 + 3.5)", 3 + 2 * 6d)]
    public void MathEvaluator_Evaluate_HasCeiling_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression, _context);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Log(1/x + Math.Sqrt((1/(x*x) + 1))", "x", 0.5, 1.4436354751788103d)]
    [InlineData("x", "x", 0.5, 0.5d)]
    [InlineData("2x", "x", 0.5, 1d)]
    [InlineData("PI", $"{nameof(Math.PI)}", Math.PI, Math.PI)]
    [InlineData("2 * PI", $"{nameof(Math.PI)}", Math.PI, 2 * Math.PI)]
    public void MathEvaluator_Evaluate_HasVariable_ExpectedValue(string expression, string varName,
        double varValue, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"{varName} = {varValue}");

        var value = expression
            .SetContext(_context)
            .BindVariable(varValue, varName)
            .Evaluate();

        Assert.Equal(expectedValue, value);
    }
}