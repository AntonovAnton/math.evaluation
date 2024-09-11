using MathEvaluation.Context;
using System.Numerics;

namespace MathEvaluation.Tests;

public class MathContextTests()
{
    [Fact]
    public void MathContext_Bind_HasNotSupportedCustomSystemFunc_ThrowNotSupportedException()
    {
        Func<double, double, double, double, double, double, double> min = (a, b, c, d, e, v) => 0d;
        var ex = Record.Exception(() => new MathContext().Bind(new { min }));
        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.Func`7[System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double] isn't supported, you can use Func<double[], double> or Func<decimal[], decimal> istead.", ex.Message);
    }

    [Fact]
    public void MathContext_Bind_HasNotSupportedConstantType_ThrowNotSupportedException()
    {
        var ex = Record.Exception(() => new MathContext().BindConstant(new Vector2(1f), 'v'));
        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.Numerics.Vector2 isn't supported.", ex.Message);
    }

    [Fact]
    public void MathContext_Bind_HasNotSupportedSystemString_ThrowNotSupportedException()
    {
        var min = "3";
        var ex = Record.Exception(() => new MathContext().Bind(new { min }));
        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("System.String isn't supported.", ex.Message);
    }
}
