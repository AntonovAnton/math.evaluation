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
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0 | .NET 8.0 |     652.10 ns |     2.005 ns |     1.778 ns | 0.0448 |      - |     568 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0 | .NET 8.0 | 270,499.46 ns | 1,575.101 ns | 1,473.351 ns | 0.4883 |      - |    8897 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 |   8,338.47 ns |    39.324 ns |    32.837 ns | 0.6714 | 0.6409 |    8783 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0 | .NET 8.0 |      66.60 ns |     0.134 ns |     0.112 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      67.39 ns |     0.815 ns |     0.762 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 9.0 | .NET 9.0 |     631.44 ns |     2.443 ns |     2.166 ns | 0.0448 |      - |     568 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 9.0 | .NET 9.0 | 250,495.00 ns | 1,229.603 ns | 1,150.171 ns | 0.4883 |      - |    8897 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 9.0 | .NET 9.0 |   8,369.39 ns |    64.293 ns |    60.139 ns | 0.7019 | 0.6714 |    8816 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 9.0 | .NET 9.0 |      67.65 ns |     0.170 ns |     0.142 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 9.0 | .NET 9.0 |      72.12 ns |     0.192 ns |     0.160 ns | 0.0025 |      - |      32 B |
