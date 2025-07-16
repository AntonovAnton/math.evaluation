```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4652/24H2/2024Update/HudsonValley)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.302
  [Host]   : .NET 8.0.18 (8.0.1825.31117), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.18 (8.0.1825.31117), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                       | Job      | Runtime  | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |--------- |--------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0 | .NET 8.0 |    685.62 ns |    13.549 ns |    13.307 ns | 0.1192 |      - |    1496 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0 | .NET 8.0 |  5,928.96 ns |    89.929 ns |    84.120 ns | 0.2899 |      - |    3720 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0 | .NET 8.0 | 69,045.99 ns | 1,360.739 ns | 1,567.029 ns | 0.4883 | 0.3662 |    7604 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0 | .NET 8.0 |  6,485.69 ns |   120.756 ns |   188.003 ns | 0.4578 | 0.4272 |    5951 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0 | .NET 8.0 | 11,834.34 ns |   235.246 ns |   393.044 ns | 0.4883 | 0.3662 |    6598 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0 | .NET 8.0 |     18.96 ns |     0.396 ns |     0.616 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0 | .NET 8.0 |     20.50 ns |     0.429 ns |     0.837 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0 | .NET 8.0 |     19.21 ns |     0.404 ns |     0.718 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 9.0 | .NET 9.0 |    628.41 ns |     9.981 ns |     9.336 ns | 0.1192 |      - |    1496 B |
| &#39;NCalc evaluation&#39;                                           | .NET 9.0 | .NET 9.0 |  5,283.43 ns |    39.829 ns |    37.256 ns | 0.2899 |      - |    3720 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 9.0 | .NET 9.0 | 67,708.34 ns |   704.663 ns |   588.425 ns | 0.4883 | 0.3662 |    7604 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 9.0 | .NET 9.0 |  6,220.87 ns |   120.698 ns |   138.996 ns | 0.4578 | 0.4272 |    6031 B |
| &#39;NCalc compilation&#39;                                          | .NET 9.0 | .NET 9.0 | 10,770.21 ns |    67.295 ns |    66.092 ns | 0.4883 | 0.3662 |    6678 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 9.0 | .NET 9.0 |     18.33 ns |     0.182 ns |     0.142 ns | 0.0032 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 9.0 | .NET 9.0 |     18.53 ns |     0.175 ns |     0.146 ns | 0.0032 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 9.0 | .NET 9.0 |     20.26 ns |     0.429 ns |     0.905 ns | 0.0032 |      - |      40 B |
