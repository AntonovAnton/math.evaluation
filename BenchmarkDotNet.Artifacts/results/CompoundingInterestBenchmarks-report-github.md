```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 6.0 | .NET 6.0 |  1,133.33 ns |    14.576 ns |    13.634 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 6.0 | .NET 6.0 |  8,709.33 ns |   149.912 ns |   140.228 ns | 0.2899 |      - |    3736 B |
| &#39;MathEvaluator compilation&#39;           | .NET 6.0 | .NET 6.0 | 76,424.34 ns | 1,502.644 ns | 1,405.574 ns | 0.4883 | 0.2441 |    7549 B |
| &#39;NCalc compilation&#39;                   | .NET 6.0 | .NET 6.0 | 92,980.87 ns | 1,657.105 ns | 1,550.057 ns | 0.6104 | 0.2441 |    8543 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 6.0 | .NET 6.0 |     26.85 ns |     0.339 ns |     0.317 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 6.0 | .NET 6.0 |     26.67 ns |     0.538 ns |     0.504 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    693.46 ns |    13.280 ns |    14.761 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  6,886.27 ns |    17.490 ns |    13.655 ns | 0.2899 |      - |    3688 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 66,121.53 ns |   431.290 ns |   360.147 ns | 0.4883 | 0.3662 |    7548 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 83,097.44 ns | 1,407.705 ns | 1,830.414 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     27.53 ns |     0.573 ns |     1.117 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     26.26 ns |     0.521 ns |     0.940 ns | 0.0032 |      - |      40 B |
