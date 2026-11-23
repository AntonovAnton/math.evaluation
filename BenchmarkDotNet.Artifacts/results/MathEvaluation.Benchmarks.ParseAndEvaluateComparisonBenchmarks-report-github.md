```

BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.10075)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
Memory: 31.77 GB Total, 15.97 GB Available
.NET SDK 11.0.100
  [Host]     : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4
  Job-GVKUBM : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  Job-IHFIKV : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4


```
| Method                 | Runtime   | Mean           | Ratio    | Rank | Gen0    | Gen1   | Allocated | 
|----------------------- |---------- |---------------:|---------:|-----:|--------:|-------:|----------:|
| MathEvaluator          | .NET 10.0 |       706.4 ns |     1.00 |    1 |  0.0283 |      - |     896 B | 
| NTDLS.ExpressionParser | .NET 10.0 |       942.9 ns |     1.34 |    2 |  0.0381 |      - |    1208 B | 
| xFunc                  | .NET 10.0 |     2,391.5 ns |     3.39 |    3 |  0.0625 |      - |    1960 B | 
| NCalc                  | .NET 10.0 |     2,947.0 ns |     4.17 |    4 |  0.1289 |      - |    4112 B | 
| NoStringEvaluating     | .NET 10.0 |    18,192.0 ns |    25.76 |    5 |  0.0938 |      - |    3152 B | 
| CalcExpr.NET           | .NET 10.0 |   337,508.0 ns |   477.89 |    6 | 10.0000 |      - |  339704 B | 
| Jace                   | .NET 10.0 | 1,397,236.5 ns | 1,978.38 |    7 |       - |      - |   27836 B | 
| Flee                   | .NET 10.0 | 3,433,214.0 ns | 4,861.18 |    8 |  3.0000 | 1.0000 |  107647 B | 
|                        |           |                |          |      |         |        |           | 
| MathEvaluator          | .NET 11.0 |       709.5 ns |     1.00 |    1 |  0.0293 |      - |     920 B | 
| NTDLS.ExpressionParser | .NET 11.0 |       838.1 ns |     1.18 |    2 |  0.0381 |      - |    1208 B | 
| xFunc                  | .NET 11.0 |     2,408.3 ns |     3.39 |    3 |  0.0625 |      - |    1960 B | 
| NCalc                  | .NET 11.0 |     2,896.0 ns |     4.08 |    4 |  0.1289 |      - |    4112 B | 
| NoStringEvaluating     | .NET 11.0 |    20,664.5 ns |    29.13 |    5 |  0.0938 |      - |    3152 B | 
| CalcExpr.NET           | .NET 11.0 |   331,436.8 ns |   467.15 |    6 | 10.0000 |      - |  338770 B | 
| Jace                   | .NET 11.0 | 1,303,645.4 ns | 1,837.46 |    7 |       - |      - |   27684 B | 
| Flee                   | .NET 11.0 | 3,424,403.7 ns | 4,826.63 |    8 |  3.0000 | 1.0000 |  107798 B | 
