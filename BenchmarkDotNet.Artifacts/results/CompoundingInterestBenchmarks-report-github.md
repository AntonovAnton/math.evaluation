```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4249/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 6.0 | .NET 6.0 |  1,123.15 ns |  20.331 ns |  18.023 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 6.0 | .NET 6.0 |  8,567.93 ns |  20.023 ns |  18.730 ns | 0.2899 |      - |    3736 B |
| &#39;MathEvaluator compilation&#39;           | .NET 6.0 | .NET 6.0 | 75,224.48 ns | 411.253 ns | 364.565 ns | 0.4883 | 0.2441 |    7548 B |
| &#39;NCalc compilation&#39;                   | .NET 6.0 | .NET 6.0 | 90,576.45 ns | 674.868 ns | 631.272 ns | 0.6104 | 0.2441 |    8542 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 6.0 | .NET 6.0 |     25.64 ns |   0.201 ns |   0.179 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 6.0 | .NET 6.0 |     25.45 ns |   0.121 ns |   0.101 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    631.23 ns |   3.150 ns |   2.946 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  6,806.79 ns |  27.848 ns |  26.049 ns | 0.2899 |      - |    3688 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 64,485.58 ns | 368.758 ns | 344.937 ns | 0.4883 | 0.3662 |    7548 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 77,866.98 ns | 524.961 ns | 491.049 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     27.85 ns |   0.577 ns |   0.730 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     26.54 ns |   0.241 ns |   0.201 ns | 0.0032 |      - |      40 B |
