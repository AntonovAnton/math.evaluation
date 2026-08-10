```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8973/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.302
  [Host]    : .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  24,544.066 ns |   169.7166 ns |   141.7211 ns | 0.1221 | 0.0610 |    5149 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,703.619 ns |    13.9944 ns |    13.0904 ns | 0.0648 | 0.0610 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |  10,730.045 ns |   153.7903 ns |   143.8556 ns | 0.2441 |      - |    9078 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  23,278.474 ns |   247.3629 ns |   193.1247 ns | 0.1221 | 0.0610 |    4647 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     632.780 ns |     3.2217 ns |     2.6902 ns | 0.0238 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   6,371.815 ns |    21.2157 ns |    19.8452 ns | 0.1984 |      - |    7424 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 122,519.289 ns |   271.7652 ns |   240.9127 ns | 0.2441 |      - |   12985 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   7,056.341 ns |    84.1863 ns |    70.2994 ns | 0.1221 | 0.0610 |    6648 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |  16,896.620 ns |   227.3123 ns |   201.5065 ns | 0.7324 | 0.1221 |   30086 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       4.741 ns |     0.0560 ns |     0.0523 ns | 0.0006 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       5.302 ns |     0.0504 ns |     0.0471 ns | 0.0006 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       5.495 ns |     0.0423 ns |     0.0395 ns | 0.0010 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 | 111,746.754 ns |   566.2720 ns |   472.8628 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,114.994 ns |    23.4136 ns |    20.7555 ns | 0.0610 | 0.0458 |    2414 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |  14,798.580 ns |   109.6666 ns |    97.2166 ns | 0.6104 | 0.1221 |   24878 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 132,147.604 ns |   688.1032 ns |   643.6522 ns |      - |      - |    7264 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   3,651.670 ns |    40.2748 ns |    37.6731 ns | 0.0916 | 0.0610 |    3467 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |  14,684.109 ns |   160.2960 ns |   149.9410 ns | 0.6104 | 0.1221 |   25605 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  13,645.541 ns |    72.4198 ns |    64.1983 ns |      - |      - |    5128 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   3,022.778 ns |    34.6824 ns |    32.4419 ns | 0.0076 | 0.0038 |    2418 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |  12,738.904 ns |    60.5383 ns |    56.6275 ns | 0.0305 | 0.0153 |    9347 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  12,827.464 ns |   133.1922 ns |   111.2215 ns |      - |      - |    4624 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     714.982 ns |     2.6625 ns |     2.2233 ns | 0.0029 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   7,880.621 ns |    51.2018 ns |    45.3890 ns | 0.0305 |      - |    7688 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  | 110,280.474 ns |   890.2438 ns |   832.7346 ns |      - |      - |   12968 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   7,626.056 ns |    77.8709 ns |    69.0305 ns |      - |      - |    6632 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |  20,259.345 ns |    66.3046 ns |    58.7773 ns | 0.1221 | 0.0916 |   31151 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       4.525 ns |     0.0143 ns |     0.0119 ns | 0.0001 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.781 ns |     0.0125 ns |     0.0098 ns | 0.0001 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       4.391 ns |     0.0185 ns |     0.0164 ns | 0.0001 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  |  97,555.090 ns |   722.2691 ns |   603.1274 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   3,364.162 ns |    37.2167 ns |    32.9916 ns | 0.0076 |      - |    2411 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |  19,150.733 ns |   111.2924 ns |   104.1029 ns |      - |      - |   26088 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 113,985.465 ns | 1,859.8672 ns | 1,739.7209 ns |      - |      - |    7264 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   4,018.963 ns |    37.6976 ns |    35.2623 ns | 0.0076 |      - |    3349 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |  19,060.871 ns |   180.6190 ns |   168.9511 ns |      - |      - |   26816 B |
