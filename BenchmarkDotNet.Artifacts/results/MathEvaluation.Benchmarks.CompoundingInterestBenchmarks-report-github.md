```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9550/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    699.80 ns |   5.024 ns |   4.699 ns | 0.0486 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 | 14,067.08 ns |  40.078 ns |  35.528 ns | 1.0986 |      - |   34208 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 88,780.44 ns | 629.728 ns | 525.851 ns | 0.2441 |      - |    7640 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,254.46 ns |  28.026 ns |  23.403 ns | 0.1831 | 0.1526 |    6079 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 20,512.57 ns | 126.779 ns | 118.589 ns | 1.0986 | 0.9766 |   37598 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     20.42 ns |   0.053 ns |   0.042 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     20.56 ns |   0.087 ns |   0.081 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     20.54 ns |   0.083 ns |   0.078 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |  1,053.39 ns |   6.652 ns |   5.897 ns | 0.0477 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  | 17,701.62 ns |  48.625 ns |  37.963 ns | 1.1292 |      - |   35472 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 73,297.20 ns | 532.310 ns | 497.923 ns | 0.2441 | 0.1221 |    7634 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,512.36 ns |  20.510 ns |  17.127 ns | 0.1831 | 0.1526 |    5983 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 25,808.74 ns | 116.658 ns | 103.414 ns | 1.2207 | 1.0986 |   38768 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     21.72 ns |   0.176 ns |   0.156 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     21.08 ns |   0.172 ns |   0.161 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     21.00 ns |   0.153 ns |   0.143 ns | 0.0013 |      - |      40 B |
