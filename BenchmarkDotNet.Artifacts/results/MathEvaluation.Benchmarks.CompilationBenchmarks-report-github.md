```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                              | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0 | .NET 8.0 |  14,704.412 ns |   293.7311 ns |   638.5474 ns | 0.3967 | 0.3815 |    5135 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   3,288.909 ns |    65.3020 ns |    80.1967 ns | 0.2098 | 0.2060 |    2648 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0 | .NET 8.0 |  29,958.639 ns |   174.0412 ns |   162.7982 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0 | .NET 8.0 |  13,226.780 ns |    56.1391 ns |    49.7659 ns | 0.3662 | 0.3510 |    4632 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |     588.045 ns |     3.2745 ns |     2.5565 ns | 0.0696 |      - |     880 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0 | .NET 8.0 |  21,176.903 ns |    95.7428 ns |    89.5579 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0 | .NET 8.0 |  94,390.612 ns |   378.0951 ns |   353.6704 ns | 0.4883 | 0.2441 |    9168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   7,695.583 ns |   131.7229 ns |   116.7690 ns | 0.3967 | 0.3662 |    5263 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 |  80,151.979 ns |   435.1462 ns |   385.7457 ns | 0.4883 | 0.2441 |    6910 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0 | .NET 8.0 |       3.070 ns |     0.0853 ns |     0.1401 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       3.987 ns |     0.0999 ns |     0.1263 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0 | .NET 8.0 |       3.136 ns |     0.0846 ns |     0.1266 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0 | .NET 8.0 |  99,464.147 ns | 1,030.7658 ns |   964.1789 ns | 0.3662 | 0.2441 |    5692 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   4,240.839 ns |    84.5169 ns |   160.8022 ns | 0.2136 | 0.2060 |    2759 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0 | .NET 8.0 |  33,074.417 ns |   557.1387 ns |   493.8889 ns | 0.6104 | 0.4883 |    7816 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 | 114,704.567 ns | 1,257.0305 ns | 1,175.8271 ns | 0.4883 | 0.2441 |    7221 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   4,902.062 ns |    83.1734 ns |    77.8004 ns | 0.2899 | 0.2747 |    3656 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0 | .NET 8.0 |  58,659.364 ns |   402.3730 ns |   376.3800 ns | 0.6104 | 0.4883 |    8358 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 9.0 | .NET 9.0 |  13,730.969 ns |   115.3624 ns |   107.9101 ns | 0.3967 | 0.3815 |    5135 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |   3,167.627 ns |    62.2330 ns |   125.7138 ns | 0.1984 | 0.1831 |    2647 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 9.0 | .NET 9.0 |  27,909.961 ns |   384.9976 ns |   360.1270 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 9.0 | .NET 9.0 |  13,292.315 ns |   208.7958 ns |   195.3077 ns | 0.3662 | 0.3510 |    4632 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 9.0 | .NET 9.0 |     557.287 ns |     4.6293 ns |     4.3303 ns | 0.0696 |      - |     880 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 9.0 | .NET 9.0 |  21,391.181 ns |   427.4500 ns |   438.9596 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 9.0 | .NET 9.0 |  95,873.143 ns |   892.8195 ns |   791.4612 ns | 0.4883 | 0.2441 |    9168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |   6,853.105 ns |    59.9552 ns |    56.0822 ns | 0.3662 | 0.3052 |    5261 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 |  75,966.463 ns |   907.9159 ns |   849.2651 ns | 0.4883 | 0.3662 |    6989 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 9.0 | .NET 9.0 |       2.499 ns |     0.0251 ns |     0.0223 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 9.0 | .NET 9.0 |       3.897 ns |     0.0971 ns |     0.1193 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 9.0 | .NET 9.0 |       2.491 ns |     0.0294 ns |     0.0245 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 9.0 | .NET 9.0 |  88,235.887 ns | 1,022.5266 ns |   853.8560 ns | 0.3662 | 0.2441 |    5692 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |   3,962.956 ns |    78.0963 ns |   116.8909 ns | 0.2136 | 0.2060 |    2759 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 9.0 | .NET 9.0 |  30,997.121 ns |   255.8574 ns |   239.3291 ns | 0.6104 | 0.4883 |    7720 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 | 104,266.382 ns |   464.8062 ns |   434.7800 ns | 0.4883 | 0.2441 |    7221 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |   4,450.380 ns |    82.8804 ns |    77.5264 ns | 0.2747 | 0.2441 |    3655 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 9.0 | .NET 9.0 |  56,543.354 ns |   294.4010 ns |   275.3829 ns | 0.6104 | 0.4883 |    8262 B |
