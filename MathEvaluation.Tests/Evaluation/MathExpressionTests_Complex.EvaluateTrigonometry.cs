using System.Numerics;

namespace MathEvaluation.Tests.Evaluation;

// ReSharper disable once InconsistentNaming
public partial class MathExpressionTests_Complex
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
    public void MathExpression_EvaluateComplex_HasPi_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateComplex();

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
    public void MathExpression_EvaluateComplex_HasTau_ExpectedValue(string mathString, double expectedValue)
    {
        using var expression = new MathExpression(mathString, _scientificContext);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateComplex();

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
    [InlineData("cot(0°)", double.NaN, double.NaN)]
    [InlineData("Cot(45°)", 1d)]
    [InlineData("sec(0°)", 1d)]
    [InlineData("Sec(60°)", 1.9999999999999996d)]
    [InlineData("csc(0°)", double.NaN, double.NaN)]
    [InlineData("Csc(30°)", 2.0000000000000004d)]
    [InlineData("CSC(90°)", 1d)]
    [InlineData("sin0 + 3", 3d)]
    [InlineData("cos1 * 2 + 3", 0.54030230586813977d * 2 + 3d)]
    [InlineData("sin(2 + 3i) * arctan(4i)/(1 - 6i)", 1.1001786515830083, 2.3907445385260218)]
    public void MathExpression_EvaluateComplex_HasTrigonometricFn_ExpectedValue(string mathString, double expectedReal, double expectedImaginary = 0)
    {
        using var expression = new MathExpression(mathString, _scientificContext);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateComplex();

        Assert.Equal(new Complex(expectedReal, expectedImaginary), value);
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
    [InlineData("coth(0)", double.NaN, double.NaN)]
    [InlineData("COTH(-∞)", -1d)]
    [InlineData("COTH(∞)", 1d)]
    [InlineData("coth-0.54930614433405489", -2d)]
    [InlineData("sech(1.3169578969248166)", 0.50000000000000011d)]
    [InlineData("Sech(0)", 1d)]
    [InlineData("csch(1.4436354751788103)", 0.5d)]
    [InlineData("Csch(0.88137358701954294)", 1.0000000000000002d)]
    [InlineData("CSCH(0)", double.NaN, double.NaN)]
    [InlineData("CSCH(∞)", 0d)]
    [InlineData("CSCH(-∞)", 0d)]
    [InlineData("CSCH -1.4436354751788103", -0.5d)]
    [InlineData("Csch-0.88137358701954294", -1.0000000000000002d)]
    public void MathExpression_EvaluateComplex_HasHyperbolicTrigonometricFn_ExpectedValue(string mathString,
        double expectedReal, double expectedImaginary = 0)
    {
        using var expression = new MathExpression(mathString, _scientificContext);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateComplex();

        Assert.Equal(new Complex(expectedReal, expectedImaginary), value);
    }

    [Theory]
    [InlineData("arcsin(-\u221e)", -1.5707963267948966, double.PositiveInfinity)]
    [InlineData("sin^-1(\u221e)", 1.5707963267948966, double.PositiveInfinity)]
    [InlineData("SIN^-1(-2)", -1.5707963267948966, 1.3169578969248166)]
    [InlineData("Arcsin(-1)", -Math.PI / 2)]
    [InlineData("ARCSIN(0)", 0)]
    [InlineData("Sin^-11", Math.PI / 2)]
    [InlineData("arccos(-\u221e)", 3.141592653589793, double.PositiveInfinity)]
    [InlineData("Cos^-1(\u221e)", 0, double.PositiveInfinity)]
    [InlineData("COS^-1(-2)", 3.141592653589793, 1.3169578969248166)]
    [InlineData("Arccos(-1)", Math.PI)]
    [InlineData("ARCCOS(0)", Math.PI / 2)]
    [InlineData("Cos^-11", 0)]
    [InlineData("arctan(-\u221e)", -Math.PI / 2)]
    [InlineData("tan^-1(\u221e)", Math.PI / 2)]
    [InlineData("Tan^-1(-2)", -1.1071487177940904d)]
    [InlineData("Arctan(-1)", -Math.PI / 4)]
    [InlineData("ARCTAN(0)", 0)]
    [InlineData("TAN^-11", Math.PI / 4)]
    [InlineData("arcsec(-\u221e)", Math.PI / 2)]
    [InlineData("sec^-1(\u221e)", Math.PI / 2)]
    [InlineData("Sec^-1(-2)", 2.0943951023931953d)]
    [InlineData("Arcsec(-1)", Math.PI)]
    [InlineData("SEC^-1(1/2)", 0, 1.3169578969248166)]
    [InlineData("ARCSEC(0)", double.NaN, double.NaN)]
    [InlineData("arcsec1", 0)]
    [InlineData("arccsc(-\u221e)", 0)]
    [InlineData("csc^-1(\u221e)", 0)]
    [InlineData("Csc^-1(-2)", -0.52359877559829893d)]
    [InlineData("Arccsc(-1)", -Math.PI / 2)]
    [InlineData("Arccsc(-1/2)", -1.5707963267948966, 1.3169578969248166)]
    [InlineData("ARCCSC(0)", double.NaN, double.NaN)]
    [InlineData("CSC^-11", Math.PI / 2)]
    [InlineData("arccsc(2)", 0.52359877559829893d)]
    [InlineData("arccot(-\u221e)", Math.PI)]
    [InlineData("Cot^-1(\u221e)", 0d)]
    [InlineData("Arccot(-2)", 2.677945044588987d)]
    [InlineData("ARCCOT(-1)", Math.PI - Math.PI / 4)]
    [InlineData("Cot^-1(0)", Math.PI / 2)]
    [InlineData("cot^-11", Math.PI / 4)]
    [InlineData("COT^-1(2)", 0.46364760900080609d)]
    [InlineData("cos(2cos^-1(2))", 6.999999999999998d)] //7
    public void MathExpression_EvaluateComplex_HasInverseTrigonometricFn_ExpectedValue(string mathString, double expectedReal, double expectedImaginary = 0)
    {
        using var expression = new MathExpression(mathString, _scientificContext);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateComplex();

        Assert.Equal(new Complex(expectedReal, expectedImaginary), value);
    }

    [Theory]
    [InlineData("arsinh(0)", 0)]
    [InlineData("sinh^-1(0.5)", 0.48121182505960347d)]
    [InlineData("Arsinh(1)", 0.88137358701954294d)]
    [InlineData("Arsinh(1)^2", 0.88137358701954294d * 0.88137358701954294d)]
    [InlineData("Sinh^-1(2)", 1.4436354751788103d)]
    [InlineData("ARSINH(∞)", double.PositiveInfinity)]
    [InlineData("SINH^-1 -0.5", -0.48121182505960347d)]
    [InlineData("Sinh^-1-1", -0.88137358701954294d)]
    [InlineData("arsinh(-2)", -1.4436354751788103d)]
    [InlineData("Sinh^-1(-∞)", double.NegativeInfinity)]
    [InlineData("arcosh(0)", 0, 1.5707963267948966)]
    [InlineData("Arcosh(1)", 0)]
    [InlineData("Cosh^-1(2)", 1.3169578969248166d)]
    [InlineData("ARCOSH(∞)", double.PositiveInfinity)]
    [InlineData("Cosh^-1-1", 0, 3.141592653589793)]
    [InlineData("arcosh(-2)", -1.3169578969248164, 3.141592653589793)]
    [InlineData("Cosh^-1(-∞)", double.PositiveInfinity, double.NaN)]
    [InlineData("artanh(0)", 0)]
    [InlineData("tanh^-1(0.5)", 0.54930614433405489d)]
    [InlineData("Artanh(1)", double.PositiveInfinity)]
    [InlineData("Tanh^-1(2)", 0.5493061443340549, -1.5707963267948966)]
    [InlineData("ARTANH(∞)", 0, -1.5707963267948966)]
    [InlineData("TANH^-1 -0.5", -0.54930614433405489d)]
    [InlineData("Tanh^-1-1", double.NegativeInfinity)]
    [InlineData("artanh(-2)", -0.5493061443340549, 1.5707963267948966)]
    [InlineData("Tanh^-1(-∞)", 0, 1.5707963267948966)]
    [InlineData("arcoth(0)", 0, -1.5707963267948966)]
    [InlineData("Coth^-1(0.5)", 0.5493061443340549, -1.5707963267948966)]
    [InlineData("Arcoth(1)", double.PositiveInfinity, 0d)]
    [InlineData("Coth^-1(2)", 0.54930614433405489d)]
    [InlineData("ARCOTH(∞)", 0)]
    [InlineData("COTH^-1 -0.5", -0.5493061443340549, -1.5707963267948966)]
    [InlineData("Coth^-1-1", double.NegativeInfinity, 0)]
    [InlineData("arcoth(-2)", -0.54930614433405489d)]
    [InlineData("Coth^-1(-∞)", 0)]
    [InlineData("arsech(0)", double.NaN, double.NaN)]
    [InlineData("Sech^-1(0.5)", 1.3169578969248166d)]
    [InlineData("Arsech(1)", 0)]
    [InlineData("ARSECH(∞)", 0, 1.5707963267948966)]
    [InlineData("SECH^-1 -0.5", -1.3169578969248164, 3.141592653589793)]
    [InlineData("Sech^-1-1", 0, 3.141592653589793)]
    [InlineData("Sech^-1(-∞)", 0, 1.5707963267948966)]
    [InlineData("arcsch(0)", double.NaN, double.NaN)]
    [InlineData("Csch^-1(0.5)", 1.4436354751788103d)]
    [InlineData("Arcsch(1)", 0.88137358701954294d)]
    [InlineData("Csch^-1(2)", 0.48121182505960347d)]
    [InlineData("ARCSCH(∞)", 0)]
    [InlineData("CSCH^-1 -0.5", -1.4436354751788103d)]
    [InlineData("Csch^-1-1", -0.88137358701954294d)]
    [InlineData("arcsch(-2)", -0.48121182505960347d)]
    [InlineData("csch^-1(-∞)", 0)]
#if NET9_0_OR_GREATER
    [InlineData("arsech(-2)", -1.1102230246251565E-16, 2.0943951023931957)]
    [InlineData("COSH^-1 -0.5", -1.1102230246251565E-16, 2.0943951023931957)]
    [InlineData("Cosh^-1(0.5)", -1.1102230246251565E-16, 1.0471975511965976)]
    [InlineData("Sech^-1(2)", -1.1102230246251565E-16, 1.0471975511965976)]
#else
    [InlineData("arsech(-2)", 0, 2.0943951023931957)]
    [InlineData("COSH^-1 -0.5", 0, 2.0943951023931957)]
    [InlineData("Cosh^-1(0.5)", 0, 1.0471975511965976)]
    [InlineData("Sech^-1(2)", 0, 1.0471975511965976)]
#endif
    public void MathExpression_EvaluateComplex_HasInverseHyperbolicFn_ExpectedValue(string mathString, double expectedReal, double expectedImaginary = 0)
    {
        using var expression = new MathExpression(mathString, _scientificContext);
        expression.Evaluating += SubscribeToEvaluating;

        var value = expression.EvaluateComplex();

        Assert.Equal(new Complex(expectedReal, expectedImaginary), value);
    }
}