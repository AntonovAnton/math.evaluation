using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MathEvaluation;
using MathEvaluation.Context;

Console.OutputEncoding = Encoding.UTF8;

BenchmarkRunner.Run<Benchmarks>();

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class Benchmarks
{
    private const double a = Math.PI / 6;
    private const double b = Math.PI / 3;

    private IMathContext _scientificContext = new ScientificMathContext();

    [Benchmark(Description = "\"22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6\".Evaluate()")]
    public double MathEvaluator_Evaluate_ComplexExpression()
    {
        return "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".Evaluate();
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

    [Benchmark(Description = "\"sin(pi/6) + cos(pi/3)\".SetContext(_scientificContext).Evaluate()")]
    public double MathEvaluator_EvaluateSinCos_ComplexExpression()
    {
        return "sin(pi/6) + cos(pi/3)".SetContext(_scientificContext).Evaluate();
    }

    [Benchmark(Description = "\"sin(a) + cos(b)\".SetContext(_scientificContext).Bind(new { a, b }).Evaluate()")]
    public double MathEvaluator_EvaluateSinCos_HasVariables_ComplexExpression()
    {
        return "sin(a) + cos(b)".SetContext(_scientificContext).Bind(new { a, b }).Evaluate();
    }
}