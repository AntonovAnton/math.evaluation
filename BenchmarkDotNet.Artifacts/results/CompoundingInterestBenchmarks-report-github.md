```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4249/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                | Job      | Runtime  | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|-------------------------------------- |--------- |--------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;            | .NET 6.0 | .NET 6.0 |  1,122.61 ns |     3.980 ns |     3.528 ns | 0.1163 |      - |    1472 B |
| &#39;NCalc evaluation&#39;                    | .NET 6.0 | .NET 6.0 |  8,589.07 ns |    26.480 ns |    23.474 ns | 0.2899 |      - |    3736 B |
| &#39;MathEvaluator compilation&#39;           | .NET 6.0 | .NET 6.0 | 72,645.90 ns |   385.938 ns |   361.007 ns | 0.4883 | 0.2441 |    7580 B |
| &#39;NCalc compilation&#39;                   | .NET 6.0 | .NET 6.0 | 89,795.21 ns | 1,413.747 ns | 1,322.420 ns | 0.6104 | 0.2441 |    8542 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 6.0 | .NET 6.0 |     25.68 ns |     0.263 ns |     0.233 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 6.0 | .NET 6.0 |     25.71 ns |     0.280 ns |     0.262 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;            | .NET 8.0 | .NET 8.0 |    700.77 ns |    13.926 ns |    19.973 ns | 0.1173 |      - |    1472 B |
| &#39;NCalc evaluation&#39;                    | .NET 8.0 | .NET 8.0 |  7,111.84 ns |   134.107 ns |   125.444 ns | 0.2899 |      - |    3688 B |
| &#39;MathEvaluator compilation&#39;           | .NET 8.0 | .NET 8.0 | 70,347.74 ns | 1,366.943 ns | 1,342.521 ns | 0.4883 | 0.3662 |    7580 B |
| &#39;NCalc compilation&#39;                   | .NET 8.0 | .NET 8.0 | 83,434.31 ns | 1,287.808 ns | 1,204.616 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     26.63 ns |     0.546 ns |     0.650 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;         | .NET 8.0 | .NET 8.0 |     26.22 ns |     0.549 ns |     0.610 ns | 0.0032 |      - |      40 B |
