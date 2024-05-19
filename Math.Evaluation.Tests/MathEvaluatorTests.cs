using System.Diagnostics;
using System.Globalization;
using Xunit.Abstractions;

namespace Math.Evaluation.Tests
{
    public class MathEvaluatorTests(ITestOutputHelper testOutputHelper)
    {
        [Theory]
        [InlineData("2 / 5 / 2 * 5", 1d)]
        [InlineData("2 + (5 - 1)", 6d)]
        [InlineData("4 * 0.1 - 2", -1.6d)]
        [InlineData("6 + -( -4)", 10d)]
        [InlineData("6 + - ( 4)", 2d)]
        [InlineData("(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6", 743.578530956848d)]
        [InlineData("2 - (5 * 10 / 2) - 1", -24d)]
        [InlineData("1 - -1", 2d)]
        public void MathEvaluator_Evaluate_ExpectedValue(string expression, double expectedValue)
        {
            testOutputHelper.WriteLine($"{expression} = {expectedValue}");

            var calculator = new MathEvaluator();
            var stopwatch = Stopwatch.StartNew();
            var value = calculator.Evaluate(expression);
            stopwatch.Stop();

            testOutputHelper.WriteLine(
                $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");
            Assert.Equal(expectedValue, value, double.Epsilon);
        }

        [Theory]
        [InlineData("(22\u00a0888,323 \u00a4 * 323 - 1 / (2 + 22\u00a0888,323 \u00a4) * 4) - 6", "nn", 7392922.328825254d)]
        public void MathEvaluator_EvaluateNumbers_ExpectedValue(string expression, string cultureName, double expectedValue)
        {
            testOutputHelper.WriteLine($"{expression} = {expectedValue}");

            var cultureInfo = new CultureInfo(cultureName);

            var calculator = new MathEvaluator();
            var stopwatch = Stopwatch.StartNew();
            var value = calculator.Evaluate(expression, cultureInfo);
            stopwatch.Stop();
            
            testOutputHelper.WriteLine(
                $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");
            Assert.Equal(expectedValue, value, double.Epsilon);
        }
    }
}