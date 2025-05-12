```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                       | Job      | Runtime  | Mean         | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |--------- |--------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0 | .NET 8.0 |    662.14 ns |   6.757 ns |   5.990 ns | 0.1173 |      - |    1480 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0 | .NET 8.0 |  5,892.99 ns |  92.609 ns |  86.626 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0 | .NET 8.0 | 66,923.48 ns | 450.310 ns | 399.188 ns | 0.4883 | 0.3662 |    7588 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0 | .NET 8.0 |  6,674.14 ns | 119.870 ns | 112.126 ns | 0.4883 | 0.4578 |    6176 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0 | .NET 8.0 | 78,955.59 ns | 278.384 ns | 260.401 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0 | .NET 8.0 |     18.88 ns |   0.162 ns |   0.135 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     23.47 ns |   0.191 ns |   0.169 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0 | .NET 8.0 |     23.16 ns |   0.289 ns |   0.270 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 9.0 | .NET 9.0 |    629.73 ns |   2.023 ns |   1.689 ns | 0.1173 |      - |    1480 B |
| &#39;NCalc evaluation&#39;                                           | .NET 9.0 | .NET 9.0 |  5,316.02 ns |  16.546 ns |  14.668 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 9.0 | .NET 9.0 | 66,435.87 ns | 323.869 ns | 287.102 ns | 0.4883 | 0.3662 |    7588 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 9.0 | .NET 9.0 |  6,385.72 ns |  54.649 ns |  48.445 ns | 0.4883 | 0.4578 |    6367 B |
| &#39;NCalc compilation&#39;                                          | .NET 9.0 | .NET 9.0 | 77,163.36 ns | 308.572 ns | 288.638 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 9.0 | .NET 9.0 |     18.55 ns |   0.101 ns |   0.079 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 9.0 | .NET 9.0 |     18.87 ns |   0.151 ns |   0.126 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 9.0 | .NET 9.0 |     18.67 ns |   0.162 ns |   0.143 ns | 0.0032 |      - |      40 B |
