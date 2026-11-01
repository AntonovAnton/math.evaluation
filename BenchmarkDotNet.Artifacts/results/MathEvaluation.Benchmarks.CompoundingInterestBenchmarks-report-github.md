```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9818)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    781.86 ns |    11.778 ns |    11.017 ns | 0.0458 |      - |    1456 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 |  3,399.46 ns |    40.154 ns |    35.595 ns | 0.1068 |      - |    3416 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 91,113.35 ns | 1,380.190 ns | 1,291.030 ns |      - |      - |    7488 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,447.49 ns |   105.055 ns |    98.269 ns | 0.1831 | 0.1526 |    5951 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 |  8,367.46 ns |    61.007 ns |    50.944 ns | 0.1831 | 0.1221 |    6804 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     23.04 ns |     0.228 ns |     0.202 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     22.60 ns |     0.117 ns |     0.098 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     22.76 ns |     0.477 ns |     0.586 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |  1,233.63 ns |     9.543 ns |     8.926 ns | 0.0458 |      - |    1456 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  |  4,011.96 ns |     8.178 ns |     7.250 ns | 0.1068 |      - |    3416 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 75,925.71 ns | 1,458.902 ns | 1,498.184 ns | 0.1221 |      - |    7500 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,804.74 ns |   135.236 ns |   132.820 ns | 0.1831 | 0.1526 |    5855 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 10,493.09 ns |   151.226 ns |   141.457 ns | 0.1831 | 0.1221 |    6709 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     21.45 ns |     0.331 ns |     0.309 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     21.36 ns |     0.196 ns |     0.164 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     21.55 ns |     0.249 ns |     0.233 ns | 0.0013 |      - |      40 B |
