#if NET11_0_OR_GREATER
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Tests.Compilation;

/// <summary>Tests for compiling expressions of the generic <see cref="Complex{T}" /> numbers (.NET 11+).</summary>
// ReSharper disable once InconsistentNaming
public class MathExpressionTests_GenericComplex(ITestOutputHelper testOutputHelper)
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private readonly ComplexScientificMathContext<double> _doubleContext = new();
    private readonly ComplexScientificMathContext<Decimal64> _decimal64Context = new();

    private static Decimal64 D64(string s) => Decimal64.Parse(s, Culture);

    [Theory]
    [InlineData("3 + 2i", 3, 2)]
    [InlineData("3 - 2i", 3, -2)]
    [InlineData("(1 + 2i) * (3 - i)", 5, 5)]
    [InlineData("2^3", 8, 0)]
    [InlineData("i^2", -1, 0)]
    [InlineData("sqrt(-4)", 0, 2)]
    [InlineData("2 < 3", 1, 0)]
    public void MathExpression_CompileComplexDoubleThenInvoke_ExpectedValue(string mathString, double expectedReal, double expectedImaginary)
    {
        testOutputHelper.WriteLine(mathString);

        var fn = new MathExpression(mathString, _doubleContext, Culture).CompileComplex<double>();
        var value = fn();

        Assert.Equal(expectedReal, value.Real, precision: 10);
        Assert.Equal(expectedImaginary, value.Imaginary, precision: 10);
    }

    [Fact]
    public void MathExpression_CompileComplexDoubleThenInvoke_HasVariables_ExpectedValue()
    {
        var parameters = new Dictionary<string, Complex<double>> { { "x", new Complex<double>(2, 1) } };
        var fn = "x * i + 1".CompileComplex<Dictionary<string, Complex<double>>, double>(parameters, _doubleContext, Culture);

        Assert.Equal(new Complex<double>(0, 2), fn(parameters)); // (2 + i) * i + 1 = 2i - 1 + 1

        parameters["x"] = new Complex<double>(0, 1);
        Assert.Equal(new Complex<double>(0, 0), fn(parameters)); // i * i + 1
    }

    [Fact]
    public void MathExpression_CompileComplexDoubleThenInvoke_HasPowerAndComparisonOfVariables_ExpectedValue()
    {
        var parameters = new Dictionary<string, Complex<double>> { { "x", new Complex<double>(0, 1) }, { "y", new Complex<double>(2, 0) } };

        var power = "x^y".CompileComplex<Dictionary<string, Complex<double>>, double>(parameters, _doubleContext, Culture);
        var value = power(parameters);
        Assert.Equal(-1, value.Real, precision: 10);
        Assert.Equal(0, value.Imaginary, precision: 10);

        parameters["x"] = new Complex<double>(1, 0);
        var less = "x < y".CompileComplex<Dictionary<string, Complex<double>>, double>(parameters, _doubleContext, Culture);
        Assert.Equal(Complex<double>.One, less(parameters));

        parameters["x"] = new Complex<double>(1, 1);
        Assert.Throws<InvalidCastException>(() => less(parameters));
    }

    [Fact]
    public void MathExpression_CompileComplexDecimal64ThenInvoke_HasVariables_ExpectedValue()
    {
        var parameters = new Dictionary<string, Complex<Decimal64>> { { "z", new Complex<Decimal64>(D64("0.1"), D64("0.2")) } };
        var fn = "z * 10 + 0.1 - 0.3i".CompileComplex<Dictionary<string, Complex<Decimal64>>, Decimal64>(parameters, _decimal64Context, Culture);

        var value = fn(parameters);

        Assert.Equal(D64("1.1"), value.Real);
        Assert.Equal(D64("1.7"), value.Imaginary);
    }

    [Theory]
    [InlineData("sqrt(-4 + 2i)")]
    [InlineData("2^(1 + i)")]
    [InlineData("exp(1 - 2i) * (3 - i)")]
    public void MathExpression_CompileComplexDoubleThenInvoke_HasComplexNumbersInOperands_SameAsEvaluate(string mathString)
    {
        testOutputHelper.WriteLine(mathString);

        var expected = new MathExpression(mathString, _doubleContext, Culture).EvaluateComplex<double>();
        var value = new MathExpression(mathString, _doubleContext, Culture).CompileComplex<double>()();

        Assert.Equal(expected.Real, value.Real, precision: 10);
        Assert.Equal(expected.Imaginary, value.Imaginary, precision: 10);
    }

    [Fact]
    public void MathExpression_CompileComplexDecimal64ThenInvoke_HasExpressionVariable_ExpectedValue()
    {
        var context = new ComplexScientificMathContext<Decimal64>();
        context.BindExpressionVariable("0.1 - 0.2i", "z");

        var value = new MathExpression("z * 2 + 1", context, Culture).CompileComplex<Decimal64>()();

        Assert.Equal(D64("1.2"), value.Real);
        Assert.Equal(D64("-0.4"), value.Imaginary);
    }

    [Fact]
    public async Task MathExpression_CompileComplexAsyncFuncDecimal64ThenInvoke_ExpectedValue()
    {
        var fn = new MathExpression("(1 + 2i) * (3 - i) + x", _decimal64Context, Culture)
            .CompileComplexAsyncFunc<Dictionary<string, Complex<Decimal64>>, Decimal64>(new Dictionary<string, Complex<Decimal64>> { { "x", default } });

        var value = await fn(new Dictionary<string, Complex<Decimal64>> { { "x", new Complex<Decimal64>(D64("0.1"), D64("0.2")) } }, CancellationToken.None);

        Assert.Equal(D64("5.1"), value.Real);
        Assert.Equal(D64("5.2"), value.Imaginary);
    }
}
#endif
