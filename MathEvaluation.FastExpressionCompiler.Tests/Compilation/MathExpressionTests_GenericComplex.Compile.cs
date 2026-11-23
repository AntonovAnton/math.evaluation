#if NET11_0_OR_GREATER
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.FastExpressionCompiler.Tests.Compilation;

/// <summary>Tests for compiling expressions of the generic <see cref="Complex{T}" /> numbers with FastExpressionCompiler (.NET 11+).</summary>
// ReSharper disable once InconsistentNaming
public class MathExpressionTests_GenericComplex(ITestOutputHelper testOutputHelper)
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private readonly ComplexScientificMathContext<double> _doubleContext = new();
    private readonly ComplexScientificMathContext<Decimal64> _decimal64Context = new();

    private static Decimal64 D64(string s) => Decimal64.Parse(s, Culture);

    [Theory]
    [InlineData("3 + 2i", 3, 2)]
    [InlineData("(1 + 2i) * (3 - i)", 5, 5)]
    [InlineData("i^2", -1, 0)]
    [InlineData("sqrt(-4)", 0, 2)]
    [InlineData("2 < 3", 1, 0)]
    public void FastMathExpression_CompileComplexDoubleThenInvoke_ExpectedValue(string mathString, double expectedReal, double expectedImaginary)
    {
        testOutputHelper.WriteLine(mathString);

        var fn = new FastMathExpression(mathString, _doubleContext, Culture).CompileComplex<double>();
        var value = fn();

        Assert.Equal(expectedReal, value.Real, precision: 10);
        Assert.Equal(expectedImaginary, value.Imaginary, precision: 10);
    }

    [Fact]
    public void MathString_CompileComplexFastThenInvoke_HasVariables_ExpectedValue()
    {
        var parameters = new Dictionary<string, Complex<double>> { { "x", new Complex<double>(2, 1) } };
        var fn = "x * i + 1".CompileComplexFast<Dictionary<string, Complex<double>>, double>(parameters, _doubleContext, Culture);

        Assert.Equal(new Complex<double>(0, 2), fn(parameters)); // (2 + i) * i + 1 = 2i - 1 + 1

        parameters["x"] = new Complex<double>(0, 1);
        Assert.Equal(new Complex<double>(0, 0), fn(parameters)); // i * i + 1
    }

    [Fact]
    public void MathString_CompileComplexFastThenInvoke_Decimal64HasVariables_ExpectedValue()
    {
        var parameters = new Dictionary<string, Complex<Decimal64>> { { "z", new Complex<Decimal64>(D64("0.1"), D64("0.2")) } };
        var fn = "z * 10 + 0.1 - 0.3i".CompileComplexFast<Dictionary<string, Complex<Decimal64>>, Decimal64>(parameters, _decimal64Context, Culture);

        var value = fn(parameters);

        Assert.Equal(D64("1.1"), value.Real);
        Assert.Equal(D64("1.7"), value.Imaginary);
    }

    [Theory]
    [InlineData("sqrt(-4 + 2i)")]
    [InlineData("2^(1 + i)")]
    [InlineData("exp(1 - 2i) * (3 - i)")]
    public void MathString_CompileComplexFastThenInvoke_SameAsEvaluate(string mathString)
    {
        testOutputHelper.WriteLine(mathString);

        var parameters = new Dictionary<string, Complex<double>>();
        var expected = mathString.EvaluateComplex<double>(_doubleContext, Culture);
        var value = mathString.CompileComplexFast<Dictionary<string, Complex<double>>, double>(parameters, _doubleContext, Culture)(parameters);

        Assert.Equal(expected.Real, value.Real, precision: 10);
        Assert.Equal(expected.Imaginary, value.Imaginary, precision: 10);
    }

    [Fact]
    public async Task MathString_CompileComplexAsyncFuncFastThenInvoke_HasAsyncFunction_ExpectedValue()
    {
        var context = new ComplexScientificMathContext<double>();
        context.BindAsyncFunction<Complex<double>>(async (z, ct) =>
        {
            await Task.Delay(1, ct);
            return z * Complex<double>.ImaginaryOne;
        }, "rotate");

        var parameters = new Dictionary<string, Complex<double>> { { "x", new Complex<double>(2, 1) } };
        var fn = "rotate(x) + 1".CompileComplexAsyncFuncFast<Dictionary<string, Complex<double>>, double>(parameters, context, Culture);

        var value = await fn(parameters, CancellationToken.None);

        Assert.Equal(new Complex<double>(0, 2), value); // (2 + i) * i + 1
    }
}
#endif
