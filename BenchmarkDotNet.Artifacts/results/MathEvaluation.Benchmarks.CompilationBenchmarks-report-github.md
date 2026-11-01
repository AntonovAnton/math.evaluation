```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9818)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  24,140.967 ns |   228.8415 ns |   214.0585 ns | 0.1221 | 0.0610 |    5146 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,901.004 ns |    48.0569 ns |    42.6012 ns | 0.0763 | 0.0725 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |   6,275.995 ns |    81.9049 ns |    72.6066 ns | 0.1221 | 0.1144 |    3951 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  24,400.860 ns |   268.7402 ns |   238.2311 ns | 0.1221 | 0.0610 |    4644 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     603.805 ns |     8.5103 ns |     7.9605 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   2,200.670 ns |    27.2034 ns |    24.1151 ns | 0.0687 |      - |    2160 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 124,429.950 ns |   803.8559 ns |   627.5978 ns | 0.2441 |      - |   12870 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   7,360.118 ns |   142.3040 ns |   126.1488 ns | 0.1831 | 0.1221 |    6613 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |   6,421.511 ns |   115.0542 ns |   123.1067 ns | 0.1373 | 0.1221 |    4304 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       3.544 ns |     0.0834 ns |     0.1439 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       3.915 ns |     0.1004 ns |     0.1269 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       4.124 ns |     0.0911 ns |     0.0852 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 | 114,953.319 ns | 2,189.3242 ns | 2,150.2091 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,560.924 ns |    69.4380 ns |   106.0392 ns | 0.0763 | 0.0610 |    2413 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |   6,270.037 ns |   121.1187 ns |   113.2945 ns | 0.1221 | 0.0610 |    4357 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 133,772.972 ns | 1,289.4370 ns | 1,143.0523 ns |      - |      - |    7168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   4,040.402 ns |    72.7108 ns |    71.4117 ns | 0.0916 | 0.0763 |    3261 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |   6,258.862 ns |    32.6904 ns |    28.9792 ns | 0.1221 | 0.0610 |    5082 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  14,706.998 ns |   293.3704 ns |   656.1649 ns | 0.1526 | 0.1221 |    5150 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   3,297.901 ns |    49.4168 ns |    46.2245 ns | 0.0763 | 0.0725 |    2423 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |   7,673.702 ns |   101.8118 ns |    85.0175 ns | 0.1221 | 0.1144 |    3951 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  14,325.378 ns |   282.5877 ns |   396.1485 ns | 0.1221 | 0.0916 |    4643 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     735.704 ns |     7.6193 ns |     7.1271 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   2,860.246 ns |    56.4217 ns |    55.4136 ns | 0.0687 |      - |    2160 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  | 113,373.423 ns |   859.1942 ns |   803.6908 ns | 0.2441 |      - |   12870 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   8,452.492 ns |   100.0355 ns |    83.5342 ns | 0.1831 | 0.1221 |    6613 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |   7,788.623 ns |    87.8654 ns |    77.8904 ns | 0.1373 | 0.1297 |    4304 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       3.594 ns |     0.0790 ns |     0.0700 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.258 ns |     0.0995 ns |     0.0882 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       3.716 ns |     0.0972 ns |     0.0909 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  | 100,109.113 ns | 1,703.0543 ns | 1,593.0380 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   4,071.396 ns |    79.7951 ns |   116.9626 ns | 0.0763 | 0.0610 |    2412 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |   7,470.083 ns |   103.3270 ns |    86.2827 ns | 0.1221 | 0.0610 |    4723 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 114,823.912 ns | 1,159.2704 ns | 1,027.6630 ns |      - |      - |    7168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   4,644.459 ns |    91.6247 ns |    94.0918 ns | 0.0916 | 0.0763 |    3261 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |   8,056.047 ns |   158.8571 ns |   169.9753 ns | 0.1221 | 0.0610 |    5561 B |
