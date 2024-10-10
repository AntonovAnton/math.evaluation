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
| &#39;MathEvaluator evaluation&#39;            | .NET 6.0 | .NET 6.0 |  1,130.55 ns |    17.670 ns |    16.529 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 6.0 | .NET 6.0 |  8,487.97 ns |    72.387 ns |    64.169 ns | 0.2899 |      - |    3736 B |
| &#39;MathEvaluator compilation&#39;           | .NET 6.0 | .NET 6.0 | 74,502.07 ns |   396.670 ns |   371.046 ns | 0.4883 | 0.2441 |    7549 B |
| &#39;NCalc compilation&#39;                   | .NET 6.0 | .NET 6.0 | 93,812.82 ns | 1,445.593 ns | 1,281.480 ns | 0.6104 | 0.2441 |    8543 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 6.0 | .NET 6.0 |     25.46 ns |     0.104 ns |     0.087 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 6.0 | .NET 6.0 |     25.49 ns |     0.120 ns |     0.112 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    649.44 ns |    12.544 ns |    12.320 ns | 0.1144 |      - |    1440 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  7,186.19 ns |   139.594 ns |   166.176 ns | 0.2899 |      - |    3688 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 67,485.25 ns | 1,186.975 ns | 1,110.297 ns | 0.4883 | 0.3662 |    7548 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 81,352.15 ns | 1,035.397 ns |   968.511 ns | 0.4883 | 0.2441 |    8570 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     25.22 ns |     0.385 ns |     0.360 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     24.95 ns |     0.219 ns |     0.194 ns | 0.0032 |      - |      40 B |
