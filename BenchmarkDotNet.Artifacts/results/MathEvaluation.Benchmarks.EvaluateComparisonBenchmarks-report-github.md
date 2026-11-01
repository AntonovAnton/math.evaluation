```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9818)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4

Alloc Ratio=NA  

```
| Method                               | Job       | Runtime   | Mean         | Ratio    | Rank | Gen0   | Allocated | 
|------------------------------------- |---------- |---------- |-------------:|---------:|-----:|-------:|----------:|
| MathEvaluator                        | .NET 10.0 | .NET 10.0 |     1.799 ns |     1.00 |    1 |      - |         - | 
| MathEvaluator.FastExpressionCompiler | .NET 10.0 | .NET 10.0 |     1.799 ns |     1.00 |    1 |      - |         - | 
| NCalc.LambdaCompilation              | .NET 10.0 | .NET 10.0 |     1.808 ns |     1.00 |    1 |      - |         - | 
| NTDLS.ExpressionParser               | .NET 10.0 | .NET 10.0 |    59.650 ns |    33.15 |    2 |      - |         - | 
| Flee                                 | .NET 10.0 | .NET 10.0 |    67.678 ns |    37.62 |    3 |      - |         - | 
| NoStringEvaluating                   | .NET 10.0 | .NET 10.0 |   143.354 ns |    79.68 |    4 |      - |         - | 
| xFunc                                | .NET 10.0 | .NET 10.0 |   180.750 ns |   100.46 |    5 | 0.0090 |     288 B | 
| Jace                                 | .NET 10.0 | .NET 10.0 |   194.155 ns |   107.91 |    6 | 0.0200 |     624 B | 
| CalcExpr.NET                         | .NET 10.0 | .NET 10.0 |   334.517 ns |   185.93 |    7 | 0.0229 |     720 B | 
| NCalc                                | .NET 10.0 | .NET 10.0 |   671.564 ns |   373.26 |    8 | 0.0605 |    1912 B | 
| &#39;Mathos Parser&#39;                      | .NET 10.0 | .NET 10.0 | 4,043.260 ns | 2,247.27 |    9 | 0.0391 |    1384 B | 
|                                      |           |           |              |          |      |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 8.0  | .NET 8.0  |     1.781 ns |     0.99 |    1 |      - |         - | 
| NCalc.LambdaCompilation              | .NET 8.0  | .NET 8.0  |     1.786 ns |     1.00 |    1 |      - |         - | 
| MathEvaluator                        | .NET 8.0  | .NET 8.0  |     1.794 ns |     1.00 |    1 |      - |         - | 
| NTDLS.ExpressionParser               | .NET 8.0  | .NET 8.0  |    69.090 ns |    38.52 |    2 |      - |         - | 
| Flee                                 | .NET 8.0  | .NET 8.0  |    72.364 ns |    40.34 |    3 |      - |         - | 
| NoStringEvaluating                   | .NET 8.0  | .NET 8.0  |   192.857 ns |   107.52 |    4 |      - |         - | 
| Jace                                 | .NET 8.0  | .NET 8.0  |   226.147 ns |   126.08 |    5 | 0.0200 |     624 B | 
| xFunc                                | .NET 8.0  | .NET 8.0  |   246.653 ns |   137.51 |    6 | 0.0088 |     288 B | 
| CalcExpr.NET                         | .NET 8.0  | .NET 8.0  |   463.447 ns |   258.38 |    7 | 0.0229 |     720 B | 
| NCalc                                | .NET 8.0  | .NET 8.0  |   801.302 ns |   446.73 |    8 | 0.0605 |    1912 B | 
| &#39;Mathos Parser&#39;                      | .NET 8.0  | .NET 8.0  | 5,294.778 ns | 2,951.90 |    9 | 0.0391 |    1384 B | 
