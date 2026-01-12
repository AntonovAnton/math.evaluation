using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System.Numerics;

namespace MathEvaluation.Tests;

public class MathParametersTests
{
    [Fact]
    public void MathParameters_Bind_HasNotSupportedCustomSystemFunc_ThrowNotSupportedException()
    {
        Func<double, double, double, double, double, double, double> min = (_, _, _, _, _, _) => 0d;

        var ex = Record.Exception(() => new MathParameters().Bind(new { min }));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal(
            "System.Func`7[System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double] isn't supported for 'min', you can use Func<T[], T> instead.",
            ex.Message);
    }

    [Fact]
    public void MathParameters_Bind_HasNotSupportedType_ThrowNotSupportedException()
    {
        var v = new Vector2(1f);

        var ex = Record.Exception(() => new MathParameters().Bind(new { v }));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.Numerics.Vector2 isn't supported for 'v'.", ex.Message);
    }

    [Fact]
    public void MathParameters_Bind_HasEmptyString_ThrowNotSupportedException()
    {
        var v = string.Empty;

        var ex = Record.Exception(() => new MathParameters().Bind(new { v }));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("Cannot bind a variable to an empty or whitespace-only expression string for 'v'.", ex.Message);
    }

    #region INumberBase Function Tests

    [Fact]
    public void MathParameters_Bind_HasInt32Function_ExpectedValue()
    {
        Func<int> getConstant = () => 42;
        Func<int, int> double_it = x => x * 2;
        Func<int, int, int> add = (a, b) => a + b;

        var parameters = new MathParameters();
        parameters.Bind(new { getConstant, double_it, add });

        var value = "getConstant() + double_it(5) + add(3, 7)".Evaluate<int>(parameters);

        Assert.Equal(42 + 10 + 10, value);
    }

    [Fact]
    public void MathParameters_Bind_HasBigIntegerFunction_ExpectedValue()
    {
        Func<BigInteger> getHuge = () => BigInteger.Parse("123456789012345678901234567890");
        Func<BigInteger, BigInteger> square = x => x * x;
        Func<BigInteger, BigInteger, BigInteger> multiply = (a, b) => a * b;

        var parameters = new MathParameters();
        parameters.Bind(new { getHuge, square, multiply });

        var result = "multiply(square(10), 100)".Evaluate<BigInteger>(parameters);

        Assert.Equal(new BigInteger(10000), result);
    }

    [Fact]
    public void MathParameters_Bind_HasHalfFunction_ExpectedValue()
    {
        Func<Half> getPi = () => (Half)3.14;
        Func<Half, Half> negate = x => (Half)(-(double)x);
        Func<Half, Half, Half> max = (a, b) => (double)a > (double)b ? a : b;

        var parameters = new MathParameters();
        parameters.Bind(new { getPi, negate, max });

        var result = "max(getPi(), negate(getPi()))".Evaluate<Half>(parameters);

        Assert.Equal((Half)3.14, result);
    }

    [Fact]
    public void MathParameters_Bind_HasFloatFunction_ExpectedValue()
    {
        Func<float> getEpsilon = () => float.Epsilon;
        Func<float, float> abs = x => Math.Abs(x);
        Func<float, float, float> min = (a, b) => Math.Min(a, b);

        var parameters = new MathParameters();
        parameters.Bind(new { getEpsilon, abs, min });

        var result = "min(abs(-5.5), 3.2)".Evaluate<float>(parameters);

        Assert.Equal(3.2f, result, precision: 5);
    }

    [Fact]
    public void MathParameters_Bind_HasLongFunction_ExpectedValue()
    {
        Func<long> getMax = () => long.MaxValue;
        Func<long, long> halve = x => x / 2;
        Func<long, long, long> subtract = (a, b) => a - b;

        var parameters = new MathParameters();
        parameters.Bind(new { getMax, halve, subtract });

        var result = "subtract(halve(100), 25)".Evaluate<long>(parameters);

        Assert.Equal(25L, result);
    }

    [Fact]
    public void MathParameters_Bind_HasByteFunction_ExpectedValue()
    {
        Func<byte> getMax = () => byte.MaxValue;
        Func<byte, byte> identity = x => x;
        Func<byte, byte, byte> min = (a, b) => a < b ? a : b;

        var parameters = new MathParameters();
        parameters.Bind(new { getMax, identity, min });

        var result = "min(identity(100), 50)".Evaluate<byte>(parameters);

        Assert.Equal((byte)50, result);
    }

    [Fact]
    public void MathParameters_Bind_HasMultipleINumberBaseFunctions_ExpectedValue()
    {
        Func<int, int, int> add_int = (a, b) => a + b;
        Func<long, long, long> multiply_long = (a, b) => a * b;
        Func<BigInteger, BigInteger> square_big = x => x * x;

        var parameters = new MathParameters();
        parameters.Bind(new { add_int, multiply_long, square_big });

        var result1 = "add_int(10, 20)".Evaluate<int>(parameters);
        var result2 = "multiply_long(1000000000, 2)".Evaluate<long>(parameters);
        var result3 = "square_big(1000)".Evaluate<BigInteger>(parameters);

        Assert.Equal(30, result1);
        Assert.Equal(2000000000L, result2);
        Assert.Equal(new BigInteger(1000000), result3);
    }

    [Fact]
    public void MathParameters_Bind_HasVariadicINumberBaseFunction_ExpectedValue()
    {
        Func<int[], int> sum = args => args.Sum();
        Func<BigInteger[], BigInteger> product = args =>
        {
            var result = BigInteger.One;
            foreach (var arg in args)
                result *= arg;
            return result;
        };

        var parameters = new MathParameters();
        parameters.Bind(new { sum, product });

        var result1 = "sum(1, 2, 3, 4, 5)".Evaluate<int>(parameters);
        var result2 = "product(10, 20, 30)".Evaluate<BigInteger>(parameters);

        Assert.Equal(15, result1);
        Assert.Equal(new BigInteger(6000), result2);
    }

    [Fact]
    public void MathParameters_Bind_HasINumberBaseFunctionWithContext_ExpectedValue()
    {
        Func<int, int, int> pow = (a, b) => (int)Math.Pow(a, b);
        var x = 5;
        var y = 10;

        var parameters = new MathParameters();
        parameters.Bind(new { pow, x, y });

        var result = "pow(x, 2) + y".Evaluate<int>(parameters, new ProgrammingMathContext());

        Assert.Equal(25 + 10, result);
    }

    [Fact]
    public void MathParameters_Bind_HasBigIntegerWithCryptographyFunctions_ExpectedValue()
    {
        Func<BigInteger, BigInteger, BigInteger, BigInteger> modPow = static (value, exponent, modulus) =>
            BigInteger.ModPow(value, exponent, modulus);

        Func<BigInteger, BigInteger, BigInteger> gcd = (a, b) =>
            BigInteger.GreatestCommonDivisor(a, b);

        var p = BigInteger.Parse("2305843009213693951");
        var q = BigInteger.Parse("2305843009213693967");
        var n = p * q;

        var parameters = new MathParameters();
        parameters.Bind(new { modPow, gcd, n });

        // Test modular exponentiation
        var result = "modPow(123456789, 65537, n)".Evaluate<BigInteger>(parameters);
        var expected = BigInteger.ModPow(123456789, 65537, n);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void MathParameters_Bind_HasMixedNumericTypeFunctions_ExpectedValue()
    {
        Func<int> getInt = () => 42;
        Func<float, float> floatAbs = x => Math.Abs(x);
        Func<BigInteger, BigInteger, BigInteger> bigAdd = (a, b) => a + b;

        var parameters = new MathParameters();
        parameters.Bind(new { getInt, floatAbs, bigAdd });

        var intResult = "getInt()".Evaluate<int>(parameters);
        var floatResult = "floatAbs(-3.14)".Evaluate<float>(parameters);
        var bigResult = "bigAdd(1000000000000, 2000000000000)".Evaluate<BigInteger>(parameters);

        Assert.Equal(42, intResult);
        Assert.Equal(3.14f, floatResult, precision: 5);
        Assert.Equal(BigInteger.Parse("3000000000000"), bigResult);
    }

    [Fact]
    public void MathParameters_BindFromDictionary_HasINumberBaseFunctions_ExpectedValue()
    {
        var dict = new Dictionary<string, object>
        {
            { "square", new Func<int, int>(x => x * x) },
            { "add", new Func<int, int, int>((a, b) => a + b) },
            { "value", 10 }
        };

        var parameters = new MathParameters(dict);

        var result = "add(square(value), 50)".Evaluate<int>(parameters);

        Assert.Equal(150, result);
    }

    [Fact]
    public void MathParameters_Bind_HasComplexINumberBaseFunctionChain_ExpectedValue()
    {
        Func<int, int, int, int> triple_add = (a, b, c) => a + b + c;
        Func<int, int, int, int, int> quad_multiply = (a, b, c, d) => a * b * c * d;
        Func<int, int, int, int, int, int> penta_sum = (a, b, c, d, e) => a + b + c + d + e;

        var parameters = new MathParameters();
        parameters.Bind(new { triple_add, quad_multiply, penta_sum });

        var result1 = "triple_add(1, 2, 3)".Evaluate<int>(parameters);
        var result2 = "quad_multiply(2, 3, 4, 5)".Evaluate<int>(parameters);
        var result3 = "penta_sum(1, 2, 3, 4, 5)".Evaluate<int>(parameters);

        Assert.Equal(6, result1);
        Assert.Equal(120, result2);
        Assert.Equal(15, result3);
    }

    #endregion
}