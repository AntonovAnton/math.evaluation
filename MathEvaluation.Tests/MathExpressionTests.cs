namespace MathEvaluation.Tests;

public class MathExpressionTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void MathExpression_MathStringNull_ThrowArgumentNullException()
    {
        var ex = Record.Exception(() => new MathExpression(null!));

        Assert.IsType<ArgumentNullException>(ex);
    }

    [Theory]
    [InlineData("", "Expression string is empty or white space. (Parameter 'mathString')")]
    [InlineData("   ", "Expression string is empty or white space. (Parameter 'mathString')")]
    public void MathExpression_MathStringEmpty_ThrowArgumentException(string expression,
        string errorMessage)
    {
        testOutputHelper.WriteLine($"{expression}");

        var ex = Record.Exception(() => new MathExpression(expression));

        Assert.IsType<ArgumentException>(ex);
        Assert.Equal(errorMessage, ex.Message);
    }
}