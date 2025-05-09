```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                           | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|--------------------------------------------------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0 | .NET 8.0 |     665.07 ns |     3.366 ns |     3.149 ns | 0.0448 |      - |     568 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0 | .NET 8.0 | 270,155.58 ns | 1,112.893 ns |   986.551 ns | 0.4883 |      - |    9017 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 |   8,243.36 ns |    84.727 ns |    79.254 ns | 0.6714 | 0.6409 |    8783 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0 | .NET 8.0 |      67.32 ns |     0.254 ns |     0.225 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      69.22 ns |     0.192 ns |     0.170 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 9.0 | .NET 9.0 |     674.84 ns |     5.023 ns |     4.453 ns | 0.0448 |      - |     568 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 9.0 | .NET 9.0 | 251,435.81 ns | 1,158.363 ns | 1,083.533 ns | 0.4883 |      - |    8897 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 9.0 | .NET 9.0 |   8,783.37 ns |   102.328 ns |    95.717 ns | 0.7019 | 0.6714 |    8816 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 9.0 | .NET 9.0 |      66.87 ns |     0.203 ns |     0.180 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 9.0 | .NET 9.0 |      66.48 ns |     0.404 ns |     0.338 ns | 0.0025 |      - |      32 B |
