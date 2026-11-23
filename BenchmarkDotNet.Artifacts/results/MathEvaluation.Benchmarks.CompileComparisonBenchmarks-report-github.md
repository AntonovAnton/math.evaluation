```

BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.10075)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
Memory: 31.77 GB Total, 15.97 GB Available
.NET SDK 11.0.100
  [Host]     : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4
  Job-GVKUBM : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  Job-IHFIKV : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4


```
| Method                               | Runtime   | FirstCall | Mean         | Ratio | Rank | Gen0   | Gen1   | Allocated | 
|------------------------------------- |---------- |---------- |-------------:|------:|-----:|-------:|-------:|----------:|
| MathEvaluator.FastExpressionCompiler | .NET 10.0 | False     |     6.378 μs |  0.06 |    1 | 0.1297 | 0.1221 |    4.1 KB | 
| NCalc.LambdaCompilation              | .NET 10.0 | False     |     7.920 μs |  0.07 |    2 | 0.1450 | 0.1373 |   4.48 KB | 
| Flee                                 | .NET 10.0 | False     |    74.482 μs |  0.65 |    3 | 3.4180 | 1.2207 | 105.03 KB | 
| MathEvaluator                        | .NET 10.0 | False     |   113.796 μs |  1.00 |    4 | 0.2441 |      - |   8.43 KB | 
| Jace                                 | .NET 10.0 | False     | 1,421.016 μs | 12.49 |    5 |      - |      - |  26.38 KB | 
|                                      |           |           |              |       |      |        |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 10.0 | True      |   112.301 μs |  0.98 |    1 | 0.1221 |      - |    4.1 KB | 
| MathEvaluator                        | .NET 10.0 | True      |   114.086 μs |  1.00 |    1 | 0.2441 | 0.1221 |   8.43 KB | 
| NCalc.LambdaCompilation              | .NET 10.0 | True      |   116.572 μs |  1.02 |    1 | 0.1221 |      - |   4.48 KB | 
| Jace                                 | .NET 10.0 | True      | 1,419.570 μs | 12.44 |    2 |      - |      - |  27.21 KB | 
| Flee                                 | .NET 10.0 | True      | 3,461.693 μs | 30.34 |    3 |      - |      - |  105.1 KB | 
|                                      |           |           |              |       |      |        |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 11.0 | False     |     5.422 μs |  0.05 |    1 | 0.1297 | 0.1221 |   4.13 KB | 
| NCalc.LambdaCompilation              | .NET 11.0 | False     |     7.196 μs |  0.07 |    2 | 0.1450 | 0.1373 |   4.49 KB | 
| Flee                                 | .NET 11.0 | False     |    72.281 μs |  0.67 |    3 | 3.4180 | 1.3428 | 105.18 KB | 
| MathEvaluator                        | .NET 11.0 | False     |   108.248 μs |  1.00 |    4 | 0.2441 | 0.1221 |   8.46 KB | 
| Jace                                 | .NET 11.0 | False     | 1,302.130 μs | 12.03 |    5 |      - |      - |  26.29 KB | 
|                                      |           |           |              |       |      |        |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 11.0 | True      |   107.244 μs |  1.00 |    1 | 0.1221 |      - |   4.13 KB | 
| MathEvaluator                        | .NET 11.0 | True      |   107.491 μs |  1.00 |    1 | 0.2441 | 0.1221 |   8.46 KB | 
| NCalc.LambdaCompilation              | .NET 11.0 | True      |   110.148 μs |  1.02 |    1 | 0.1221 |      - |   4.49 KB | 
| Jace                                 | .NET 11.0 | True      | 1,302.287 μs | 12.12 |    2 |      - |      - |  27.11 KB | 
| Flee                                 | .NET 11.0 | True      | 3,454.764 μs | 32.14 |    3 |      - |      - | 105.25 KB | 
