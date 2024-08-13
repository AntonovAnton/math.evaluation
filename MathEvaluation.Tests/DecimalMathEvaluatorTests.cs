using System.Globalization;
using Xunit.Abstractions;
using MathEvaluation.Context.Decimal;

namespace MathEvaluation.Tests;

public class DecimalMathEvaluatorTests(ITestOutputHelper testOutputHelper)
{
    private readonly DecimalScientificMathContext _scientificContext = new();
    private readonly DecimalProgrammingMathContext _programmingContext = new();

    [Fact]
    public void MathEvaluator_EvaluateDecimal_Null_ThrowArgumentNullException()
    {
        var ex = Record.Exception(() => MathEvaluator.EvaluateDecimal(null!));
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Theory]
    [InlineData("", "Expression is empty or white space. (Parameter 'expression')")]
    [InlineData("   ", "Expression is empty or white space. (Parameter 'expression')")]
    [InlineData("+", "Expression cannot be evaluated. (Parameter 'expression')")]
    [InlineData("-", "Expression cannot be evaluated. (Parameter 'expression')")]
    public void MathEvaluator_EvaluateDecimal_Empty_ThrowArgumentException(string expression,
        string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => MathEvaluator.EvaluateDecimal(expression));
        Assert.IsType<ArgumentException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    [Fact]
    public void MathEvaluator_EvaluateDecimal_DivideByZero_ThrowDivideByZeroException()
    {
        testOutputHelper.WriteLine("0/0");

        var ex = Record.Exception(() => MathEvaluator.EvaluateDecimal("0/0"));
        Assert.IsType<DivideByZeroException>(ex);
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
    public void MathEvaluator_EvaluateDecimal_ExpectedValue(string? expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression!);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("false = true", false)]
    [InlineData("not(False)", true)]
    [InlineData("FALSE <> True", true)]
    [InlineData("false or TRUE", true)]
    [InlineData("True xor True", false)]
    [InlineData("200 >= 2.4", 200 >= 2.4)]
    [InlineData("200 <= 2.4", 200 <= 2.4)]
    [InlineData("1.0 >= 0.1 and 5.4 <= 5.4", 1.0 >= 0.1 & 5.4 <= 5.4)]
    [InlineData("1 > -0 And 2 < 3 Or 2 > 1", 1 > -0 && 2 < 1 || 2 > 1)]
    [InlineData("5.4 < 5.4", 5.4 < 5.4)]
    [InlineData("1.0 > 1.0 + -0.7 AND 5.4 < 5.5", 1.0 > 1.0 + -0.7 && 5.4 < 5.5)]
    [InlineData("1.0 - 1.95 >= 0.1", 1.0 - 1.95 >= 0.1)]
    [InlineData("2 ** 3 = 8", true)]
    [InlineData("3 % 2 <> 1.1", true)]
    [InlineData("4 <> 4 OR 5.4 = 5.4", true)]
    [InlineData("4 <> 4 OR 5.4 = 5.4 AND NOT true", false)]
    [InlineData("4 <> 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", true)]
    public void MathEvaluator_EvaluateDecimal_HasBooleanLogic_ExpectedValue(string expression, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateBoolean(expression, _programmingContext);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("(3 · 2)//(2 × 2)", 1d)]
    [InlineData("(3 * 2)//(2 * 2)", 1d)]
    [InlineData("6//-10", -1d)]
    [InlineData("4π // (3)π", 1d)]
    public void MathEvaluator_EvaluateDecimal_HasFloorDivision_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("(3 + 1)[5 + 2(5 - 1)]", (3 + 1) * (5 + 2 * (5 - 1)))]
    [InlineData("(3 · 2)÷(5 × 2)", 3 * 2 / (5d * 2))]
    [InlineData("(3 + 1)[5 + 2(5 - 1)]^2", (3 + 1) * (5 + 2 * (5 - 1)) * (5 + 2 * (5 - 1)))]
    public void MathEvaluator_EvaluateDecimal_HasScientificNotation_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

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
    public void MathEvaluator_EvaluateDecimal_HasNumbersInSpecificCulture_ExpectedValue(string expression, string? cultureName)
    {
        var expectedValue = 22888.32m * 30 / 323.34m / .5m - -1 / (2 + 22888.32m) * 4 - 6;
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var cultureInfo = cultureName == null ? null : new CultureInfo(cultureName);
        var value = MathEvaluator.EvaluateDecimal(expression, cultureInfo);

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("3**4", 81)]
    [InlineData("3**4**2", 81 * 81 * 81 * 81)]
    [InlineData("0.5**2*3", 0.75d)]
    [InlineData("-3**4", -81)]
    [InlineData("3 + 2(2 + 3.5)**2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    [InlineData("3 + 2(2 + 3.5)  **2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathEvaluator_EvaluateDecimal_HasProgrammingPower_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _programmingContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("3^4", 81d)]
    [InlineData("3^4^2", 81d * 81 * 81 * 81)]
    [InlineData("0.5^2*3", 0.75d)]
    [InlineData("-3^4", -81d)]
    [InlineData("2^3pi", 687.29133511454552d)]
    [InlineData("-3^4sin(-PI/2)", 81d)]
    [InlineData("3 + 2(2 + 3.5)^ 2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    [InlineData("3 + 2(2 + 3.5)  ^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathEvaluator_EvaluateDecimal_HasScientificPower_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("4 % 3", 1)]
    public void MathEvaluator_EvaluateDecimal_HasProgrammingModulus_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _programmingContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("4 mod 3", 1)]
    [InlineData("4 modulo 3 - 2", -1)]
    public void MathEvaluator_EvaluateDecimal_HasScientificModulus_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("0.4e3", 0.4e3)]
    [InlineData("0.4e-3", 0.4e-3)]
    [InlineData("0.4E3", 0.4E3)]
    [InlineData("0.4E-3", 0.4E-3)]
    [InlineData("6.022e23", 6.022e23)] //Avogadro's number
    [InlineData("6.626e-34", 6.626e-34)] //Planck's constant
    [InlineData(".2E3 - 2", .2E3 - 2)]
    public void MathEvaluator_EvaluateDecimal_HasExpNotationNumbers_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("e", Math.E)]
    [InlineData("200e", 200 * Math.E)]
    [InlineData("200e^- 0.15", 172.14159528501156d)]
    public void MathEvaluator_EvaluateDecimal_HasLnBase_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("π", Math.PI)]
    [InlineData("((5 - 1)π)", 4 * Math.PI)]
    [InlineData("(1/2)π", Math.PI / 2)]
    [InlineData("2π * 2 / 2 + π", Math.PI * 3)]
    [InlineData("pi", Math.PI)]
    [InlineData("((5 - 1)Pi)", 4 * Math.PI)]
    public void MathEvaluator_EvaluateDecimal_HasPi_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("τ", Math.Tau)]
    [InlineData("((5 - 1)τ)", 4 * Math.Tau)]
    [InlineData("(1/2)τ", Math.Tau / 2)]
    public void MathEvaluator_EvaluateDecimal_HasTau_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("sin(30\u00b0)", 0.49999999999999994d)]
    [InlineData("sin 0.5π", 1d)]
    [InlineData("cos1", 0.54030230586813977d)]
    [InlineData("cos(1)^2", 0.54030230586813977d * 0.54030230586813977d)]
    [InlineData("cos1^4", 0.54030230586813977d)]
    [InlineData("cos1^4/2", 0.54030230586813977d / 2)]
    [InlineData("Sin(15° + 15°)", 0.49999999999999994d)]
    [InlineData("Sin(π/12 + π/12)", 0.49999999999999994d)]
    [InlineData("SIN((1/6)π)", 0.49999999999999994d)]
    [InlineData("sin(1 + 2.4)", -0.25554110202683122d)]
    [InlineData("sin(3.4)", -0.25554110202683122d)]
    [InlineData("sin( +3.4)", -0.25554110202683122d)]
    [InlineData("sin( -3 * 2)", 0.27941549819892586d)]
    [InlineData("sin(-3)", -0.14112000805986721d)]
    [InlineData("cos(60°)", 0.50000000000000011d)]
    [InlineData("Cos(30° + 30°)", 0.50000000000000011d)]
    [InlineData("COS((1/3)π)", 0.50000000000000011d)]
    [InlineData("sin(30°) + cos(60°)", 1d)]
    [InlineData("tan(0°)", 0)]
    [InlineData("Tan(45°)", 1d)]
    [InlineData("Tan45°", 1d)]
    [InlineData("Cot(45°)", 1d)]
    [InlineData("sec(0°)", 1d)]
    [InlineData("Sec(60°)", 1.9999999999999996d)]
    [InlineData("Csc(30°)", 2.0000000000000004d)]
    [InlineData("CSC(90°)", 1d)]
    [InlineData("sin0 + 3", 3d)]
    [InlineData("cos1 * 2 + 3", 0.54030230586813977d * 2 + 3d)]
    public void MathEvaluator_EvaluateDecimal_HasTrigonometricFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("sinh(0)", 0d)]
    [InlineData("Sinh(0.88137358701954305)", 1d)]
    [InlineData("SINH -0.48121182505960347", -0.5d)]
    [InlineData("Sinh-0.88137358701954305", -1d)]
    [InlineData("Cosh(0)", 1d)]
    [InlineData("cosh(1.3169578969248166)", 1.9999999999999998d)]
    [InlineData("tanh(0)", 0)]
    [InlineData("TANH -0.54930614433405489", -0.5d)]
    [InlineData("Tanh-∞", -1d)]
    [InlineData("Coth(0.54930614433405489)", 2d)]
    [InlineData("COTH(-∞)", -1d)]
    [InlineData("COTH(∞)", 1d)]
    [InlineData("coth-0.54930614433405489", -2d)]
    [InlineData("sech(1.3169578969248166)", 0.50000000000000011d)]
    [InlineData("Sech(0)", 1d)]
    [InlineData("csch(1.4436354751788103)", 0.5d)]
    [InlineData("Csch(0.88137358701954294)", 1.0000000000000002d)]
    [InlineData("CSCH(∞)", 0d)]
    [InlineData("CSCH(-∞)", 0d)]
    [InlineData("CSCH -1.4436354751788103", -0.5d)]
    [InlineData("Csch-0.88137358701954294", -1.0000000000000002d)]
    public void MathEvaluator_EvaluateDecimal_HasHyperbolicTrigonometricFn_ExpectedValue(string expression,
        double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("Arcsin(-1)", -Math.PI / 2)]
    [InlineData("ARCSIN(0)", 0)]
    [InlineData("Sin^-11", Math.PI / 2)]
    [InlineData("Arccos(-1)", Math.PI)]
    [InlineData("ARCCOS(0)", Math.PI / 2)]
    [InlineData("Cos^-11", 0)]
    [InlineData("arctan(-∞)", -Math.PI / 2)]
    [InlineData("tan^-1(∞)", Math.PI / 2)]
    [InlineData("Tan^-1(-2)", -1.1071487177940904d)]
    [InlineData("Arctan(-1)", -Math.PI / 4)]
    [InlineData("ARCTAN(0)", 0)]
    [InlineData("TAN^-11", Math.PI / 4)]
    [InlineData("arcsec(-∞)", Math.PI / 2)]
    [InlineData("sec^-1(∞)", Math.PI / 2)]
    [InlineData("Sec^-1(-2)", 2.0943951023931957d)]
    [InlineData("Arcsec(-1)", Math.PI)]
    [InlineData("arcsec1", 0)]
    [InlineData("arccsc(-∞)", 0)]
    [InlineData("csc^-1(∞)", 0)]
    [InlineData("Csc^-1(-2)", -0.52359877559829893d)]
    [InlineData("Arccsc(-1)", -Math.PI / 2)]
    [InlineData("CSC^-11", Math.PI / 2)]
    [InlineData("arccsc(2)", 0.52359877559829893d)]
    [InlineData("arccot(-∞)", Math.PI)]
    [InlineData("Cot^-1(∞)", 0d)]
    [InlineData("Arccot(-2)", 2.677945044588987d)]
    [InlineData("ARCCOT(-1)", Math.PI - Math.PI / 4)]
    [InlineData("Cot^-1(0)", Math.PI / 2)]
    [InlineData("cot^-11", Math.PI / 4)]
    [InlineData("COT^-1(2)", 0.46364760900080609d)]
    public void MathEvaluator_EvaluateDecimal_HasInverseTrigonometricFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("arsinh(0)", 0)]
    [InlineData("sinh^-1(0.5)", 0.48121182505960347d)]
    [InlineData("Arsinh(1)", 0.88137358701954294d)]
    [InlineData("Sinh^-1(2)", 1.4436354751788103d)]
    [InlineData("SINH^-1 -0.5", -0.48121182505960347d)]
    [InlineData("Sinh^-1-1", -0.88137358701954294d)]
    [InlineData("arsinh(-2)", -1.4436354751788103d)]
    [InlineData("Arcosh(1)", 0)]
    [InlineData("Cosh^-1(2)", 1.3169578969248166d)]
    [InlineData("artanh(0)", 0)]
    [InlineData("tanh^-1(0.5)", 0.54930614433405489d)]
    [InlineData("TANH^-1 -0.5", -0.54930614433405489d)]
    [InlineData("Coth^-1(2)", 0.54930614433405489d)]
    [InlineData("ARCOTH(∞)", 0)]
    [InlineData("arcoth(-2)", -0.54930614433405489d)]
    [InlineData("Coth^-1(-∞)", 0)]
    [InlineData("Sech^-1(0.5)", 1.3169578969248166d)]
    [InlineData("Arsech(1)", 0)]
    [InlineData("Csch^-1(0.5)", 1.4436354751788103d)]
    [InlineData("Arcsch(1)", 0.88137358701954294d)]
    [InlineData("Csch^-1(2)", 0.48121182505960347d)]
    [InlineData("ARCSCH(∞)", 0)]
    [InlineData("CSCH^-1 -0.5", -1.4436354751788103d)]
    [InlineData("Csch^-1-1", -0.88137358701954294d)]
    [InlineData("arcsch(-2)", -0.48121182505960347d)]
    [InlineData("csch^-1(-∞)", 0)]
    public void MathEvaluator_EvaluateDecimal_HasInverseHyperbolicFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

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
    [InlineData("3abs-5", 15d)]
    [InlineData("3 * Abs(  -5)", 15d)]
    [InlineData("3 / ABS(  -(9/3))", 1d)]
    [InlineData("abs(sin(-3))", 0.14112000805986721d)]
    [InlineData("|sin-3|", 0.14112000805986721d)]
    [InlineData("3 + 2|-2 + -3.5|  ^2", 3 + 2 * (2 + 3.5d) * (2 + 3.5d))]
    public void MathEvaluator_EvaluateDecimal_HasAbs_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

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
    public void MathEvaluator_EvaluateDecimal_HasRoot_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

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
    [InlineData("-2ln[1/0.5 + √(1/0.5^2 + 1)]", -2 * 1.4436354751788103d)]
    public void MathEvaluator_EvaluateDecimal_HasLogarithmFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

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
    public void MathEvaluator_EvaluateDecimal_HasFloor_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

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
    public void MathEvaluator_EvaluateDecimal_HasCeiling_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

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
    public void MathEvaluator_EvaluateDecimal_HasFactorial_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.EvaluateDecimal(expression, _scientificContext);

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("ln[1/x + √(1/x^2 + 1)]", "x", 0.5, 1.4436354751788103d)]
    [InlineData("x", "x", 0.5, 0.5d)]
    [InlineData("2x", "x", 0.5, 1d)]
    [InlineData("Math.PI", $"{nameof(Math)}.{nameof(Math.PI)}", Math.PI, Math.PI)]
    public void MathEvaluator_EvaluateDecimal_HasVariable_ExpectedValue(string expression, string varName,
        decimal varValue, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"{varName} = {varValue}");

        var value = expression
            .SetContext(_scientificContext)
            .BindVariable(varValue, varName)
            .EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("getX1 + getX2", 0.5, 0.2, 0.5 + 0.2)]
    [InlineData("ln[1/-getX1 + √(1/getX2^2 + 1)]", -0.5, 0.5, 1.4436354751788103d)]
    public void MathEvaluator_EvaluateDecimal_HasGetVariableFns_ExpectedValue(string expression,
        decimal x1, decimal x2, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"x1 = {x1}, x2 = {x2}");

        var getX1 = () => x1;
        var getX2 = () => x2;
        var value = expression
            .SetContext(_scientificContext)
            .Bind(new { getX1, getX2 })
            .EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("ln100 + x1 + x2", -1, 1, 4.6051701859880918d)]
    [InlineData("ln(1/-x1 + sqrt(1/(x2*x2) + 1))", -0.5, 0.5, 1.4436354751788103d)]
    public void MathEvaluator_EvaluateDecimal_HasVariablesAndCustomLnSqrtFns_ExpectedValue(string expression,
        decimal x1, decimal x2, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");
        testOutputHelper.WriteLine($"x1 = {x1}, x2 = {x2}");

        var sqrt = Math.Sqrt;
        Func<double, double> ln = Math.Log;
        var value = expression
            .Bind(new { x1, x2, sqrt, ln })
            .EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("1 + min(2, 1, 0.5, 4)", 1 + 0.5)]
    [InlineData("2min(-1, 1, 0.5, 4, 9999)", -2d)]
    public void MathEvaluator_EvaluateDecimal_HasCustomMinFn_ExpectedValue(string expression, double expectedValue)
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
            .Bind(new { min })
            .EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Fact]
    public void MathEvaluator_Bind_HasNotSupportedCustomSystemFunc_ThrowNotSupportedException()
    {
        Func<decimal, decimal, decimal, decimal, decimal, decimal, decimal> min = (a, b, c, d, e, v) => 0m;
        var ex = Record.Exception(() => "min(3, 4)".Bind(new { min }));
        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.Func`7[System.Decimal,System.Decimal,System.Decimal,System.Decimal,System.Decimal,System.Decimal,System.Decimal] isn't supported, you can use Func<T[], T> istead.", ex.Message);
    }

    [Fact]
    public void MathEvaluator_EvaluateDecimal_HasFactorialOfNotIntegerNumber_ThrowArgumentException()
    {
        var expression = "0.2!";
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => MathEvaluator.EvaluateDecimal(expression, _scientificContext));
        Assert.IsType<ArgumentException>(ex);
        Assert.Equal("Not integer number 0.2 isn't supported by the factorial function.", ex.Message);
    }

    [Theory]
    [InlineData("1 + ctng(3 + 4)", "'ctng' isn't supported.")]
    [InlineData("p", "'p' isn't supported.")]
    public void MathEvaluator_EvaluateDecimal_HasNotSupportedFn_ThrowNotSupportedException(string expression,
        string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => MathEvaluator.EvaluateDecimal(expression));
        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }

    [Fact]
    public void MathEvaluator_EvaluateDecimal_HasIncorrectNumberFormat_ThrowFormatException()
    {
        var expression = "888,32 CHF";
        var cultureInfo = new CultureInfo("de-CH");
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => MathEvaluator.EvaluateDecimal(expression, cultureInfo));
        Assert.IsType<FormatException>(ex);
        Assert.Equal("The input string '888,32' was not in a correct format.", ex.Message);
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
    public void MathEvaluator_EvaluateDecimal_ReturnsNanOrInfinity_ThrowOverflowException(string expression)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => MathEvaluator.EvaluateDecimal(expression, _scientificContext));
        Assert.IsType<OverflowException>(ex);
    }
}