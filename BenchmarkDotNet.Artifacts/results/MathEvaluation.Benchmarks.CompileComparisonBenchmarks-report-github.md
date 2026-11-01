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
| MathEvaluator.FastExpressionCompiler | .NET 10.0 | .NET 10.0 | False     |     5.798 μs |  0.05 |    1 | 0.1221 | 0.0916 |    4.1 KB | 
| NCalc.LambdaCompilation              | .NET 10.0 | .NET 10.0 | False     |     9.599 μs |  0.08 |    2 | 0.1221 | 0.0610 |   4.69 KB | 
| Flee                                 | .NET 10.0 | .NET 10.0 | False     |    73.047 μs |  0.64 |    3 | 3.4180 | 1.2207 | 105.03 KB | 
| MathEvaluator                        | .NET 10.0 | .NET 10.0 | False     |   114.673 μs |  1.00 |    4 | 0.2441 |      - |   8.43 KB | 
| Jace                                 | .NET 10.0 | .NET 10.0 | False     | 1,391.478 μs | 12.13 |    5 |      - |      - |  26.35 KB | 
|                                      |           |           |           |              |       |      |        |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 10.0 | .NET 10.0 | True      |   110.108 μs |  0.95 |    1 |      - |      - |   4.08 KB | 
| MathEvaluator                        | .NET 10.0 | .NET 10.0 | True      |   116.077 μs |  1.00 |    2 | 0.2441 |      - |   8.43 KB | 
| NCalc.LambdaCompilation              | .NET 10.0 | .NET 10.0 | True      |   119.685 μs |  1.03 |    2 |      - |      - |   4.67 KB | 
| Jace                                 | .NET 10.0 | .NET 10.0 | True      | 1,368.652 μs | 11.79 |    3 |      - |      - |  27.17 KB | 
| Flee                                 | .NET 10.0 | .NET 10.0 | True      | 3,491.887 μs | 30.09 |    4 |      - |      - | 105.03 KB | 
|                                      |           |           |           |              |       |      |        |        |           | 
| MathEvaluator.FastExpressionCompiler | .NET 8.0  | .NET 8.0  | False     |     6.610 μs |  0.07 |    1 | 0.1221 | 0.0916 |    4.1 KB | 
| NCalc.LambdaCompilation              | .NET 8.0  | .NET 8.0  | False     |    11.357 μs |  0.13 |    2 | 0.1221 |      - |   4.69 KB | 
| Flee                                 | .NET 8.0  | .NET 8.0  | False     |    82.476 μs |  0.91 |    3 | 3.4180 | 1.2207 | 104.61 KB | 
| MathEvaluator                        | .NET 8.0  | .NET 8.0  | False     |    90.447 μs |  1.00 |    4 | 0.2441 |      - |   8.43 KB | 
| Jace                                 | .NET 8.0  | .NET 8.0  | False     |   222.105 μs |  2.46 |    5 | 0.4883 |      - |  26.74 KB | 
|                                      |           |           |           |              |       |      |        |        |           | 
| MathEvaluator                        | .NET 8.0  | .NET 8.0  | True      |    90.214 μs |  1.00 |    1 | 0.2441 |      - |   8.43 KB | 
| MathEvaluator.FastExpressionCompiler | .NET 8.0  | .NET 8.0  | True      |    91.117 μs |  1.01 |    1 | 0.1221 |      - |    4.1 KB | 
| NCalc.LambdaCompilation              | .NET 8.0  | .NET 8.0  | True      |    94.345 μs |  1.05 |    1 |      - |      - |   4.67 KB | 
| Flee                                 | .NET 8.0  | .NET 8.0  | True      |   174.784 μs |  1.94 |    2 | 3.4180 | 0.9766 | 104.64 KB | 
| Jace                                 | .NET 8.0  | .NET 8.0  | True      |   229.759 μs |  2.55 |    3 | 0.4883 |      - |  27.56 KB | 
