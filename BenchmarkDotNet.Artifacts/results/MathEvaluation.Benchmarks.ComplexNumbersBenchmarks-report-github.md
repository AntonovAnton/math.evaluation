```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                                           | Job       | Runtime   | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|--------------------------------------------------------------------------------- |---------- |---------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 10.0 | .NET 10.0 |     637.82 ns |     2.466 ns |     1.925 ns | 0.0153 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 10.0 | .NET 10.0 | 360,658.25 ns |   925.969 ns |   820.847 ns |      - |      - |    8944 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 10.0 | .NET 10.0 |   6,940.18 ns |    47.352 ns |    41.977 ns | 0.2136 | 0.1831 |    8478 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 10.0 | .NET 10.0 |      71.33 ns |     0.241 ns |     0.225 ns | 0.0008 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 10.0 | .NET 10.0 |      72.55 ns |     0.408 ns |     0.381 ns | 0.0008 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0  | .NET 8.0  |     725.45 ns |     1.721 ns |     1.526 ns | 0.0019 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0  | .NET 8.0  | 274,959.19 ns | 2,183.979 ns | 2,042.896 ns |      - |      - |    8944 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0  | .NET 8.0  |   8,639.94 ns |    73.352 ns |    68.614 ns | 0.0305 |      - |    8445 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0  | .NET 8.0  |      71.46 ns |     0.489 ns |     0.457 ns | 0.0001 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0  | .NET 8.0  |      71.00 ns |     0.233 ns |     0.218 ns | 0.0001 |      - |      32 B |
