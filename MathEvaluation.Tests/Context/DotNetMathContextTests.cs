using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Numerics;

#if NET8_0_OR_GREATER

namespace MathEvaluation.Tests.Context;

public class DotNetMathContextTests
{
    private readonly DotNetMathContext _context = new();

    #region Math Functions Tests

    [Fact]
    public void DotNetMathContext_MathAbs_ExpectedValue()
    {
        var result = "Math.Abs(-42.5)".Evaluate(null, _context);
        Assert.Equal(42.5, result);
    }

    [Fact]
    public void DotNetMathContext_MathLog2_ExpectedValue()
    {
        var result = "Math.Log2(8)".Evaluate(null, _context);
        Assert.Equal(3.0, result);
    }

    [Fact]
    public void DotNetMathContext_MathTau_ExpectedValue()
    {
        var result = "Math.Tau / 2".Evaluate(null, _context);
        Assert.Equal(Math.PI, result, precision: 15);
    }

    [Fact]
    public void DotNetMathContext_MathBitIncrement_ExpectedValue()
    {
        var value = 1.0;
        var result = "Math.BitIncrement(x)".Evaluate(new { x = value }, _context);
        var expected = Math.BitIncrement(value);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DotNetMathContext_MathCopySign_ExpectedValue()
    {
        var result = "Math.CopySign(5.0, -1.0)".Evaluate(null, _context);
        Assert.Equal(-5.0, result);
    }

    [Fact]
    public void DotNetMathContext_MathILogB_ExpectedValue()
    {
        var result = "Math.ILogB(1024)".Evaluate(null, _context);
        Assert.Equal(10.0, result);
    }

    [Fact]
    public void DotNetMathContext_MathScaleB_ExpectedValue()
    {
        var result = "Math.ScaleB(1.5, 3)".Evaluate(null, _context);
        Assert.Equal(12.0, result); // 1.5 * 2^3 = 12
    }

    #endregion

    #region BigInteger Tests

    [Fact]
    public void DotNetMathContext_BigIntegerConstants_ExpectedValue()
    {
        var result1 = "BigInteger.Zero".Evaluate<BigInteger>(null, _context);
        var result2 = "BigInteger.One".Evaluate<BigInteger>(null, _context);
        var result3 = "BigInteger.MinusOne".Evaluate<BigInteger>(null, _context);

        Assert.Equal(BigInteger.Zero, result1);
        Assert.Equal(BigInteger.One, result2);
        Assert.Equal(BigInteger.MinusOne, result3);
    }

    [Fact]
    public void DotNetMathContext_BigIntegerAdd_ExpectedValue()
    {
        var result = "BigInteger.Add(x, y)".Evaluate<BigInteger>(
            new { x = BigInteger.Parse("999999999999999999"), y = BigInteger.Parse("1") },
            _context);

        Assert.Equal(BigInteger.Parse("1000000000000000000"), result);
    }

    [Fact]
    public void DotNetMathContext_BigIntegerPow_ExpectedValue()
    {
        var result = "BigInteger.Pow(x, 10)".Evaluate<BigInteger>(
            new { x = new BigInteger(2) },
            _context);

        Assert.Equal(new BigInteger(1024), result);
    }

    [Fact]
    public void DotNetMathContext_BigIntegerModPow_ExpectedValue()
    {
        var p = BigInteger.Parse("2305843009213693951");
        var q = BigInteger.Parse("2305843009213693967");
        var n = p * q;

        var result = "BigInteger.ModPow(message, e, n)".Evaluate<BigInteger>(
            new { message = new BigInteger(123456789), e = new BigInteger(65537), n },
            _context);

        var expected = BigInteger.ModPow(123456789, 65537, n);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DotNetMathContext_BigIntegerGCD_ExpectedValue()
    {
        var result = "BigInteger.GreatestCommonDivisor(a, b)".Evaluate<BigInteger>(
            new { a = new BigInteger(48), b = new BigInteger(18) },
            _context);

        Assert.Equal(new BigInteger(6), result);
    }

    #endregion

    #region Half Tests

    [Fact]
    public void DotNetMathContext_HalfConstants_ExpectedValue()
    {
        var maxValue = "Half.MaxValue".Evaluate<Half>(null, _context);
        var minValue = "Half.MinValue".Evaluate<Half>(null, _context);
        var epsilon = "Half.Epsilon".Evaluate<Half>(null, _context);

        Assert.Equal(Half.MaxValue, maxValue);
        Assert.Equal(Half.MinValue, minValue);
        Assert.Equal(Half.Epsilon, epsilon);
    }

    [Fact]
    public void DotNetMathContext_HalfCalculation_ExpectedValue()
    {
        var result = "x + y".Evaluate<Half>(
            new { x = (Half)2.5, y = (Half)3.5 },
            _context);

        Assert.Equal((Half)6.0, result);
    }

    #endregion

    #region Int128/UInt128 Tests

    [Fact]
    public void DotNetMathContext_Int128Constants_ExpectedValue()
    {
        var zero = "Int128.Zero".Evaluate<Int128>(null, _context);
        var one = "Int128.One".Evaluate<Int128>(null, _context);
        var max = "Int128.MaxValue".Evaluate<Int128>(null, _context);

        Assert.Equal(Int128.Zero, zero);
        Assert.Equal(Int128.One, one);
        Assert.Equal(Int128.MaxValue, max);
    }

    [Fact]
    public void DotNetMathContext_UInt128Constants_ExpectedValue()
    {
        var zero = "UInt128.Zero".Evaluate<UInt128>(null, _context);
        var one = "UInt128.One".Evaluate<UInt128>(null, _context);

        Assert.Equal(UInt128.Zero, zero);
        Assert.Equal(UInt128.One, one);
    }

    [Fact]
    public void DotNetMathContext_Int128Calculation_ExpectedValue()
    {
        var result = "x + y".Evaluate<Int128>(
            new { x = Int128.MaxValue / 2, y = Int128.One },
            _context);

        Assert.Equal(Int128.MaxValue / 2 + Int128.One, result);
    }

    #endregion

    #region Complex Number Tests

    [Fact]
    public void DotNetMathContext_ComplexConstants_ExpectedValue()
    {
        var zero = "Complex.Zero".Evaluate<Complex>(null, _context);
        var one = "Complex.One".Evaluate<Complex>(null, _context);
        var imaginary = "Complex.ImaginaryOne".Evaluate<Complex>(null, _context);

        Assert.Equal(Complex.Zero, zero);
        Assert.Equal(Complex.One, one);
        Assert.Equal(Complex.ImaginaryOne, imaginary);
    }

    [Fact]
    public void DotNetMathContext_ComplexPow_ExpectedValue()
    {
        var result = "Complex.Pow(base, exp)".Evaluate<Complex>(
            new { @base = new Complex(2, 0), exp = new Complex(3, 0) },
            _context);

        Assert.Equal(new Complex(8, 0), result);
    }

    [Fact]
    public void DotNetMathContext_ComplexSin_ExpectedValue()
    {
        var result = "Complex.Sin(z)".Evaluate<Complex>(
            new { z = new Complex(0, 1) },
            _context);

        var expected = Complex.Sin(new Complex(0, 1));
        Assert.Equal(expected.Real, result.Real, precision: 10);
        Assert.Equal(expected.Imaginary, result.Imaginary, precision: 10);
    }

    #endregion

    #region Default Values Tests

    [Fact]
    public void DotNetMathContext_DefaultHalf_ExpectedValue()
    {
        var result = "default(Half)".Evaluate<Half>(null, _context);
        Assert.Equal((Half)0.0, result);
    }

    [Fact]
    public void DotNetMathContext_DefaultBigInteger_ExpectedValue()
    {
        var result = "default(BigInteger)".Evaluate<BigInteger>(null, _context);
        Assert.Equal(BigInteger.Zero, result);
    }

    [Fact]
    public void DotNetMathContext_DefaultInt128_ExpectedValue()
    {
        var result = "default(Int128)".Evaluate<Int128>(null, _context);
        Assert.Equal(Int128.Zero, result);
    }

    #endregion

    #region Mixed Type Operations

    [Fact]
    public void DotNetMathContext_MixedOperations_ExpectedValue()
    {
        var result = "Math.Pow(2, 10) + BigInteger.Pow(x, 2)".Evaluate<double>(
            new { x = new BigInteger(10) },
            _context);

        Assert.Equal(1124.0, result); // 1024 + 100
    }

    [Fact]
    public void DotNetMathContext_ComplexExpression_ExpectedValue()
    {
        var result = "Math.Log2(1024) + Math.Sqrt(16) - Math.Abs(-5)".Evaluate(null, _context);
        Assert.Equal(9.0, result); // 10 + 4 - 5
    }

    #endregion

    #region Compilation Tests

    [Fact]
    public void DotNetMathContext_CompileBigIntegerExpression_ExpectedValue()
    {
        var dict = new Dictionary<string, object>
        {
            { "a", BigInteger.Parse("1000000000000") },
            { "b", new BigInteger(2) }
        };

        var fn = "BigInteger.Multiply(a, b)".Compile<Dictionary<string, object>, BigInteger>(dict, _context);
        var result = fn(dict);

        Assert.Equal(BigInteger.Parse("2000000000000"), result);
    }

    [Fact]
    public void DotNetMathContext_CompileInt128Expression_ExpectedValue()
    {
        var dict = new Dictionary<string, object>
        {
            { "x", Int128.MaxValue / 2 },
            { "y", Int128.One }
        };

        var fn = "x + y".Compile<Dictionary<string, object>, Int128>(dict, _context);
        var result = fn(dict);

        Assert.Equal(Int128.MaxValue / 2 + Int128.One, result);
    }

    [Fact]
    public void DotNetMathContext_CompileMathFunction_ExpectedValue()
    {
        var dict = new Dictionary<string, object> { { "x", 8.0 } };
        var fn = "Math.Log2(x) + Math.Sqrt(x)".Compile<Dictionary<string, object>, double>(dict, _context);

        dict["x"] = 8.0;
        var result = fn(dict);
        Assert.Equal(3.0 + Math.Sqrt(8.0), result, precision: 10);
    }

    #endregion

    [Fact]
    public void DotNetMathContext_MathMaxMagnitude_ExpectedValue()
    {
        var result = "Math.MaxMagnitude(-5.5, 3.2)".Evaluate(null, _context);
        Assert.Equal(Math.MaxMagnitude(-5.5, 3.2), result);
    }

    [Fact]
    public void DotNetMathContext_MathMinMagnitude_ExpectedValue()
    {
        var result = "Math.MinMagnitude(-5.5, 3.2)".Evaluate(null, _context);
        Assert.Equal(Math.MinMagnitude(-5.5, 3.2), result);
    }

    [Fact]
    public void DotNetMathContext_HalfConstants_Net9_ExpectedValue()
    {
        var e = "Half.E".Evaluate<Half>(null, _context);
        var pi = "Half.Pi".Evaluate<Half>(null, _context);

        Assert.Equal(Half.E, e);
        Assert.Equal(Half.Pi, pi);
    }

    [Fact]
    public void DotNetMathContext_Int128Functions_ExpectedValue()
    {
        var max = "Int128.Max(a, b)".Evaluate<Int128>(
            new { a = Int128.MaxValue / 2, b = Int128.One },
            _context);

        var min = "Int128.Min(a, b)".Evaluate<Int128>(
            new { a = Int128.MaxValue / 2, b = Int128.One },
            _context);

        var abs = "Int128.Abs(x)".Evaluate<Int128>(
            new { x = Int128.NegativeOne },
            _context);

        Assert.Equal(Int128.MaxValue / 2, max);
        Assert.Equal(Int128.One, min);
        Assert.Equal(Int128.One, abs);
    }

    [Fact]
    public void DotNetMathContext_UInt128Functions_ExpectedValue()
    {
        var max = "UInt128.Max(a, b)".Evaluate<UInt128>(
            new { a = UInt128.MaxValue / 2, b = UInt128.One },
            _context);

        var min = "UInt128.Min(a, b)".Evaluate<UInt128>(
            new { a = UInt128.MaxValue / 2, b = UInt128.One },
            _context);

        Assert.Equal(UInt128.MaxValue / 2, max);
        Assert.Equal(UInt128.One, min);
    }
}

#endif
