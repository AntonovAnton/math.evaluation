﻿using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using MathTrigonometric;
using System.Globalization;
using Xunit.Abstractions;

namespace MathEvaluation.Tests.Evaluation;

public partial class MathExpressionTests(ITestOutputHelper testOutputHelper)
{
    private readonly ScientificMathContext _scientificContext = new();
    private readonly ProgrammingMathContext _programmingContext = new();

    [Theory]
    [InlineData("+", "Error of evaluating the expression. It is not recognizable. Invalid token at position 1.")]
    [InlineData("-", "Error of evaluating the expression. It is not recognizable. Invalid token at position 1.")]
    public void MathExpression_Evaluate_NotRecognizable_ThrowMathEvaluationException(string expression,
        string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression).Evaluate());
        Assert.IsType<MathExpressionException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    [Theory]
    [InlineData("0/0", double.NaN)]
    [InlineData("-0", -0.0)]
    [InlineData(" \r\n\t -0", -0.0)]
    [InlineData("0 - 0", 0.0)]
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
    [InlineData("2 - 5 * +10 / 2 - 1", 2 - 5 * +10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / - -2 / - 2 - 1", 2 - 5 * -10d / - -2 / -2 - 1)]
    [InlineData("2 - 5 * -10 / +2 / - 2 - 1", 2 - 5 * -10d / +2 / -2 - 1)]
    [InlineData("2 - 5 * -10 / -2 / - 2 - 1", 2 - 5 * -10d / -2 / -2 - 1)]
    [InlineData("1 - -1", 1 - -1)]
    [InlineData("2 + \n(5 - 1) - \r\n 3", 2 + (5 - 1) - 3)]
    public void MathExpression_Evaluate_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("(3 · 2)//(2 × 2)", 1d)]
    [InlineData("(3 * 2)//(2 * 2)", 1d)]
    [InlineData("6//-10", -1d)]
    [InlineData("4π // (3)π", 1d)]
    public void MathExpression_Evaluate_HasFloorDivision_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("(3 + 1)(5 + 2(5 - 1))", (3 + 1) * (5 + 2 * (5 - 1)))]
    [InlineData("(3 · 2)÷(5 × 2)", 3 * 2 / (5d * 2))]
    [InlineData("(3 + 1)(5 + 2(5 - 1))^2", (3 + 1) * (5 + 2 * (5 - 1)) * (5 + 2 * (5 - 1)))]
    public void MathExpression_Evaluate_HasScientificNotation_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
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
    public void MathExpression_Evaluate_HasNumbersInSpecificCulture_ExpectedValue(string mathString, string? cultureName)
    {
        var expectedValue = 22888.32d * 30 / 323.34d / .5d - -1 / (2 + 22888.32d) * 4 - 6;

        var cultureInfo = cultureName == null ? CultureInfo.InvariantCulture : new CultureInfo(cultureName);
        using var expression = new MathExpression(mathString, null, cultureInfo);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("pow(1,257, 2)", 1.257 * 1.257, "ru-RU")]
    [InlineData("pow(1,257, 2)", 1257 * 1257, "en-US")]
    [InlineData("pow(,23, 2)", 0.23 * 0.23, "af")]
    [InlineData("pow( \r\n\t,23, 2)", 0.23 * 0.23, "af")]
    [InlineData("pow( \r\n\t,23, ,2 * 10)", 0.23 * 0.23, "af")]
    public void MathExpression_Evaluate_HasCommaAsDecimalSeparatorInNumbers_ExpectedValue(string mathString, double expectedValue, string cultureName)
    {
        var context = new MathContext();
        context.BindFunction(Math.Pow, "pow");

        using var expression = new MathExpression(mathString, context, new CultureInfo(cultureName));
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("|-2 CHF|^2 CHF * 30", "de-CH", 4d * 30)]
    [InlineData("|$-2|^$2 * 30", "en-US", 4d * 30)]
    public void MathExpression_Evaluate_HasNumbersInSpecificCultureAsOperand_ExpectedValue(string mathString, string? cultureName, double expectedValue)
    {
        var cultureInfo = cultureName == null ? null : new CultureInfo(cultureName);
        using var expression = new MathExpression(mathString, _scientificContext, cultureInfo);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("3**4", 81)]
    [InlineData("3**4**2", 81 * 81 * 81 * 81)]
    [InlineData("2/3**4", 2 / 81d)]
    [InlineData("0.5**2*3", 0.75d)]
    [InlineData("-3**4", -81)]
    [InlineData("(-3)**0.5", double.NaN)]
    [InlineData("3 + 2(2 + 3.5)**2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    [InlineData("3 + 2(2 + 3.5)  **2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathExpression_Evaluate_HasProgrammingPower_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _programmingContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("rad(45)", Math.PI / 4)]
    [InlineData("Rad(90)", Math.PI / 2)]
    [InlineData("RAD(360)", 2 * Math.PI)]
    public void MathExpression_Evaluate_HasDegreesToRadians_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("deg(pi / 4)", 45d)]
    [InlineData("Deg(pi / 2)", 90d)]
    [InlineData("DEG(2Pi)", 360d)]
    public void MathExpression_Evaluate_HasRadiansToDegrees_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("3^4", 81d)]
    [InlineData("3^4^2", 81d * 81 * 81 * 81)]
    [InlineData("2/3^4", 2 / 81d)]
    [InlineData("0.5^2*3", 0.75d)]
    [InlineData("-3^4", -81d)]
    [InlineData("2^3pi", 687.29133511454552d)]
    [InlineData("-3^4sin(-PI/2)", 81d)]
    [InlineData("(-3)^0.5", double.NaN)]
    [InlineData("3 + 2(2 + 3.5)^ 2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    [InlineData("3 + 2(2 + 3.5)  ^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathExpression_Evaluate_HasScientificPower_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("4 % 3", 1)]
    [InlineData("3 - 4.5 % 3.1 / 3 * 2 + 4", 3 - 4.5 % 3.1 / 3 * 2 + 4)]
    [InlineData("3 - 4.5 % 3.1 * 3 * 2 + 4", 3 - 4.5 % 3.1 * 3 * 2 + 4)]
    [InlineData("3 - 2 / 4.5 % 3.1 / 3 * 2 + 4", 3 - 2 / 4.5 % 3.1 / 3 * 2 + 4)]
    public void MathExpression_Evaluate_HasProgrammingModulus_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _programmingContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("4 mod 3", 1)]
    [InlineData("4π mod 3π", Math.PI)]
    [InlineData("4π mod 3 * π", 1.7793057612799139d)]
    [InlineData("4π mod (3)π", Math.PI)]
    [InlineData("4π mod (3 - 1)π", 0d)]
    [InlineData("4 modulo 3 - 2", -1)]
    [InlineData("3 - 4.5 Modulo 3.1 / 3 * 2 + 4", 3 - 4.5 % 3.1 / 3 * 2 + 4)]
    [InlineData("3 - 2 / 4.5 MOD 3.1 / 3 * 2 + 4", 3 - 2 / 4.5 % 3.1 / 3 * 2 + 4)]
    public void MathExpression_Evaluate_HasScientificModulus_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("0.4e3", 0.4e3)]
    [InlineData("123.0E2", 123.0E2)]
    [InlineData("0.4e-3", 0.4e-3)]
    [InlineData("0.4E3", 0.4E3)]
    [InlineData("0.4E-3", 0.4E-3)]
    [InlineData("6.022e23", 6.022e23)] //Avogadro's number
    [InlineData("6.626e-34", 6.626e-34)] //Planck's constant
    [InlineData(".2E3 - 2", .2E3 - 2)]
    [InlineData("3 - 0.4e3 / 3 * 2 + 4", 3 - 0.4e3 / 3 * 2 + 4)]
    [InlineData(".1 - 0.4e3 / .3E-2 * .1E+10 + 2e+3", .1 - 0.4e3 / .3E-2 * .1E+10 + 2e+3)]
    public void MathExpression_Evaluate_HasExpNotationNumbers_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("e", Math.E)]
    [InlineData("200e", 200 * Math.E)]
    [InlineData("200e^- 0.15", 172.14159528501156d)]
    public void MathExpression_Evaluate_HasLnBase_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("|-20.3|", 20.3d)]
    [InlineData("-|20.3|", -20.3d)]
    [InlineData("3|-5|", 15d)]
    [InlineData("2 / |-5| / 2 * 5", 2d / 5 / 2 * 5)]
    [InlineData("|2 + (5 - 1)|", 2 + (5 - 1))]
    [InlineData("2(5 - |-1|)", 2 * (5 - 1))]
    [InlineData("|-(5 - 1)|(3 + 1)", (3 + 1) * (5 - 1))]
    [InlineData("(3 + 1)*|-(5 - 1)|", (3 + 1) * (5 - 1))]
    [InlineData("6 + |( -4)|", 6 + 4)]
    [InlineData("6 + - |4|", 6 - 4)]
    [InlineData("2 - 5 * |-8 + (2 - |-4|)| / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("sin|-1|^2", 0.8414709848078965d)]
    [InlineData("abs-|-1|^2", 1d)]
    [InlineData("3abs-5", 15d)]
    [InlineData("3 * Abs(  -5)", 15d)]
    [InlineData("3 / ABS(  -(9/3))", 1d)]
    [InlineData("abs(sin(-3))", 0.14112000805986721d)]
    [InlineData("|sin-3|", 0.14112000805986721d)]
    [InlineData("3 + 2|-2 + -3.5|  ^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathExpression_Evaluate_HasAbs_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("\u221a25", 5d)]
    [InlineData("sqrt(25)", 5d)]
    [InlineData("Sqrt(25)", 5d)]
    [InlineData("SQRT(25)", 5d)]
    [InlineData("√0", 0d)]
    [InlineData("√-25", double.NaN)]
    [InlineData("√(9*9)", 9d)]
    [InlineData("√9√9", 9d)]
    [InlineData("√9(1 + 2)", 9d)]
    [InlineData("√9/√9", 1d)]
    [InlineData("√1", 1d)]
    [InlineData("1/√9", 1 / 3d)]
    [InlineData("∛8", 2)]
    [InlineData("cbrt(8)", 2)]
    [InlineData("Cbrt(8)", 2)]
    [InlineData("CBRT(8)", 2)]
    [InlineData("∛-8", double.NaN)]
    [InlineData("∛8∛8", 4d)]
    [InlineData("√9∛8", 6d)]
    [InlineData("∜16", 2d)]
    [InlineData("∜-16", double.NaN)]
    [InlineData("∜16∜16", 4d)]
    [InlineData("1/√9^2", 1 / 9d)]
    public void MathExpression_Evaluate_HasRoot_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("log(0)", double.NegativeInfinity)]
    [InlineData("Log(1)", 0d)]
    [InlineData("LOG(10)", 1d)]
    [InlineData("LOG(e)", 0.43429448190325182d)]
    [InlineData("log100", 2d)]
    [InlineData("log(-100)", double.NaN)]
    [InlineData("log(∞)", double.PositiveInfinity)]
    [InlineData("ln(0)", double.NegativeInfinity)]
    [InlineData("Ln(1)", 0d)]
    [InlineData("LN(10)", 2.3025850929940459d)]
    [InlineData("LN(10)^2", 2.3025850929940459d * 2.3025850929940459d)]
    [InlineData("LNe", 1d)]
    [InlineData("ln100", 4.6051701859880918d)]
    [InlineData("ln-100", double.NaN)]
    [InlineData("ln(∞)", double.PositiveInfinity)]
    [InlineData("-2ln(1/0.5 + √(1/0.5^2 + 1))", -2 * 1.4436354751788103d)]
    public void MathExpression_Evaluate_HasLogarithmFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("⌊-20.3⌋", -21d)]
    [InlineData("floor(-20.3)", -21d)]
    [InlineData("Floor(-20.3)", -21d)]
    [InlineData("FLOOR(-20.3)", -21d)]
    [InlineData("-⌊20.3⌋", -20d)]
    [InlineData("-⌊0⌋", 0d)]
    [InlineData("⌊-0.1⌋", -1d)]
    [InlineData("3⌊-5⌋", -15d)]
    [InlineData("2 / ⌊-5⌋ / 2 * 5", 2d / -5 / 2 * 5)]
    [InlineData("⌊2 + (5 - 1)⌋", 2 + (5 - 1))]
    [InlineData("2(5 - ⌊-1⌋)", 2 * (5 + 1))]
    [InlineData("⌊-(5 - 1)⌋(3 + 1)", -(3 + 1) * (5 - 1))]
    [InlineData("(3 + 1)*⌊-(5 - 1)⌋", (3 + 1) * -(5 - 1))]
    [InlineData("6 + ⌊( -4)⌋", 6 - 4)]
    [InlineData("6 + - ⌊4⌋", 6 - 4)]
    [InlineData("2 - 5 * ⌊-10⌋ / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("⌊sin3⌋", 0d)]
    [InlineData("⌊sin-3⌋", -1d)]
    [InlineData("3 + 2⌊2 + 3.5⌋  ^2", 3 + 2 * 25d)]
    public void MathExpression_Evaluate_HasFloor_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("⌈-20.3⌉", -20d)]
    [InlineData("ceil(-20.3)", -20d)]
    [InlineData("Ceil(-20.3)", -20d)]
    [InlineData("CEIL(-20.3)", -20d)]
    [InlineData("-⌈20.3⌉", -21d)]
    [InlineData("-⌈0⌉", 0d)]
    [InlineData("⌈-0.1⌉", 0d)]
    [InlineData("3⌈-5⌉", -15d)]
    [InlineData("2 / ⌈-5⌉ / 2 * 5", 2d / -5 / 2 * 5)]
    [InlineData("⌈2 + (5 - 1)⌉", 2 + (5 - 1))]
    [InlineData("2(5 - ⌈-1⌉)", 2 * (5 + 1))]
    [InlineData("⌈-(5 - 1)⌉(3 + 1)", -(3 + 1) * (5 - 1))]
    [InlineData("(3 + 1)*⌈-(5 - 1)⌉", (3 + 1) * -(5 - 1))]
    [InlineData("6 + ⌈( -4)⌉", 6 - 4)]
    [InlineData("6 + - ⌈4⌉", 6 - 4)]
    [InlineData("2 - 5 * ⌈-10⌉ / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("⌈sin3⌉", 1d)]
    [InlineData("⌈sin-3⌉", 0d)]
    [InlineData("⌈2 + 3.5⌉", 6d)]
    [InlineData("3 + 2⌈2 + 3.5⌉  ^2", 3 + 2 * 6d * 6d)]
    public void MathExpression_Evaluate_HasCeiling_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("(4-1)!", 6d)]
    [InlineData("2/2!^3", 0.25d)]
    [InlineData("3! mod 2!", 0d)]
    [InlineData("5!/(2!(5 - 2)!)", 10d)]
    [InlineData("cos1!^3", 0.54030230586813977d)]
    [InlineData("cos(0)!^3", 1d)]
    [InlineData("2!^(3)!", 64d)]
    [InlineData("2!^(3)!^2!", 68719476736d)]
    [InlineData("2!^3^2!", 512d)]
    [InlineData("2!^(3)^2!", 512d)]
    public void MathExpression_Evaluate_HasFactorial_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("ln(1/x + √(1/x^2 + 1))", "x", 0.5, 1.4436354751788103d)]
    [InlineData("x", "x", 0.5, 0.5d)]
    [InlineData("2x", "x", 0.5, 1d)]
    [InlineData("200x^- 0.15", "x", Math.E, 172.14159528501156d)]
    [InlineData("Math.PI", $"{nameof(Math)}.{nameof(Math.PI)}", Math.PI, Math.PI)]
    [InlineData("2 * Math.PI", $"{nameof(Math)}.{nameof(Math.PI)}", Math.PI, 2 * Math.PI)]
    public void MathExpression_Evaluate_HasVariable_ExpectedValue(string expression, string varName,
        double varValue, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"{varName} = {varValue}");

        var parameters = new MathParameters();
        parameters.BindVariable(varValue, varName);

        var value = expression.Evaluate(parameters, _scientificContext, CultureInfo.InvariantCulture);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("getX1() + getX2( )", 0.5, 0.2, 0.5 + 0.2)]
    [InlineData("getX1()^2 + 2^getX2^2", 0.5, 3, 0.5 * 0.5 + 512)]
    [InlineData("sin2getX2^2", 0.5, 3, 8.1836768414311347d)]
    [InlineData("sin2getX2()^2", 0.5, 3, 8.1836768414311347d)]
    [InlineData("SINgetX1^getX2", 0.5, 3, 0.12467473338522769d)]
    [InlineData("ln(1/-getX1 + √(1/getX2^2 + 1))", -0.5, 0.5, 1.4436354751788103d)]
    public void MathExpression_Evaluate_HasGetValueFns_ExpectedValue(string expression,
        double x1, double x2, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"x1 = {x1}, x2 = {x2}");

        var getX1 = () => x1;
        var getX2 = () => x2;

        _scientificContext.Bind(new { getX1, getX2 });

        var value = expression.Evaluate(null, _scientificContext, CultureInfo.InvariantCulture);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("ln100 + x1 + x2", -1, 1, 4.6051701859880918d)]
    [InlineData("ln(1/-x1 + sqrt(1/(x2*x2) + 1))", -0.5, 0.5, 1.4436354751788103d)]
    public void MathExpression_Evaluate_HasVariablesAndCustomFns_ExpectedValue(string expression,
        double x1, double x2, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"x1 = {x1}, x2 = {x2}");

        var sqrt = Math.Sqrt;
        Func<double, double> ln = Math.Log;

        var context = new MathContext(new { sqrt, ln });
        var value = expression.Evaluate(new { x1, x2 }, context, CultureInfo.InvariantCulture);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("1 + min(2, 1, 0.5, 4)", 1 + 0.5)]
    [InlineData("2min(-1, 1, 0.5, 4, 9999)", -2d)]
    public void MathExpression_Evaluate_HasCustomMinFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var min = (double[] args) =>
        {
            var minValue = args[0];
            for (var i = 1; args.Length > i; i++)
            {
                if (args[i] < minValue)
                    minValue = args[i];
            }

            return minValue;
        };

        var context = new MathContext();
        context.Bind(new { min });

        var value = expression.Evaluate(null, context, CultureInfo.InvariantCulture);

        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public void MathExpression_Evaluate_HasExpressionVariable_ExpectedValue()
    {
        var x = "x1 + x2";
        var mathString = "x + sin(x)";
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate(new { x, x1 = 0.5d, x2 = 0.2d });

        Assert.Equal(0.7 + Math.Sin(0.7), value);
    }

    [Fact]
    public void MathExpression_Evaluate_HasExpressionVariableInContext_ExpectedValue()
    {
        var x = "x1 + x2";
        var context = new MathContext();
        context.BindExpressionVariable(x);
        context.BindFunction(Math.Sin, "sin");

        var mathString = "x + sin(x)";
        using var expression = new MathExpression(mathString, context, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate(new { x1 = 0.5d, x2 = 0.2d });

        Assert.Equal(0.7 + Math.Sin(0.7), value);
    }

    [Fact]
    public void MathExpression_Evaluate_BindExpressionVariable_ExpectedValue()
    {
        var x = "x1 + x2";
        var mathString = "cos(x) + sin(x)";
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var parameters = new MathParameters();
        parameters.BindVariable(0.5, "x1");
        parameters.BindVariable(0.2, "x2");
        parameters.BindExpressionVariable(x);
        var value = expression.Evaluate(parameters);

        Assert.Equal(Math.Cos(0.7) + Math.Sin(0.7), value);
    }

    [Fact]
    public void MathExpression_Evaluate_HasFactorialOfNotIntegerNumber_ThrowArgumentException()
    {
        var expression = "0.2!";
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, _scientificContext, CultureInfo.InvariantCulture).Evaluate());
        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<ArgumentException>(ex.InnerException);

        Assert.Equal($"Error of evaluating the expression. Not integer number {0.2} isn't supported by the factorial function.", ex.Message);
        Assert.Equal($"Not integer number {0.2} isn't supported by the factorial function.", ex.InnerException.Message);
    }

    [Theory]
    [InlineData("1 + ctng(3 + 4)",
        "Error of evaluating the expression. 'ctng' is not recognizable, maybe setting the appropriate MathContext could help. Invalid token at position 4.")]
    [InlineData("p",
        "Error of evaluating the expression. 'p' is not recognizable, maybe setting the appropriate MathContext could help. Invalid token at position 0.")]
    public void MathExpression_Evaluate_HasUnknownToken_ThrowMathEvaluationException(string expression, string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression).Evaluate());
        Assert.IsType<MathExpressionException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    [Theory]
    [InlineData("123.E2")]
    [InlineData("888e3.2")]
    public void MathExpression_Evaluate_HasIncorrectNumberFormat_ThrowFormatException(string expression)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, null, CultureInfo.InvariantCulture).EvaluateDecimal());

        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<FormatException>(ex.InnerException);

        Assert.Contains($"Error of evaluating the expression. The input string '", ex.Message);
        Assert.Contains("' was not in a correct format.", ex.Message);

        Assert.Contains($"The input string '", ex.InnerException.Message);
        Assert.Contains("' was not in a correct format.", ex.InnerException.Message);
    }

    [Theory]
    [InlineData("12 + 3 * (120 +5", "Error of evaluating the expression. It doesn't have the ')' closing symbol. Invalid token at position 9.")]
    [InlineData("abs(1/.5 + √(1/(0.5*0.5) + 1)", "Error of evaluating the expression. It doesn't have the ')' closing symbol. Invalid token at position 3.")]
    [InlineData("2 - 5 * ⌈-10 / 2 - 1", "Error of evaluating the expression. It doesn't have the '⌉' closing symbol. Invalid token at position 8.")]
    public void MathExpression_Evaluate_ParenthesesOrFuncAreNotClosed_ThrowMathEvaluationException(string expression, string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, _scientificContext, CultureInfo.InvariantCulture).Evaluate());
        Assert.IsType<MathExpressionException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    [Theory]
    [InlineData("12 + abs()", "Error of evaluating the expression. The operand is not recognizable. Invalid token at position 9.")]
    [InlineData("12 + abs / 1", "Error of evaluating the expression. The operand is not recognizable. Invalid token at position 8.")]
    [InlineData("0.5 + abs+1", "Error of evaluating the expression. The operand is not recognizable. Invalid token at position 9.")]
    public void MathExpression_Evaluate_HasInvalidOperand_ThrowMathEvaluationException(string expression, string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, _scientificContext, CultureInfo.InvariantCulture).Evaluate());
        Assert.IsType<MathExpressionException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    private void SubscribeToEvaluating(object? sender, EvaluatingEventArgs args)
    {
        var comment = args.IsCompleted ? " //completed" : string.Empty;
        var msg = $"{args.Step}: {args.MathString[args.Start..(args.End + 1)]} = {args.Value};{comment}";
        testOutputHelper.WriteLine(msg);
    }
}