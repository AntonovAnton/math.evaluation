```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    759.71 ns |   2.368 ns |   2.099 ns | 0.0401 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 |  6,593.23 ns |  20.077 ns |  18.780 ns | 0.0992 |      - |    3832 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 79,953.47 ns | 411.320 ns | 384.749 ns | 0.1221 |      - |    7630 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,169.86 ns |  99.149 ns |  92.744 ns | 0.1526 | 0.1221 |    6078 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 11,148.28 ns |  72.652 ns |  60.668 ns | 0.1221 |      - |    6736 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     23.18 ns |   0.100 ns |   0.094 ns | 0.0010 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     23.34 ns |   0.085 ns |   0.075 ns | 0.0010 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     23.44 ns |   0.123 ns |   0.109 ns | 0.0010 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |  1,043.10 ns |   3.831 ns |   3.583 ns | 0.0057 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  |  7,808.09 ns |  41.072 ns |  34.297 ns | 0.0153 |      - |    3864 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 73,051.03 ns | 293.866 ns | 245.392 ns |      - |      - |    7616 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,023.63 ns |  34.993 ns |  31.021 ns |      - |      - |    5960 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 12,884.67 ns | 111.707 ns |  99.025 ns |      - |      - |    6656 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     21.73 ns |   0.063 ns |   0.059 ns | 0.0001 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     21.65 ns |   0.158 ns |   0.140 ns | 0.0001 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     21.98 ns |   0.077 ns |   0.072 ns | 0.0001 |      - |      40 B |
