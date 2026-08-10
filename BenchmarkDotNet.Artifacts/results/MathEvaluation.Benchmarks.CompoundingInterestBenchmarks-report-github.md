```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8973/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.302
  [Host]    : .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    818.53 ns |     8.349 ns |     7.810 ns | 0.0401 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 | 15,739.85 ns |   160.236 ns |   142.045 ns | 0.9155 |      - |   34192 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 91,952.76 ns | 1,483.017 ns | 1,238.386 ns | 0.2441 |      - |    7635 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,344.57 ns |    58.505 ns |    48.854 ns | 0.1526 | 0.1221 |    6078 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 21,624.84 ns |   423.626 ns |   487.849 ns | 0.9766 |      - |   37583 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     22.61 ns |     0.467 ns |     0.480 ns | 0.0010 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     22.81 ns |     0.147 ns |     0.130 ns | 0.0010 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     22.73 ns |     0.140 ns |     0.124 ns | 0.0010 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |  1,042.35 ns |     3.675 ns |     3.258 ns | 0.0057 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  | 17,796.68 ns |    53.889 ns |    47.771 ns | 0.1221 |      - |   35456 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 72,687.74 ns |   497.313 ns |   465.187 ns |      - |      - |    7696 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,098.53 ns |    42.213 ns |    39.486 ns |      - |      - |    5960 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 27,088.66 ns |   469.462 ns |   392.022 ns |      - |      - |   38728 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     22.91 ns |     0.133 ns |     0.125 ns | 0.0001 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     22.07 ns |     0.107 ns |     0.100 ns | 0.0001 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     22.19 ns |     0.115 ns |     0.102 ns | 0.0001 |      - |      40 B |
