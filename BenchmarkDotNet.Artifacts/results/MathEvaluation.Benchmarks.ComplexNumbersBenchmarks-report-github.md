```

BenchmarkDotNet v0.15.4, Windows 11 (10.0.26100.6725/24H2/2024Update/HudsonValley)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.305
  [Host]   : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v4
  .NET 8.0 : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v4
  .NET 9.0 : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v4


```
| Method                                                                           | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|--------------------------------------------------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0 | .NET 8.0 |     672.93 ns |     6.418 ns |     5.689 ns | 0.0458 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0 | .NET 8.0 | 279,882.31 ns | 2,508.552 ns | 2,346.501 ns | 0.4883 |      - |    8913 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 |   8,272.67 ns |   163.555 ns |   307.196 ns | 0.6409 | 0.6104 |    8399 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0 | .NET 8.0 |      69.96 ns |     1.390 ns |     1.365 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      67.35 ns |     0.741 ns |     0.693 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 9.0 | .NET 9.0 |     621.90 ns |     2.462 ns |     2.183 ns | 0.0458 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 9.0 | .NET 9.0 | 252,666.16 ns | 1,087.340 ns | 1,017.099 ns | 0.4883 |      - |    8913 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 9.0 | .NET 9.0 |   7,516.17 ns |    34.479 ns |    30.565 ns | 0.6714 | 0.6409 |    8432 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 9.0 | .NET 9.0 |      70.25 ns |     1.112 ns |     0.986 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 9.0 | .NET 9.0 |      70.48 ns |     1.434 ns |     2.010 ns | 0.0025 |      - |      32 B |
