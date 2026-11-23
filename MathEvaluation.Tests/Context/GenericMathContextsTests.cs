using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Tests.Context;

/// <summary>Tests for <see cref="ScientificMathContext{T}" />, <see cref="ProgrammingMathContext{T}" />, and <see cref="DecimalScientificMathContext" />.</summary>
public class GenericMathContextsTests(ITestOutputHelper testOutputHelper)
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    // decimal implements the constants of IFloatingPointConstants<T> explicitly
    private static T Pi<T>() where T : IFloatingPointConstants<T> => T.Pi;
    private static T E<T>() where T : IFloatingPointConstants<T> => T.E;
    private static T Tau<T>() where T : IFloatingPointConstants<T> => T.Tau;

    #region DecimalScientificMathContext

    [Fact]
    public void DecimalScientificMathContext_CalculatesConstantsAndOperatorsInDecimal()
    {
        var context = new DecimalScientificMathContext();

        // the constants have the decimal precision (28 digits), not the double one
        Assert.Equal(Pi<decimal>(), "π".Evaluate<decimal>(context, Invariant));
        Assert.Equal(E<decimal>(), "e".Evaluate<decimal>(context, Invariant));
        Assert.Equal(Tau<decimal>(), "τ".Evaluate<decimal>(context, Invariant));

        Assert.Equal(0.1m, "100000000000000000.3 mod 0.2".Evaluate<decimal>(context, Invariant));
        Assert.Equal(3m, "7.5 // 2.5".Evaluate<decimal>(context, Invariant));
        Assert.Equal(3628800m, "10!".Evaluate<decimal>(context, Invariant));
        Assert.Equal(2432902008176640000m * 21m, "21!".Evaluate<decimal>(context, Invariant)); // the base context overflows long
        Assert.Equal(5m, "⌈3.1⌉ + ⌊0.9⌋ + |-0.5| + pos(-2) + 0.5".Evaluate<decimal>(context, Invariant));
        Assert.Equal(Pi<decimal>(), "180°".Evaluate<decimal>(context, Invariant));
        Assert.Equal(180m, "deg(π)".Evaluate<decimal>(context, Invariant));
        Assert.Equal(1m, "T ∧ ¬F → (1 ≡ 1)".Evaluate<decimal>(context, Invariant));

        Assert.Equal(new MathExpression("π * 2 + 7.5 mod 2", context, Invariant).Evaluate<decimal>(),
            new MathExpression("π * 2 + 7.5 mod 2", context, Invariant).Compile<decimal>()());
    }

    [Theory]
    [InlineData("(-1)!")]
    [InlineData("1.5!")]
    public void DecimalScientificMathContext_FactorialOfNotNaturalNumber_ThrowsArgumentException(string mathString)
    {
        var ex = Record.Exception(() => mathString.Evaluate<decimal>(new DecimalScientificMathContext(), Invariant));

        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<ArgumentException>(ex.InnerException);
    }

    #endregion

    #region ProgrammingMathContext<T>

    [Theory]
    [InlineData("TRUE + true + False", "2")]
    [InlineData("7.5 // 2", "3")]
    [InlineData("100000000000000000.3 % 0.2", "0.1")]
    [InlineData("iif(1 < 2, 1.25, 2.5)", "1.25")]
    [InlineData("iif(0, 1.25, 2.5)", "2.5")]
    [InlineData("2 ** 10", "1024")]
    public void ProgrammingMathContext_Decimal_ExpectedValue(string mathString, string expected)
    {
        var context = new ProgrammingMathContext<decimal>();

        Assert.Equal(decimal.Parse(expected, Invariant), mathString.Evaluate<decimal>(context, Invariant));
        Assert.Equal(decimal.Parse(expected, Invariant), new MathExpression(mathString, context, Invariant).Compile<decimal>()());
    }

    #endregion

    #region ScientificMathContext<T>

    [Theory]
    [InlineData("sin")]
    [InlineData("cos")]
    [InlineData("tan")]
    [InlineData("sec")]
    [InlineData("csc")]
    [InlineData("cot")]
    [InlineData("sinh")]
    [InlineData("cosh")]
    [InlineData("tanh")]
    [InlineData("sech")]
    [InlineData("csch")]
    [InlineData("coth")]
    [InlineData("arcsin")]
    [InlineData("arccos")]
    [InlineData("arctan")]
    [InlineData("arcsec")]
    [InlineData("arccsc")]
    [InlineData("arccot")]
    [InlineData("arsinh")]
    [InlineData("arcosh")]
    [InlineData("artanh")]
    [InlineData("arsech")]
    [InlineData("arcsch")]
    [InlineData("arcoth")]
    [InlineData("sqrt")]
    [InlineData("cbrt")]
    [InlineData("ln")]
    [InlineData("log")]
    [InlineData("exp")]
    [InlineData("abs")]
    [InlineData("floor")]
    [InlineData("rad")]
    [InlineData("deg")]
    public void ScientificMathContext_Double_SameAsMathTrig(string fn)
    {
        // MathTrig and Math are the double functions of ScientificMathContext before it was generic
        Func<double, double> reference = fn switch
        {
            "sin" => MathTrigonometric.MathTrig.Sin,
            "cos" => MathTrigonometric.MathTrig.Cos,
            "tan" => MathTrigonometric.MathTrig.Tan,
            "sec" => MathTrigonometric.MathTrig.Sec,
            "csc" => MathTrigonometric.MathTrig.Csc,
            "cot" => MathTrigonometric.MathTrig.Cot,
            "sinh" => MathTrigonometric.MathTrig.Sinh,
            "cosh" => MathTrigonometric.MathTrig.Cosh,
            "tanh" => MathTrigonometric.MathTrig.Tanh,
            "sech" => MathTrigonometric.MathTrig.Sech,
            "csch" => MathTrigonometric.MathTrig.Csch,
            "coth" => MathTrigonometric.MathTrig.Coth,
            "arcsin" => MathTrigonometric.MathTrig.Asin,
            "arccos" => MathTrigonometric.MathTrig.Acos,
            "arctan" => MathTrigonometric.MathTrig.Atan,
            "arcsec" => MathTrigonometric.MathTrig.Asec,
            "arccsc" => MathTrigonometric.MathTrig.Acsc,
            "arccot" => MathTrigonometric.MathTrig.Acot,
            "arsinh" => MathTrigonometric.MathTrig.Asinh,
            "arcosh" => MathTrigonometric.MathTrig.Acosh,
            "artanh" => MathTrigonometric.MathTrig.Atanh,
            "arsech" => MathTrigonometric.MathTrig.Asech,
            "arcsch" => MathTrigonometric.MathTrig.Acsch,
            "arcoth" => MathTrigonometric.MathTrig.Acoth,
            "sqrt" => Math.Sqrt,
            "cbrt" => v => Math.Pow(v, 1 / 3d),
            "ln" => Math.Log,
            "log" => Math.Log10,
            "exp" => Math.Exp,
            "abs" => Math.Abs,
            "floor" => Math.Floor,
            "rad" => MathTrigonometric.MathTrig.DegreesToRadians,
            "deg" => MathTrigonometric.MathTrig.RadiansToDegrees,
            _ => throw new ArgumentOutOfRangeException(nameof(fn))
        };

        // the context calls the generic MathTrig overloads, which for double may differ from the double overloads in the last bits
        Func<double, double> genericReference = fn switch
        {
            "sin" => MathTrigonometric.MathTrig.Sin<double>,
            "cos" => MathTrigonometric.MathTrig.Cos<double>,
            "tan" => MathTrigonometric.MathTrig.Tan<double>,
            "sec" => MathTrigonometric.MathTrig.Sec<double>,
            "csc" => MathTrigonometric.MathTrig.Csc<double>,
            "cot" => MathTrigonometric.MathTrig.Cot<double>,
            "sinh" => MathTrigonometric.MathTrig.Sinh<double>,
            "cosh" => MathTrigonometric.MathTrig.Cosh<double>,
            "tanh" => MathTrigonometric.MathTrig.Tanh<double>,
            "sech" => MathTrigonometric.MathTrig.Sech<double>,
            "csch" => MathTrigonometric.MathTrig.Csch<double>,
            "coth" => MathTrigonometric.MathTrig.Coth<double>,
            "arcsin" => MathTrigonometric.MathTrig.Asin<double>,
            "arccos" => MathTrigonometric.MathTrig.Acos<double>,
            "arctan" => MathTrigonometric.MathTrig.Atan<double>,
            "arcsec" => MathTrigonometric.MathTrig.Asec<double>,
            "arccsc" => MathTrigonometric.MathTrig.Acsc<double>,
            "arccot" => MathTrigonometric.MathTrig.Acot<double>,
            "arsinh" => MathTrigonometric.MathTrig.Asinh<double>,
            "arcosh" => MathTrigonometric.MathTrig.Acosh<double>,
            "artanh" => MathTrigonometric.MathTrig.Atanh<double>,
            "arsech" => MathTrigonometric.MathTrig.Asech<double>,
            "arcsch" => MathTrigonometric.MathTrig.Acsch<double>,
            "arcoth" => MathTrigonometric.MathTrig.Acoth<double>,
            "rad" => MathTrigonometric.MathTrig.DegreesToRadians<double>,
            "deg" => MathTrigonometric.MathTrig.RadiansToDegrees<double>,
            _ => reference
        };
        var context = new ScientificMathContext();

        foreach (var x in new[] { 0, 0.5, -0.5, 1, -1, 1.5, -2.5, 30, 1e-9, -1e-9, 25, 77, 1e9, 711, -712, double.PositiveInfinity, double.NegativeInfinity })
        {
            var mathString = $"{fn}(x)";
            var e = reference(x);
            var value = mathString.Evaluate(new { x }, context, Invariant);
            testOutputHelper.WriteLine($"{fn}({x}) = {value:R} (MathTrig: {e:R}){(value.Equals(e) ? "" : "  DIFFERS")}");

            Assert.Equal(genericReference(x), value); // bit-exact
            Assert.True(value.Equals(e) || Math.Abs(value - e) <= 4 * (Math.BitIncrement(Math.Abs(e)) - Math.Abs(e)),
                $"{fn}({x}) = {value:R} differs from the double overload {e:R} by more than 4 ulps");
        }
    }

    [Theory]
    [InlineData("2^10 + pow(2, 0.5) - √2")]
    [InlineData("3^-2 + ∛27 + ∜16")]
    [InlineData("2^3^2")]
    [InlineData("10 mod 3 + 7 // 2 + 4!")]
    [InlineData("sin(30°) + cos(π/3)")]
    public void ScientificMathContext_Float_HasOperators_SameAsDouble(string mathString)
    {
        var expected = mathString.Evaluate(new ScientificMathContext(), Invariant);
        var context = new ScientificMathContext<float>();

        // float has 7 significant digits
        Assert.Equal(expected, mathString.Evaluate<float>(context, Invariant), Math.Abs(expected) * 1e-6);
        Assert.Equal(expected, new MathExpression(mathString, context, Invariant).Compile<float>()(), Math.Abs(expected) * 1e-6);
    }

    [Fact]
    public void ScientificMathContext_FloatAndHalf_CalculatesInTheirPrecision()
    {
        Assert.Equal(MathF.Sqrt(2f) + MathF.Sin(0.5f), "sqrt(2) + sin(0.5)".Evaluate<float>(new ScientificMathContext<float>(), Invariant));
        Assert.Equal(Half.Sqrt((Half)2) * Half.Pi, "sqrt(2) * π".Evaluate<Half>(new ScientificMathContext<Half>(), Invariant));
    }

#if NET11_0_OR_GREATER
    [Fact]
    public void ScientificMathContext_Decimal128_CalculatesInDecimal128Precision()
    {
        var context = new ScientificMathContext<Decimal128>();
        Decimal128 P(string s) => Decimal128.Parse(s, Invariant);

        Assert.Equal(Decimal128.Sqrt(P("2")), "sqrt(2)".Evaluate<Decimal128>(context, Invariant));
        Assert.Equal(Decimal128.Pow(P("2"), P("0.5")), "2^0.5".Evaluate<Decimal128>(context, Invariant));
        Assert.Equal(Decimal128.Log(P("10")), "ln(10)".Evaluate<Decimal128>(context, Invariant));
        Assert.Equal(Decimal128.Sin(P("0.5")) + Decimal128.Pi, "sin(0.5) + π".Evaluate<Decimal128>(context, Invariant));
        Assert.Equal(P("0.1"), "100000000000000000000000000000.3 mod 0.2".Evaluate<Decimal128>(context, Invariant));
        Assert.Equal(Decimal128.PositiveInfinity, "∞".Evaluate<Decimal128>(context, Invariant));

        // the double context has 16 digits only
        Assert.NotEqual(Decimal128.Sqrt(P("2")), "sqrt(2)".Evaluate<Decimal128>(new ScientificMathContext(), Invariant));

        Assert.Equal("sqrt(2) + 2^0.5 + sin(0.5)".Evaluate<Decimal128>(context, Invariant),
            new MathExpression("sqrt(2) + 2^0.5 + sin(0.5)", context, Invariant).Compile<Decimal128>()());
    }

    [Fact]
    public void ScientificMathContext_Decimal64_CalculatesConstantsAndModuloInDecimal64()
    {
        var context = new ScientificMathContext<Decimal64>();

        Assert.Equal(Decimal64.Pi, "π".Evaluate<Decimal64>(context, Invariant));
        Assert.Equal(Decimal64.Parse("0.1", Invariant), "7.3 mod 0.4".Evaluate<Decimal64>(context, Invariant));
        Assert.Equal(Decimal64.Parse("1", Invariant), "TRUE".Evaluate<Decimal64>(new ProgrammingMathContext<Decimal64>(), Invariant));
    }
#endif

    #endregion

    #region DecimalProgrammingMathContext

    [Fact]
    public void DecimalProgrammingMathContext_CalculatesInDecimal()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        var context = new DecimalProgrammingMathContext();
#pragma warning restore CS0618 // Type or member is obsolete

        Assert.Equal(2.5m, "iif(0, 1.25, 2.5)".Evaluate<decimal>(context, Invariant));
        Assert.Equal(3m, "7.5 // 2.5".Evaluate<decimal>(context, Invariant));
        Assert.Equal(0.1m, "100000000000000000.3 % 0.2".Evaluate<decimal>(context, Invariant));
        var parameters = new { x = 100000000000000000.3m };
        Assert.Equal(0.1m, "x % 0.2".CompileDecimal(parameters, context, Invariant)(parameters));
    }

    #endregion
}
