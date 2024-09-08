using System.Text;
using BenchmarkDotNet.Running;

Console.OutputEncoding = Encoding.UTF8;

BenchmarkRunner.Run<EvaluationBenchmarks>();
//BenchmarkRunner.Run<CompilationBenchmarks>();
