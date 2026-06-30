```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8737/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.301
  [Host]    : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4


```
| Method                                                                           | Job       | Runtime   | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|--------------------------------------------------------------------------------- |---------- |---------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 10.0 | .NET 10.0 |     638.80 ns |     3.157 ns |     2.953 ns | 0.0153 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 10.0 | .NET 10.0 | 405,595.72 ns | 2,645.196 ns | 2,208.859 ns |      - |      - |    8944 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 10.0 | .NET 10.0 |   7,281.28 ns |    98.785 ns |    87.570 ns | 0.2136 | 0.1831 |    8478 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 10.0 | .NET 10.0 |      73.03 ns |     0.338 ns |     0.282 ns | 0.0008 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 10.0 | .NET 10.0 |      73.88 ns |     0.218 ns |     0.182 ns | 0.0008 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0  | .NET 8.0  |     724.95 ns |    11.740 ns |    10.982 ns | 0.0019 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0  | .NET 8.0  | 283,893.16 ns | 3,174.853 ns | 2,814.425 ns |      - |      - |    8944 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0  | .NET 8.0  |   9,245.13 ns |    71.867 ns |    63.709 ns | 0.0305 |      - |    8445 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0  | .NET 8.0  |      72.16 ns |     0.645 ns |     0.603 ns | 0.0001 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0  | .NET 8.0  |      71.64 ns |     0.586 ns |     0.548 ns | 0.0001 |      - |      32 B |
