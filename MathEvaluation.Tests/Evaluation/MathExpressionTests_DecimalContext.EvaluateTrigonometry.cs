namespace MathEvaluation.Tests.Evaluation;

public partial class MathExpressionTests_DecimalContext
{
    [Theory]
    [InlineData("π", Math.PI)]
    [InlineData("((5 - 1)π)", 4 * Math.PI)]
    [InlineData("(1/2)π", Math.PI / 2)]
    [InlineData("2π * 2 / 2 + π", Math.PI * 3)]
    [InlineData("pi", Math.PI)]
    [InlineData("((5 - 1)Pi)", 4 * Math.PI)]
    public void MathExpression_EvaluateDecimal_HasPi_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = new MathExpression(expression, _scientificContext).EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("τ", Math.Tau)]
    [InlineData("((5 - 1)τ)", 4 * Math.Tau)]
    [InlineData("(1/2)τ", Math.Tau / 2)]
    public void MathExpression_EvaluateDecimal_HasTau_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = new MathExpression(expression, _scientificContext).EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }

    [Theory]
    [InlineData("sin(30\u00b0)", 0.49999999999999994d)]
    [InlineData("sin 0.5π", 1d)]
    [InlineData("sin 0.5π^2", -0.97536797208363146d)]
    [InlineData("sin 90°^2", 0.6242659526396992d)]
    [InlineData("sin(90°)^2", 1d)]
    [InlineData("cos1", 0.54030230586813977d)]
    [InlineData("cos(+1)", 0.54030230586813977d)]
    [InlineData("cos-(-1)", 0.54030230586813977d)]
    [InlineData("2^sin(1)^2", 1.6336211145430648d)]
    [InlineData("2^sin1^2", 1.791876223827922d)]
    [InlineData("2^sin(1)^sin1^2", 1.8211046249505032d)]
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
    public void MathExpression_EvaluateDecimal_HasTrigonometricFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = new MathExpression(expression, _scientificContext).EvaluateDecimal();

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
    public void MathExpression_EvaluateDecimal_HasHyperbolicTrigonometricFn_ExpectedValue(string expression,
        double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = new MathExpression(expression, _scientificContext).EvaluateDecimal();

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
    public void MathExpression_EvaluateDecimal_HasInverseTrigonometricFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = new MathExpression(expression, _scientificContext).EvaluateDecimal();

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
    public void MathExpression_EvaluateDecimal_HasInverseHyperbolicFn_ExpectedValue(string expression, double expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var value = new MathExpression(expression, _scientificContext).EvaluateDecimal();

        Assert.Equal((decimal)expectedValue, value);
    }
}