```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4112/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 6.0 | .NET 6.0 |   1,208.48 ns |     8.700 ns |     8.138 ns | 0.1125 |      - |    1432 B |
| &#39;NCalc evaluation&#39;                    | .NET 6.0 | .NET 6.0 |   7,906.47 ns |    44.201 ns |    39.183 ns | 0.2899 |      - |    3736 B |
| &#39;MathEvaluator compilation&#39;           | .NET 6.0 | .NET 6.0 |  98,274.76 ns | 1,038.659 ns |   920.744 ns | 0.6104 | 0.2441 |    7889 B |
| &#39;NCalc compilation&#39;                   | .NET 6.0 | .NET 6.0 |  88,786.94 ns |   820.848 ns |   767.822 ns | 0.6104 | 0.2441 |    8623 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 6.0 | .NET 6.0 |      29.83 ns |     0.182 ns |     0.170 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 6.0 | .NET 6.0 |      25.86 ns |     0.442 ns |     0.392 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |     724.03 ns |    14.142 ns |    24.014 ns | 0.1135 |      - |    1432 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |   6,400.82 ns |    82.756 ns |    77.410 ns | 0.2899 |      - |    3688 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 107,224.06 ns | 1,770.542 ns | 1,656.166 ns | 0.4883 | 0.2441 |    7880 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 |  79,003.06 ns |   645.997 ns |   604.266 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |      26.68 ns |     0.266 ns |     0.236 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |      26.38 ns |     0.527 ns |     0.517 ns | 0.0032 |      - |      40 B |
