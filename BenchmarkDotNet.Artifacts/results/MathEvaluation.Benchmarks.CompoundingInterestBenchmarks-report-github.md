```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                       | Job      | Runtime  | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |--------- |--------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0 | .NET 8.0 |    638.37 ns |     6.946 ns |     6.157 ns | 0.1173 |      - |    1480 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0 | .NET 8.0 |  5,781.15 ns |    38.208 ns |    33.871 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0 | .NET 8.0 | 66,817.54 ns |   274.011 ns |   242.904 ns | 0.4883 | 0.3662 |    7588 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0 | .NET 8.0 |  6,452.89 ns |    69.488 ns |    65.000 ns | 0.4883 | 0.4578 |    6216 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0 | .NET 8.0 | 79,083.57 ns |   649.757 ns |   575.993 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0 | .NET 8.0 |     21.76 ns |     0.177 ns |     0.166 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     18.65 ns |     0.130 ns |     0.122 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0 | .NET 8.0 |     18.83 ns |     0.163 ns |     0.144 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 9.0 | .NET 9.0 |    617.94 ns |     1.391 ns |     1.161 ns | 0.1173 |      - |    1480 B |
| &#39;NCalc evaluation&#39;                                           | .NET 9.0 | .NET 9.0 |  5,122.37 ns |    14.671 ns |    13.005 ns | 0.2899 |      - |    3712 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 9.0 | .NET 9.0 | 77,768.83 ns | 1,281.369 ns | 1,198.593 ns | 0.6104 | 0.4883 |    7672 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 9.0 | .NET 9.0 |  6,399.03 ns |   119.893 ns |   117.751 ns | 0.4883 | 0.4578 |    6415 B |
| &#39;NCalc compilation&#39;                                          | .NET 9.0 | .NET 9.0 | 77,480.59 ns |   491.415 ns |   459.670 ns | 0.4883 | 0.2441 |    8490 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 9.0 | .NET 9.0 |     18.40 ns |     0.157 ns |     0.139 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 9.0 | .NET 9.0 |     18.50 ns |     0.109 ns |     0.102 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 9.0 | .NET 9.0 |     18.50 ns |     0.196 ns |     0.184 ns | 0.0032 |      - |      40 B |
