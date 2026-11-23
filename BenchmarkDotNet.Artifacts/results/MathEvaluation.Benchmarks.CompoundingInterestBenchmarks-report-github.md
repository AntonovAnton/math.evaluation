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
| Method                                                       | Runtime   | Mean         | Error        | StdDev     | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------- |---------- |-------------:|-------------:|-----------:|-------:|-------:|----------:|
| &#39;MathEvaluator evaluation&#39;                                   | .NET 10.0 |    745.93 ns |     7.623 ns |   4.536 ns | 0.0458 |      - |    1456 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 10.0 | 95,419.37 ns |   822.523 ns | 489.470 ns | 0.1221 |      - |    7500 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 10.0 |  5,703.39 ns |   106.132 ns |  63.158 ns | 0.1907 | 0.1831 |    5952 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 10.0 |     21.85 ns |     0.492 ns |   0.293 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 10.0 |     22.14 ns |     0.329 ns |   0.196 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator evaluation&#39;                                   | .NET 11.0 |    747.29 ns |    15.182 ns |  10.042 ns | 0.0467 |      - |    1480 B |
| &#39;MathEvaluator compilation&#39;                                  | .NET 11.0 | 91,088.97 ns | 1,372.523 ns | 907.839 ns | 0.2441 | 0.1221 |    7530 B |
| &#39;MathEvaluator.FastExpressionCompiler compilation&#39;           | .NET 11.0 |  5,600.85 ns |   172.784 ns | 114.286 ns | 0.1907 | 0.1831 |    5960 B |
| &#39;MathEvaluator invoke fn(P, r, n, d)&#39;                        | .NET 11.0 |     21.85 ns |     0.265 ns |   0.175 ns | 0.0013 |      - |      40 B |
| &#39;MathEvaluator.FastExpressionCompiler invoke fn(P, r, n, d)&#39; | .NET 11.0 |     22.23 ns |     0.507 ns |   0.335 ns | 0.0013 |      - |      40 B |
