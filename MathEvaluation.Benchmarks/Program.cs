using BenchmarkDotNet.Running;
using MathEvaluation.Benchmarks;
using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Checks that every compared library returns the expected result, without benchmarking.
// An optional culture name (e.g. --verify de-DE) starts with that culture, to check that the benchmarks pin their own.
if (args is ["--verify", ..])
{
    if (args.Length > 1)
    {
        var culture = CultureInfo.GetCultureInfo(args[1]);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.CurrentCulture = culture;
    }

    new ParseAndEvaluateComparisonBenchmarks().Setup();
    new EvaluateComparisonBenchmarks().Setup();
    new CompileComparisonBenchmarks().Setup();
    Console.WriteLine("All compared libraries return the expected results.");
    return;
}

// Pass BenchmarkDotNet arguments to run selected benchmarks, e.g. --filter *Comparison*
if (args.Length > 0)
{
    BenchmarkSwitcher.FromAssembly(typeof(EvaluationBenchmarks).Assembly).Run(args);
    return;
}

BenchmarkRunner.Run<EvaluationBenchmarks>();
BenchmarkRunner.Run<CompilationBenchmarks>();
BenchmarkRunner.Run<AsyncCompilationBenchmarks>();
BenchmarkRunner.Run<CompoundingInterestBenchmarks>();
BenchmarkRunner.Run<ComplexNumbersBenchmarks>();
BenchmarkRunner.Run<ParseAndEvaluateComparisonBenchmarks>();
BenchmarkRunner.Run<EvaluateComparisonBenchmarks>();
BenchmarkRunner.Run<CompileComparisonBenchmarks>();
