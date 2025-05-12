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
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0 | .NET 8.0 |     682.82 ns |    11.145 ns |    10.425 ns | 0.0448 |      - |     568 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0 | .NET 8.0 | 271,540.59 ns | 1,313.003 ns | 1,096.417 ns | 0.4883 |      - |    8897 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0 | .NET 8.0 |   8,697.29 ns |    77.322 ns |    72.327 ns | 0.6714 | 0.6409 |    8743 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0 | .NET 8.0 |      69.07 ns |     1.084 ns |     1.014 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0 | .NET 8.0 |      69.29 ns |     0.911 ns |     0.807 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 9.0 | .NET 9.0 |     634.02 ns |    12.285 ns |    11.492 ns | 0.0448 |      - |     568 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 9.0 | .NET 9.0 | 251,712.95 ns |   948.302 ns |   887.042 ns | 0.4883 |      - |    8897 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 9.0 | .NET 9.0 |   8,247.56 ns |   154.996 ns |   144.983 ns | 0.6714 | 0.6104 |    8775 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 9.0 | .NET 9.0 |      70.39 ns |     0.825 ns |     0.772 ns | 0.0025 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 9.0 | .NET 9.0 |      69.24 ns |     1.128 ns |     1.055 ns | 0.0025 |      - |      32 B |
