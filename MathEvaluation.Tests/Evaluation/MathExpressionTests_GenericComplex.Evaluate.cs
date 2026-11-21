#if NET11_0_OR_GREATER
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Tests.Evaluation;

/// <summary>Tests for the generic <see cref="Complex{T}" /> numbers (.NET 11+).</summary>
// ReSharper disable once InconsistentNaming
public class MathExpressionTests_GenericComplex(ITestOutputHelper testOutputHelper)
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private readonly ComplexScientificMathContext _scientificContext = new();
    private readonly ComplexScientificMathContext<double> _doubleContext = new();
    private readonly ComplexScientificMathContext<Decimal64> _decimal64Context = new();
    private readonly ComplexScientificMathContext<Decimal128> _decimal128Context = new();

    private static Decimal64 D64(string s) => Decimal64.Parse(s, Culture);

    [Theory]
    [InlineData("3 + 2i", "3", "2")]
    [InlineData("3 - 2i", "3", "-2")]
    [InlineData("-3 - 2i", "-3", "-2")]
    [InlineData("0.1 + 0.2i", "0.1", "0.2")]
    [InlineData("i", "0", "1")]
    [InlineData("-i", "0", "-1")]
    [InlineData("(1 + 2i) * (3 - i)", "5", "5")]
    [InlineData("(1 + 2i) / (1 - i)", "-0.5", "1.5")]
    [InlineData("0.1 + 0.2", "0.3", "0")]
    public void MathExpression_EvaluateComplexDecimal64_ExpectedValue(string mathString, string expectedReal, string expectedImaginary)
    {
        testOutputHelper.WriteLine(mathString);

        var value = new MathExpression(mathString, null, Culture).EvaluateComplex<Decimal64>();

        // the decimal components are exact, e.g. 0.1 + 0.2 is exactly 0.3
        Assert.Equal(D64(expectedReal), value.Real);
        Assert.Equal(D64(expectedImaginary), value.Imaginary);
    }

    [Theory]
    [InlineData("2^3", 8, 0)]
    [InlineData("i^2", -1, 0)]
    [InlineData("(1 + i)^2", 0, 2)]
    [InlineData("2 < 3", 1, 0)]
    [InlineData("2 >= 3", 0, 0)]
    [InlineData("7 mod 4", 3, 0)]
    public void MathExpression_EvaluateComplexDouble_HasOperators_ExpectedValue(string mathString, double expectedReal, double expectedImaginary)
    {
        testOutputHelper.WriteLine(mathString);

        var value = new MathExpression(mathString, _doubleContext, Culture).EvaluateComplex<double>();

        Assert.Equal(expectedReal, value.Real, precision: 10);
        Assert.Equal(expectedImaginary, value.Imaginary, precision: 10);
    }

    [Fact]
    public void MathExpression_EvaluateComplexDouble_ComparesComplexNumber_ThrowsInvalidCastException()
    {
        var ex = Record.Exception(() => new MathExpression("2i < 3", _doubleContext, Culture).EvaluateComplex<double>());

        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<InvalidCastException>(ex.InnerException);
    }

    [Theory]
    [InlineData("sqrt(-4)", 0, 2)]
    [InlineData("√(-4)", 0, 2)]
    [InlineData("∛8", 2, 0)]
    [InlineData("∜16", 2, 0)]
    [InlineData("|3 + 4i|", 5, 0)]
    [InlineData("abs(-3 - 4i)", 5, 0)]
    [InlineData("ln(e)", 1, 0)]
    [InlineData("log(100)", 2, 0)]
    [InlineData("exp(i*π) + 1", 0, 0)]
    [InlineData("pow(i, 2)", -1, 0)]
    public void MathExpression_EvaluateComplexDouble_HasScientificFn_ExpectedValue(string mathString, double expectedReal, double expectedImaginary)
    {
        testOutputHelper.WriteLine(mathString);

        var value = new MathExpression(mathString, _doubleContext, Culture).EvaluateComplex<double>();

        Assert.Equal(expectedReal, value.Real, precision: 10);
        Assert.Equal(expectedImaginary, value.Imaginary, precision: 10);
    }

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
    public void MathExpression_EvaluateComplexDouble_HasTrigonometricFn_SameAsComplexScientificMathContext(string fn)
    {
        foreach (var z in new[] { "0.5 + 0.3i", "1.5 - 0.7i", "-2 + i", "-0.4 - 1.2i" })
        {
            var mathString = $"{fn}({z})";
            testOutputHelper.WriteLine(mathString);

            var expected = new MathExpression(mathString, _scientificContext, Culture).Evaluate<Complex>();
            var value = new MathExpression(mathString, _doubleContext, Culture).EvaluateComplex<double>();

            Assert.Equal(expected.Real, value.Real, precision: 10);
            Assert.Equal(expected.Imaginary, value.Imaginary, precision: 10);
        }
    }

    [Fact]
    public void MathExpression_EvaluateComplexDecimal128_Pi_HasDecimal128Precision()
    {
        var value = new MathExpression("π", _decimal128Context, Culture).EvaluateComplex<Decimal128>();

        Assert.Equal(Decimal128.Pi, value.Real);
        Assert.Equal(Decimal128.Zero, value.Imaginary);
    }

    [Fact]
    public void MathExpression_EvaluateComplexDecimal64_HasScientificFn_ExpectedValue()
    {
        var value = new MathExpression("sqrt(-0.09) + ∛8", _decimal64Context, Culture).EvaluateComplex<Decimal64>();

        Assert.Equal(2.0, double.CreateChecked(value.Real), precision: 12);
        Assert.Equal(0.3, double.CreateChecked(value.Imaginary), precision: 12);
    }

    [Fact]
    public void MathExpression_EvaluateComplexDecimal64_HasVariables_ExpectedValue()
    {
        var z = new Complex<Decimal64>(D64("0.1"), D64("0.2"));
        var parameters = new Dictionary<string, object> { { "z", z }, { "k", D64("10") } };

        var value = "z * k + 1".EvaluateComplex<Decimal64>(parameters, _decimal64Context, Culture);

        Assert.Equal(D64("2"), value.Real);
        Assert.Equal(D64("2"), value.Imaginary);
    }

    [Fact]
    public void MathExpression_EvaluateComplexFloat_HasAnonymousObjectVariables_ExpectedValue()
    {
        var value = "a + b".EvaluateComplex<float>(new { a = new Complex<float>(1f, 2f), b = new Complex<float>(3f, -1f) },
            new ComplexScientificMathContext<float>(), Culture);

        Assert.Equal(new Complex<float>(4f, 1f), value);
    }

    [Theory]
    [InlineData("sqrt(-4 + 2i)")]
    [InlineData("2^(1 + i)")]
    [InlineData("exp(1 - 2i) * (3 - i)")]
    [InlineData("ln(-1 - i) + |3 - 4i|")]
    public void MathExpression_EvaluateComplexDouble_HasComplexNumbersInOperands_SameAsComplexScientificMathContext(string mathString)
    {
        testOutputHelper.WriteLine(mathString);

        var expected = new MathExpression(mathString, _scientificContext, Culture).Evaluate<Complex>();
        var value = new MathExpression(mathString, _doubleContext, Culture).EvaluateComplex<double>();

        Assert.Equal(expected.Real, value.Real, precision: 10);
        Assert.Equal(expected.Imaginary, value.Imaginary, precision: 10);
    }

    [Fact]
    public void MathExpression_EvaluateComplexDecimal64_HasExpressionVariable_ExpectedValue()
    {
        var context = new ComplexScientificMathContext<Decimal64>();
        context.BindExpressionVariable("0.1 - 0.2i", "z");

        var value = "z * 2 + 1".EvaluateComplex<Decimal64>(context, Culture);

        Assert.Equal(D64("1.2"), value.Real);
        Assert.Equal(D64("-0.4"), value.Imaginary);
    }

    [Fact]
    public void MathExpression_EvaluateGenericComplexWithoutEvaluateComplex_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() => new MathExpression("3 + 2i", _doubleContext, Culture).Evaluate<Complex<double>>());
        Assert.Throws<NotSupportedException>(() => new MathExpression("3 + 2i", _doubleContext, Culture).Compile<Complex<double>>());
    }

    [Fact]
    public void MathExpression_EvaluateComplexDouble_HasComparisonInDotNetMathContext_ExpectedValue()
    {
        var context = new DotNetMathContext();

        Assert.Equal(Complex<double>.One, "1 < 2".EvaluateComplex<double>(context, Culture));
        Assert.Equal(Complex<double>.Zero, "1 > 2".EvaluateComplex<double>(context, Culture));

        var ex = Record.Exception(() => "2i < 3".EvaluateComplex<double>(context, Culture));
        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<InvalidCastException>(ex.InnerException);
    }

    [Theory]
    [InlineData("a < b", 1)]
    [InlineData("a <= b", 1)]
    [InlineData("a > b", 0)]
    [InlineData("a >= b", 0)]
    [InlineData("b > a", 1)]
    [InlineData("a == a", 1)]
    public void MathExpression_EvaluateComplexDecimal128_HasComparisonInDotNetMathContext_ComparesInDecimal128Precision(string mathString, int expected)
    {
        // the numbers differ in the 34th digit, they are equal as doubles
        var parameters = new Dictionary<string, Complex<Decimal128>>
        {
            { "a", new Complex<Decimal128>(Decimal128.Parse("0.1", Culture), Decimal128.Zero) },
            { "b", new Complex<Decimal128>(Decimal128.Parse("0.1000000000000000000000000000000001", Culture), Decimal128.Zero) }
        };

        var value = mathString.EvaluateComplex(parameters, new DotNetMathContext(), Culture);
        var fn = mathString.CompileComplex<Dictionary<string, Complex<Decimal128>>, Decimal128>(parameters, new DotNetMathContext(), Culture);

        Assert.Equal(expected == 1 ? Complex<Decimal128>.One : Complex<Decimal128>.Zero, value);
        Assert.Equal(value, fn(parameters));
    }

    [Theory]
    [InlineData("(1 + i) ** 2", "programming")]
    [InlineData("2 ** (1 - i)", "programming")]
    [InlineData("i^2", "scientific")]
    [InlineData("(1 + i)^(2 - i) + 1", "scientific")]
    public void MathExpression_EvaluateComplex_HasPowerOperatorOfOtherContext_SameAsComplex(string mathString, string contextName)
    {
        // the power operator of the context is the C# compatible operator, it calculates Complex<T>.Pow, not Math.Pow(double, double)
        MathContext context = contextName == "programming" ? new ProgrammingMathContext() : new ScientificMathContext();
        testOutputHelper.WriteLine(mathString);

        var expected = new MathExpression(mathString, context, Culture).Evaluate<Complex>();
        var value = new MathExpression(mathString, context, Culture).EvaluateComplex<double>();
        var compiled = new MathExpression(mathString, context, Culture).CompileComplex<double>()();

        Assert.Equal(expected.Real, value.Real, precision: 14);
        Assert.Equal(expected.Imaginary, value.Imaginary, precision: 14);
        Assert.Equal(value, compiled);
    }

    [Fact]
    public void MathExpression_EvaluateComplexDecimal128_HasPowerAndModuloOfOtherContext_CalculatesInDecimal128()
    {
        var context = new ProgrammingMathContext();
        context.BindExpressionVariable("(1 + i) ** 2", "z"); // the expression-defined variable uses the operators too

        var expected = Complex<Decimal128>.Pow(new Complex<Decimal128>(Decimal128.One, Decimal128.One), new Complex<Decimal128>(2, 0));
        Assert.Equal(expected, "z".EvaluateComplex<Decimal128>(context, Culture));
        Assert.Equal(expected, new MathExpression("z", context, Culture).CompileComplex<Decimal128>()());

        // the remainder of the real parts in the Decimal128 precision
        var remainder = new Complex<Decimal128>(Decimal128.Parse("0.0000000000000000000000000000001", Culture), Decimal128.Zero);
        Assert.Equal(remainder, "1.0000000000000000000000000000001 % 1".EvaluateComplex<Decimal128>(context, Culture));
        Assert.Equal(remainder, new MathExpression("1.0000000000000000000000000000001 % 1", context, Culture).CompileComplex<Decimal128>()());
    }

    [Fact]
    public void MathExpression_EvaluateComplex_HasModuloOfComplexNumberInOtherContext_ThrowsInvalidCastException()
    {
        var context = new ProgrammingMathContext();

        var ex = Record.Exception(() => "7i % 4".EvaluateComplex<double>(context, Culture));
        Assert.IsType<MathExpressionException>(ex);
        Assert.IsType<InvalidCastException>(ex.InnerException);

        var fn = new MathExpression("7i % 4", context, Culture).CompileComplex<double>();
        Assert.Throws<InvalidCastException>(() => fn());
    }
}
#endif