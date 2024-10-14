```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                    | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Allocated |
|---------------------------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;  | .NET 6.0 | .NET 6.0 |     783.43 ns |     3.676 ns |     3.439 ns | 0.0439 |     552 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 6.0 | .NET 6.0 | 253,362.77 ns | 2,328.577 ns | 2,064.222 ns | 0.4883 |    8881 B |
| &#39;MathEvaluator invoke fn(a)&#39;                              | .NET 6.0 | .NET 6.0 |      68.08 ns |     0.270 ns |     0.240 ns | 0.0025 |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;  | .NET 8.0 | .NET 8.0 |     640.69 ns |     1.868 ns |     1.748 ns | 0.0439 |     552 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 | 264,744.33 ns |   809.450 ns |   717.556 ns | 0.4883 |    8881 B |
| &#39;MathEvaluator invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      73.83 ns |     0.754 ns |     0.705 ns | 0.0025 |      32 B |
