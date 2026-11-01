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
| MathEvaluator          | .NET 10.0 | .NET 10.0 |       684.5 ns |     1.00 |    1 |  0.0283 |      - |     896 B | 
| NTDLS.ExpressionParser | .NET 10.0 | .NET 10.0 |       990.4 ns |     1.45 |    2 |  0.0381 |      - |    1208 B | 
| xFunc                  | .NET 10.0 | .NET 10.0 |     2,360.7 ns |     3.45 |    3 |  0.0625 |      - |    1960 B | 
| &#39;Mathos Parser&#39;        | .NET 10.0 | .NET 10.0 |     5,064.4 ns |     7.40 |    4 |  0.0859 |      - |    2766 B | 
| NCalc                  | .NET 10.0 | .NET 10.0 |     5,238.3 ns |     7.65 |    4 |  0.1328 |      - |    4328 B | 
| NoStringEvaluating     | .NET 10.0 | .NET 10.0 |    18,021.0 ns |    26.33 |    5 |  0.0938 |      - |    3152 B | 
| CalcExpr.NET           | .NET 10.0 | .NET 10.0 |   333,776.3 ns |   487.66 |    6 | 10.0000 |      - |  339699 B | 
| Jace                   | .NET 10.0 | .NET 10.0 | 1,373,822.3 ns | 2,007.23 |    7 |       - |      - |   27836 B | 
| Flee                   | .NET 10.0 | .NET 10.0 | 3,366,255.5 ns | 4,918.28 |    8 |  3.0000 | 1.0000 |  107647 B | 
|                        |           |           |                |          |      |         |        |           | 
| MathEvaluator          | .NET 8.0  | .NET 8.0  |       843.2 ns |     1.00 |    1 |  0.0283 |      - |     896 B | 
| NTDLS.ExpressionParser | .NET 8.0  | .NET 8.0  |     1,115.3 ns |     1.32 |    2 |  0.0371 |      - |    1208 B | 
| xFunc                  | .NET 8.0  | .NET 8.0  |     2,703.8 ns |     3.21 |    3 |  0.0625 |      - |    1960 B | 
| NCalc                  | .NET 8.0  | .NET 8.0  |     6,125.7 ns |     7.26 |    4 |  0.1328 |      - |    4328 B | 
| &#39;Mathos Parser&#39;        | .NET 8.0  | .NET 8.0  |     6,555.8 ns |     7.77 |    5 |  0.0859 |      - |    2766 B | 
| NoStringEvaluating     | .NET 8.0  | .NET 8.0  |    26,125.8 ns |    30.98 |    6 |  0.0938 |      - |    3152 B | 
| Flee                   | .NET 8.0  | .NET 8.0  |   173,439.5 ns |   205.69 |    7 |  3.0000 | 1.0000 |  107160 B | 
| Jace                   | .NET 8.0  | .NET 8.0  |   221,379.0 ns |   262.54 |    8 |  0.5000 |      - |   28223 B | 
| CalcExpr.NET           | .NET 8.0  | .NET 8.0  |   387,482.2 ns |   459.53 |    9 | 11.0000 |      - |  344420 B | 
