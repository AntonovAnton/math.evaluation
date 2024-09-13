```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4169/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean          | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |--------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 6.0 | .NET 6.0 |   1,099.43 ns |   5.065 ns |   4.490 ns | 0.1144 |      - |    1456 B |
| &#39;NCalc evaluation&#39;                    | .NET 6.0 | .NET 6.0 |   7,982.50 ns |  25.045 ns |  22.201 ns | 0.2899 |      - |    3736 B |
| &#39;MathEvaluator compilation&#39;           | .NET 6.0 | .NET 6.0 |  99,542.35 ns | 881.475 ns | 824.532 ns | 0.6104 | 0.2441 |    8175 B |
| &#39;NCalc compilation&#39;                   | .NET 6.0 | .NET 6.0 |  90,209.09 ns | 712.158 ns | 666.153 ns | 0.6104 | 0.2441 |    8622 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 6.0 | .NET 6.0 |      28.17 ns |   0.168 ns |   0.149 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 6.0 | .NET 6.0 |      25.55 ns |   0.377 ns |   0.315 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |     630.90 ns |   4.065 ns |   3.603 ns | 0.1154 |      - |    1456 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |   6,421.98 ns |  15.880 ns |  14.854 ns | 0.2899 |      - |    3688 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 107,219.82 ns | 638.952 ns | 597.676 ns | 0.4883 | 0.2441 |    8247 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 |  78,820.86 ns | 576.807 ns | 539.546 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |      27.25 ns |   0.111 ns |   0.098 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |      24.71 ns |   0.070 ns |   0.062 ns | 0.0032 |      - |      40 B |
