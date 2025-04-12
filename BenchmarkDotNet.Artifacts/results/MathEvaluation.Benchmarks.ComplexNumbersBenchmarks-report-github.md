```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3775)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                    | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Allocated |
|---------------------------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;  | .NET 8.0 | .NET 8.0 |     652.79 ns |     2.211 ns |     1.847 ns | 0.0439 |     560 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 | 268,092.59 ns | 1,783.644 ns | 1,668.422 ns | 0.4883 |    8889 B |
| &#39;MathEvaluator invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      67.77 ns |     0.178 ns |     0.167 ns | 0.0025 |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;  | .NET 9.0 | .NET 9.0 |     633.22 ns |     3.411 ns |     3.024 ns | 0.0439 |     560 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 9.0 | .NET 9.0 | 249,132.71 ns | 1,391.729 ns | 1,301.824 ns | 0.4883 |    8889 B |
| &#39;MathEvaluator invoke fn(a)&#39;                              | .NET 9.0 | .NET 9.0 |      67.10 ns |     0.258 ns |     0.229 ns | 0.0025 |      32 B |
