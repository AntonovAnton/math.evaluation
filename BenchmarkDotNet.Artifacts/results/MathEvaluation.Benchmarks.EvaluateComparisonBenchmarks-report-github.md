```

BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.10075)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
Memory: 31.77 GB Total, 15.97 GB Available
.NET SDK 11.0.100
  [Host]     : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4
  Job-GVKUBM : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  Job-IHFIKV : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4

Alloc Ratio=NA  

```
| Method                               | Runtime   | Mean       | Ratio  | Rank | Gen0   | Allocated | 
|------------------------------------- |---------- |-----------:|-------:|-----:|-------:|----------:|
| MathEvaluator                        | .NET 10.0 |   1.811 ns |   1.00 |    1 |      - |         - | 
| NCalc.LambdaCompilation              | .NET 10.0 |   1.818 ns |   1.00 |    1 |      - |         - | 
| MathEvaluator.FastExpressionCompiler | .NET 10.0 |   1.818 ns |   1.00 |    1 |      - |         - | 
| NTDLS.ExpressionParser               | .NET 10.0 |  57.257 ns |  31.61 |    2 |      - |         - | 
| Flee                                 | .NET 10.0 |  70.514 ns |  38.93 |    3 |      - |         - | 
| NoStringEvaluating                   | .NET 10.0 | 144.104 ns |  79.56 |    4 |      - |         - | 
| xFunc                                | .NET 10.0 | 204.831 ns | 113.09 |    5 | 0.0090 |     288 B | 
| Jace                                 | .NET 10.0 | 207.140 ns | 114.36 |    5 | 0.0200 |     624 B | 
| CalcExpr.NET                         | .NET 10.0 | 355.379 ns | 196.21 |    6 | 0.0229 |     720 B | 
| NCalc                                | .NET 10.0 | 754.936 ns | 416.80 |    7 | 0.0605 |    1912 B | 
|                                      |           |            |        |      |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 11.0 |   1.812 ns |   1.00 |    1 |      - |         - | 
| MathEvaluator                        | .NET 11.0 |   1.817 ns |   1.00 |    1 |      - |         - | 
| NCalc.LambdaCompilation              | .NET 11.0 |   1.873 ns |   1.03 |    1 |      - |         - | 
| NTDLS.ExpressionParser               | .NET 11.0 |  54.217 ns |  29.84 |    2 |      - |         - | 
| Flee                                 | .NET 11.0 |  64.831 ns |  35.68 |    3 |      - |         - | 
| NoStringEvaluating                   | .NET 11.0 | 162.544 ns |  89.46 |    4 |      - |         - | 
| Jace                                 | .NET 11.0 | 191.440 ns | 105.37 |    5 | 0.0200 |     624 B | 
| xFunc                                | .NET 11.0 | 192.829 ns | 106.13 |    5 | 0.0090 |     288 B | 
| CalcExpr.NET                         | .NET 11.0 | 349.445 ns | 192.33 |    6 | 0.0229 |     720 B | 
| NCalc                                | .NET 11.0 | 748.647 ns | 412.05 |    7 | 0.0605 |    1912 B | 
