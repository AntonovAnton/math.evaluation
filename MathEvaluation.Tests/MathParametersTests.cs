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

#if !NET8_0_OR_GREATER

    [Fact]
    public void MathContext_BindVariable_HasNotSupportedType_ThrowNotSupportedException()
    {
        var ex = Record.Exception(() => new MathParameters().BindVariable(new Vector2(1f), 'v'));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.Numerics.Vector2 isn't supported for 'v'.", ex.Message);
    }

#endif

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
}