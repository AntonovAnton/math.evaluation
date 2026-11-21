#if NET11_0_OR_GREATER
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Tests.Context;

/// <summary>
///     Tests for the .NET 11 types bound by <see cref="DotNetMathContext" />: Decimal32, Decimal64, Decimal128, BFloat16, and Complex&lt;T&gt;.
///     The expected values are calculated by the same .NET methods, so the tests verify the bindings and parsing.
/// </summary>
public class DotNetMathContextNet11Tests(ITestOutputHelper testOutputHelper)
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;
    private readonly DotNetMathContext _context = new();

    #region IEEE 754 floating-point types: Decimal32, Decimal64, Decimal128, BFloat16

    [Fact]
    public void DotNetMathContext_Decimal32_ConstantsAndFunctions_ExpectedValue()
        => AssertFloatingPointIeee754<Decimal32>("Decimal32");

    [Fact]
    public void DotNetMathContext_Decimal64_ConstantsAndFunctions_ExpectedValue()
        => AssertFloatingPointIeee754<Decimal64>("Decimal64");

    [Fact]
    public void DotNetMathContext_Decimal128_ConstantsAndFunctions_ExpectedValue()
        => AssertFloatingPointIeee754<Decimal128>("Decimal128");

    [Fact]
    public void DotNetMathContext_BFloat16_ConstantsAndFunctions_ExpectedValue()
        => AssertFloatingPointIeee754<BFloat16>("BFloat16");

    [Fact]
    public void DotNetMathContext_DecimalQuantize_ExpectedValue()
    {
        Assert.Equal(Decimal32.Quantize(P<Decimal32>("2.345"), P<Decimal32>("0.01")),
            "Decimal32.Quantize(2.345, 0.01)".Evaluate<Decimal32>(_context, Invariant));
        Assert.Equal(Decimal64.Quantize(P<Decimal64>("2.345"), P<Decimal64>("0.01")),
            "Decimal64.Quantize(2.345, 0.01)".Evaluate<Decimal64>(_context, Invariant));
        Assert.Equal(Decimal128.Quantize(P<Decimal128>("2.345"), P<Decimal128>("0.01")),
            "Decimal128.Quantize(2.345, 0.01)".Evaluate<Decimal128>(_context, Invariant));
    }

    [Theory]
    [InlineData("0.1 + 0.2", "0.3")]
    [InlineData("0.1 * 3 - 0.3", "0")]
    [InlineData("1.1 + 2.2", "3.3")]
    public void DotNetMathContext_DecimalTypes_HaveDecimalFractionsExactly(string mathString, string expected)
    {
        Assert.Equal(P<Decimal64>(expected), mathString.Evaluate<Decimal64>(_context, Invariant));
        Assert.Equal(P<Decimal128>(expected), mathString.Evaluate<Decimal128>(_context, Invariant));
        Assert.Equal(P<Decimal128>(expected), new MathExpression(mathString, _context, Invariant).Compile<Decimal128>()());
    }

    #endregion

    #region Complex<T>

    [Fact]
    public void DotNetMathContext_ComplexDouble_ConstantsAndFunctions_ExpectedValue()
    {
        AssertGenericComplex<double>("double");
        AssertGenericComplex<double>("Double");
    }

    [Fact]
    public void DotNetMathContext_ComplexFloat_ConstantsAndFunctions_ExpectedValue()
    {
        AssertGenericComplex<float>("float");
        AssertGenericComplex<float>("Single");
    }

    [Fact]
    public void DotNetMathContext_ComplexHalf_ConstantsAndFunctions_ExpectedValue()
        => AssertGenericComplex<Half>("Half");

    [Fact]
    public void DotNetMathContext_ComplexBFloat16_ConstantsAndFunctions_ExpectedValue()
        => AssertGenericComplex<BFloat16>("BFloat16");

    [Fact]
    public void DotNetMathContext_ComplexDecimal32_ConstantsAndFunctions_ExpectedValue()
        => AssertGenericComplex<Decimal32>("Decimal32");

    [Fact]
    public void DotNetMathContext_ComplexDecimal64_ConstantsAndFunctions_ExpectedValue()
        => AssertGenericComplex<Decimal64>("Decimal64");

    [Fact]
    public void DotNetMathContext_ComplexDecimal128_ConstantsAndFunctions_ExpectedValue()
        => AssertGenericComplex<Decimal128>("Decimal128");

    #endregion

    #region private Methods

    private static T P<T>(string s)
        where T : INumberBase<T>
        => T.Parse(s, NumberStyles.Float, Invariant);

    private void AssertFloatingPointIeee754<T>(string t)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        var a = P<T>("2.5");
        var b = P<T>("-1.5");
        var half = P<T>("0.5");

        var cases = new (string MathString, T Expected)[]
        {
            // constants
            ($"default({t})", T.Zero),
            ($"({t})2.5", a),
            ($"{t}.E", T.E),
            ($"{t}.Pi", T.Pi),
            ($"{t}.Tau", T.Tau),
            ($"{t}.Epsilon", T.Epsilon),
            ($"{t}.MaxValue", T.MaxValue),
            ($"{t}.MinValue", T.MinValue),
            ($"{t}.PositiveInfinity", T.PositiveInfinity),
            ($"{t}.NegativeInfinity", T.NegativeInfinity),
            ($"{t}.NegativeOne", T.NegativeOne),
            ($"{t}.One", T.One),
            ($"{t}.Zero", T.Zero),

            // functions
            ($"{t}.Abs(-1.5)", T.Abs(b)),
            ($"{t}.Acos(0.5)", T.Acos(half)),
            ($"{t}.Acosh(2.5)", T.Acosh(a)),
            ($"{t}.Asin(0.5)", T.Asin(half)),
            ($"{t}.Asinh(2.5)", T.Asinh(a)),
            ($"{t}.Atan(2.5)", T.Atan(a)),
            ($"{t}.Atanh(0.5)", T.Atanh(half)),
            ($"{t}.Atan2(2.5, -1.5)", T.Atan2(a, b)),
            ($"{t}.Cbrt(2.5)", T.Cbrt(a)),
            ($"{t}.Ceiling(2.5)", T.Ceiling(a)),
            ($"{t}.Clamp(2.5, -1.5, 0.5)", T.Clamp(a, b, half)),
            ($"{t}.CopySign(2.5, -1.5)", T.CopySign(a, b)),
            ($"{t}.Cos(2.5)", T.Cos(a)),
            ($"{t}.Cosh(2.5)", T.Cosh(a)),
            ($"{t}.Exp(2.5)", T.Exp(a)),
            ($"{t}.Exp2(2.5)", T.Exp2(a)),
            ($"{t}.Exp10(2.5)", T.Exp10(a)),
            ($"{t}.Floor(-1.5)", T.Floor(b)),
            ($"{t}.FusedMultiplyAdd(2.5, -1.5, 0.5)", T.FusedMultiplyAdd(a, b, half)),
            ($"{t}.Hypot(2.5, -1.5)", T.Hypot(a, b)),
            ($"{t}.Ieee754Remainder(2.5, -1.5)", T.Ieee754Remainder(a, b)),
            ($"{t}.Log(2.5)", T.Log(a)),
            ($"{t}.Log(2.5, 0.5)", T.Log(a, half)),
            ($"{t}.Log2(2.5)", T.Log2(a)),
            ($"{t}.Log10(2.5)", T.Log10(a)),
            ($"{t}.Max(2.5, -1.5)", T.Max(a, b)),
            ($"{t}.MaxMagnitude(0.5, -1.5)", T.MaxMagnitude(half, b)),
            ($"{t}.Min(2.5, -1.5)", T.Min(a, b)),
            ($"{t}.MinMagnitude(0.5, -1.5)", T.MinMagnitude(half, b)),
            ($"{t}.Pow(2.5, -1.5)", T.Pow(a, b)),
            ($"{t}.RootN(2.5, 3)", T.RootN(a, 3)),
            ($"{t}.Round(2.5)", T.Round(a)),
            ($"{t}.Round(2.45, 1)", T.Round(P<T>("2.45"), 1)),
            ($"{t}.ScaleB(2.5, 3)", T.ScaleB(a, 3)),
            ($"{t}.Sign(-1.5)", T.NegativeOne),
            ($"{t}.Sin(2.5)", T.Sin(a)),
            ($"{t}.Sinh(2.5)", T.Sinh(a)),
            ($"{t}.Sqrt(2.5)", T.Sqrt(a)),
            ($"{t}.Tan(2.5)", T.Tan(a)),
            ($"{t}.Tanh(2.5)", T.Tanh(a)),
            ($"{t}.Truncate(-1.5)", T.Truncate(b)),
            ($"{t}.BitDecrement(2.5)", T.BitDecrement(a)),
            ($"{t}.BitIncrement(2.5)", T.BitIncrement(a)),
            ($"{t}.CreateChecked(2.5)", a),
            ($"{t}.CreateSaturating(2.5)", a),
            ($"{t}.CreateTruncating(2.5)", a),

            // operators
            ("2.5 * -1.5 + 0.5 / 2.5", a * b + half / a),
            ("2.5 % 1.5", a % -b)
        };

        foreach (var (mathString, expected) in cases)
        {
            testOutputHelper.WriteLine($"{mathString} = {expected}");

            Assert.Equal(expected, mathString.Evaluate<T>(_context, Invariant));
            Assert.Equal(expected, new MathExpression(mathString, _context, Invariant).Compile<T>()());
        }

        Assert.True(T.IsNaN($"{t}.NaN".Evaluate<T>(_context, Invariant)));
        Assert.True(T.IsNegative($"{t}.NegativeZero".Evaluate<T>(_context, Invariant)));
    }

    private void AssertGenericComplex<T>(string t)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        var c = $"Complex<{t}>";
        var z = new Complex<T>(P<T>("1.5"), P<T>("-0.5"));
        var w = new Complex<T>(P<T>("0.5"), P<T>("2"));
        const string zs = "1.5 - 0.5i";
        const string ws = "0.5 + 2i";

        var cases = new (string MathString, Complex<T> Expected)[]
        {
            // constants
            ($"default({c})", default),
            ($"{c}.Zero", Complex<T>.Zero),
            ($"{c}.One", Complex<T>.One),
            ($"{c}.ImaginaryOne", Complex<T>.ImaginaryOne),
            ($"{c}.Infinity", Complex<T>.Infinity),
            ($"new {c}(1.5, -0.5)", z),

            // functions
            ($"{c}.Abs({zs})", new Complex<T>(Complex<T>.Abs(z), T.Zero)),
            ($"{c}.Acos({zs})", Complex<T>.Acos(z)),
            ($"{c}.Asin({zs})", Complex<T>.Asin(z)),
            ($"{c}.Atan({zs})", Complex<T>.Atan(z)),
            ($"{c}.Cos({zs})", Complex<T>.Cos(z)),
            ($"{c}.Cosh({zs})", Complex<T>.Cosh(z)),
            ($"{c}.Exp({zs})", Complex<T>.Exp(z)),
            ($"{c}.Log({zs})", Complex<T>.Log(z)),
            ($"{c}.Log({zs}, 2)", Complex<T>.Log(z, P<T>("2"))),
            ($"{c}.Log10({zs})", Complex<T>.Log10(z)),
            ($"{c}.Pow({zs}, {ws})", Complex<T>.Pow(z, w)),
            ($"{c}.Sin({zs})", Complex<T>.Sin(z)),
            ($"{c}.Sinh({zs})", Complex<T>.Sinh(z)),
            ($"{c}.Sqrt({zs})", Complex<T>.Sqrt(z)),
            ($"{c}.Tan({zs})", Complex<T>.Tan(z)),
            ($"{c}.Tanh({zs})", Complex<T>.Tanh(z)),
            ($"{c}.Add({zs}, {ws})", Complex<T>.Add(z, w)),
            ($"{c}.Subtract({zs}, {ws})", Complex<T>.Subtract(z, w)),
            ($"{c}.Multiply({zs}, {ws})", Complex<T>.Multiply(z, w)),
            ($"{c}.Divide({zs}, {ws})", Complex<T>.Divide(z, w)),
            ($"{c}.Conjugate({zs})", Complex<T>.Conjugate(z)),
            ($"{c}.Negate({zs})", Complex<T>.Negate(z)),
            ($"{c}.Reciprocal({zs})", Complex<T>.Reciprocal(z)),
            ($"{c}.FromPolarCoordinates(2, 0.5)", Complex<T>.FromPolarCoordinates(P<T>("2"), P<T>("0.5"))),
            ($"{c}.MaxMagnitude({zs}, {ws})", Complex<T>.MaxMagnitude(z, w)),
            ($"{c}.MinMagnitude({zs}, {ws})", Complex<T>.MinMagnitude(z, w)),
            ($"{c}.CreateChecked({zs})", z),
            ($"{c}.CreateSaturating({zs})", z),
            ($"{c}.CreateTruncating({zs})", z),

            // operators
            ($"({zs}) * ({ws}) - ({zs}) / ({ws})", z * w - z / w)
        };

        foreach (var (mathString, expected) in cases)
        {
            testOutputHelper.WriteLine($"{mathString} = {expected}");

            Assert.Equal(expected, mathString.EvaluateComplex<T>(_context, Invariant));
            Assert.Equal(expected, new MathExpression(mathString, _context, Invariant).CompileComplex<T>()());
        }

        Assert.True(Complex<T>.IsNaN($"{c}.NaN".EvaluateComplex<T>(_context, Invariant)));
    }

    #endregion
}
#endif
