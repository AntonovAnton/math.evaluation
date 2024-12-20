```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2605)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.101
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                    | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Allocated |
|---------------------------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;  | .NET 8.0 | .NET 8.0 |     649.27 ns |     4.214 ns |     3.941 ns | 0.0439 |     552 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 | 272,715.71 ns | 3,094.186 ns | 2,894.303 ns | 0.4883 |    8881 B |
| &#39;MathEvaluator invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      68.21 ns |     0.253 ns |     0.224 ns | 0.0025 |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;  | .NET 9.0 | .NET 9.0 |     594.06 ns |     3.031 ns |     2.687 ns | 0.0439 |     552 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 9.0 | .NET 9.0 | 252,173.57 ns | 1,625.641 ns | 1,520.625 ns | 0.4883 |    8881 B |
| &#39;MathEvaluator invoke fn(a)&#39;                              | .NET 9.0 | .NET 9.0 |      69.09 ns |     0.440 ns |     0.390 ns | 0.0025 |      32 B |
