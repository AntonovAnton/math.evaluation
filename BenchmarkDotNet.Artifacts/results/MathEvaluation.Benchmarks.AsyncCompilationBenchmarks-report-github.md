```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9818)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4


```
| Method                                                                                    | Job       | Runtime   | Mean           | Error         | StdDev        | Gen0   | Allocated |
|------------------------------------------------------------------------------------------ |---------- |---------- |---------------:|--------------:|--------------:|-------:|----------:|
| &#39;MathEvaluator: Compile &quot;priceSync(x) * qty + taxSync(priceSync(x))&quot;&#39;                     | .NET 10.0 | .NET 10.0 | 189,375.391 ns | 2,376.7927 ns | 2,106.9649 ns |      - |    8000 B |
| &#39;MathEvaluator: CompileAsyncFunc &quot;price(x) * qty + tax(price(x))&quot;&#39;                        | .NET 10.0 | .NET 10.0 | 959,598.281 ns | 5,748.1167 ns | 5,376.7919 ns |      - |   30690 B |
| &#39;MathEvaluator.FastExpressionCompiler: CompileAsyncFunc &quot;price(x) * qty + tax(price(x))&quot;&#39; | .NET 10.0 | .NET 10.0 | 963,373.603 ns | 4,768.9625 ns | 3,982.2997 ns |      - |   30690 B |
| &#39;MathEvaluator: fn(variables), sync functions&#39;                                            | .NET 10.0 | .NET 10.0 |       5.577 ns |     0.0773 ns |     0.0685 ns |      - |         - |
| &#39;MathEvaluator: await fn(variables), completed async functions&#39;                           | .NET 10.0 | .NET 10.0 |   1,729.044 ns |    22.5749 ns |    18.8510 ns | 0.0134 |     464 B |
| &#39;MathEvaluator.FastExpressionCompiler: await fn(variables), completed async functions&#39;    | .NET 10.0 | .NET 10.0 |   1,731.122 ns |     6.2050 ns |     5.8042 ns | 0.0114 |     464 B |
| &#39;MathEvaluator: await fn(variables), yielding async functions&#39;                            | .NET 10.0 | .NET 10.0 |   8,921.291 ns |    58.5492 ns |    54.7670 ns | 0.0305 |    1327 B |
| &#39;MathEvaluator: Compile &quot;priceSync(x) * qty + taxSync(priceSync(x))&quot;&#39;                     | .NET 8.0  | .NET 8.0  | 171,652.169 ns |   840.3561 ns |   701.7354 ns | 0.2441 |    8031 B |
| &#39;MathEvaluator: CompileAsyncFunc &quot;price(x) * qty + tax(price(x))&quot;&#39;                        | .NET 8.0  | .NET 8.0  | 849,523.359 ns | 5,847.7830 ns | 5,470.0199 ns | 0.9766 |   31410 B |
| &#39;MathEvaluator.FastExpressionCompiler: CompileAsyncFunc &quot;price(x) * qty + tax(price(x))&quot;&#39; | .NET 8.0  | .NET 8.0  | 848,493.562 ns | 2,070.0387 ns | 1,728.5760 ns | 0.9766 |   31409 B |
| &#39;MathEvaluator: fn(variables), sync functions&#39;                                            | .NET 8.0  | .NET 8.0  |       5.517 ns |     0.0169 ns |     0.0132 ns |      - |         - |
| &#39;MathEvaluator: await fn(variables), completed async functions&#39;                           | .NET 8.0  | .NET 8.0  |   2,014.547 ns |    28.8807 ns |    24.1167 ns | 0.0114 |     464 B |
| &#39;MathEvaluator.FastExpressionCompiler: await fn(variables), completed async functions&#39;    | .NET 8.0  | .NET 8.0  |   1,989.224 ns |    10.3767 ns |     9.1986 ns | 0.0114 |     464 B |
| &#39;MathEvaluator: await fn(variables), yielding async functions&#39;                            | .NET 8.0  | .NET 8.0  |   9,360.261 ns |    78.0598 ns |    65.1835 ns | 0.0305 |    1327 B |
