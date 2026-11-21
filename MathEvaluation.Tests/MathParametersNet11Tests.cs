#if NET11_0_OR_GREATER
using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System.Globalization;
using System.Numerics;

namespace MathEvaluation.Tests;

/// <summary>
///     Tests for the parameters and conversions of the .NET 11 types: Decimal32, Decimal64, Decimal128, BFloat16, and Complex&lt;T&gt;.
/// </summary>
public class MathParametersNet11Tests
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    #region binding of the parameters

    [Fact]
    public void MathParameters_Decimal32_BindsVariables() => AssertBindsNumber<Decimal32>();

    [Fact]
    public void MathParameters_Decimal64_BindsVariables() => AssertBindsNumber<Decimal64>();

    [Fact]
    public void MathParameters_Decimal128_BindsVariables() => AssertBindsNumber<Decimal128>();

    [Fact]
    public void MathParameters_BFloat16_BindsVariables() => AssertBindsNumber<BFloat16>();

    [Fact]
    public void MathParameters_ComplexDouble_BindsVariables() => AssertBindsComplex<double>();

    [Fact]
    public void MathParameters_ComplexFloat_BindsVariables() => AssertBindsComplex<float>();

    [Fact]
    public void MathParameters_ComplexHalf_BindsVariables() => AssertBindsComplex<Half>();

    [Fact]
    public void MathParameters_ComplexBFloat16_BindsVariables() => AssertBindsComplex<BFloat16>();

    [Fact]
    public void MathParameters_ComplexDecimal32_BindsVariables() => AssertBindsComplex<Decimal32>();

    [Fact]
    public void MathParameters_ComplexDecimal64_BindsVariables() => AssertBindsComplex<Decimal64>();

    [Fact]
    public void MathParameters_ComplexDecimal128_BindsVariables() => AssertBindsComplex<Decimal128>();

    [Fact]
    public void MathContext_Bind_Net11Types_BindsConstantsAndFunctions()
    {
        var context = new MathContext();
        context.Bind(new
        {
            d32 = P<Decimal32>("0.5"),
            d64 = P<Decimal64>("0.1"),
            d128 = P<Decimal128>("0.2"),
            bf = P<BFloat16>("1.5"),
            half = new Complex<Half>((Half)0.5, (Half)(-1)),
            twice = (Func<Decimal64, Decimal64>)(v => v * P<Decimal64>("2")),
            conj = (Func<Complex<Decimal128>, Complex<Decimal128>>)Complex<Decimal128>.Conjugate
        });

        Assert.Equal(P<Decimal32>("1.5"), "d32 + 1".Evaluate<Decimal32>(context, Invariant));
        Assert.Equal(P<Decimal64>("0.3"), "twice(d64) + 0.1".Evaluate<Decimal64>(context, Invariant));
        Assert.Equal(P<Decimal128>("0.3"), "d128 + 0.1".Evaluate<Decimal128>(context, Invariant));
        Assert.Equal(P<BFloat16>("3"), "bf * 2".Evaluate<BFloat16>(context, Invariant));
        Assert.Equal(new Complex<Half>((Half)1, (Half)(-2)), "half * 2".EvaluateComplex<Half>(context, Invariant));
        Assert.Equal(new Complex<Decimal128>(P<Decimal128>("1"), P<Decimal128>("-2")), "conj(1 + 2i)".EvaluateComplex<Decimal128>(context, Invariant));
    }

    #endregion

    #region conversions between the types

    [Fact]
    public void MathExpression_EvaluateDecimal64_HasDecimalAndDoubleVariables_ConvertsThem()
    {
        var parameters = new { m = 0.2m, d = 0.5d };

        Assert.Equal(P<Decimal64>("0.3"), "m + 0.1".Evaluate<Decimal64>(parameters, null, Invariant));
        Assert.Equal(P<Decimal64>("0.6"), "d + 0.1".Evaluate<Decimal64>(parameters, null, Invariant));

        var fn = "m + 0.1".CompileWith(parameters, default(Decimal64), null, Invariant);
        Assert.Equal(P<Decimal64>("0.3"), fn(parameters));
    }

    [Fact]
    public void MathExpression_EvaluateDecimalAndDouble_HasNet11TypeVariables_ConvertsThem()
    {
        var parameters = new { a = P<Decimal64>("0.2"), b = P<BFloat16>("0.5"), c = P<Decimal128>("1.25") };

        Assert.Equal(0.3m, "a + 0.1".Evaluate<decimal>(parameters, null, Invariant));
        Assert.Equal(1.0, "b * 2".Evaluate<double>(parameters, null, Invariant));
        Assert.Equal(2.5m, "c * 2".Evaluate<decimal>(parameters, null, Invariant));

        var fn = "c * 2".CompileWith(parameters, default(decimal), null, Invariant);
        Assert.Equal(2.5m, fn(parameters));
    }

    [Fact]
    public void MathExpression_EvaluateDecimal64_HasDoubleFunction_ConvertsResult()
    {
        // sin of the scientific context is the double function, its result is converted to Decimal64
        var context = new ScientificMathContext();
        var expected = Decimal64.CreateChecked(Math.Sin(0.5)) + P<Decimal64>("1");

        Assert.Equal(expected, "sin(0.5) + 1".Evaluate<Decimal64>(context, Invariant));
        Assert.Equal(expected, new MathExpression("sin(0.5) + 1", context, Invariant).Compile<Decimal64>()());
    }

    [Fact]
    public void MathExpression_EvaluateComplex_HasDoubleFunctionAndVariables_ConvertsThem()
    {
        // floor of the base scientific context is the double function, the Complex<T> argument and result are converted
        var context = new ComplexScientificMathContext<Decimal64>();
        var expected = new Complex<Decimal64>(P<Decimal64>("2"), P<Decimal64>("1"));

        Assert.Equal(expected, "floor(2.5) + i".EvaluateComplex<Decimal64>(context, Invariant));
        Assert.Equal(expected, new MathExpression("floor(2.5) + i", context, Invariant).CompileComplex<Decimal64>()());

        // the double and decimal variables are converted to Complex<T>
        var parameters = new { d = 2.5d, m = 0.5m };
        var value = "d + m + i".EvaluateComplex<double>(parameters, new ComplexScientificMathContext<double>(), Invariant);
        var fn = "d + m + i".CompileComplexWith(parameters, default(double), new ComplexScientificMathContext<double>(), Invariant);

        Assert.Equal(new Complex<double>(3, 1), value);
        Assert.Equal(new Complex<double>(3, 1), fn(parameters));
    }

    #endregion

    #region API

    [Fact]
    public void StringExtensions_EvaluateComplexAndCompileComplex_AllOverloads_ExpectedValue()
    {
        var context = new ComplexScientificMathContext<Decimal64>();
        var x = new Complex<Decimal64>(P<Decimal64>("0.5"), P<Decimal64>("-1"));
        var expected = x * new Complex<Decimal64>(P<Decimal64>("2"), Decimal64.Zero) + Complex<Decimal64>.ImaginaryOne;

        var mathParameters = new MathParameters();
        mathParameters.BindVariable(x, "x");

        Assert.Equal(expected, "x * 2 + i".EvaluateComplex<Decimal64>(new { x }, context, Invariant));
        Assert.Equal(expected, "x * 2 + i".EvaluateComplex(new Dictionary<string, Complex<Decimal64>> { { "x", x } }, context, Invariant));
        Assert.Equal(expected, "x * 2 + i".EvaluateComplex<Decimal64>(new Dictionary<string, object> { { "x", x } }, context, Invariant));
        Assert.Equal(expected, "x * 2 + i".EvaluateComplex<Decimal64>(mathParameters, context, Invariant));

        var expression = new MathExpression("x * 2 + i", context, Invariant);
        Assert.Equal(expected, expression.EvaluateComplex<Decimal64>(new { x }));
        Assert.Equal(expected, expression.EvaluateComplex(new Dictionary<string, Complex<Decimal64>> { { "x", x } }));
        Assert.Equal(expected, expression.EvaluateComplex<Decimal64>(new Dictionary<string, object> { { "x", x } }));
        Assert.Equal(expected, CompileWithExtensions.CompileComplexWith(expression, new { x }, default(Decimal64))(new { x }));
    }

    [Fact]
    public async Task StringExtensions_CompileComplexAsyncFunc_ExpectedValue()
    {
        var context = new ComplexScientificMathContext<double>();
        var parameters = new { x = new Complex<double>(1, 2) };

        var fn = "x * i".CompileComplexAsyncFuncWith(parameters, default(double), context, Invariant);
        Assert.Equal(new Complex<double>(-2, 1), await fn(parameters, CancellationToken.None));

        var noParameters = new MathExpression("(1 + 2i) * i", context, Invariant).CompileComplexAsyncFunc<double>();
        Assert.Equal(new Complex<double>(-2, 1), await noParameters(CancellationToken.None));
    }

    [Fact]
    public void MathExpression_CompileGenericComplexWithoutCompileComplex_ThrowsNotSupportedException()
    {
        var expression = new MathExpression("1 + 2i", new ComplexScientificMathContext<double>(), Invariant);

        Assert.Throws<NotSupportedException>(() => expression.Evaluate<Complex<double>>());
        Assert.Throws<NotSupportedException>(() => expression.Compile<Complex<double>>());
        Assert.Throws<NotSupportedException>(() => expression.CompileAsyncFunc<Complex<double>>());
        Assert.Throws<NotSupportedException>(() => expression.Compile<object, Complex<double>>(new { }));
    }

    [Fact]
    public void MathExpression_EvaluateComplex_RaisesEvaluatingEvent()
    {
        var expression = new MathExpression("(1 + 2i) * (3 - i)", new ComplexScientificMathContext<Decimal64>(), Invariant);
        var steps = new List<EvaluatingEventArgs>();
        expression.Evaluating += (_, args) => steps.Add(args);

        var value = expression.EvaluateComplex<Decimal64>();

        Assert.NotEmpty(steps);
        var last = steps[^1];
        Assert.True(last.IsCompleted);
        Assert.Equal(value, Assert.IsType<Complex<Decimal64>>(last.Value));
        Assert.Contains(steps, s => s.Value is Complex<Decimal64> { Imaginary: var im } && im != Decimal64.Zero);
    }

    #endregion

    #region private Methods

    private static T P<T>(string s)
        where T : INumberBase<T>
        => T.Parse(s, NumberStyles.Float, Invariant);

    private static void AssertBindsNumber<T>()
        where T : struct, INumberBase<T>
    {
        var x = P<T>("1.5");
        var expected = x * P<T>("2") + T.One;
        const string mathString = "x * 2 + 1";

        var mathParameters = new MathParameters();
        mathParameters.BindVariable(x, "x");

        Assert.Equal(expected, mathString.Evaluate<T>(new { x }, null, Invariant));
        Assert.Equal(expected, mathString.Evaluate(new Dictionary<string, T> { { "x", x } }, null, Invariant));
        Assert.Equal(expected, mathString.Evaluate<T>(new Dictionary<string, object> { { "x", x } }, null, Invariant));
        Assert.Equal(expected, mathString.Evaluate<T>(mathParameters, null, Invariant));

        var anonymous = new { x };
        Assert.Equal(expected, mathString.CompileWith(anonymous, default(T), null, Invariant)(anonymous));

        var dictionary = new Dictionary<string, T> { { "x", x } };
        Assert.Equal(expected, mathString.Compile<Dictionary<string, T>, T>(dictionary, null, Invariant)(dictionary));
    }

    private static void AssertBindsComplex<T>()
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        var x = new Complex<T>(P<T>("1.5"), P<T>("-0.5"));
        var expected = x * new Complex<T>(P<T>("2"), T.Zero) + Complex<T>.ImaginaryOne;
        const string mathString = "x * 2 + i";

        var mathParameters = new MathParameters();
        mathParameters.BindVariable(x, "x");

        Assert.Equal(expected, mathString.EvaluateComplex<T>(new { x }, null, Invariant));
        Assert.Equal(expected, mathString.EvaluateComplex(new Dictionary<string, Complex<T>> { { "x", x } }, null, Invariant));
        Assert.Equal(expected, mathString.EvaluateComplex<T>(new Dictionary<string, object> { { "x", x } }, null, Invariant));
        Assert.Equal(expected, mathString.EvaluateComplex<T>(mathParameters, null, Invariant));

        var anonymous = new { x };
        Assert.Equal(expected, mathString.CompileComplexWith(anonymous, default(T), null, Invariant)(anonymous));

        var dictionary = new Dictionary<string, Complex<T>> { { "x", x } };
        Assert.Equal(expected, mathString.CompileComplex<Dictionary<string, Complex<T>>, T>(dictionary, null, Invariant)(dictionary));
    }

    #endregion
}

/// <summary>Infers the anonymous type of the parameters, the type of the result is specified by the default value.</summary>
internal static class CompileWithExtensions
{
    public static Func<TParameters, TResult> CompileWith<TParameters, TResult>(this string mathString, TParameters parameters, TResult _,
        MathContext? context, IFormatProvider? provider)
        where TResult : struct, INumberBase<TResult>
        => mathString.Compile<TParameters, TResult>(parameters, context, provider);

    public static Func<TParameters, Complex<T>> CompileComplexWith<TParameters, T>(this string mathString, TParameters parameters, T _,
        MathContext? context, IFormatProvider? provider)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
        => mathString.CompileComplex<TParameters, T>(parameters, context, provider);

    public static Func<TParameters, Complex<T>> CompileComplexWith<TParameters, T>(MathExpression expression, TParameters parameters, T _)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
        => expression.CompileComplex<TParameters, T>(parameters);

    public static Func<TParameters, CancellationToken, ValueTask<Complex<T>>> CompileComplexAsyncFuncWith<TParameters, T>(this string mathString,
        TParameters parameters, T _, MathContext? context, IFormatProvider? provider)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
        => mathString.CompileComplexAsyncFunc<TParameters, T>(parameters, context, provider);
}
#endif
