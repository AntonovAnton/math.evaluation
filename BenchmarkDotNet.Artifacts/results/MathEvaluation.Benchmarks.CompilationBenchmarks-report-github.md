```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9550/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error       | StdDev      | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|------------:|------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  23,792.580 ns |  85.6583 ns |  75.9338 ns | 0.1221 | 0.0610 |    5146 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,804.346 ns |   9.2598 ns |   7.7324 ns | 0.0763 | 0.0725 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |  10,413.667 ns |  40.5238 ns |  37.9060 ns | 0.2747 | 0.2441 |    9103 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  22,888.263 ns |  63.1518 ns |  59.0722 ns | 0.1221 | 0.0610 |    4643 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     579.443 ns |   4.1037 ns |   3.6378 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   6,101.313 ns |  13.9401 ns |  13.0396 ns | 0.2365 |      - |    7440 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 121,184.534 ns | 255.9119 ns | 226.8593 ns | 0.2441 |      - |   12870 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   7,484.919 ns | 145.9063 ns | 121.8384 ns | 0.2136 | 0.1526 |    6694 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |  15,909.905 ns |  87.2449 ns |  81.6089 ns | 0.9155 | 0.8545 |   30095 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       3.610 ns |   0.0918 ns |   0.0901 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       3.969 ns |   0.0207 ns |   0.0193 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       4.229 ns |   0.0232 ns |   0.0217 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 | 111,762.884 ns | 334.9577 ns | 313.3196 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,541.999 ns |  52.3743 ns |  48.9910 ns | 0.0763 | 0.0610 |    2413 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |  13,798.023 ns |  62.7076 ns |  52.3637 ns | 0.7935 | 0.7324 |   24896 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 129,781.530 ns | 415.6983 ns | 388.8444 ns |      - |      - |    7168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   3,836.320 ns |  45.8085 ns |  40.6080 ns | 0.0916 | 0.0763 |    3261 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |  13,810.810 ns | 100.0906 ns |  88.7277 ns | 0.7324 | 0.6104 |   25621 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  14,206.340 ns | 274.0669 ns | 281.4466 ns | 0.1526 | 0.1221 |    5150 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   3,200.014 ns |  13.6338 ns |  12.7531 ns | 0.0763 | 0.0725 |    2423 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |  12,625.320 ns |  37.5091 ns |  35.0860 ns | 0.2899 | 0.2747 |    9367 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  12,779.589 ns | 100.0403 ns |  93.5778 ns | 0.1221 | 0.0916 |    4643 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     702.750 ns |   1.7346 ns |   1.5377 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   7,542.416 ns |  13.4482 ns |  12.5794 ns | 0.2441 |      - |    7704 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  | 110,360.732 ns | 300.8503 ns | 281.4156 ns | 0.2441 |      - |   12870 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   8,202.080 ns | 118.5704 ns |  92.5720 ns | 0.1831 | 0.1526 |    6613 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |  19,368.871 ns | 139.6328 ns | 123.7809 ns | 1.0071 | 0.9766 |   31160 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       4.114 ns |   0.0561 ns |   0.0497 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.486 ns |   0.0206 ns |   0.0183 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       4.077 ns |   0.1013 ns |   0.1126 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  |  96,268.794 ns | 289.5747 ns | 270.8683 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   3,947.695 ns |  45.8680 ns |  40.6608 ns | 0.0763 | 0.0687 |    2415 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |  17,642.385 ns | 109.6409 ns |  97.1938 ns | 0.7324 | 0.6104 |   26125 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 111,836.196 ns | 318.0842 ns | 248.3392 ns |      - |      - |    7168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   4,540.083 ns |  14.0272 ns |  13.1210 ns | 0.0916 | 0.0763 |    3261 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |  17,568.295 ns | 106.7006 ns |  94.5873 ns | 0.8545 | 0.7324 |   26856 B |
