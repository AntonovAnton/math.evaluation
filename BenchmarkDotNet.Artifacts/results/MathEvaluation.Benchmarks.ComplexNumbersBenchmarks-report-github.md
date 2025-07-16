```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4652/24H2/2024Update/HudsonValley)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.302
  [Host]   : .NET 8.0.18 (8.0.1825.31117), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.18 (8.0.1825.31117), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                           | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|--------------------------------------------------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0 | .NET 8.0 |     683.89 ns |    13.340 ns |    18.260 ns | 0.0458 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0 | .NET 8.0 | 273,185.22 ns | 1,728.166 ns | 1,443.097 ns | 0.4883 |      - |    8913 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 |   7,755.12 ns |    98.794 ns |    87.579 ns | 0.6409 | 0.6104 |    8399 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0 | .NET 8.0 |      69.42 ns |     0.705 ns |     0.660 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      67.20 ns |     1.195 ns |     1.059 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 9.0 | .NET 9.0 |     627.99 ns |     7.105 ns |     6.646 ns | 0.0458 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 9.0 | .NET 9.0 | 252,219.27 ns | 1,242.116 ns |   969.762 ns | 0.4883 |      - |    8913 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 9.0 | .NET 9.0 |   7,592.50 ns |   126.981 ns |   106.034 ns | 0.6714 | 0.6409 |    8432 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 9.0 | .NET 9.0 |      66.47 ns |     0.431 ns |     0.403 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 9.0 | .NET 9.0 |      66.11 ns |     0.731 ns |     0.684 ns | 0.0025 |      - |      32 B |
