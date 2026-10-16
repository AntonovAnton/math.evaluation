```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    711.18 ns |   6.260 ns |   5.856 ns | 0.0458 |      - |    1456 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 | 13,944.50 ns |  27.080 ns |  25.331 ns | 1.0986 |      - |   34208 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 87,988.65 ns | 537.662 ns | 502.929 ns | 0.2441 |      - |    7512 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,299.01 ns |  25.056 ns |  23.437 ns | 0.1831 | 0.1526 |    5951 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 19,719.16 ns | 135.780 ns | 120.366 ns | 1.0986 | 0.9766 |   37598 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     20.05 ns |   0.039 ns |   0.033 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     20.16 ns |   0.062 ns |   0.058 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     21.13 ns |   0.045 ns |   0.042 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |    973.89 ns |   2.572 ns |   2.148 ns | 0.0458 |      - |    1456 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  | 17,539.56 ns |  74.339 ns |  62.076 ns | 1.1292 |      - |   35472 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 73,046.22 ns | 109.783 ns |  91.674 ns | 0.1221 |      - |    7500 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,469.91 ns |  36.235 ns |  32.121 ns | 0.1831 | 0.1526 |    5855 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 25,380.93 ns | 175.193 ns | 163.875 ns | 1.2207 | 1.0986 |   38768 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     21.46 ns |   0.103 ns |   0.096 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     20.49 ns |   0.080 ns |   0.075 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     20.37 ns |   0.064 ns |   0.053 ns | 0.0013 |      - |      40 B |
