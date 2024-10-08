﻿using BenchmarkDotNet.Running;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

BenchmarkRunner.Run<EvaluationBenchmarks>();
BenchmarkRunner.Run<CompilationBenchmarks>();
BenchmarkRunner.Run<CompoundingInterestBenchmarks>();
