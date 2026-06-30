```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8737/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.301
  [Host]    : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    818.09 ns |     5.346 ns |     5.001 ns | 0.0401 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 |  6,166.59 ns |    45.976 ns |    43.006 ns | 0.0916 |      - |    2808 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 91,272.60 ns | 1,177.700 ns | 1,101.622 ns | 0.2441 |      - |    7635 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,409.35 ns |    32.265 ns |    30.180 ns | 0.1526 | 0.1221 |    6078 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 11,310.71 ns |   143.730 ns |   127.413 ns | 0.1221 |      - |    6313 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     23.75 ns |     0.144 ns |     0.128 ns | 0.0010 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     23.81 ns |     0.127 ns |     0.119 ns | 0.0011 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     23.85 ns |     0.128 ns |     0.119 ns | 0.0010 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |  1,050.77 ns |     7.877 ns |     6.578 ns | 0.0057 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  |  7,211.08 ns |    49.437 ns |    46.243 ns | 0.0076 |      - |    2864 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 75,210.82 ns |   935.866 ns |   875.409 ns |      - |      - |    7616 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,119.82 ns |    89.079 ns |    78.966 ns |      - |      - |    5960 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 12,800.48 ns |   156.020 ns |   138.308 ns |      - |      - |    6256 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     22.12 ns |     0.078 ns |     0.069 ns | 0.0001 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     22.48 ns |     0.175 ns |     0.164 ns | 0.0001 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     22.36 ns |     0.207 ns |     0.173 ns | 0.0001 |      - |      40 B |
