```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.403
  [Host]   : .NET 6.0.35 (6.0.3524.45918), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.35 (6.0.3524.45918), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 6.0 | .NET 6.0 |  1,093.19 ns |   6.103 ns |   5.709 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 6.0 | .NET 6.0 |  8,574.34 ns |  31.496 ns |  27.920 ns | 0.2899 |      - |    3736 B |
| &#39;MathEvaluator compilation&#39;           | .NET 6.0 | .NET 6.0 | 73,472.16 ns | 409.495 ns | 363.006 ns | 0.4883 | 0.2441 |    7549 B |
| &#39;NCalc compilation&#39;                   | .NET 6.0 | .NET 6.0 | 89,164.89 ns | 507.531 ns | 423.811 ns | 0.6104 | 0.2441 |    8543 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 6.0 | .NET 6.0 |     25.96 ns |   0.217 ns |   0.182 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 6.0 | .NET 6.0 |     26.82 ns |   0.262 ns |   0.245 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    630.42 ns |   3.724 ns |   3.301 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  6,676.70 ns |  26.207 ns |  23.232 ns | 0.2899 |      - |    3688 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 64,583.80 ns | 224.451 ns | 198.970 ns | 0.4883 | 0.3662 |    7548 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 78,502.34 ns | 594.027 ns | 555.653 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     24.93 ns |   0.170 ns |   0.142 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     25.75 ns |   0.157 ns |   0.147 ns | 0.0032 |      - |      40 B |
