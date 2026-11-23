```

BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.10075)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
Memory: 31.77 GB Total, 15.97 GB Available
.NET SDK 11.0.100
  [Host]     : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4
  Job-WNYEYB : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  Job-ZJTCNP : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4

IterationCount=10  WarmupCount=3  

```
| Method                                                                           | Runtime   | Mean          | Error         | StdDev       | Gen0   | Gen1   | Allocated |
|--------------------------------------------------------------------------------- |---------- |--------------:|--------------:|-------------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 10.0 |     613.34 ns |     10.646 ns |     6.335 ns | 0.0181 |      - |     584 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 10.0 | 404,524.75 ns |  2,518.406 ns | 1,498.663 ns |      - |      - |    8904 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 10.0 |   7,471.27 ns |    226.933 ns |   135.044 ns | 0.2594 | 0.2441 |    8439 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 10.0 |      66.69 ns |      1.165 ns |     0.770 ns | 0.0010 |      - |      32 B |
| &#39;MathEvaluator evaluation: sin(a) * arctan(4i)/(1 - 6i)&#39;                         | .NET 11.0 |     594.85 ns |      7.361 ns |     4.869 ns | 0.0191 |      - |     608 B |
| &#39;MathEvaluator compilation: sin(a) * arctan(4i)/(1 - 6i)&#39;                        | .NET 11.0 | 849,431.26 ns | 10,748.238 ns | 7,109.294 ns |      - |      - |    8928 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation: sin(a) * arctan(4i)/(1 - 6i)&#39; | .NET 11.0 |   7,220.31 ns |    200.472 ns |   132.600 ns | 0.2747 | 0.2670 |    8648 B |
| &#39;MathEvaluator invoke fn(a)&#39;                                                     | .NET 11.0 |      68.22 ns |      0.960 ns |     0.502 ns | 0.0010 |      - |      32 B |
