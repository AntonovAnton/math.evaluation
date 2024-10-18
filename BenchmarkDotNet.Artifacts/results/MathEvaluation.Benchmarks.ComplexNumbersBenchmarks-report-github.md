```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.403
  [Host]   : .NET 6.0.35 (6.0.3524.45918), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.35 (6.0.3524.45918), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                    | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Allocated |
|---------------------------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;  | .NET 6.0 | .NET 6.0 |     795.20 ns |     7.187 ns |     6.722 ns | 0.0439 |     552 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 6.0 | .NET 6.0 | 255,247.64 ns | 1,470.631 ns | 1,375.629 ns | 0.4883 |    8881 B |
| &#39;MathEvaluator invoke fn(a)&#39;                              | .NET 6.0 | .NET 6.0 |      67.94 ns |     0.209 ns |     0.195 ns | 0.0025 |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;  | .NET 8.0 | .NET 8.0 |     627.77 ns |     5.963 ns |     5.578 ns | 0.0439 |     552 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 | 263,693.22 ns |   368.845 ns |   287.970 ns | 0.4883 |    8881 B |
| &#39;MathEvaluator invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      73.88 ns |     0.242 ns |     0.214 ns | 0.0025 |      32 B |
