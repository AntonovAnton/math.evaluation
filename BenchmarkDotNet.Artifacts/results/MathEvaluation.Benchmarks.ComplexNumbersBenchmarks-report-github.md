```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9550/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4


```
| Method                                                                           | Job       | Runtime   | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|--------------------------------------------------------------------------------- |---------- |---------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 10.0 | .NET 10.0 |     612.58 ns |     1.958 ns |     1.832 ns | 0.0181 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 10.0 | .NET 10.0 | 390,525.31 ns | 1,625.750 ns | 1,441.185 ns |      - |      - |    8904 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 10.0 | .NET 10.0 |   7,059.31 ns |    59.485 ns |    52.732 ns | 0.2747 | 0.2136 |    8590 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 10.0 | .NET 10.0 |      70.05 ns |     0.207 ns |     0.184 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 10.0 | .NET 10.0 |      68.96 ns |     0.129 ns |     0.121 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0  | .NET 8.0  |     729.21 ns |     3.060 ns |     2.862 ns | 0.0181 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0  | .NET 8.0  | 271,732.10 ns |   749.413 ns |   701.002 ns |      - |      - |    8904 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0  | .NET 8.0  |   8,250.30 ns |    77.881 ns |    72.850 ns | 0.2441 | 0.2136 |    8406 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0  | .NET 8.0  |      67.36 ns |     0.168 ns |     0.157 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0  | .NET 8.0  |      67.18 ns |     0.148 ns |     0.139 ns | 0.0010 |      - |      32 B |
