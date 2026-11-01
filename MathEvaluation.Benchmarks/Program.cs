using BenchmarkDotNet.Running;
using MathEvaluation.Benchmarks;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Checks that every compared library returns the expected result, without benchmarking.
if (args is ["--verify"])
{
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
