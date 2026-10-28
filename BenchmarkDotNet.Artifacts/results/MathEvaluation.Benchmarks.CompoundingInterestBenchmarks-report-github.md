```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9711)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    712.30 ns |   2.268 ns |   2.121 ns | 0.0458 |      - |    1448 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 | 14,149.21 ns |  29.188 ns |  25.875 ns | 1.0834 |      - |   34208 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 89,443.51 ns | 361.012 ns | 337.691 ns | 0.2441 |      - |    7504 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,321.97 ns |  59.776 ns |  52.989 ns | 0.1831 | 0.1526 |    5943 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 20,376.73 ns | 106.300 ns |  99.433 ns | 1.0986 | 0.9766 |   37598 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     20.50 ns |   0.062 ns |   0.058 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     20.65 ns |   0.034 ns |   0.030 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     20.75 ns |   0.103 ns |   0.096 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |  1,012.10 ns |   2.340 ns |   2.075 ns | 0.0458 |      - |    1448 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  | 17,791.00 ns |  29.068 ns |  24.273 ns | 1.1292 |      - |   35472 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 73,199.86 ns | 202.175 ns | 168.825 ns | 0.1221 |      - |    7492 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,600.14 ns |  29.397 ns |  27.498 ns | 0.1831 | 0.1526 |    5847 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 25,824.75 ns | 188.570 ns | 176.389 ns | 1.2207 | 1.0986 |   38768 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     23.01 ns |   0.098 ns |   0.092 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     21.15 ns |   0.043 ns |   0.038 ns | 0.0013 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     20.96 ns |   0.042 ns |   0.037 ns | 0.0013 |      - |      40 B |
