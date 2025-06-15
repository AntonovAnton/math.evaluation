using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System.Globalization;
using Xunit.Abstractions;

namespace MathEvaluation.Tests.Evaluation;

// ReSharper disable once InconsistentNaming
public partial class MathExpressionTests_Decimal(ITestOutputHelper testOutputHelper)
{
    private readonly DecimalScientificMathContext _scientificContext = new();
    private readonly DecimalProgrammingMathContext _programmingContext = new();

    [Theory]
    [InlineData("+", "Error of evaluating the expression. It is not recognizable. Invalid token at position 1.")]
    [InlineData("-", "Error of evaluating the expression. It is not recognizable. Invalid token at position 1.")]
    public void MathExpression_EvaluateDecimal_NotRecognizable_ThrowMathEvaluationException(string expression,
        string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression).EvaluateDecimal());

        Assert.IsType<MathExpressionException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    [Fact]
    public void MathExpression_EvaluateDecimal_DivideByZero_ThrowDivideByZeroException()
    {
        testOutputHelper.WriteLine("0/0");

        var ex = Record.Exception(() => new MathExpression("0/0").EvaluateDecimal());

        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<DivideByZeroException>(ex.InnerException);

        Assert.Equal("Error of evaluating the expression. Attempted to divide by zero.", ex.Message);
    }

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
    [InlineData("2 - 5 * 10 / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("2 - 5 * +10 / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / - -2 / - 2 - 1", 2 - 5 * -10d / 2 / -2 - 1)]
    [InlineData("2 - 5 * -10 / +2 / - 2 - 1", 2 - 5 * -10d / 2 / -2 - 1)]
    [InlineData("2 - 5 * -10 / -2 / - 2 - 1", 2 - 5 * -10d / -2 / -2 - 1)]
    [InlineData("1 - -1", 1 - -1)]
    [InlineData("2 + \n(5 - 1) - \n\r 3", 2 + (5 - 1) - 3)]
    public void MathExpression_EvaluateDecimal_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("(3 · 2)//(2 × 2)", 1d)]
    [InlineData("(3 * 2)//(2 * 2)", 1d)]
    [InlineData("6//-10", -1d)]
    [InlineData("4π // (3)π", 1d)]
    public void MathExpression_EvaluateDecimal_HasFloorDivision_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("(3 + 1)(5 + 2(5 - 1))", (3 + 1) * (5 + 2 * (5 - 1)))]
    [InlineData("(3 · 2)÷(5 × 2)", 3 * 2 / (5d * 2))]
    [InlineData("(3 + 1)(5 + 2(5 - 1))^2", (3 + 1) * (5 + 2 * (5 - 1)) * (5 + 2 * (5 - 1)))]
    public void MathExpression_EvaluateDecimal_HasScientificNotation_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
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
    public void MathExpression_EvaluateDecimal_HasNumbersInSpecificCulture_ExpectedValue(string mathString, string? cultureName)
    {
        var expectedValue = 22888.32m * 30 / 323.34m / .5m - -1 / (2 + 22888.32m) * 4 - 6;

        var cultureInfo = cultureName == null ? CultureInfo.InvariantCulture : new CultureInfo(cultureName);
        using var expression = new MathExpression(mathString, null, cultureInfo);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("|-2 CHF|^2 CHF * 30", "de-CH", 4d * 30)]
    [InlineData("|$-2|^$2 * 30", "en-US", 4d * 30)]
    public void MathExpression_EvaluateDecimal_HasNumbersInSpecificCultureAsOperand_ExpectedValue(string mathString, string? cultureName, double expectedValue)
    {
        var cultureInfo = cultureName == null ? null : new CultureInfo(cultureName);
        using var expression = new MathExpression(mathString, _scientificContext, cultureInfo);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("3**4", 81)]
    [InlineData("3**4**2", 81 * 81 * 81 * 81)]
    [InlineData("0.5**2*3", 0.75d)]
    [InlineData("-3**4", -81)]
    [InlineData("3 + 2(2 + 3.5)**2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    [InlineData("3 + 2(2 + 3.5)  **2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathExpression_EvaluateDecimal_HasProgrammingPower_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _programmingContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("3^4", 81d)]
    [InlineData("3^4^2", 81d * 81 * 81 * 81)]
    [InlineData("0.5^2*3", 0.75d)]
    [InlineData("-3^4", -81d)]
    [InlineData("2^3pi", 687.291335114541d)]
    [InlineData("-3^4sin(-PI/2)", 81d)]
    [InlineData("3 + 2(2 + 3.5)^ 2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    [InlineData("3 + 2(2 + 3.5)  ^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathExpression_EvaluateDecimal_HasScientificPower_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("4 % 3", 1)]
    public void MathExpression_EvaluateDecimal_HasProgrammingModulus_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _programmingContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("4 mod 3", 1)]
    [InlineData("4 modulo 3 - 2", -1)]
    public void MathExpression_EvaluateDecimal_HasScientificModulus_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
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
    public void MathExpression_EvaluateDecimal_HasExpNotationNumbers_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, null, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("e", Math.E)]
    [InlineData("200e", 543.656365691808d)]
    [InlineData("200e^- 0.1", 180.967483607192d)]
    public void MathExpression_EvaluateDecimal_HasLnBase_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
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
    [InlineData("abs-|-1|^2", 1d)]
    [InlineData("3abs-5", 15d)]
    [InlineData("3 * Abs(  -5)", 15d)]
    [InlineData("3 / ABS(  -(9/3))", 1d)]
    [InlineData("abs(sin(-3))", 0.14112000805986721d)]
    [InlineData("|sin-3|", 0.14112000805986721d)]
    [InlineData("3 + 2|-2 + -3.5|  ^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathExpression_EvaluateDecimal_HasAbs_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("\u221a25", 5d)]
    [InlineData("√0", 0d)]
    [InlineData("√(9*9)", 9d)]
    [InlineData("√9√9", 9d)]
    [InlineData("√9(1 + 2)", 9d)]
    [InlineData("√9/√9", 1d)]
    [InlineData("√1", 1d)]
    [InlineData("∛8", 2)]
    [InlineData("∛8∛8", 4d)]
    [InlineData("√9∛8", 6d)]
    [InlineData("∜16", 2d)]
    [InlineData("∜16∜16", 4d)]
    public void MathExpression_EvaluateDecimal_HasRoot_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Log(1)", 0d)]
    [InlineData("LOG(10)", 1d)]
    [InlineData("LOG(e)", 0.43429448190325182d)]
    [InlineData("log100", 2d)]
    [InlineData("Ln(1)", 0d)]
    [InlineData("LN(10)", 2.3025850929940459d)]
    [InlineData("LNe", 1d)]
    [InlineData("ln100", 4.6051701859880918d)]
    [InlineData("-2ln(1/0.5 + √(1/0.5^2 + 1))", -2 * 1.4436354751788103d)]
    public void MathExpression_EvaluateDecimal_HasLogarithmFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("⌊-20.3⌋", -21d)]
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
    public void MathExpression_EvaluateDecimal_HasFloor_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("⌈-20.3⌉", -20d)]
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
    public void MathExpression_EvaluateDecimal_HasCeiling_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
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
    public void MathExpression_EvaluateDecimal_HasFactorial_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("ln(1/x + √(1/x^2 + 1))", "x", 0.5, 1.4436354751788103d)]
    [InlineData("x", "x", 0.5, 0.5d)]
    [InlineData("2x", "x", 0.5, 1d)]
    [InlineData("200x^- 0.15", "x", 2, 180.25009252216600d)]
    [InlineData("Math.PI", $"{nameof(Math)}.{nameof(Math.PI)}", Math.PI, Math.PI)]
    public void MathExpression_EvaluateDecimal_HasVariable_ExpectedValue(string expression, string varName,
        decimal varValue, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"{varName} = {varValue}");

        var parameters = new MathParameters();
        parameters.BindVariable(varValue, varName);

        var value = expression.EvaluateDecimal(parameters, _scientificContext, CultureInfo.InvariantCulture);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("getX1() + getX2( )", 0.5, 0.2, 0.5 + 0.2)]
    [InlineData("getX1()^2 + 2^getX2^2", 0.5, 3, 0.5 * 0.5 + 512)]
    [InlineData("ln(1/-getX1 + √(1/getX2^2 + 1))", -0.5, 0.5, 1.4436354751788103d)]
    public void MathExpression_EvaluateDecimal_HasGetVariableFns_ExpectedValue(string expression,
        decimal x1, decimal x2, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"x1 = {x1}, x2 = {x2}");

        var getX1 = () => x1;
        var getX2 = () => x2;

        var value = expression.EvaluateDecimal(new { getX1, getX2 }, _scientificContext, CultureInfo.InvariantCulture);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("ln100 + x1 + x2", -1, 1, 4.6051701859880918d)]
    [InlineData("ln(1/-x1 + sqrt(1/(x2*x2) + 1))", -0.5, 0.5, 1.4436354751788103d)]
    public void MathExpression_EvaluateDecimal_HasVariablesAndCustomLnSqrtFns_ExpectedValue(string expression,
        decimal x1, decimal x2, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"x1 = {x1}, x2 = {x2}");

        var sqrt = Math.Sqrt;
        Func<double, double> ln = Math.Log;

        var parameters = new MathParameters();
        parameters.Bind(new { x1, x2, sqrt, ln });

        var value = expression
            .EvaluateDecimal(parameters);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("1 + min(2, 1, 0.5, 4)", 1 + 0.5)]
    [InlineData("2min(-1, 1, 0.5, 4, 9999)", -2d)]
    public void MathExpression_EvaluateDecimal_HasCustomMinFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var min = (decimal[] args) =>
        {
            var minValue = args[0];
            for (var i = 1; args.Length > i; i++)
            {
                if (args[i] < minValue)
                    minValue = args[i];
            }

            return minValue;
        };

        var value = expression
            .EvaluateDecimal(new { min }, null, CultureInfo.InvariantCulture);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Fact]
    public void MathExpression_EvaluateDecimal_HasFactorialOfNotIntegerNumber_ThrowArgumentException()
    {
        var expression = "0.2!";
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, _scientificContext, CultureInfo.InvariantCulture).EvaluateDecimal());
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
    public void MathExpression_EvaluateDecimal_HasUnknownToken_ThrowMathEvaluationException(string expression,
        string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression).EvaluateDecimal());

        Assert.IsType<MathExpressionException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    [Fact]
    public void MathExpression_EvaluateDecimal_HasIncorrectNumberFormat_ThrowFormatException()
    {
        var expression = "888e3.2";
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression).EvaluateDecimal());

        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<FormatException>(ex.InnerException);

        Assert.Equal("Error of evaluating the expression. The input string '888e3.2' was not in a correct format.", ex.Message);
        Assert.Equal("The input string '888e3.2' was not in a correct format.", ex.InnerException.Message);
    }

    [Theory]
    [InlineData("(-3)^0.5")]
    [InlineData("cot(0°)")]
    [InlineData("csc(0°)")]
    [InlineData("coth(0)")]
    [InlineData("CSCH(0)")]
    [InlineData("arcsin(-∞)")]
    [InlineData("sin^-1(∞)")]
    [InlineData("SIN^-1(-2)")]
    [InlineData("arccos(-∞)")]
    [InlineData("Cos^-1(∞)")]
    [InlineData("COS^-1(-2)")]
    [InlineData("SEC^-1(1/2)")]
    [InlineData("ARCSEC(0)")]
    [InlineData("Arccsc(-1/2)")]
    [InlineData("ARCCSC(0)")]
    [InlineData("arcosh(0)")]
    [InlineData("Cosh^-1(0.5)")]
    [InlineData("COSH^-1 -0.5")]
    [InlineData("Cosh^-1-1")]
    [InlineData("arcosh(-2)")]
    [InlineData("Cosh^-1(-∞)")]
    [InlineData("Tanh^-1(2)")]
    [InlineData("ARTANH(∞)")]
    [InlineData("artanh(-2)")]
    [InlineData("Tanh^-1(-∞)")]
    [InlineData("arcoth(0)")]
    [InlineData("Coth^-1(0.5)")]
    [InlineData("Arcoth(1)")]
    [InlineData("COTH^-1 -0.5")]
    [InlineData("Coth^-1-1")]
    [InlineData("arsech(0)")]
    [InlineData("Sech^-1(2)")]
    [InlineData("ARSECH(∞)")]
    [InlineData("SECH^-1 -0.5")]
    [InlineData("Sech^-1-1")]
    [InlineData("arsech(-2)")]
    [InlineData("Sech^-1(-∞)")]
    [InlineData("arcsch(0)")]
    [InlineData("√-25")]
    [InlineData("∛-8")]
    [InlineData("∜-16")]
    [InlineData("log(-100)")]
    [InlineData("ln-100")]
    [InlineData("SINH(79228162514264337593543950335)")]
    [InlineData("Artanh(1)")]
    [InlineData("SINH(∞)")]
    [InlineData("ARSINH(∞)")]
    [InlineData("ARCOSH(∞)")]
    [InlineData("log(∞)")]
    [InlineData("ln(∞)")]
    [InlineData("sinh(-79228162514264337593543950335)")]
    [InlineData("Tanh^-1-1")]
    [InlineData("sinh(-∞)")]
    [InlineData("Sinh^-1(-∞)")]
    public void MathExpression_EvaluateDecimal_ReturnsNanOrInfinity_ThrowOverflowException(string expression)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, _scientificContext, CultureInfo.InvariantCulture).EvaluateDecimal());

        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<OverflowException>(ex.InnerException);

        Assert.Equal("Error of evaluating the expression. Value was either too large or too small for a Decimal.", ex.Message);
    }

    [Theory]
    [InlineData("12 + 3 * (120 +5", "Error of evaluating the expression. It doesn't have the ')' closing symbol. Invalid token at position 9.")]
    [InlineData("abs(1/.5 + √(1/(0.5*0.5) + 1)", "Error of evaluating the expression. It doesn't have the ')' closing symbol. Invalid token at position 3.")]
    [InlineData("2 - 5 * ⌈-10 / 2 - 1", "Error of evaluating the expression. It doesn't have the '⌉' closing symbol. Invalid token at position 8.")]
    public void MathExpression_EvaluateDecimal_ParenthesesOrFuncAreNotClosed_ThrowMathEvaluationException(string expression, string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, _scientificContext, CultureInfo.InvariantCulture).EvaluateDecimal());

        Assert.IsType<MathExpressionException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    [Theory]
    [InlineData("12 + abs()", "Error of evaluating the expression. The operand is not recognizable. Invalid token at position 9.")]
    [InlineData("12 + abs / 1", "Error of evaluating the expression. The operand is not recognizable. Invalid token at position 8.")]
    [InlineData("0.5 + abs+1", "Error of evaluating the expression. The operand is not recognizable. Invalid token at position 9.")]
    public void MathExpression_EvaluateDecimal_HasInvalidOperand_ThrowMathEvaluationException(string expression, string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression, _scientificContext, CultureInfo.InvariantCulture).EvaluateDecimal());

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