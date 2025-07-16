using System.Globalization;

namespace MathEvaluation.Tests.Evaluation;

public partial class MathExpressionTests
{
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
    [InlineData("Pi((5 - 1))", 4 * Math.PI)]
    [InlineData("1/2PI", 1 / (2 * Math.PI))]
    public void MathExpression_Evaluate_HasPi_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
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
    public void MathExpression_Evaluate_HasTau_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("sin(30\u00b0)", 0.49999999999999994d)]
    [InlineData("sin 0.5π", 1d)]
    [InlineData("sin 0.5π^2", -0.97536797208363146d)]
    [InlineData("sin 90°^2", 0.6242659526396992d)]
    [InlineData("sin(90°)^2", 1d)]
    [InlineData("sin0.5/2", 0.2397127693021015d)]
    [InlineData("cos1", 0.54030230586813977d)]
    [InlineData("cos(+1)", 0.54030230586813977d)]
    [InlineData("cos-(-1)", 0.54030230586813977d)]
    [InlineData("2^sin(1)^2", 1.6336211145430648d)]
    [InlineData("2^sin1^2", 1.791876223827922d)]
    [InlineData("2^sin(1)^sin1^2", 1.8211046249505032d)]
    [InlineData("cos(1)^2", 0.54030230586813977d * 0.54030230586813977d)]
    [InlineData("cos1(1 + 2)", 0.54030230586813977d * 3)]
    [InlineData("cos1(1 + 2) mod cos1+0.5", 0.5d)]
    [InlineData("sin-3/cos1", -0.14112000805986721d / 0.54030230586813977d)]
    [InlineData("sin-3cos1", -0.14112000805986721d * 0.54030230586813977d)]
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
    public void MathExpression_Evaluate_HasTrigonometricFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
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
    public void MathExpression_Evaluate_HasHyperbolicTrigonometricFn_ExpectedValue(string mathString,
        double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("arcsin(-\u221e)", double.NaN)]
    [InlineData("sin^-1(\u221e)", double.NaN)]
    [InlineData("SIN^-1(-2)", double.NaN)]
    [InlineData("Arcsin(-1)", -Math.PI / 2)]
    [InlineData("Asin(-1)", -Math.PI / 2)]
    [InlineData("asin(-1)", -Math.PI / 2)]
    [InlineData("ASIN(-1)", -Math.PI / 2)]
    [InlineData("ARCSIN(0)", 0)]
    [InlineData("Sin^-11", Math.PI / 2)]
    [InlineData("arccos(-\u221e)", double.NaN)]
    [InlineData("Cos^-1(\u221e)", double.NaN)]
    [InlineData("COS^-1(-2)", double.NaN)]
    [InlineData("Arccos(-1)", Math.PI)]
    [InlineData("acos(-1)", Math.PI)]
    [InlineData("Acos(-1)", Math.PI)]
    [InlineData("ACOS(-1)", Math.PI)]
    [InlineData("ARCCOS(0)", Math.PI / 2)]
    [InlineData("Cos^-11", 0)]
    [InlineData("arctan(-\u221e)", -Math.PI / 2)]
    [InlineData("atan(-\u221e)", -Math.PI / 2)]
    [InlineData("Atan(-\u221e)", -Math.PI / 2)]
    [InlineData("ATAN(-\u221e)", -Math.PI / 2)]
    [InlineData("tan^-1(\u221e)", Math.PI / 2)]
    [InlineData("Tan^-1(-2)", -1.1071487177940904d)]
    [InlineData("Arctan(-1)", -Math.PI / 4)]
    [InlineData("ARCTAN(0)", 0)]
    [InlineData("TAN^-11", Math.PI / 4)]
    [InlineData("arcsec(-\u221e)", Math.PI / 2)]
    [InlineData("asec(-\u221e)", Math.PI / 2)]
    [InlineData("Asec(-\u221e)", Math.PI / 2)]
    [InlineData("ASEC(-\u221e)", Math.PI / 2)]
    [InlineData("sec^-1(\u221e)", Math.PI / 2)]
    [InlineData("Sec^-1(-2)", 2.0943951023931957d)]
    [InlineData("Arcsec(-1)", Math.PI)]
    [InlineData("SEC^-1(1/2)", double.NaN)]
    [InlineData("ARCSEC(0)", double.NaN)]
    [InlineData("arcsec1", 0)]
    [InlineData("arccsc(-\u221e)", 0)]
    [InlineData("csc^-1(\u221e)", 0)]
    [InlineData("Csc^-1(-2)", -0.52359877559829893d)]
    [InlineData("Arccsc(-1)", -Math.PI / 2)]
    [InlineData("acsc(-1)", -Math.PI / 2)]
    [InlineData("Acsc(-1)", -Math.PI / 2)]
    [InlineData("ACSC(-1)", -Math.PI / 2)]
    [InlineData("Arccsc(-1/2)", double.NaN)]
    [InlineData("ARCCSC(0)", double.NaN)]
    [InlineData("CSC^-11", Math.PI / 2)]
    [InlineData("arccsc(2)", 0.52359877559829893d)]
    [InlineData("arccot(-\u221e)", Math.PI)]
    [InlineData("Cot^-1(\u221e)", 0d)]
    [InlineData("Arccot(-2)", 2.677945044588987d)]
    [InlineData("acot(-2)", 2.677945044588987d)]
    [InlineData("Acot(-2)", 2.677945044588987d)]
    [InlineData("ACOT(-2)", 2.677945044588987d)]
    [InlineData("ARCCOT(-1)", Math.PI - Math.PI / 4)]
    [InlineData("Cot^-1(0)", Math.PI / 2)]
    [InlineData("cot^-11", Math.PI / 4)]
    [InlineData("COT^-1(2)", 0.46364760900080609d)]
    public void MathExpression_Evaluate_HasInverseTrigonometricFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("arsinh(0)", 0)]
    [InlineData("sinh^-1(0.5)", 0.48121182505960347d)]
    [InlineData("Arsinh(1)", 0.88137358701954294d)]
    [InlineData("asinh(1)", 0.88137358701954294d)]
    [InlineData("Asinh(1)", 0.88137358701954294d)]
    [InlineData("ASINH(1)", 0.88137358701954294d)]
    [InlineData("Arsinh(1)^2", 0.88137358701954294d * 0.88137358701954294d)]
    [InlineData("Sinh^-1(2)", 1.4436354751788103d)]
    [InlineData("ARSINH(∞)", double.PositiveInfinity)]
    [InlineData("SINH^-1 -0.5", -0.48121182505960347d)]
    [InlineData("Sinh^-1-1", -0.88137358701954294d)]
    [InlineData("arsinh(-2)", -1.4436354751788103d)]
    [InlineData("Sinh^-1(-∞)", double.NegativeInfinity)]
    [InlineData("arcosh(0)", double.NaN)]
    [InlineData("Cosh^-1(0.5)", double.NaN)]
    [InlineData("Arcosh(1)", 0)]
    [InlineData("Cosh^-1(2)", 1.3169578969248166d)]
    [InlineData("acosh(2)", 1.3169578969248166d)]
    [InlineData("Acosh(2)", 1.3169578969248166d)]
    [InlineData("ACOSH(2)", 1.3169578969248166d)]
    [InlineData("ARCOSH(∞)", double.PositiveInfinity)]
    [InlineData("COSH^-1 -0.5", double.NaN)]
    [InlineData("Cosh^-1-1", double.NaN)]
    [InlineData("arcosh(-2)", double.NaN)]
    [InlineData("Cosh^-1(-∞)", double.NaN)]
    [InlineData("artanh(0)", 0)]
    [InlineData("tanh^-1(0.5)", 0.54930614433405489d)]
    [InlineData("atanh(0.5)", 0.54930614433405489d)]
    [InlineData("Atanh(0.5)", 0.54930614433405489d)]
    [InlineData("ATANH(0.5)", 0.54930614433405489d)]
    [InlineData("Artanh(1)", double.PositiveInfinity)]
    [InlineData("Tanh^-1(2)", double.NaN)]
    [InlineData("ARTANH(∞)", double.NaN)]
    [InlineData("TANH^-1 -0.5", -0.54930614433405489d)]
    [InlineData("Tanh^-1-1", double.NegativeInfinity)]
    [InlineData("artanh(-2)", double.NaN)]
    [InlineData("Tanh^-1(-∞)", double.NaN)]
    [InlineData("arcoth(0)", double.NaN)]
    [InlineData("Coth^-1(0.5)", double.NaN)]
    [InlineData("Arcoth(1)", double.NaN)]
    [InlineData("Coth^-1(2)", 0.54930614433405489d)]
    [InlineData("acoth(2)", 0.54930614433405489d)]
    [InlineData("Acoth(2)", 0.54930614433405489d)]
    [InlineData("ACOTH(2)", 0.54930614433405489d)]
    [InlineData("ARCOTH(∞)", 0)]
    [InlineData("COTH^-1 -0.5", double.NaN)]
    [InlineData("Coth^-1-1", double.NaN)]
    [InlineData("arcoth(-2)", -0.54930614433405489d)]
    [InlineData("Coth^-1(-∞)", 0)]
    [InlineData("arsech(0)", double.NaN)]
    [InlineData("Sech^-1(0.5)", 1.3169578969248166d)]
    [InlineData("asech(0.5)", 1.3169578969248166d)]
    [InlineData("Asech(0.5)", 1.3169578969248166d)]
    [InlineData("ASECH(0.5)", 1.3169578969248166d)]
    [InlineData("Arsech(1)", 0)]
    [InlineData("Sech^-1(2)", double.NaN)]
    [InlineData("ARSECH(∞)", double.NaN)]
    [InlineData("SECH^-1 -0.5", double.NaN)]
    [InlineData("Sech^-1-1", double.NaN)]
    [InlineData("arsech(-2)", double.NaN)]
    [InlineData("Sech^-1(-∞)", double.NaN)]
    [InlineData("arcsch(0)", double.NaN)]
    [InlineData("Csch^-1(0.5)", 1.4436354751788103d)]
    [InlineData("acsch(0.5)", 1.4436354751788103d)]
    [InlineData("Acsch(0.5)", 1.4436354751788103d)]
    [InlineData("ACSCH(0.5)", 1.4436354751788103d)]
    [InlineData("Arcsch(1)", 0.88137358701954294d)]
    [InlineData("Csch^-1(2)", 0.48121182505960347d)]
    [InlineData("ARCSCH(∞)", 0)]
    [InlineData("CSCH^-1 -0.5", -1.4436354751788103d)]
    [InlineData("Csch^-1-1", -0.88137358701954294d)]
    [InlineData("arcsch(-2)", -0.48121182505960347d)]
    [InlineData("csch^-1(-∞)", 0)]
    public void MathExpression_Evaluate_HasInverseHyperbolicFn_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.Evaluate();

        Assert.Equal(expectedValue, value);
    }
}