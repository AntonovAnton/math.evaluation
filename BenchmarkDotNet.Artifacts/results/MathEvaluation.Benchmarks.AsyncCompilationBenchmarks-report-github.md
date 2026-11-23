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
| Method                                                                                    | Runtime   | Mean           | Error          | StdDev        | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------------------------------------ |---------- |---------------:|---------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: Compile &quot;priceSync(x) * qty + taxSync(priceSync(x))&quot;&#39;                     | .NET 10.0 | 194,078.284 ns |  3,725.2410 ns | 2,464.0163 ns | 0.2441 |      - |    8023 B |
| &#39;MathEvaluator: CompileAsyncFunc &quot;price(x) * qty + tax(price(x))&quot;&#39;                        | .NET 10.0 | 976,360.981 ns | 15,774.8816 ns | 9,387.3790 ns |      - |      - |   30690 B |
| &#39;MathEvaluator.FastExpressionCompiler: CompileAsyncFunc &quot;price(x) * qty + tax(price(x))&quot;&#39; | .NET 10.0 |  25,027.488 ns |    121.6219 ns |    63.6106 ns | 0.4883 | 0.4578 |   15292 B |
| &#39;MathEvaluator: fn(variables), sync functions&#39;                                            | .NET 10.0 |       7.230 ns |      0.0396 ns |     0.0236 ns |      - |      - |         - |
| &#39;MathEvaluator: await fn(variables), completed async functions&#39;                           | .NET 10.0 |   1,866.101 ns |     11.2315 ns |     7.4289 ns | 0.0134 |      - |     464 B |
| &#39;MathEvaluator: await fn(variables), yielding async functions&#39;                            | .NET 10.0 |   6,802.179 ns |    184.3756 ns |   121.9530 ns | 0.0381 |      - |    1354 B |
| &#39;MathEvaluator: Compile &quot;priceSync(x) * qty + taxSync(priceSync(x))&quot;&#39;                     | .NET 11.0 | 182,209.736 ns |  3,087.1375 ns | 2,041.9503 ns | 0.2441 |      - |    8047 B |
| &#39;MathEvaluator: CompileAsyncFunc &quot;price(x) * qty + tax(price(x))&quot;&#39;                        | .NET 11.0 | 989,266.094 ns | 13,369.1760 ns | 8,842.8821 ns |      - |      - |   30738 B |
| &#39;MathEvaluator.FastExpressionCompiler: CompileAsyncFunc &quot;price(x) * qty + tax(price(x))&quot;&#39; | .NET 11.0 |  24,755.150 ns |    336.6033 ns |   222.6423 ns | 0.4883 | 0.4578 |   15351 B |
| &#39;MathEvaluator: fn(variables), sync functions&#39;                                            | .NET 11.0 |       7.200 ns |      0.1001 ns |     0.0662 ns |      - |      - |         - |
| &#39;MathEvaluator: await fn(variables), completed async functions&#39;                           | .NET 11.0 |   1,867.818 ns |     42.1555 ns |    27.8833 ns | 0.0134 |      - |     440 B |
| &#39;MathEvaluator: await fn(variables), yielding async functions&#39;                            | .NET 11.0 |   4,484.888 ns |    221.0604 ns |   146.2178 ns | 0.0381 |      - |    1292 B |
