#if NET11_0_OR_GREATER
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathTrigonometric;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Tests.Context;

/// <summary>
///     Tests for <see cref="ComplexScientificMathContext{T}" /> with each supported component type.
///     The expected values are calculated by the same .NET and MathTrig methods, so the tests verify the bindings, parsing, and precision.
/// </summary>
public class ComplexScientificMathContextGenericTests(ITestOutputHelper testOutputHelper)
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    [Fact]
    public void ComplexScientificMathContext_Double_ExpectedValue() => AssertContext<double>();

    [Fact]
    public void ComplexScientificMathContext_Float_ExpectedValue() => AssertContext<float>();

    [Fact]
    public void ComplexScientificMathContext_Half_ExpectedValue() => AssertContext<Half>();

    [Fact]
    public void ComplexScientificMathContext_BFloat16_ExpectedValue() => AssertContext<BFloat16>();

    [Fact]
    public void ComplexScientificMathContext_Decimal32_ExpectedValue() => AssertContext<Decimal32>();

    [Fact]
    public void ComplexScientificMathContext_Decimal64_ExpectedValue() => AssertContext<Decimal64>();

    [Fact]
    public void ComplexScientificMathContext_Decimal128_ExpectedValue() => AssertContext<Decimal128>();

    [Theory]
    [InlineData("2i mod 3", "x mod 3")]
    [InlineData("2i < 3", "x < 3")]
    [InlineData("2 ≥ 3i", "2 ≥ 1.5x")]
    [InlineData("1 ⪯ i", "1 ⪯ x")]
    public void ComplexScientificMathContext_ComparesOrDividesComplexNumber_ThrowsInvalidCastException(string mathString, string mathStringWithVariable)
    {
        var context = new ComplexScientificMathContext<Decimal64>();

        var ex = Record.Exception(() => mathString.EvaluateComplex<Decimal64>(context, Invariant));
        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<InvalidCastException>(ex.InnerException);

        // the operators of constants are calculated when the expression is compiled, as for Complex
        ex = Record.Exception(() => new MathExpression(mathString, context, Invariant).CompileComplex<Decimal64>());
        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<InvalidCastException>(ex.InnerException);

        // with variables, the compiled delegate throws
        var parameters = new Dictionary<string, Complex<Decimal64>> { { "x", default } };
        var fn = new MathExpression(mathStringWithVariable, context, Invariant)
            .CompileComplex<Dictionary<string, Complex<Decimal64>>, Decimal64>(parameters);
        parameters["x"] = new Complex<Decimal64>(0, 2); // 2i
        Assert.Throws<InvalidCastException>(() => fn(parameters));
    }

    [Fact]
    public void ComplexScientificMathContext_Decimal128_KeepsDecimal128Precision()
    {
        var context = new ComplexScientificMathContext<Decimal128>();

        // π, e and τ are bound in the precision of Decimal128 (34 digits), the base context binds them as double
        Assert.Equal(new Complex<Decimal128>(Decimal128.Pi, Decimal128.Zero), "pi".EvaluateComplex<Decimal128>(context, Invariant));
        Assert.Equal(new Complex<Decimal128>(Decimal128.E, Decimal128.Zero), "e".EvaluateComplex<Decimal128>(context, Invariant));
        Assert.Equal(new Complex<Decimal128>(Decimal128.Tau, Decimal128.Zero), "τ".EvaluateComplex<Decimal128>(context, Invariant));

        // the fourth root uses the exact 0.25 in Decimal128
        var value = "∜(0.0001)".EvaluateComplex<Decimal128>(context, Invariant);
        Assert.Equal(Complex<Decimal128>.Pow(new Complex<Decimal128>(P<Decimal128>("0.0001"), Decimal128.Zero), P<Decimal128>("0.25")), value);
    }

    #region private Methods

    private static T P<T>(string s)
        where T : INumberBase<T>
        => T.Parse(s, NumberStyles.Float, Invariant);

    private static Complex<T> C<T>(string real, string imaginary = "0")
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
        => new(P<T>(real), P<T>(imaginary));

    private void AssertContext<T>()
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        var context = new ComplexScientificMathContext<T>();
        var z = C<T>("0.5", "0.25");
        var one = Complex<T>.One;
        var zero = Complex<T>.Zero;
        const string zs = "(0.5 + 0.25i)";

        var cases = new (string MathString, Complex<T> Expected)[]
        {
            // constants in the precision of T
            ("π", new Complex<T>(T.Pi, T.Zero)),
            ("Pi", new Complex<T>(T.Pi, T.Zero)),
            ("e", new Complex<T>(T.E, T.Zero)),
            ("τ", new Complex<T>(T.Tau, T.Zero)),
            ("∞", new Complex<T>(T.PositiveInfinity, T.Zero)),
            ("i", Complex<T>.ImaginaryOne),

            // scientific functions
            ("sqrt(-4)", Complex<T>.Sqrt(C<T>("-4"))),
            ("√(-4)", Complex<T>.Sqrt(C<T>("-4"))),
            ("SQRT(-4)", Complex<T>.Sqrt(C<T>("-4"))),
            ("∛8", Complex<T>.Pow(C<T>("8"), T.One / T.CreateTruncating(3))),
            ("cbrt(8)", Complex<T>.Pow(C<T>("8"), T.One / T.CreateTruncating(3))),
            ("∜16", Complex<T>.Pow(C<T>("16"), T.One / T.CreateTruncating(4))),
            ("|3 - 4i|", new Complex<T>(Complex<T>.Abs(C<T>("3", "-4")), T.Zero)),
            ("Abs(3 - 4i)", new Complex<T>(Complex<T>.Abs(C<T>("3", "-4")), T.Zero)),
            ($"ln{zs}", Complex<T>.Log(z)),
            ($"LN{zs}", Complex<T>.Log(z)),
            ($"log{zs}", Complex<T>.Log10(z)),
            ($"exp{zs}", Complex<T>.Exp(z)),
            ($"Exp{zs}", Complex<T>.Exp(z)),
            ($"pow({zs}, 2)", Complex<T>.Pow(z, C<T>("2"))),
            ($"POW({zs}, 2)", Complex<T>.Pow(z, C<T>("2"))),

            // trigonometric functions
            ($"sin{zs}", MathTrig.Sin(z)),
            ($"Sin{zs}", MathTrig.Sin(z)),
            ($"SIN{zs}", MathTrig.Sin(z)),
            ($"cos{zs}", MathTrig.Cos(z)),
            ($"tan{zs}", MathTrig.Tan(z)),
            ($"sec{zs}", MathTrig.Sec(z)),
            ($"csc{zs}", MathTrig.Csc(z)),
            ($"cot{zs}", MathTrig.Cot(z)),
            ($"sinh{zs}", MathTrig.Sinh(z)),
            ($"cosh{zs}", MathTrig.Cosh(z)),
            ($"tanh{zs}", MathTrig.Tanh(z)),
            ($"sech{zs}", MathTrig.Sech(z)),
            ($"csch{zs}", MathTrig.Csch(z)),
            ($"coth{zs}", MathTrig.Coth(z)),
            ($"sin^-1{zs}", MathTrig.Asin(z)),
            ($"arcsin{zs}", MathTrig.Asin(z)),
            ($"ASIN{zs}", MathTrig.Asin(z)),
            ($"arccos{zs}", MathTrig.Acos(z)),
            ($"arctan{zs}", MathTrig.Atan(z)),
            ($"arcsec{zs}", MathTrig.Asec(z)),
            ($"arccsc{zs}", MathTrig.Acsc(z)),
            ($"arccot{zs}", MathTrig.Acot(z)),
            ($"arsinh{zs}", MathTrig.Asinh(z)),
            ($"cosh^-1{zs}", MathTrig.Acosh(z)),
            ($"atanh{zs}", MathTrig.Atanh(z)),
            ($"Arsech{zs}", MathTrig.Asech(z)),
            ($"acsch{zs}", MathTrig.Acsch(z)),
            ($"ARCOTH{zs}", MathTrig.Acoth(z)),

            // operators in the precision of T
            ("2^(1 + i)", Complex<T>.Pow(C<T>("2"), C<T>("1", "1"))),
            ("i^2", Complex<T>.Pow(Complex<T>.ImaginaryOne, C<T>("2"))),
            ("(1 + 2i) * (3 - i) / (1 - i)", C<T>("1", "2") * C<T>("3", "-1") / C<T>("1", "-1")),
            ("7 mod 4", C<T>("3")),
            ("7.5 Modulo 2", C<T>("1.5")),
            ("2 < 3", one),
            ("3 > 2", one),
            ("2 ≤ 2", one),
            ("3 ⪯ 2", zero),
            ("2 ≥ 3", zero),
            ("3 ⪰ 3", one),

            // boolean logic
            ("1 ↔ 1", one),
            ("1 ⇔ 0", zero),
            ("i ≡ i", one),
            ("1 ↮ 0", one),
            ("i ⇎ i", zero),
            ("1 ≢ i", one),
            ("1 → 0", zero),
            ("0 ⇒ 0", one),
            ("0 ← 1", zero),
            ("1 ⟸ 0", one)
        };

        foreach (var (mathString, expected) in cases)
        {
            testOutputHelper.WriteLine($"{typeof(T).Name}: {mathString} = {expected}");

            Assert.Equal(expected, mathString.EvaluateComplex<T>(context, Invariant));
            Assert.Equal(expected, new MathExpression(mathString, context, Invariant).CompileComplex<T>()());
        }
    }

    #endregion
}
#endif
