using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Globalization;
using Xunit.Abstractions;

#if NET8_0_OR_GREATER

namespace MathEvaluation.Tests.Evaluation;

// ReSharper disable once InconsistentNaming
public partial class MathExpressionTests_Number(ITestOutputHelper testOutputHelper)
{
    private readonly ScientificMathContext _scientificContext = new();
    private readonly ProgrammingMathContext _programmingContext = new();

    #region Int32 Tests

    [Theory]
    [InlineData("2 + 5 - 1", 2 + 5 - 1)]
    [InlineData("2 * 5 / 2", 2 * 5 / 2)]
    [InlineData("(3 + 1) * (5 - 1)", (3 + 1) * (5 - 1))]
    [InlineData("10 / 3", 10 / 3)]
    [InlineData("-20", -20)]
    [InlineData("6 + -(4)", 6 + -(4))]
    public void MathExpression_Evaluate_Int32_ExpectedValue(string mathString, int expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<int>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("4 % 3", 1)]
    [InlineData("10 % 3", 1)]
    [InlineData("7 % 4", 3)]
    public void MathExpression_Evaluate_Int32_HasModulus_ExpectedValue(string mathString, int expectedValue)
    {
        using var expression = new MathExpression(mathString, _programmingContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<int>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("2 ** 3", 8)]
    [InlineData("2 ** 4", 16)]
    [InlineData("3 ** 3", 27)]
    public void MathExpression_Evaluate_Int32_HasPower_ExpectedValue(string mathString, int expectedValue)
    {
        using var expression = new MathExpression(mathString, _programmingContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<int>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("x + y", 5, 3, 8)]
    [InlineData("x * y", 5, 3, 15)]
    [InlineData("x - y", 5, 3, 2)]
    public void MathExpression_Evaluate_Int32_HasVariables_ExpectedValue(string mathString, int x, int y, int expectedValue)
    {
        testOutputHelper.WriteLine($"{mathString} = {expectedValue}");
        testOutputHelper.WriteLine($"x = {x}, y = {y}");

        var value = mathString.Evaluate(new { x, y }, null, CultureInfo.InvariantCulture);

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    #endregion

    #region Int64 Tests

    [Theory]
    [InlineData("2 + 5 - 1", 2L + 5 - 1)]
    [InlineData("2 * 5 / 2", 2L * 5 / 2)]
    [InlineData("1000000000 * 2", 1000000000L * 2)]
    [InlineData("-20", -20L)]
    public void MathExpression_Evaluate_Int64_ExpectedValue(string mathString, long expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<long>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("x + y", 5000000000L, 3000000000L, 8000000000L)]
    [InlineData("x - y", 5000000000L, 3000000000L, 2000000000L)]
    public void MathExpression_Evaluate_Int64_HasVariables_ExpectedValue(string mathString, long x, long y, long expectedValue)
    {
        testOutputHelper.WriteLine($"{mathString} = {expectedValue}");
        testOutputHelper.WriteLine($"x = {x}, y = {y}");

        var value = mathString.Evaluate(new { x, y }, null, CultureInfo.InvariantCulture);

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    #endregion

    #region Single (Float) Tests

    [Theory]
    [InlineData("2.5 + 1.5", 2.5f + 1.5f)]
    [InlineData("10.0 / 4.0", 10.0f / 4.0f)]
    [InlineData("-20.5", -20.5f)]
    [InlineData("2.5 * 2.0", 2.5f * 2.0f)]
    public void MathExpression_Evaluate_Single_ExpectedValue(string mathString, float expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<float>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value, precision: 5);
    }

    [Theory]
    [InlineData("x + y", 2.5f, 1.5f, 4.0f)]
    [InlineData("x * y", 2.5f, 2.0f, 5.0f)]
    public void MathExpression_Evaluate_Single_HasVariables_ExpectedValue(string mathString, float x, float y, float expectedValue)
    {
        testOutputHelper.WriteLine($"{mathString} = {expectedValue}");
        testOutputHelper.WriteLine($"x = {x}, y = {y}");

        var value = mathString.Evaluate(new { x, y }, null, CultureInfo.InvariantCulture);

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value, precision: 5);
    }

    #endregion

    #region Half Tests

    [Theory]
    [InlineData("2.5 + 1.5", 4.0)]
    [InlineData("10.0 / 4.0", 2.5)]
    [InlineData("-20.5", -20.5)]
    [InlineData("2.5 * 2.0", 5.0)]
    public void MathExpression_Evaluate_Half_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<Half>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal((Half)expectedValue, value);
    }

    //[Theory]
    //[InlineData("x + y", 2.5, 1.5, 4.0)]
    //[InlineData("x * y", 2.5, 2.0, 5.0)]
    //public void MathExpression_Evaluate_Half_HasVariables_ExpectedValue(string mathString, double x, double y, double expectedValue)
    //{
    //    testOutputHelper.WriteLine($"{mathString} = {expectedValue}");
    //    testOutputHelper.WriteLine($"x = {x}, y = {y}");

    //    var value = mathString.Evaluate(new { x = (Half)x, y = (Half)y }, null, CultureInfo.InvariantCulture);

    //    testOutputHelper.WriteLine($"result: {value}");

    //    Assert.Equal((Half)expectedValue, value);
    //}

    #endregion

    #region Byte Tests

    [Theory]
    [InlineData("2 + 5", (byte)(2 + 5))]
    [InlineData("10 * 2", (byte)(10 * 2))]
    [InlineData("20 - 5", (byte)(20 - 5))]
    public void MathExpression_Evaluate_Byte_ExpectedValue(string mathString, byte expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<byte>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    #endregion

    #region UInt32 Tests

    [Theory]
    [InlineData("2 + 5", 2u + 5u)]
    [InlineData("10 * 2", 10u * 2u)]
    [InlineData("20 - 5", 20u - 5u)]
    public void MathExpression_Evaluate_UInt32_ExpectedValue(string mathString, uint expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<uint>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    #endregion

    #region UInt64 Tests

    [Theory]
    [InlineData("2 + 5", 2UL + 5UL)]
    [InlineData("10 * 2", 10UL * 2UL)]
    [InlineData("5000000000 + 3000000000", 5000000000UL + 3000000000UL)]
    public void MathExpression_Evaluate_UInt64_ExpectedValue(string mathString, ulong expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<ulong>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    #endregion

    #region Int16 Tests

    [Theory]
    [InlineData("2 + 5", (short)(2 + 5))]
    [InlineData("10 * 2", (short)(10 * 2))]
    [InlineData("-20", (short)-20)]
    public void MathExpression_Evaluate_Int16_ExpectedValue(string mathString, short expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<short>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    #endregion

    #region UInt16 Tests

    [Theory]
    [InlineData("2 + 5", (ushort)(2 + 5))]
    [InlineData("10 * 2", (ushort)(10 * 2))]
    [InlineData("100 / 5", (ushort)(100 / 5))]
    public void MathExpression_Evaluate_UInt16_ExpectedValue(string mathString, ushort expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<ushort>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    #endregion

    #region SByte Tests

    [Theory]
    [InlineData("2 + 5", (sbyte)(2 + 5))]
    [InlineData("10 - 5", (sbyte)(10 - 5))]
    [InlineData("-20", (sbyte)-20)]
    public void MathExpression_Evaluate_SByte_ExpectedValue(string mathString, sbyte expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate<sbyte>();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    #endregion

    private void SubscribeToEvaluating(object? sender, EvaluatingEventArgs args)
    {
        var comment = args.IsCompleted ? " //completed" : string.Empty;
        var msg = $"{args.Step}: {args.MathString[args.Start..(args.End + 1)]} = {args.Value};{comment}";
        testOutputHelper.WriteLine(msg);
    }
}

#endif
