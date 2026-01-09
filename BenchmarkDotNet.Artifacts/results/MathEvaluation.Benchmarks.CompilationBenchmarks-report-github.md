```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  19,553.916 ns |   145.5905 ns |   121.5747 ns | 0.1221 | 0.0610 |    5149 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,466.274 ns |    17.2732 ns |    16.1574 ns | 0.0648 | 0.0610 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |   9,362.130 ns |   183.0296 ns |   237.9902 ns |      - |      - |    3920 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  19,340.825 ns |   355.6776 ns |   332.7011 ns | 0.1831 | 0.1221 |    4643 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     566.520 ns |     5.5625 ns |     5.2032 ns | 0.0238 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   4,190.031 ns |    20.8270 ns |    19.4816 ns | 0.0381 |      - |    1704 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 103,819.023 ns | 1,460.7881 ns | 1,366.4221 ns | 0.2441 |      - |    9232 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   5,874.581 ns |    71.0684 ns |    63.0003 ns | 0.1221 | 0.0610 |    4767 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |   8,092.062 ns |   129.0021 ns |   120.6687 ns | 0.0610 |      - |    3846 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       3.936 ns |     0.1007 ns |     0.1237 ns | 0.0006 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       5.223 ns |     0.0463 ns |     0.0433 ns | 0.0006 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       5.066 ns |     0.0560 ns |     0.0524 ns | 0.0006 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 |  98,511.795 ns |   345.1120 ns |   305.9328 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   2,917.152 ns |    11.1271 ns |     9.8639 ns | 0.0648 | 0.0610 |    2416 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |   8,221.353 ns |    56.1071 ns |    52.4827 ns | 0.0610 |      - |    4388 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 116,828.703 ns |   634.6904 ns |   529.9952 ns |      - |      - |    7953 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   3,819.235 ns |    17.5615 ns |    16.4270 ns | 0.1068 | 0.1030 |    4048 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |   8,278.304 ns |    31.2710 ns |    27.7209 ns | 0.1221 | 0.0610 |    4838 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  13,194.431 ns |    30.8421 ns |    27.3407 ns | 0.0153 |      - |    5145 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   2,961.196 ns |    57.9305 ns |   104.4606 ns | 0.0076 | 0.0038 |    2419 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |  11,050.654 ns |    58.0814 ns |    54.3294 ns | 0.0153 |      - |    3975 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  12,268.271 ns |    26.5749 ns |    20.7479 ns |      - |      - |    4624 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     674.428 ns |     1.5634 ns |     1.3859 ns | 0.0029 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   5,013.947 ns |    14.9353 ns |    13.2398 ns |      - |      - |    1736 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  |  96,848.464 ns |   858.6181 ns |   803.1519 ns |      - |      - |    9304 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   6,272.566 ns |   100.8804 ns |    89.4278 ns |      - |      - |    4952 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |   9,553.018 ns |    38.7816 ns |    34.3789 ns | 0.0153 |      - |    3887 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       3.898 ns |     0.0183 ns |     0.0162 ns | 0.0001 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.396 ns |     0.0222 ns |     0.0197 ns | 0.0001 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       4.399 ns |     0.0129 ns |     0.0108 ns | 0.0001 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  |  96,975.007 ns |   617.6519 ns |   547.5323 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   3,236.138 ns |    25.3969 ns |    22.5137 ns | 0.0076 |      - |    2411 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |   9,792.432 ns |    31.5651 ns |    29.5260 ns | 0.0153 |      - |    4524 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 114,314.204 ns |   742.2626 ns |   657.9965 ns |      - |      - |    8017 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   4,553.061 ns |    40.1625 ns |    37.5680 ns | 0.0153 | 0.0076 |    4110 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |   9,006.036 ns |   101.1480 ns |    89.6651 ns |      - |      - |    4944 B |
