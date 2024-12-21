```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2605)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.101
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    624.85 ns |     5.290 ns |     4.949 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  5,896.55 ns |    24.179 ns |    22.617 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 67,282.40 ns | 1,238.889 ns | 1,158.858 ns | 0.4883 | 0.3662 |    7548 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 80,610.20 ns |   561.996 ns |   469.292 ns | 0.4883 | 0.2441 |    8609 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     20.13 ns |     0.178 ns |     0.158 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     20.22 ns |     0.312 ns |     0.261 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 9.0 | .NET 9.0 |    610.75 ns |     8.237 ns |     7.705 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 9.0 | .NET 9.0 |  5,448.49 ns |    32.042 ns |    29.972 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;           | .NET 9.0 | .NET 9.0 | 67,473.22 ns |   239.541 ns |   212.347 ns | 0.4883 | 0.3662 |    7548 B |
| &#39;NCalc compilation&#39;                   | .NET 9.0 | .NET 9.0 | 78,543.08 ns |   901.843 ns |   843.584 ns | 0.4883 | 0.2441 |    8602 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 9.0 | .NET 9.0 |     19.91 ns |     0.175 ns |     0.146 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 9.0 | .NET 9.0 |     19.72 ns |     0.155 ns |     0.145 ns | 0.0032 |      - |      40 B |
