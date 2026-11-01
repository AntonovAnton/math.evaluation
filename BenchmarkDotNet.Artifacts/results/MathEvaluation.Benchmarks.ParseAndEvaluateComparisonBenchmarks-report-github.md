```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9818)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4


```
| Method                 | Job       | Runtime   | Mean           | Ratio    | Rank | Gen0    | Gen1   | Allocated | 
|----------------------- |---------- |---------- |---------------:|---------:|-----:|--------:|-------:|----------:|
| MathEvaluator          | .NET 10.0 | .NET 10.0 |       687.4 ns |     1.00 |    1 |  0.0283 |      - |     896 B | 
| NTDLS.ExpressionParser | .NET 10.0 | .NET 10.0 |       969.1 ns |     1.41 |    2 |  0.0371 |      - |    1208 B | 
| xFunc                  | .NET 10.0 | .NET 10.0 |     2,423.3 ns |     3.53 |    3 |  0.0625 |      - |    1960 B | 
| NCalc                  | .NET 10.0 | .NET 10.0 |     5,353.6 ns |     7.79 |    4 |  0.1328 |      - |    4328 B | 
| NoStringEvaluating     | .NET 10.0 | .NET 10.0 |    17,171.9 ns |    24.98 |    5 |  0.0938 |      - |    3152 B | 
| CalcExpr.NET           | .NET 10.0 | .NET 10.0 |   337,818.0 ns |   491.44 |    6 | 10.0000 |      - |  339656 B | 
| Jace                   | .NET 10.0 | .NET 10.0 | 1,396,988.2 ns | 2,032.28 |    7 |       - |      - |   27836 B | 
| Flee                   | .NET 10.0 | .NET 10.0 | 3,401,684.8 ns | 4,948.63 |    8 |  3.0000 | 1.0000 |  107647 B | 
|                        |           |           |                |          |      |         |        |           | 
| MathEvaluator          | .NET 8.0  | .NET 8.0  |       842.6 ns |     1.00 |    1 |  0.0283 |      - |     896 B | 
| NTDLS.ExpressionParser | .NET 8.0  | .NET 8.0  |     1,144.1 ns |     1.36 |    2 |  0.0371 |      - |    1208 B | 
| xFunc                  | .NET 8.0  | .NET 8.0  |     2,749.6 ns |     3.26 |    3 |  0.0625 |      - |    1960 B | 
| NCalc                  | .NET 8.0  | .NET 8.0  |     6,280.9 ns |     7.46 |    4 |  0.1328 |      - |    4328 B | 
| NoStringEvaluating     | .NET 8.0  | .NET 8.0  |    26,582.3 ns |    31.56 |    5 |  0.0938 |      - |    3152 B | 
| Flee                   | .NET 8.0  | .NET 8.0  |   177,293.5 ns |   210.48 |    6 |  3.0000 |      - |  107221 B | 
| Jace                   | .NET 8.0  | .NET 8.0  |   227,006.3 ns |   269.50 |    7 |  0.5000 |      - |   28222 B | 
| CalcExpr.NET           | .NET 8.0  | .NET 8.0  |   390,161.0 ns |   463.20 |    8 | 11.0000 |      - |  344300 B | 
