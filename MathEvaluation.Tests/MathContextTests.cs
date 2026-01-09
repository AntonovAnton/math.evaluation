using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Numerics;

namespace MathEvaluation.Tests;

public class MathContextTests
{
    [Fact]
    public void MathContext_Bind_HasNotSupportedCustomSystemFunc_ThrowNotSupportedException()
    {
        Func<double, double, double, double, double, double, double> min = (_, _, _, _, _, _) => 0d;

        var ex = Record.Exception(() => new MathContext().Bind(new { min }));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal(
            "System.Func`7[System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double] isn't supported for 'min', you can use Func<T[], T> instead.",
            ex.Message);
    }

#if !NET8_0_OR_GREATER

    [Fact]
    public void MathContext_Bind_HasNotSupportedConstantType_ThrowNotSupportedException()
    {
        var ex = Record.Exception(() => new MathContext().BindConstant(new Vector2(1f), 'v'));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.Numerics.Vector2 isn't supported for 'v'.", ex.Message);
    }

#endif

    [Fact]
    public void MathContext_Bind_HasNotSupportedSystemString_ThrowNotSupportedException()
    {
        var v = new Vector2(1f);

        var ex = Record.Exception(() => new MathContext().Bind(new { v }));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.Numerics.Vector2 isn't supported for 'v'.", ex.Message);
    }

    [Fact]
    public void MathContext_Bind_HasEmptyString_ThrowNotSupportedException()
    {
        var v = string.Empty;

        var ex = Record.Exception(() => new MathContext().Bind(new { v }));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("Cannot bind a variable to an empty or whitespace-only expression string for 'v'.", ex.Message);
    }

#if NET8_0_OR_GREATER

    #region INumberBase Function Tests

    [Fact]
    public void MathContext_Bind_HasInt32Function_ExpectedValue()
    {
        Func<int> getConstant = () => 42;
        Func<int, int> double_it = x => x * 2;
        Func<int, int, int> add = (a, b) => a + b;

        var context = new MathContext();
        context.Bind(new { getConstant, double_it, add });

        var value = "getConstant() + double_it(5) + add(3, 7)".Evaluate<int>(null, context);

        Assert.Equal(42 + 10 + 10, value);
    }

    [Fact]
    public void MathContext_Bind_HasBigIntegerFunction_ExpectedValue()
    {
        Func<BigInteger> getHuge = () => BigInteger.Parse("123456789012345678901234567890");
        Func<BigInteger, BigInteger> square = x => x * x;
        Func<BigInteger, BigInteger, BigInteger> multiply = (a, b) => a * b;

        var context = new MathContext();
        context.Bind(new { getHuge, square, multiply });

        var result = "multiply(square(10), 100)".Evaluate<BigInteger>(null, context);

        Assert.Equal(new BigInteger(10000), result);
    }

    [Fact]
    public void MathContext_Bind_HasHalfFunction_ExpectedValue()
    {
        Func<Half> getPi = () => (Half)3.14;
        Func<Half, Half> negate = x => (Half)(-(double)x);
        Func<Half, Half, Half> max = (a, b) => (double)a > (double)b ? a : b;

        var context = new MathContext();
        context.Bind(new { getPi, negate, max });

        var result = "max(getPi(), negate(getPi()))".Evaluate<Half>(null, context);

        Assert.Equal((Half)3.14, result);
    }

    [Fact]
    public void MathContext_Bind_HasFloatFunction_ExpectedValue()
    {
        Func<float> getEpsilon = () => float.Epsilon;
        Func<float, float> abs = x => Math.Abs(x);
        Func<float, float, float> min = (a, b) => Math.Min(a, b);

        var context = new MathContext();
        context.Bind(new { getEpsilon, abs, min });

        var result = "min(abs(-5.5), 3.2)".Evaluate<float>(null, context);

        Assert.Equal(3.2f, result, precision: 5);
    }

    [Fact]
    public void MathContext_Bind_HasLongFunction_ExpectedValue()
    {
        Func<long> getMax = () => long.MaxValue;
        Func<long, long> halve = x => x / 2;
        Func<long, long, long> subtract = (a, b) => a - b;

        var context = new MathContext();
        context.Bind(new { getMax, halve, subtract });

        var result = "subtract(halve(100), 25)".Evaluate<long>(null, context);

        Assert.Equal(25L, result);
    }

    [Fact]
    public void MathContext_Bind_HasByteFunction_ExpectedValue()
    {
        Func<byte> getMax = () => byte.MaxValue;
        Func<byte, byte> identity = x => x;
        Func<byte, byte, byte> min = (a, b) => a < b ? a : b;

        var context = new MathContext();
        context.Bind(new { getMax, identity, min });

        var result = "min(identity(100), 50)".Evaluate<byte>(null, context);

        Assert.Equal((byte)50, result);
    }

    [Fact]
    public void MathContext_Bind_HasMultipleINumberBaseFunctions_ExpectedValue()
    {
        Func<int, int, int> add_int = (a, b) => a + b;
        Func<long, long, long> multiply_long = (a, b) => a * b;
        Func<BigInteger, BigInteger> square_big = x => x * x;

        var context = new MathContext();
        context.Bind(new { add_int, multiply_long, square_big });

        var result1 = "add_int(10, 20)".Evaluate<int>(null, context);
        var result2 = "multiply_long(1000000000, 2)".Evaluate<long>(null, context);
        var result3 = "square_big(1000)".Evaluate<BigInteger>(null, context);

        Assert.Equal(30, result1);
        Assert.Equal(2000000000L, result2);
        Assert.Equal(new BigInteger(1000000), result3);
    }

    [Fact]
    public void MathContext_Bind_HasVariadicINumberBaseFunction_ExpectedValue()
    {
        Func<int[], int> sum = args => args.Sum();
        Func<BigInteger[], BigInteger> product = args =>
        {
            var result = BigInteger.One;
            foreach (var arg in args)
                result *= arg;
            return result;
        };

        var context = new MathContext();
        context.Bind(new { sum, product });

        var result1 = "sum(1, 2, 3, 4, 5)".Evaluate<int>(null, context);
        var result2 = "product(10, 20, 30)".Evaluate<BigInteger>(null, context);

        Assert.Equal(15, result1);
        Assert.Equal(new BigInteger(6000), result2);
    }

    [Fact]
    public void MathContext_Bind_HasINumberBaseFunctionWithProgrammingContext_ExpectedValue()
    {
        Func<int, int, int> pow = (a, b) => (int)Math.Pow(a, b);

        var context = new ProgrammingMathContext();
        context.Bind(new { pow });

        var result = "pow(5, 2) + 10".Evaluate<int>(null, context);

        Assert.Equal(25 + 10, result);
    }

    [Fact]
    public void MathContext_Bind_HasBigIntegerWithCryptographyFunctions_ExpectedValue()
    {
        Func<BigInteger, BigInteger, BigInteger, BigInteger> modPow = static (value, exponent, modulus) =>
            BigInteger.ModPow(value, exponent, modulus);

        Func<BigInteger, BigInteger, BigInteger> gcd = (a, b) =>
            BigInteger.GreatestCommonDivisor(a, b);

        var p = BigInteger.Parse("2305843009213693951");
        var q = BigInteger.Parse("2305843009213693967");
        var n = p * q;

        var context = new MathContext();
        context.Bind(new { modPow, gcd, n });

        // Test modular exponentiation
        var result = "modPow(123456789, 65537, n)".Evaluate<BigInteger>(null, context);
        var expected = BigInteger.ModPow(123456789, 65537, n);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void MathContext_Bind_HasMixedNumericTypeFunctions_ExpectedValue()
    {
        Func<int> getInt = () => 42;
        Func<float, float> floatAbs = x => Math.Abs(x);
        Func<BigInteger, BigInteger, BigInteger> bigAdd = (a, b) => a + b;

        var context = new MathContext();
        context.Bind(new { getInt, floatAbs, bigAdd });

        var intResult = "getInt()".Evaluate<int>(null, context);
        var floatResult = "floatAbs(-3.14)".Evaluate<float>(null, context);
        var bigResult = "bigAdd(1000000000000, 2000000000000)".Evaluate<BigInteger>(null, context);

        Assert.Equal(42, intResult);
        Assert.Equal(3.14f, floatResult, precision: 5);
        Assert.Equal(BigInteger.Parse("3000000000000"), bigResult);
    }

    [Fact]
    public void MathContext_Bind_HasComplexINumberBaseFunctionChain_ExpectedValue()
    {
        Func<int, int, int, int> triple_add = (a, b, c) => a + b + c;
        Func<int, int, int, int, int> quad_multiply = (a, b, c, d) => a * b * c * d;
        Func<int, int, int, int, int, int> penta_sum = (a, b, c, d, e) => a + b + c + d + e;

        var context = new MathContext();
        context.Bind(new { triple_add, quad_multiply, penta_sum });

        var result1 = "triple_add(1, 2, 3)".Evaluate<int>(null, context);
        var result2 = "quad_multiply(2, 3, 4, 5)".Evaluate<int>(null, context);
        var result3 = "penta_sum(1, 2, 3, 4, 5)".Evaluate<int>(null, context);

        Assert.Equal(6, result1);
        Assert.Equal(120, result2);
        Assert.Equal(15, result3);
    }

    [Fact]
    public void MathContext_BindConstant_HasINumberBaseConstants_ExpectedValue()
    {
        var context = new MathContext();
        context.BindConstant(42, "meaning");
        context.BindConstant(BigInteger.Parse("999999999999999999"), "huge");
        context.BindConstant((Half)3.14, "approxPi");

        var result1 = "meaning * 2".Evaluate<int>(null, context);
        var result2 = "huge + 1".Evaluate<BigInteger>(null, context);
        var result3 = "approxPi * 2".Evaluate<Half>(null, context);

        Assert.Equal(84, result1);
        Assert.Equal(BigInteger.Parse("1000000000000000000"), result2);
        Assert.Equal((Half)6.28, result3);
    }

    [Fact]
    public void MathContext_Constructor_HasINumberBaseFunctions_ExpectedValue()
    {
        Func<int, int> square = x => x * x;
        Func<BigInteger, BigInteger, BigInteger> add = (a, b) => a + b;

        var context = new MathContext(new { square, add });

        var result1 = "square(10)".Evaluate<int>(null, context);
        var result2 = "add(1000000000000, 2000000000000)".Evaluate<BigInteger>(null, context);

        Assert.Equal(100, result1);
        Assert.Equal(BigInteger.Parse("3000000000000"), result2);
    }

    [Fact]
    public void MathContext_Bind_HasINumberBaseWithScientificContext_ExpectedValue()
    {
        Func<int, int, int> min = (a, b) => Math.Min(a, b);
        Func<int, int, int> max = (a, b) => Math.Max(a, b);

        var context = new ScientificMathContext();
        context.Bind(new { min, max });

        var result = "max(min(100, 50), 25) ^ 2".Evaluate<int>(null, context);

        Assert.Equal(2500, result);
    }

    [Fact]
    public void MathContext_Bind_HasBigIntegerFactorial_ExpectedValue()
    {
        Func<BigInteger, BigInteger> factorial = n =>
        {
            if (n <= 1) return BigInteger.One;
            var result = BigInteger.One;
            for (var i = 2; i <= (int)n; i++)
                result *= i;
            return result;
        };

        var context = new MathContext();
        context.Bind(new { factorial });

        var result = "factorial(20)".Evaluate<BigInteger>(null, context);
        var expected = BigInteger.Parse("2432902008176640000");

        Assert.Equal(expected, result);
    }

    #endregion

#endif
}