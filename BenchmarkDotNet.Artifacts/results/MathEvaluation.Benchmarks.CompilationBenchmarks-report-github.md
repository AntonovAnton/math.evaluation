```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  24,078.704 ns |   247.5684 ns |   231.5757 ns | 0.1221 | 0.0610 |    5146 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,934.552 ns |    58.0494 ns |    59.6125 ns | 0.0763 | 0.0725 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |  10,723.462 ns |   172.5555 ns |   161.4085 ns | 0.2747 | 0.2441 |    9103 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  23,114.129 ns |   268.2425 ns |   237.7900 ns | 0.1221 | 0.0610 |    4643 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     601.571 ns |     7.7264 ns |     7.2273 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   6,155.073 ns |    94.8946 ns |    88.7645 ns | 0.2365 |      - |    7440 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 123,489.399 ns | 1,076.9222 ns | 1,007.3537 ns | 0.2441 |      - |   12870 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   7,822.408 ns |   154.3233 ns |   235.6680 ns | 0.2136 | 0.1831 |    6614 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |  16,017.489 ns |   306.1020 ns |   314.3442 ns | 0.9155 | 0.8545 |   30095 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       3.767 ns |     0.0980 ns |     0.1342 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       3.766 ns |     0.0565 ns |     0.0501 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       4.012 ns |     0.0390 ns |     0.0346 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 | 110,392.535 ns |   418.2586 ns |   370.7753 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,513.505 ns |    65.8134 ns |    70.4196 ns | 0.0763 | 0.0610 |    2413 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |  14,470.537 ns |   147.6790 ns |   130.9136 ns | 0.7324 | 0.6104 |   24894 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 131,820.306 ns | 2,130.7267 ns | 1,993.0831 ns |      - |      - |    7168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   4,069.872 ns |    75.7290 ns |    98.4691 ns | 0.0916 | 0.0763 |    3261 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |  13,841.156 ns |    82.2526 ns |    68.6846 ns | 0.7324 | 0.6104 |   25765 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  14,037.612 ns |   181.5974 ns |   169.8663 ns | 0.1526 | 0.1221 |    5150 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   3,270.494 ns |    59.8154 ns |    53.0248 ns | 0.0763 | 0.0725 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |  12,400.084 ns |   129.4931 ns |   121.1279 ns | 0.2899 | 0.2747 |    9367 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  13,487.477 ns |   269.2880 ns |   330.7099 ns | 0.1221 | 0.0916 |    4644 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     700.726 ns |     5.6196 ns |     4.9817 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   7,489.441 ns |    96.6631 ns |    90.4187 ns | 0.2441 |      - |    7704 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  | 112,723.369 ns | 1,444.1587 ns | 1,350.8670 ns | 0.2441 |      - |   12950 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   8,739.859 ns |   167.9187 ns |   148.8556 ns | 0.1831 | 0.1526 |    6613 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |  19,713.420 ns |   381.1096 ns |   438.8864 ns | 0.9766 | 0.9155 |   31159 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       3.771 ns |     0.0930 ns |     0.1528 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.471 ns |     0.1091 ns |     0.1213 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       4.068 ns |     0.1053 ns |     0.1640 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  | 100,478.014 ns | 1,373.6043 ns | 1,284.8703 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   4,075.168 ns |    79.9906 ns |    95.2232 ns | 0.0763 | 0.0687 |    2415 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |  18,652.467 ns |   372.2408 ns |   457.1451 ns | 0.7324 | 0.6104 |   26125 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 116,301.673 ns | 1,873.5947 ns | 1,752.5617 ns |      - |      - |    7168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   4,492.782 ns |    24.8542 ns |    19.4045 ns | 0.0916 | 0.0763 |    3261 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |  17,584.829 ns |    94.7157 ns |    83.9630 ns | 0.8545 | 0.7324 |   26855 B |
