```

BenchmarkDotNet v0.15.7, Windows 11 (10.0.26200.7171/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    614.70 ns |   3.308 ns |   3.095 ns | 0.0486 |      - |    1544 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 |  6,226.65 ns |  17.301 ns |  16.183 ns | 0.1144 |      - |    3792 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 81,417.25 ns | 243.915 ns | 216.224 ns | 0.2441 | 0.1221 |    7650 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,461.26 ns |  26.969 ns |  25.227 ns | 0.1831 | 0.1526 |    6246 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 11,175.55 ns |  46.445 ns |  43.444 ns | 0.2136 | 0.1831 |    6768 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     20.04 ns |   0.126 ns |   0.118 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     20.22 ns |   0.152 ns |   0.142 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     20.06 ns |   0.086 ns |   0.072 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |    737.96 ns |   1.819 ns |   1.701 ns | 0.0486 |      - |    1544 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  |  7,568.48 ns |  37.288 ns |  31.137 ns | 0.1221 |      - |    3824 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 72,955.01 ns | 343.117 ns | 304.164 ns | 0.2441 | 0.1221 |    7650 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,119.11 ns |  20.988 ns |  16.386 ns | 0.1831 | 0.1526 |    5999 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 13,497.52 ns |  59.352 ns |  55.518 ns | 0.1831 | 0.1221 |    6780 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     20.29 ns |   0.062 ns |   0.058 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     20.36 ns |   0.045 ns |   0.037 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     20.34 ns |   0.048 ns |   0.043 ns | 0.0013 |      - |      40 B |
