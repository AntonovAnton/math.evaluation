```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9818)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4


```
| Method                               | Job       | Runtime   | FirstCall | Mean         | Ratio | Rank | Gen0   | Gen1   | Allocated | 
|------------------------------------- |---------- |---------- |---------- |-------------:|------:|-----:|-------:|-------:|----------:|
| MathEvaluator.FastExpressionCompiler | .NET 10.0 | .NET 10.0 | False     |     6.102 μs |  0.05 |    1 | 0.1373 | 0.1297 |   4.26 KB | 
| NCalc.LambdaCompilation              | .NET 10.0 | .NET 10.0 | False     |    10.213 μs |  0.09 |    2 | 0.1221 | 0.0610 |   4.69 KB | 
| Flee                                 | .NET 10.0 | .NET 10.0 | False     |    76.084 μs |  0.64 |    3 | 3.4180 | 1.4648 | 105.03 KB | 
| MathEvaluator                        | .NET 10.0 | .NET 10.0 | False     |   119.024 μs |  1.00 |    4 | 0.2441 |      - |   8.43 KB | 
| Jace                                 | .NET 10.0 | .NET 10.0 | False     | 1,377.062 μs | 11.57 |    5 |      - |      - |  26.35 KB | 
|                                      |           |           |           |              |       |      |        |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 10.0 | .NET 10.0 | True      |   109.585 μs |  0.98 |    1 |      - |      - |   4.08 KB | 
| MathEvaluator                        | .NET 10.0 | .NET 10.0 | True      |   111.478 μs |  1.00 |    1 | 0.2441 |      - |   8.43 KB | 
| NCalc.LambdaCompilation              | .NET 10.0 | .NET 10.0 | True      |   117.986 μs |  1.06 |    2 |      - |      - |   4.67 KB | 
| Jace                                 | .NET 10.0 | .NET 10.0 | True      | 1,360.582 μs | 12.21 |    3 |      - |      - |  27.22 KB | 
| Flee                                 | .NET 10.0 | .NET 10.0 | True      | 3,367.917 μs | 30.21 |    4 |      - |      - |  105.1 KB | 
|                                      |           |           |           |              |       |      |        |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 8.0  | .NET 8.0  | False     |     6.649 μs |  0.08 |    1 | 0.1221 | 0.0916 |    4.1 KB | 
| NCalc.LambdaCompilation              | .NET 8.0  | .NET 8.0  | False     |    12.098 μs |  0.14 |    2 | 0.1526 | 0.1373 |    4.7 KB | 
| Flee                                 | .NET 8.0  | .NET 8.0  | False     |    80.648 μs |  0.92 |    3 | 3.4180 | 1.2207 |  104.6 KB | 
| MathEvaluator                        | .NET 8.0  | .NET 8.0  | False     |    88.038 μs |  1.00 |    4 | 0.2441 |      - |   8.43 KB | 
| Jace                                 | .NET 8.0  | .NET 8.0  | False     |   217.528 μs |  2.47 |    5 | 0.4883 |      - |  26.74 KB | 
|                                      |           |           |           |              |       |      |        |        |           | 
| MathEvaluator                        | .NET 8.0  | .NET 8.0  | True      |    86.387 μs |  1.00 |    1 | 0.2441 |      - |   8.43 KB | 
| MathEvaluator.FastExpressionCompiler | .NET 8.0  | .NET 8.0  | True      |    87.914 μs |  1.02 |    1 | 0.1221 |      - |    4.1 KB | 
| NCalc.LambdaCompilation              | .NET 8.0  | .NET 8.0  | True      |    90.657 μs |  1.05 |    2 |      - |      - |   4.67 KB | 
| Flee                                 | .NET 8.0  | .NET 8.0  | True      |   169.637 μs |  1.96 |    3 | 3.4180 | 0.9766 | 104.62 KB | 
| Jace                                 | .NET 8.0  | .NET 8.0  | True      |   218.427 μs |  2.53 |    4 | 0.4883 |      - |  27.56 KB | 
