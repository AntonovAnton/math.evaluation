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
| Method                                                            | Runtime   | Mean       | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------------ |---------- |-----------:|----------:|----------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                        | .NET 10.0 | 127.183 μs | 5.2423 μs | 3.4674 μs | 0.2441 |      - |  12.56 KB |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39; | .NET 10.0 |   7.796 μs | 0.3772 μs | 0.2495 μs | 0.2060 | 0.1984 |   6.46 KB |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                | .NET 10.0 | 134.919 μs | 2.1322 μs | 1.4103 μs |      - |      - |   6.99 KB |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;         | .NET 10.0 |   4.011 μs | 0.1165 μs | 0.0771 μs | 0.0992 | 0.0916 |   3.19 KB |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                        | .NET 11.0 | 120.369 μs | 0.9493 μs | 0.5649 μs | 0.2441 |      - |  12.58 KB |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39; | .NET 11.0 |   7.287 μs | 0.0828 μs | 0.0547 μs | 0.2060 | 0.1984 |   6.49 KB |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                | .NET 11.0 | 129.360 μs | 3.5320 μs | 2.1018 μs |      - |      - |   7.02 KB |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;         | .NET 11.0 |   4.003 μs | 0.0725 μs | 0.0479 μs | 0.0992 | 0.0916 |   3.22 KB |
