```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9711)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4


```
| Method                                                                           | Job       | Runtime   | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|--------------------------------------------------------------------------------- |---------- |---------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 10.0 | .NET 10.0 |     627.57 ns |     1.387 ns |     1.158 ns | 0.0181 |      - |     576 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 10.0 | .NET 10.0 | 396,265.58 ns | 1,262.161 ns | 1,053.962 ns |      - |      - |    8896 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 10.0 | .NET 10.0 |   6,953.35 ns |    55.259 ns |    51.689 ns | 0.2441 | 0.2136 |    8430 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 10.0 | .NET 10.0 |      70.41 ns |     0.201 ns |     0.188 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 10.0 | .NET 10.0 |      71.98 ns |     0.639 ns |     0.534 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 8.0  | .NET 8.0  |     740.87 ns |     1.561 ns |     1.384 ns | 0.0181 |      - |     576 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 8.0  | .NET 8.0  | 275,341.84 ns | 1,111.846 ns |   985.622 ns |      - |      - |    8896 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 8.0  | .NET 8.0  |   8,433.48 ns |    48.288 ns |    42.806 ns | 0.2441 | 0.2136 |    8398 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 8.0  | .NET 8.0  |      68.19 ns |     0.350 ns |     0.327 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(a)&#39;                              | .NET 8.0  | .NET 8.0  |      67.48 ns |     0.210 ns |     0.186 ns | 0.0010 |      - |      32 B |
