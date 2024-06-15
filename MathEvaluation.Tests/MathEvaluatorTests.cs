using System.Globalization;
using Xunit.Abstractions;

namespace MathEvaluation.Tests;

public class MathEvaluatorTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData(null, double.NaN)]
    [InlineData("", double.NaN)]
    [InlineData("undefined", double.NaN)]
    [InlineData("   ", double.NaN)]
    [InlineData("+", double.NaN)]
    [InlineData("-", double.NaN)]
    [InlineData("-20.3", -20.3d)]
    [InlineData("2 / 5 / 2 * 5", 2d / 5 / 2 * 5)]
    [InlineData("2 + (5 - 1)", 2 + (5 - 1))]
    [InlineData("2(5 - 1)", 2 * (5 - 1))]
    [InlineData("(3 + 1) (5 - 1)", (3 + 1) * (5 - 1))]
    [InlineData("(3 + 1)(5 + 2(5 - 1))", (3 + 1) * (5 + 2 * (5 - 1)))]
    [InlineData("(3 + 1)[5 + 2(5 - 1)]", (3 + 1) * (5 + 2 * (5 - 1)))]
    [InlineData("2 + (5 * 2 - 1)", 2 + (5 * 2 - 1))]
    [InlineData("4 * 0.1 - 2", 4 * 0.1 - 2)]
    [InlineData("6 + -( -4)", 6 + - -4)]
    [InlineData("6 + - ( 4)", 6 + -4)]
    [InlineData("(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6", 2.323 * 323 - 1 / (2 + 3.33) * 4 - 6)]
    [InlineData("2 - 5 * 10 / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / 2 - 1", 2 - 5 * -10 / 2 - 1)]
    [InlineData("2 - 5 * -10 / -2 / - 2 - 1", 2 - 5 * -10d / -2 / -2 - 1)]
    [InlineData("1 - -1", 1 - -1)]
    [InlineData("(3 · 2)\u00f7(5 \u00d7 2)", 3 * 2 / (5d * 2))]
    public void MathEvaluator_Evaluate_ExpectedValue(string? expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression!);

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
    [InlineData("3 ** 4", 81)]
    [InlineData("-3 ** 4", -81)]
    [InlineData("3^4", 81)]
    [InlineData("3^4^2", 81 * 81 * 81 * 81)]
    [InlineData("2/3^4", 2 / 81d)]
    [InlineData("0.5^2*3", 0.75d)]
    [InlineData("0.5**2*3", 0.75d)]
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
    [InlineData("τ", Math.Tau)]
    [InlineData("((5 - 1)τ)", 4 * Math.Tau)]
    [InlineData("1/2τ", 1 / (2 * Math.Tau))]
    [InlineData("1/(4 - 2)τ", 1 / (2 * Math.Tau))]
    [InlineData("(1/2)τ", Math.Tau / 2)]
    [InlineData("-1/τ", -1 / Math.Tau)]
    [InlineData("1 - -1/τ", 1 + 1 / Math.Tau)]
    [InlineData("2τ * 2 / 2 + τ", Math.Tau * 3)]
    [InlineData("2τ / τ / 2 * τ", Math.Tau)]
    [InlineData("ττ", Math.Tau * Math.Tau)]
    [InlineData("+ττ", Math.Tau * Math.Tau)]
    public void MathEvaluator_Evaluate_HasTau_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("sin(30\u00b0)", 0.49999999999999994d)]
    [InlineData("sin 0.5π", 1d)]
    [InlineData("sin0.5/2", 0.2397127693021015d)]
    [InlineData("cos1", 0.54030230586813977d)]
    [InlineData("Sin(15° + 15°)", 0.49999999999999994d)]
    [InlineData("SIN((1/6)π)", 0.49999999999999994d)]
    [InlineData("sin(3.4)", -0.25554110202683122d)]
    [InlineData("sin( +3.4)", -0.25554110202683122d)]
    [InlineData("sin( -3 * 2)", 0.27941549819892586d)]
    [InlineData("sin(-3)", -0.14112000805986721d)]
    [InlineData("3 + 2sin(PI)", 3.0000000000000004d)]
    [InlineData("cos(60°)", 0.50000000000000011d)]
    [InlineData("Cos(30° + 30°)", 0.50000000000000011d)]
    [InlineData("COS((1/3)π)", 0.50000000000000011d)]
    [InlineData("sin(30°) + cos(60°)", 1d)]
    [InlineData("tan(0°)", 0)]
    [InlineData("Tan(45°)", 1d)]
    [InlineData("Tan45°", 1d)]
    [InlineData("cot(0°)", double.NaN)]
    [InlineData("Cot(45°)", 1d)]
    [InlineData("sec(0°)", 1d)]
    [InlineData("Sec(60°)", 1.9999999999999996d)]
    [InlineData("csc(0°)", double.NaN)]
    [InlineData("Csc(30°)", 2.0000000000000004d)]
    [InlineData("CSC(90°)", 1d)]
    [InlineData("sin0 + 3", 3d)]
    [InlineData("cos1 * 2 + 3", 0.54030230586813977d * 2 + 3d)]
    public void MathEvaluator_Evaluate_HasTrigonometricFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("sinh(0)", 0d)]
    [InlineData("Sinh(0.88137358701954305)", 1d)]
    [InlineData("SINH(∞)", double.PositiveInfinity)]
    [InlineData("SINH -0.48121182505960347", -0.5d)]
    [InlineData("Sinh-0.88137358701954305", -1d)]
    [InlineData("sinh(-∞)", double.NegativeInfinity)]
    [InlineData("Cosh(0)", 1d)]
    [InlineData("cosh(1.3169578969248166)", 1.9999999999999998d)]
    [InlineData("COSH(∞)", double.PositiveInfinity)]
    [InlineData("tanh(0)", 0)]
    [InlineData("TANH -0.54930614433405489", -0.5d)]
    [InlineData("Tanh-∞", -1d)]
    [InlineData("Coth(0.54930614433405489)", 2d)]
    [InlineData("coth(0)", double.NaN)]
    [InlineData("COTH(-∞)", -1d)]
    [InlineData("COTH(∞)", 1d)]
    [InlineData("coth-0.54930614433405489", -2d)]
    [InlineData("sech(1.3169578969248166)", 0.50000000000000011d)]
    [InlineData("Sech(0)", 1d)]
    [InlineData("csch(1.4436354751788103)", 0.5d)]
    [InlineData("Csch(0.88137358701954294)", 1.0000000000000002d)]
    [InlineData("CSCH(0)", double.NaN)]
    [InlineData("CSCH(∞)", 0d)]
    [InlineData("CSCH(-∞)", 0d)]
    [InlineData("CSCH -1.4436354751788103", -0.5d)]
    [InlineData("Csch-0.88137358701954294", -1.0000000000000002d)]
    public void MathEvaluator_Evaluate_HasHyperbolicTrigonometricFn_ExpectedValue(string expression,
        double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("arcsin(-\u221e)", double.NaN)]
    [InlineData("asin(\u221e)", double.NaN)]
    [InlineData("ASIN(-2)", double.NaN)]
    [InlineData("Arcsin(-1)", -Math.PI / 2)]
    [InlineData("ARCSIN(0)", 0)]
    [InlineData("Asin1", Math.PI / 2)]
    [InlineData("arccos(-\u221e)", double.NaN)]
    [InlineData("acos(\u221e)", double.NaN)]
    [InlineData("ACOS(-2)", double.NaN)]
    [InlineData("Arccos(-1)", Math.PI)]
    [InlineData("ARCCOS(0)", Math.PI / 2)]
    [InlineData("Acos1", 0)]
    [InlineData("arctan(-\u221e)", -Math.PI / 2)]
    [InlineData("atan(\u221e)", Math.PI / 2)]
    [InlineData("Atan(-2)", -1.1071487177940904d)]
    [InlineData("Arctan(-1)", -Math.PI / 4)]
    [InlineData("ARCTAN(0)", 0)]
    [InlineData("ATAN1", Math.PI / 4)]
    [InlineData("arcsec(-\u221e)", Math.PI / 2)]
    [InlineData("asec(\u221e)", Math.PI / 2)]
    [InlineData("Asec(-2)", 2.0943951023931957d)]
    [InlineData("Arcsec(-1)", Math.PI)]
    [InlineData("ASEC(1/2)", double.NaN)]
    [InlineData("ARCSEC(0)", double.NaN)]
    [InlineData("arcsec1", 0)]
    [InlineData("arccsc(-\u221e)", 0)]
    [InlineData("acsc(\u221e)", 0)]
    [InlineData("Acsc(-2)", -0.52359877559829893d)]
    [InlineData("Arccsc(-1)", -Math.PI / 2)]
    [InlineData("Arccsc(-1/2)", double.NaN)]
    [InlineData("ARCCSC(0)", double.NaN)]
    [InlineData("ACSC1", Math.PI / 2)]
    [InlineData("arccsc(2)", 0.52359877559829893d)]
    [InlineData("arccot(-\u221e)", Math.PI)]
    [InlineData("acot(\u221e)", 0d)]
    [InlineData("Arccot(-2)", 2.677945044588987d)]
    [InlineData("ARCCOT(-1)", Math.PI - Math.PI / 4)]
    [InlineData("Acot(0)", Math.PI / 2)]
    [InlineData("acot1", Math.PI / 4)]
    [InlineData("ACOT(2)", 0.46364760900080609d)]
    public void MathEvaluator_Evaluate_HasInverseTrigonometricFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("arsinh(0)", 0)]
    [InlineData("asinh(0.5)", 0.48121182505960347d)]
    [InlineData("Arsinh(1)", 0.88137358701954294d)]
    [InlineData("Asinh(2)", 1.4436354751788103d)]
    [InlineData("ARSINH(∞)", double.PositiveInfinity)]
    [InlineData("ASINH -0.5", -0.48121182505960347d)]
    [InlineData("Asinh-1", -0.88137358701954294d)]
    [InlineData("arsinh(-2)", -1.4436354751788103d)]
    [InlineData("asinh(-∞)", double.NegativeInfinity)]
    [InlineData("arcosh(0)", double.NaN)]
    [InlineData("acosh(0.5)", double.NaN)]
    [InlineData("Arcosh(1)", 0)]
    [InlineData("Acosh(2)", 1.3169578969248166d)]
    [InlineData("ARCOSH(∞)", double.PositiveInfinity)]
    [InlineData("ACOSH -0.5", double.NaN)]
    [InlineData("Acosh-1", double.NaN)]
    [InlineData("arcosh(-2)", double.NaN)]
    [InlineData("acosh(-∞)", double.NaN)]
    [InlineData("artanh(0)", 0)]
    [InlineData("atanh(0.5)", 0.54930614433405489d)]
    [InlineData("Artanh(1)", double.PositiveInfinity)]
    [InlineData("Atanh(2)", double.NaN)]
    [InlineData("ARTANH(∞)", double.NaN)]
    [InlineData("ATANH -0.5", -0.54930614433405489d)]
    [InlineData("Atanh-1", double.NegativeInfinity)]
    [InlineData("artanh(-2)", double.NaN)]
    [InlineData("atanh(-∞)", double.NaN)]
    [InlineData("arcoth(0)", double.NaN)]
    [InlineData("acoth(0.5)", double.NaN)]
    [InlineData("Arcoth(1)", double.NaN)]
    [InlineData("Acoth(2)", 0.54930614433405489d)]
    [InlineData("ARCOTH(∞)", 0)]
    [InlineData("ACOTH -0.5", double.NaN)]
    [InlineData("Acoth-1", double.NaN)]
    [InlineData("arcoth(-2)", -0.54930614433405489d)]
    [InlineData("acoth(-∞)", 0)]
    [InlineData("arsech(0)", double.NaN)]
    [InlineData("asech(0.5)", 1.3169578969248166d)]
    [InlineData("Arsech(1)", 0)]
    [InlineData("Asech(2)", double.NaN)]
    [InlineData("ARSECH(∞)", double.NaN)]
    [InlineData("ASECH -0.5", double.NaN)]
    [InlineData("Asech-1", double.NaN)]
    [InlineData("arsech(-2)", double.NaN)]
    [InlineData("asech(-∞)", double.NaN)]
    [InlineData("arcsch(0)", double.NaN)]
    [InlineData("acsch(0.5)", 1.4436354751788103d)]
    [InlineData("Arcsch(1)", 0.88137358701954294d)]
    [InlineData("Acsch(2)", 0.48121182505960347d)]
    [InlineData("ARCSCH(∞)", 0)]
    [InlineData("ACSCH -0.5", -1.4436354751788103d)]
    [InlineData("Acsch-1", -0.88137358701954294d)]
    [InlineData("arcsch(-2)", -0.48121182505960347d)]
    [InlineData("acsch(-∞)", 0)]
    public void MathEvaluator_Evaluate_HasInverseHyperbolicFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression);

        Assert.Equal(expectedValue, value, double.Epsilon);
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
    [InlineData("2 - 5 * |-10| / 2 - 1", 2 - 5 * 10 / 2 - 1)]
    [InlineData("3abs-5", 15d)]
    [InlineData("3 * Abs(  -5)", 15d)]
    [InlineData("3 / ABS(  -(9/3))", 1d)]
    [InlineData("abs(sin(-3))", 0.14112000805986721d)]
    [InlineData("|sin-3|", 0.14112000805986721d)]
    public void MathEvaluator_Evaluate_HasAbs_ExpectedValue(string? expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression!);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("\u221a25", 5)]
    [InlineData("√0.25", 0.5)]
    [InlineData("√0", 0)]
    [InlineData("√-25", double.NaN)]
    [InlineData("√(9*9)", 9)]
    [InlineData("√9√9", 9)]
    [InlineData("√9/√9", 1)]
    [InlineData("√1", 1)]
    [InlineData("1/√9", 1 / 3d)]
    [InlineData("∛8", 2)]
    [InlineData("∛-8", double.NaN)]
    [InlineData("∛8∛8", 4)]
    [InlineData("∜16", 2)]
    [InlineData("∜-16", double.NaN)]
    [InlineData("∜16∜16", 4)]
    public void MathEvaluator_Evaluate_HasSquareRoot_ExpectedValue(string? expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = MathEvaluator.Evaluate(expression!);

        Assert.Equal(expectedValue, value, double.Epsilon);
    }

    [Theory]
    [InlineData("1 + ctng(3 + 4)", "'ctng' isn't supported")]
    [InlineData("p", "'p' isn't supported")]
    public void MathEvaluator_Evaluate_HasNotSupportedFn_ThrowNotSupportedException(string expression,
        string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => MathEvaluator.Evaluate(expression));
        Assert.IsType<NotSupportedException>(ex);
        Assert.Contains(errorMessage, ex.Message);
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