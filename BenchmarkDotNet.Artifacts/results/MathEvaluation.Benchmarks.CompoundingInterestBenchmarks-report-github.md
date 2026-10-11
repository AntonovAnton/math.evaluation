```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9550/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    700.21 ns |     1.557 ns |     1.456 ns | 0.0467 |      - |    1456 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 | 14,125.49 ns |    26.732 ns |    23.697 ns | 1.0986 |      - |   34208 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 87,537.83 ns |   341.214 ns |   319.172 ns |      - |      - |    7488 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,250.11 ns |    25.703 ns |    22.785 ns | 0.1831 | 0.1526 |    5951 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 19,784.33 ns |   130.312 ns |   121.894 ns | 1.0986 | 0.9766 |   37598 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     19.72 ns |     0.050 ns |     0.047 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     19.92 ns |     0.103 ns |     0.086 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     19.91 ns |     0.086 ns |     0.080 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |    994.75 ns |     4.563 ns |     4.268 ns | 0.0458 |      - |    1456 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  | 17,272.44 ns |    49.877 ns |    44.215 ns | 1.1292 |      - |   35472 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 72,313.82 ns | 1,143.592 ns | 1,069.717 ns | 0.1221 |      - |    7500 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,403.77 ns |    23.030 ns |    20.416 ns | 0.1831 | 0.1526 |    5855 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 24,508.30 ns |    90.467 ns |    80.197 ns | 1.2207 | 1.0986 |   38768 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     20.19 ns |     0.074 ns |     0.066 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     20.19 ns |     0.050 ns |     0.047 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     21.53 ns |     0.117 ns |     0.109 ns | 0.0013 |      - |      40 B |
