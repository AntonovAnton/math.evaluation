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
            "System.Func`7[System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double] isn't supported, you can use Func<T[], T> instead.",
            ex.Message);
    }

    [Fact]
    public void MathContext_Bind_HasNotSupportedVariableType_ThrowNotSupportedException()
    {
        var ex = Record.Exception(() => new MathParameters().BindVariable(new Vector2(1f), 'v'));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.Numerics.Vector2 isn't supported.", ex.Message);
    }

    [Fact]
    public void MathParameters_Bind_HasNotSupportedSystemString_ThrowNotSupportedException()
    {
        const string min = "3";

        var ex = Record.Exception(() => new MathParameters().Bind(new { min }));

        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.String isn't supported.", ex.Message);
    }
}