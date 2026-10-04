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
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 10.0 | .NET 10.0 |     618.37 ns |     3.978 ns |     3.721 ns | 0.0181 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 10.0 | .NET 10.0 | 393,174.01 ns | 1,969.342 ns | 1,842.124 ns |      - |      - |    8944 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 10.0 | .NET 10.0 |   7,145.59 ns |    97.289 ns |    91.005 ns | 0.2441 | 0.2136 |    8478 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 10.0 | .NET 10.0 |      70.36 ns |     0.479 ns |     0.448 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 10.0 | .NET 10.0 |      70.32 ns |     0.350 ns |     0.311 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0  | .NET 8.0  |     745.55 ns |     3.706 ns |     3.467 ns | 0.0181 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0  | .NET 8.0  | 271,946.75 ns | 1,494.413 ns | 1,397.875 ns |      - |      - |    8944 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0  | .NET 8.0  |   8,603.70 ns |    59.081 ns |    52.373 ns | 0.2747 | 0.2441 |    8447 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0  | .NET 8.0  |      67.39 ns |     0.154 ns |     0.120 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0  | .NET 8.0  |      67.36 ns |     0.614 ns |     0.575 ns | 0.0010 |      - |      32 B |
