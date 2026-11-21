global using static MathEvaluation.FastExpressionCompiler.Tests.TestDecimal;
using System.Globalization;

namespace MathEvaluation.FastExpressionCompiler.Tests;

/// <summary>Converts the <see cref="double" /> test data to <see cref="decimal" /> and compares the results.</summary>
internal static class TestDecimal
{
    /// <summary>
    ///     Converts the <see cref="double" /> to the <see cref="decimal" /> as it's written, e.g. <c>20.3d</c> to <c>20.3m</c>.
    ///     Since .NET 11 the explicit conversion is exact, e.g. <c>(decimal)20.3d</c> is <c>20.300000000000000710542735760</c>.
    /// </summary>
    public static decimal ToDecimal(double value)
        => decimal.Parse(value.ToString("R", CultureInfo.InvariantCulture), NumberStyles.Float, CultureInfo.InvariantCulture);

    /// <summary>
    ///     Verifies that the <see cref="decimal" /> result equals the expected value as it's written,
    ///     or that it equals it in the double precision when the result is calculated by the double functions, e.g. sin, ln, or π.
    ///     The double result is converted to decimal by .NET, since .NET 11 the conversion is exact,
    ///     e.g. <c>(decimal)Math.PI</c> is <c>3.1415926535897931159979634685</c>.
    /// </summary>
    public static void AssertDecimal(double expected, decimal actual)
    {
        if (actual == ToDecimal(expected))
            return;

        const double relativeTolerance = 1e-14; // a few units in the last place of the double
        Assert.Equal(expected, (double)actual, Math.Max(Math.Abs(expected), 1d) * relativeTolerance);
    }
}
