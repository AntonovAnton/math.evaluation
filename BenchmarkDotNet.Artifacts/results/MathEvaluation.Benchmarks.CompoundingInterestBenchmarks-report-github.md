```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    666.67 ns |     3.176 ns |     2.971 ns | 0.1173 |      - |    1480 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  5,757.15 ns |    25.249 ns |    22.383 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 66,249.19 ns |   334.384 ns |   312.783 ns | 0.4883 | 0.3662 |    7588 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 78,128.90 ns |   381.768 ns |   357.106 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     18.65 ns |     0.048 ns |     0.043 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     18.61 ns |     0.044 ns |     0.039 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 9.0 | .NET 9.0 |    651.44 ns |     2.894 ns |     2.565 ns | 0.1173 |      - |    1480 B |
| &#39;NCalc evaluation&#39;                    | .NET 9.0 | .NET 9.0 |  5,316.35 ns |     7.696 ns |     6.009 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;           | .NET 9.0 | .NET 9.0 | 66,950.83 ns | 1,233.231 ns | 1,153.565 ns | 0.4883 | 0.3662 |    7588 B |
| &#39;NCalc compilation&#39;                   | .NET 9.0 | .NET 9.0 | 76,895.99 ns |   318.232 ns |   282.105 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 9.0 | .NET 9.0 |     18.42 ns |     0.068 ns |     0.060 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 9.0 | .NET 9.0 |     18.40 ns |     0.060 ns |     0.050 ns | 0.0032 |      - |      40 B |
