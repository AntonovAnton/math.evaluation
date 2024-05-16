using System.Diagnostics;
using System.Globalization;
using NUnit.Framework;

namespace EvaluateMathExpression.Tests
{
    [TestFixture]
    public class RecursionCalculatorTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            // I don't know why but NUnit doesn't log class name or doesn't group tests
            // Also, Console.WriteLine doesn't work, and even TestContext.Out.WriteLine doesn't
            TestContext.Progress.WriteLine($"{nameof(RecursionCalculatorTests)}");
        }

        [Test]
        [TestCase("2 / 5 / 2 * 5", 1d)]
        [TestCase("2 + (5 - 1)", 6d)]
        [TestCase("4 * 0.1 - 2", -1.6d)]
        [TestCase("6 + -( -4)", 10d)]
        [TestCase("6 + - ( 4)", 2d)]
        [TestCase("(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6", 743.578530956848d)]
        [TestCase("2 - (5 * 10 / 2) - 1", -24d)]
        [TestCase("1 - -1", 2d)]
        public void RecursionCalculator_Calculate_ExpectedValue(string expression, double expectedValue)
        {
            TestContext.Progress.WriteLine($"{expression} = {expectedValue}");

            var calculator = new RecursionCalculator();
            var stopwatch = Stopwatch.StartNew();
            var value = calculator.Eval(expression);
            stopwatch.Stop();

            TestContext.Progress.WriteLine(
                $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");
            Assert.AreEqual(expectedValue, value, double.Epsilon);
        }

        [Test]
        [TestCase("(22\u00a0888,323 \u00a4 * 323 - 1 / (2 + 22\u00a0888,323 \u00a4) * 4) - 6", "nn", 7392922.328825254d)]
        public void RecursionCalculator_CalculateNumbers_ExpectedValue(string expression, string cultureName, double expectedValue)
        {
            TestContext.Progress.WriteLine($"{expression} = {expectedValue}");

            var cultureInfo = new CultureInfo(cultureName);

            var calculator = new RecursionCalculator();
            var stopwatch = Stopwatch.StartNew();
            var value = calculator.Eval(expression, cultureInfo);
            stopwatch.Stop();

            TestContext.Progress.WriteLine(
                $"Execution time: {stopwatch.Elapsed:g} ({stopwatch.ElapsedMilliseconds}ms)");
            Assert.AreEqual(expectedValue, value, double.Epsilon);
        }
    }
}