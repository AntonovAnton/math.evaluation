using MathEvaluation.Context;
using MathEvaluation.Extensions;
using Xunit.Abstractions;
// ReSharper disable EqualExpressionComparison
// ReSharper disable RedundantLogicalConditionalExpressionOperand

namespace MathEvaluation.Tests.Compilation;

public class DecimalDotNetStandartMathContextTests(ITestOutputHelper testOutputHelper)
{
    private readonly DecimalDotNetStandartMathContext _context = new();

    [Theory]
    [InlineData("double.Epsilon", double.Epsilon)]
    [InlineData("float.Epsilon", float.Epsilon)]
    [InlineData("Single.Epsilon", float.Epsilon)]
    [InlineData("Double.Epsilon", double.Epsilon)]
    [InlineData("decimal.One", (double)decimal.One)]
    [InlineData("decimal.Zero", (double)decimal.Zero)]
    [InlineData("decimal.MinusOne", (double)decimal.MinusOne)]
    [InlineData("-20.3m", -20.3d)]
    [InlineData("2D / 5d / 2f * 5m", 2d / 5 / 2 * 5)]
    [InlineData("2ul / 5d / 2UL * 5Ul", 2d / 5 / 2 * 5)]
    [InlineData("2u / 5d / 2M * 5lU", 2d / 5 / 2 * 5)]
    [InlineData("2M + (5l - 1L)", 2 + (5 - 1))]
    [InlineData("2lu + (5Lu - 1LU)", 2 + (5 - 1))]
    [InlineData("(double)2 / (decimal)5 / (float)2 * (int)5", 2d / 5 / 2 * 5)]
    [InlineData("(ulong)2 / (double) 5 / (long) 2 * (short) 5f", 2d / 5 / 2 * 5)]
    [InlineData("(uint)2 / 5d / 2 * (ushort)5", 2d / 5 / 2 * 5)]
    [InlineData("(decimal)2 + ((long)5 - 1)", 2 + (5 - 1))]
    [InlineData("(uint)2 + ((byte)5 - (sbyte)1)", 2 + (5 - 1))]
    [InlineData("Complex.One + ((Complex)5 - Complex.Zero + new Complex(2, 0))", 1 + (5 - 0) + 2)]
    public void MathExpression_CompileDecimalThenInvoke_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("0++ -2/5", 0d - 2 / 5d)]
    [InlineData("a++ -2/5", 5d - 2 / 5d)]
    [InlineData("2 / 5d / a++ * 5", 2 / 5d / 5 * 5)]
    [InlineData("2 / 5d /a++\n * 5", 2 / 5d / 5 * 5)]
    [InlineData("2 / 5d / 2 * a++ + 5d", 2 / 5d / 2 * 5 + 5)]
    public void MathExpression_CompileDecimalThenInvoke_HasPostfixIncrement_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var fn = expression.CompileDecimal(new { a = 0 }, _context);
        var value = fn(new { a = 5 });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("0-- -2/5", 0d - 2 / 5d)]
    [InlineData("a-- -2/5", 5d - 2 / 5d)]
    [InlineData("2 / 5d / a-- * 5", 2 / 5d / 5 * 5)]
    [InlineData("2 / 5d /a--\n * 5", 2 / 5d / 5 * 5)]
    [InlineData("2 / 5d / 2 * a-- + 5d", 2 / 5d / 2 * 5 + 5)]
    public void MathExpression_CompileDecimalThenInvoke_HasPostfixDecrement_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var fn = expression.CompileDecimal(new { a = 0 }, _context);
        var value = fn(new { a = 5 });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("false == Math.E", false)]
    [InlineData("!false", true)]
    [InlineData("false != Math.E", true)]
    [InlineData("false || Math.E", true)]
    [InlineData("true ^ true", false)]
    [InlineData("200 >= 2.4", 200 >= 2.4)]
    [InlineData("200 <= 2.4", 200 <= 2.4)]
    [InlineData("1.0 >= 0.1 & 5.4 <= 5.4", (1.0 >= 0.1) & (5.4 <= 5.4))]
    [InlineData("1 > -0 && 2 < 3 || 2 > 1", (1 > -0 && 2 < 1) || 2 > 1)]
    [InlineData("5.4 < 5.4", 5.4 < 5.4)]
    [InlineData("1.0 > 1.0 + -0.7 && 5.4 < 5.5", 1.0 > 1.0 + -0.7 && 5.4 < 5.5)]
    [InlineData("1.0 - 1.95 >= 0.1", 1.0 - 1.95 >= 0.1)]
    [InlineData("Math.Sin(0) == Math.Tan(0)", true)]
    [InlineData("Math.Sin(0) != Math.Cos(1)", true)]
    [InlineData("4 != 4 | 5.4 == 5.4 & !true ^ 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", true)]
    [InlineData("4 != 4 || 5.4 == 5.4 && !true ^ 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", true)]
    public void MathExpression_CompileDecimalThenInvoke_HasBooleanLogic_ExpectedValue(string mathString, bool expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value == 1.0m);
    }

    [Theory]
    [InlineData("2 ^ 3", 1)]
    [InlineData("2 & 3", 2)]
    [InlineData("2 | 3", 3)]
    [InlineData("2 & 3 ^ 2 | 3", 3)]
    [InlineData("2 & 3 ^ (2 | 3)", 1)]
    [InlineData("2 & ~1 ^ 3 | 4", 5)]
    [InlineData("2345345345345345344L ^ 3", 2345345345345345347)]
    [InlineData("2345345345345345344UL ^ 3", 2345345345345345347)]
    public void MathExpression_CompileDecimalThenInvoke_HasBitwiseBooleanLogic_ExpectedValue(string mathString, long expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("Math.E", Math.E)]
    [InlineData("200 * Math.E", 543.656365691808d)]
    public void MathExpression_CompileDecimalThenInvoke_HasLnBase_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.PI", Math.PI)]
    public void MathExpression_CompileDecimalThenInvoke_HasMathPI_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Sin(Math.PI / 6)", 0.49999999999999994d)]
    [InlineData("Math.Sin(0.5 * Math.PI)", 1d)]
    [InlineData("Math.Cos(1)", 0.54030230586813977d)]
    [InlineData("Math.Cos(1)*(1 + 2)", 0.54030230586813977d * 3)]
    [InlineData("(Math.Cos(1)*(1 + 2)) % Math.Cos(1)+0.5", 0.5d)]
    [InlineData("Math.Cos(Math.Pow(1,4))", 0.54030230586813977d)]
    [InlineData("Math.Cos(Math.Pow(1,4))/2", 0.54030230586813977d / 2)]
    [InlineData("Math.Sin(Math.PI/12 + Math.PI/12)", 0.49999999999999994d)]
    [InlineData("Math.Sin((1/6)*Math.PI)", 0.49999999999999994d)]
    [InlineData("Math.Sin(1 + 2.4)", -0.25554110202683122d)]
    [InlineData("Math.Sin(3.4)", -0.25554110202683122d)]
    [InlineData("Math.Sin( +3.4)", -0.25554110202683122d)]
    [InlineData("Math.Sin( -3 * 2)", 0.27941549819892586d)]
    [InlineData("Math.Sin(-3)", -0.14112000805986721d)]
    [InlineData("Math.Cos(Math.PI/3)", 0.50000000000000011d)]
    [InlineData("Math.Cos(Math.PI/6 + Math.PI/6)", 0.50000000000000011d)]
    [InlineData("Math.Cos((1/3)*Math.PI)", 0.50000000000000011d)]
    [InlineData("Math.Sin(Math.PI/6) + Math.Cos(Math.PI/3)", 1d)]
    [InlineData("Math.Tan(0)", 0)]
    [InlineData("Math.Tan(Math.PI/4)", 0.99999999999999989d)]
    [InlineData("Math.Sin(0) + 3", 3d)]
    [InlineData("Math.Cos(1) * 2 + 3", 0.54030230586813977d * 2 + 3d)]
    public void MathExpression_CompileDecimalThenInvoke_HasTrigonometricFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Sinh(0)", 0d)]
    [InlineData("Math.Sinh(0.88137358701954305)", 1d)]
    [InlineData("Math.Sinh( -0.48121182505960347)", -0.5d)]
    [InlineData("Math.Sinh(-0.88137358701954305)", -1d)]
    [InlineData("Math.Cosh(0)", 1d)]
    [InlineData("Math.Cosh(1.3169578969248166)", 1.9999999999999998d)]
    [InlineData("Math.Tanh(0)", 0)]
    [InlineData("Math.Tanh( -0.54930614433405489)", -0.5d)]
    [InlineData("Math.Tanh(double.NegativeInfinity)", -1d)]
    public void MathExpression_CompileDecimalThenInvoke_HasHyperbolicTrigonometricFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Asin(-1)", -Math.PI / 2)]
    [InlineData("Math.Acos(-1)", Math.PI)]
    [InlineData("Math.Atan(-double.PositiveInfinity)", -Math.PI / 2)]
    [InlineData("Math.Atan(-2)", -1.1071487177940904d)]
    public void MathExpression_CompileDecimalThenInvoke_HasInverseTrigonometricFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Asinh(0)", 0)]
    [InlineData("Math.Asinh(0.5)", 0.48121182505960347d)]
    [InlineData("Math.Asinh(2)", 1.4436354751788103d)]
    [InlineData("Math.Asinh(-2)", -1.4436354751788103d)]
    [InlineData("Math.Acosh(1)", 0)]
    [InlineData("Math.Acosh(2)", 1.3169578969248166d)]
    [InlineData("Math.Atanh(0)", 0)]
    [InlineData("Math.Atanh(0.5)", 0.54930614433405489d)]
    public void MathExpression_CompileDecimalThenInvoke_HasInverseHyperbolicFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("3 * Math.Abs(  -5)", 15d)]
    [InlineData("3 / Math.Abs(  -(9/3))", 1d)]
    [InlineData("Math.Abs(Math.Sin(-3))", 0.14112000805986721d)]
    [InlineData("3 + 2* Math.Pow(Math.Abs(-2 + -3.5), 2)", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathExpression_CompileDecimalThenInvoke_HasAbs_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Sqrt(25)", 5d)]
    [InlineData("Math.Sqrt(0)", 0d)]
    [InlineData("Math.Sqrt(9*9)", 9d)]
    [InlineData("Math.Sqrt(9)*Math.Sqrt(9)", 9d)]
    [InlineData("Math.Sqrt(9)*(1 + 2)", 9d)]
    [InlineData("Math.Sqrt(9)/Math.Sqrt(9)", 1d)]
    [InlineData("Math.Sqrt(1)", 1d)]
    [InlineData("Math.Pow(8, 1/3)", 2)]
    [InlineData("Math.Pow(Math.Pow(8, 1/3), 2)", 4d)]
    [InlineData("Math.Sqrt(9) * Math.Pow(8, 1/3)", 6d)]
    [InlineData("Math.Pow(16, 0.25)", 2d)]
    public void MathExpression_CompileDecimalThenInvoke_HasRoot_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Log10(1)", 0d)]
    [InlineData("Math.Log10(10)", 1d)]
    [InlineData("Math.Log10(Math.E)", 0.43429448190325182d)]
    [InlineData("Math.Log10(100)", 2d)]
    [InlineData("Math.Log(1)", 0d)]
    [InlineData("Math.Log(10)", 2.3025850929940459d)]
    [InlineData("Math.Log(10, Math.E)", 2.3025850929940459d)]
    [InlineData("Math.Log(Math.E)", 1d)]
    [InlineData("Math.Log(100)", 4.6051701859880918d)]
    [InlineData("-2*Math.Log(1/0.5 + Math.Sqrt(1/(0.5*0.5) + 1))", -2 * 1.4436354751788103d)]
    public void MathExpression_CompileDecimalThenInvoke_HasLogarithmFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
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
    public void MathExpression_CompileDecimalThenInvoke_HasFloor_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
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
    public void MathExpression_CompileDecimalThenInvoke_HasCeiling_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Round(-20.3)", -20d)]
    [InlineData("-Math.Round(20.3)", -20d)]
    [InlineData("-Math.Round(20.3474, 2)", -20.35d)]
    [InlineData("-Math.Round(20.3434, 2)", -20.34d)]
    [InlineData("Math.Round(-0.1)", 0d)]
    [InlineData("Math.Round(-0.1, 0)", 0d)]
    [InlineData("Math.Round(-0.1, 1)", -0.1d)]
    public void MathExpression_CompileDecimalThenInvoke_HasRound_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _context);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileDecimal();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Math.Log(1/x + Math.Sqrt(1/(x*x) + 1))", 0.5, 1.4436354751788103d)]
    [InlineData("x", 0.5, 0.5d)]
    [InlineData("2x", 0.5, 1d)]
    [InlineData("PI", Math.PI, Math.PI)]
    public void MathExpression_CompileDecimalThenInvoke_HasVariable_ExpectedValue(string expression,
        double varValue, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"variable value = {varValue}");

        var fn = expression.CompileDecimal(new { x = 0.0m, PI = 0.0m }, _context);
        var value = fn(new { x = (decimal)varValue, PI = (decimal)varValue });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Infinity")]
    [InlineData("∞")]
    [InlineData("double.NaN")]
    [InlineData("double.PositiveInfinity")]
    [InlineData("double.NegativeInfinity")]
    [InlineData("double.MinValue")]
    [InlineData("double.MaxValue")]
    [InlineData("Double.NaN")]
    [InlineData("Double.PositiveInfinity")]
    [InlineData("Double.NegativeInfinity")]
    [InlineData("Double.MinValue")]
    [InlineData("Double.MaxValue")]
    [InlineData("float.NaN")]
    [InlineData("float.PositiveInfinity")]
    [InlineData("float.NegativeInfinity")]
    [InlineData("float.MinValue")]
    [InlineData("float.MaxValue")]
    [InlineData("Single.NaN")]
    [InlineData("Single.PositiveInfinity")]
    [InlineData("Single.NegativeInfinity")]
    [InlineData("Single.MinValue")]
    [InlineData("Single.MaxValue")]
    [InlineData("Math.Sinh(float.PositiveInfinity)")]
    [InlineData("Math.Sinh(double.NegativeInfinity)")]
    [InlineData("Math.Cosh(double.PositiveInfinity)")]
    [InlineData("Math.Asinh(double.PositiveInfinity)")]
    [InlineData("Math.Asinh(float.NegativeInfinity)")]
    [InlineData("Math.Acosh(0)")]
    [InlineData("Math.Acosh(0.5)")]
    [InlineData("Math.Acosh(-2)")]
    [InlineData("Math.Acosh(double.NegativeInfinity)")]
    [InlineData("Math.Atanh(1)")]
    [InlineData("Math.Atanh(2)")]
    [InlineData("Math.Atanh(double.PositiveInfinity)")]
    [InlineData("Math.Atanh(-2)")]
    [InlineData("Math.Asin(Double.NegativeInfinity)")]
    [InlineData("Math.Asin(Double.PositiveInfinity)")]
    [InlineData("Math.Asin(-2)")]
    [InlineData("Math.Acos(-2)")]
    [InlineData("Math.Sqrt(-25)")]
    [InlineData("Math.Pow(-8, 1/3)")]
    [InlineData("Math.Log10(0)")]
    [InlineData("Math.Log10(-100)")]
    [InlineData("Math.Log10(double.PositiveInfinity)")]
    [InlineData("Math.Log(0)")]
    [InlineData("Math.Log(-100)")]
    [InlineData("Math.Log(double.PositiveInfinity)")]
    public void MathExpression_CompileDecimalThenInvoke_ReturnsNanOrInfinity_ThrowOverflowException(string expression)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, _context).CompileDecimal()());
        Assert.IsType<OverflowException>(ex?.InnerException ?? ex);
    }

    private void SubscribeToEvaluating(object? sender, EvaluatingEventArgs args)
    {
        var comment = args.IsCompleted ? " //completed" : string.Empty;
        var msg = $"{args.Step}: {args.MathString[args.Start..(args.End + 1)]} = {args.Value};{comment}";
        testOutputHelper.WriteLine(msg);
    }
}