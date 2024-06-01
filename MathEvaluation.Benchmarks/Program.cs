using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MathEvaluation;

Console.OutputEncoding = Encoding.UTF8;

BenchmarkRunner.Run<Benchmarks>();

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser()]
public class Benchmarks
{
    [Benchmark(Description = "MathEvaluator.Evaluate(\"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\")")]
    public double MathEvaluator_Evaluate_ComplexExpression()
    {
        return MathEvaluator.Evaluate("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6");
    }

    [Benchmark(Description =
        "Parse(\"22888.32\") * Parse(\"30\") / Parse(\"323.34\") / Parse(\".5\") - Parse(\"-1\") / (Parse(\"2\") + Parse(\"22888.32\")) * Parse(\"4\") - Parse(\"6\"))")]
    public double Double_Parse_ComplexExpression()
    {
        return double.Parse("22888.32") * double.Parse("30") / double.Parse("323.34") / double.Parse(".5") -
               double.Parse("-1") / (double.Parse("2") + double.Parse("22888.32")) * double.Parse("4") -
               double.Parse("6");
    }

    [Benchmark(Description = "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)")]
    public double CSharp_ComplexExpression()
    {
        return 22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6;
    }

    [Benchmark(Description = "MathEvaluator.Evaluate(\"sin(30°) + cos(60°)\")")]
    public double MathEvaluator_EvaluateSinCos_ComplexExpression()
    {
        return MathEvaluator.Evaluate("sin(π/6) + cos(π/3)");
    }
}