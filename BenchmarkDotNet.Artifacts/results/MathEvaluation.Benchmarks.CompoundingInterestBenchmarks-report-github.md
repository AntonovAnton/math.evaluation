```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8737/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.301
  [Host]    : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4


```
| Method                                                       | Job       | Runtime   | Mean         | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |---------- |-------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 | .NET 10.0 |    818.11 ns |     6.158 ns |     5.760 ns | 0.0401 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 10.0 | .NET 10.0 |  6,156.36 ns |    71.846 ns |    63.689 ns | 0.0916 |      - |    2808 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | .NET 10.0 | 92,071.15 ns | 1,806.705 ns | 2,473.039 ns |      - |      - |    7616 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 | .NET 10.0 |  5,601.33 ns |   110.729 ns |   143.979 ns | 0.1526 | 0.1221 |    6078 B |
| &#39;NCalc compilation&#39;                                          | .NET 10.0 | .NET 10.0 | 11,232.61 ns |   148.059 ns |   123.636 ns | 0.1221 |      - |    6313 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 | .NET 10.0 |     23.09 ns |     0.209 ns |     0.195 ns | 0.0010 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 | .NET 10.0 |     23.38 ns |     0.285 ns |     0.267 ns | 0.0010 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 10.0 | .NET 10.0 |     23.12 ns |     0.159 ns |     0.133 ns | 0.0010 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 8.0  | .NET 8.0  |  1,090.68 ns |    21.264 ns |    35.528 ns | 0.0057 |      - |    1520 B |
| &#39;NCalc evaluation&#39;                                           | .NET 8.0  | .NET 8.0  |  7,171.54 ns |   143.023 ns |   140.468 ns |      - |      - |    2864 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 8.0  | .NET 8.0  | 76,323.56 ns | 1,491.283 ns | 2,232.083 ns |      - |      - |    7616 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 8.0  | .NET 8.0  |  6,071.85 ns |    15.878 ns |    13.259 ns |      - |      - |    5960 B |
| &#39;NCalc compilation&#39;                                          | .NET 8.0  | .NET 8.0  | 12,783.11 ns |   249.190 ns |   365.259 ns |      - |      - |    6256 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 8.0  | .NET 8.0  |     22.41 ns |     0.381 ns |     0.356 ns | 0.0001 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 8.0  | .NET 8.0  |     22.28 ns |     0.464 ns |     0.587 ns | 0.0001 |      - |      40 B |
| &#39;NCalc invoke fn(P, r, n, d)&#39;                                | .NET 8.0  | .NET 8.0  |     22.77 ns |     0.380 ns |     0.355 ns | 0.0001 |      - |      40 B |
