```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4169/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 6.0 | .NET 6.0 |  1,087.49 ns |     2.770 ns |     2.313 ns | 0.1125 |      - |    1432 B |
| &#39;NCalc evaluation&#39;                    | .NET 6.0 | .NET 6.0 |  8,213.99 ns |    37.791 ns |    35.350 ns | 0.2899 |      - |    3736 B |
| &#39;MathEvaluator compilation&#39;           | .NET 6.0 | .NET 6.0 | 74,871.82 ns |   761.963 ns |   675.460 ns | 0.4883 | 0.2441 |    7540 B |
| &#39;NCalc compilation&#39;                   | .NET 6.0 | .NET 6.0 | 91,595.21 ns | 1,742.793 ns | 1,937.110 ns | 0.6104 | 0.2441 |    8542 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 6.0 | .NET 6.0 |     26.39 ns |     0.539 ns |     0.839 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 6.0 | .NET 6.0 |     26.31 ns |     0.306 ns |     0.286 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    670.03 ns |    12.613 ns |    22.743 ns | 0.1135 |      - |    1432 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  6,739.22 ns |   120.620 ns |   112.828 ns | 0.2899 |      - |    3688 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 67,737.77 ns |   531.536 ns |   497.199 ns | 0.4883 | 0.3662 |    7540 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 82,854.66 ns | 1,099.325 ns | 1,028.309 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     25.84 ns |     0.399 ns |     0.373 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     24.94 ns |     0.183 ns |     0.171 ns | 0.0032 |      - |      40 B |
