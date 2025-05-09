```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error        | StdDev     | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-------------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    674.27 ns |     3.385 ns |   2.827 ns | 0.1173 |      - |    1480 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  5,725.44 ns |    18.047 ns |  16.882 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 66,977.80 ns | 1,051.822 ns | 983.875 ns | 0.4883 | 0.3662 |    7588 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 78,899.96 ns |   315.774 ns | 295.375 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     18.72 ns |     0.137 ns |   0.115 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     19.75 ns |     0.082 ns |   0.073 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 9.0 | .NET 9.0 |    626.42 ns |     9.662 ns |   9.038 ns | 0.1173 |      - |    1480 B |
| &#39;NCalc evaluation&#39;                    | .NET 9.0 | .NET 9.0 |  5,334.57 ns |    16.385 ns |  15.327 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;           | .NET 9.0 | .NET 9.0 | 76,272.65 ns |   986.261 ns | 922.549 ns | 0.6104 | 0.4883 |    7672 B |
| &#39;NCalc compilation&#39;                   | .NET 9.0 | .NET 9.0 | 77,252.61 ns |   477.973 ns | 447.096 ns | 0.4883 | 0.2441 |    8609 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 9.0 | .NET 9.0 |     18.64 ns |     0.267 ns |   0.249 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 9.0 | .NET 9.0 |     18.73 ns |     0.066 ns |   0.059 ns | 0.0032 |      - |      40 B |
