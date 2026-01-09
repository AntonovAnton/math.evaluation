```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error        | StdDev       | Median       | Gen0   | Gen1   | Gen2   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-------------:|-------------:|-------------:|-------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |  1,088.07 ns |     6.315 ns |     5.907 ns |  1,090.64 ns | 0.0706 |      - |      - |    2704 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 |  6,669.19 ns |    28.087 ns |    26.273 ns |  6,664.24 ns | 0.0992 |      - |      - |    3832 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 82,576.85 ns | 1,103.442 ns |   978.172 ns | 82,583.49 ns | 0.1221 |      - |      - |    8878 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,957.70 ns |   112.887 ns |   110.870 ns |  5,965.51 ns | 0.2060 | 0.1068 | 0.0076 |    7329 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 11,540.66 ns |   215.884 ns |   212.027 ns | 11,498.60 ns | 0.1221 |      - |      - |    6736 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     23.57 ns |     0.490 ns |     0.979 ns |     23.20 ns | 0.0010 |      - |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     22.96 ns |     0.157 ns |     0.123 ns |     22.97 ns | 0.0010 |      - |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     23.41 ns |     0.230 ns |     0.215 ns |     23.48 ns | 0.0010 |      - |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |  1,310.77 ns |    24.836 ns |    27.605 ns |  1,300.82 ns | 0.0114 |      - |      - |    2904 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  |  8,758.88 ns |   155.769 ns |   145.706 ns |  8,739.52 ns | 0.0153 |      - |      - |    3864 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 76,436.11 ns | 1,460.481 ns | 1,681.892 ns | 76,345.40 ns |      - |      - |      - |    8993 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  7,219.54 ns |   139.117 ns |   136.632 ns |  7,242.04 ns | 0.0229 | 0.0153 |      - |    7354 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 13,658.20 ns |   241.347 ns |   201.535 ns | 13,653.11 ns |      - |      - |      - |    6656 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     22.40 ns |     0.469 ns |     0.610 ns |     22.29 ns | 0.0001 |      - |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     22.92 ns |     0.472 ns |     0.597 ns |     22.67 ns | 0.0001 |      - |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     22.72 ns |     0.311 ns |     0.291 ns |     22.66 ns | 0.0001 |      - |      - |      40 B |
