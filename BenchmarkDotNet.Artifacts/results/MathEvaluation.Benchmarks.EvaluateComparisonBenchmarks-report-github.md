```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9818)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4

Alloc Ratio=NA  

```
| Method                               | Job       | Runtime   | Mean       | Ratio  | Rank | Gen0   | Allocated | 
|------------------------------------- |---------- |---------- |-----------:|-------:|-----:|-------:|----------:|
| NCalc.LambdaCompilation              | .NET 10.0 | .NET 10.0 |   1.820 ns |   0.98 |    1 |      - |         - | 
| MathEvaluator.FastExpressionCompiler | .NET 10.0 | .NET 10.0 |   1.822 ns |   0.98 |    1 |      - |         - | 
| MathEvaluator                        | .NET 10.0 | .NET 10.0 |   1.866 ns |   1.00 |    1 |      - |         - | 
| NTDLS.ExpressionParser               | .NET 10.0 | .NET 10.0 |  60.252 ns |  32.32 |    2 |      - |         - | 
| Flee                                 | .NET 10.0 | .NET 10.0 |  64.919 ns |  34.82 |    3 |      - |         - | 
| NoStringEvaluating                   | .NET 10.0 | .NET 10.0 | 150.928 ns |  80.95 |    4 |      - |         - | 
| xFunc                                | .NET 10.0 | .NET 10.0 | 180.528 ns |  96.83 |    5 | 0.0090 |     288 B | 
| Jace                                 | .NET 10.0 | .NET 10.0 | 195.311 ns | 104.76 |    6 | 0.0200 |     624 B | 
| CalcExpr.NET                         | .NET 10.0 | .NET 10.0 | 315.615 ns | 169.28 |    7 | 0.0229 |     720 B | 
| NCalc                                | .NET 10.0 | .NET 10.0 | 665.623 ns | 357.01 |    8 | 0.0605 |    1912 B | 
|                                      |           |           |            |        |      |        |           | 
| MathEvaluator                        | .NET 8.0  | .NET 8.0  |   1.788 ns |   1.00 |    1 |      - |         - | 
| NCalc.LambdaCompilation              | .NET 8.0  | .NET 8.0  |   1.812 ns |   1.01 |    1 |      - |         - | 
| MathEvaluator.FastExpressionCompiler | .NET 8.0  | .NET 8.0  |   1.994 ns |   1.12 |    2 |      - |         - | 
| NTDLS.ExpressionParser               | .NET 8.0  | .NET 8.0  |  67.769 ns |  37.90 |    3 |      - |         - | 
| Flee                                 | .NET 8.0  | .NET 8.0  |  72.550 ns |  40.58 |    4 |      - |         - | 
| NoStringEvaluating                   | .NET 8.0  | .NET 8.0  | 190.851 ns | 106.74 |    5 |      - |         - | 
| Jace                                 | .NET 8.0  | .NET 8.0  | 227.717 ns | 127.36 |    6 | 0.0200 |     624 B | 
| xFunc                                | .NET 8.0  | .NET 8.0  | 245.352 ns | 137.22 |    7 | 0.0088 |     288 B | 
| CalcExpr.NET                         | .NET 8.0  | .NET 8.0  | 475.574 ns | 265.98 |    8 | 0.0225 |     720 B | 
| NCalc                                | .NET 8.0  | .NET 8.0  | 794.758 ns | 444.49 |    9 | 0.0605 |    1912 B | 
