using MathEvaluation.Context;
using MathEvaluation.Extensions;

namespace MathEvaluation.Tests.Context;

public class CompositeMathContextTests
{
    [Fact]
    public void CompositeMathContext_ScientificAndCustomFunction_ExpectedValue()
    {
        var custom = new MathContext();
        custom.BindFunction<double>(x => x * 2, "twice");
        var context = new CompositeMathContext(new ScientificMathContext(), custom);

        var result = "twice(sin(π/2)) + 2^3".Evaluate(context);

        Assert.Equal(10.0, result);
    }

    [Fact]
    public void CompositeMathContext_LongestKeyWins_ExpectedValue()
    {
        var first = new MathContext();
        first.BindConstant(1.0, "a");
        var second = new MathContext();
        second.BindConstant(2.0, "ab");
        var context = new CompositeMathContext(first, second);

        Assert.Equal(2.0, "ab".Evaluate(context));
        Assert.Equal(1.0, "a".Evaluate(context));
    }

    [Fact]
    public void CompositeMathContext_SameKey_FirstAddedContextWins()
    {
        var first = new MathContext();
        first.BindConstant(1.0, "x");
        var second = new MathContext();
        second.BindConstant(2.0, "x");

        Assert.Equal(1.0, "x".Evaluate(new CompositeMathContext(first, second)));
        Assert.Equal(2.0, "x".Evaluate(new CompositeMathContext(second, first)));
    }

    [Fact]
    public void CompositeMathContext_OwnBinding_HasPriority()
    {
        var custom = new MathContext();
        custom.BindConstant(1.0, "x");
        var context = new CompositeMathContext(custom);
        context.BindConstant(5.0, "x");

        Assert.Equal(5.0, "x".Evaluate(context));
    }

    [Fact]
    public void CompositeMathContext_Nested_ExpectedValue()
    {
        var custom = new MathContext();
        custom.BindConstant(3.0, "k");
        var inner = new CompositeMathContext(new ScientificMathContext());
        var context = new CompositeMathContext(inner, custom);

        Assert.Equal(9.0, "k^2".Evaluate(context));
    }

    [Fact]
    public void CompositeMathContext_ExpressionVariableUsesOtherContext_ExpectedValue()
    {
        var custom = new MathContext();
        custom.BindExpressionVariable("sin(π/2)", "y");
        var context = new CompositeMathContext(new ScientificMathContext(), custom);

        Assert.Equal(2.0, "y + 1".Evaluate(context));
        Assert.Equal(2.0, "y + x".Evaluate(new { x = 1.0 }, context));
    }

    [Fact]
    public void CompositeMathContext_Decimal_ExpectedValue()
    {
        var custom = new MathContext();
        custom.BindConstant(0.05m, "rate");
        var context = new CompositeMathContext(new DecimalScientificMathContext(), custom);

        var result = "P * (1 + rate)^2".Evaluate<decimal>(new { P = 100m }, context);

        Assert.Equal(110.25m, result);
    }

    [Fact]
    public void CompositeMathContext_Compile_ExpectedValue()
    {
        var custom = new MathContext();
        custom.BindConstant(3.0, "k");
        var context = new CompositeMathContext(new ScientificMathContext(), custom);

        var fn = "cos(x) + k".Compile(new { x = 0.0 }, context);

        Assert.Equal(4.0, fn(new { x = 0.0 }));
    }

    [Fact]
    public void CompositeMathContext_Contexts_InAddedOrder()
    {
        var first = new MathContext();
        var second = new MathContext();
        var context = new CompositeMathContext(first);
        context.Add(second);

        Assert.Equal([first, second], context.Contexts);
    }

    [Fact]
    public void CompositeMathContext_NullContext_ThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new CompositeMathContext(null!));
        Assert.Throws<ArgumentNullException>(() => new CompositeMathContext(new MathContext(), null!));
        Assert.Throws<ArgumentNullException>(() => new CompositeMathContext().Add(null!));
    }

    [Fact]
    public void CompositeMathContext_AddItself_ThrowArgumentException()
    {
        var context = new CompositeMathContext();
        var outer = new CompositeMathContext(context);

        Assert.Throws<ArgumentException>(() => context.Add(context));
        Assert.Throws<ArgumentException>(() => context.Add(outer));
    }
}
