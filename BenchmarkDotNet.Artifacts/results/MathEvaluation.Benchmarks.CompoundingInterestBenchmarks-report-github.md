```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3775)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    640.96 ns |   4.615 ns |   3.854 ns | 0.1173 |      - |    1472 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  6,051.73 ns |  34.193 ns |  30.311 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 67,182.06 ns | 939.964 ns | 879.243 ns | 0.4883 | 0.3662 |    7580 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 79,406.45 ns | 941.967 ns | 835.029 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     18.93 ns |   0.333 ns |   0.278 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     18.96 ns |   0.399 ns |   0.459 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 9.0 | .NET 9.0 |    613.69 ns |  11.677 ns |  10.923 ns | 0.1173 |      - |    1472 B |
| &#39;NCalc evaluation&#39;                    | .NET 9.0 | .NET 9.0 |  5,482.00 ns | 104.631 ns | 102.762 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;           | .NET 9.0 | .NET 9.0 | 65,197.16 ns | 538.062 ns | 503.304 ns | 0.6104 | 0.1221 |    7660 B |
| &#39;NCalc compilation&#39;                   | .NET 9.0 | .NET 9.0 | 76,261.55 ns | 636.939 ns | 595.793 ns | 0.4883 | 0.2441 |    8486 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 9.0 | .NET 9.0 |     18.72 ns |   0.287 ns |   0.255 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 9.0 | .NET 9.0 |     19.00 ns |   0.121 ns |   0.107 ns | 0.0032 |      - |      40 B |
