using BenchmarkDotNet.Running;
using MathEvaluation.Benchmarks;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

BenchmarkRunner.Run<EvaluationBenchmarks>();
BenchmarkRunner.Run<CompilationBenchmarks>();
BenchmarkRunner.Run<CompoundingInterestBenchmarks>();
BenchmarkRunner.Run<ComplexNumbersBenchmarks>();