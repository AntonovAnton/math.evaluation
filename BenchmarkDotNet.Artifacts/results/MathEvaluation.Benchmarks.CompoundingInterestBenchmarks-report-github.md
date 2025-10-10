```

BenchmarkDotNet v0.15.4, Windows 11 (10.0.26100.6725/24H2/2024Update/HudsonValley)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.305
  [Host]   : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v4
  .NET 8.0 : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v4
  .NET 9.0 : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job      | Runtime  | Mean         | Error      | StdDev     | Median       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |--------- |--------- |-------------:|-----------:|-----------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0 | .NET 8.0 |    655.99 ns |  10.940 ns |  11.235 ns |    651.10 ns | 0.1192 |      - |    1496 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0 | .NET 8.0 |  5,867.19 ns | 104.311 ns |  97.572 ns |  5,847.13 ns | 0.2975 |      - |    3824 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0 | .NET 8.0 | 68,031.70 ns | 452.346 ns | 400.993 ns | 67,993.45 ns | 0.4883 | 0.3662 |    7604 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0 | .NET 8.0 |  6,377.11 ns |  62.833 ns |  52.468 ns |  6,368.42 ns | 0.4578 | 0.4272 |    5951 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0 | .NET 8.0 | 11,622.67 ns |  93.843 ns |  83.189 ns | 11,620.04 ns | 0.4883 | 0.3662 |    6702 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0 | .NET 8.0 |     20.03 ns |   0.349 ns |   0.309 ns |     19.94 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     18.94 ns |   0.344 ns |   0.322 ns |     18.96 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0 | .NET 8.0 |     20.28 ns |   0.428 ns |   0.458 ns |     20.36 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 9.0 | .NET 9.0 |    689.84 ns |  12.514 ns |  11.706 ns |    689.27 ns | 0.1192 |      - |    1496 B |
| &#39;NCalc evaluation&#39;                                           | .NET 9.0 | .NET 9.0 |  5,201.47 ns |  62.304 ns |  55.231 ns |  5,201.50 ns | 0.2975 |      - |    3792 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 9.0 | .NET 9.0 | 67,973.68 ns | 862.795 ns | 764.845 ns | 67,741.08 ns | 0.4883 | 0.3662 |    7604 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 9.0 | .NET 9.0 |  6,392.26 ns | 108.341 ns |  90.470 ns |  6,430.34 ns | 0.4807 | 0.4730 |    6032 B |
| &#39;NCalc compilation&#39;                                          | .NET 9.0 | .NET 9.0 | 11,260.69 ns | 221.065 ns | 350.633 ns | 11,245.12 ns | 0.4883 | 0.3662 |    6750 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 9.0 | .NET 9.0 |     19.91 ns |   0.413 ns |   1.139 ns |     19.65 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 9.0 | .NET 9.0 |     19.04 ns |   0.399 ns |   0.610 ns |     18.99 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 9.0 | .NET 9.0 |     18.89 ns |   0.397 ns |   0.607 ns |     18.57 ns | 0.0032 |      - |      40 B |
