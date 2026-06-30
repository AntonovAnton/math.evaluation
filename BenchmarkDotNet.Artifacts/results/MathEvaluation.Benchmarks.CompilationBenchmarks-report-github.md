```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8737/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.301
  [Host]    : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error         | StdDev        | Median         | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|--------------:|--------------:|---------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  24,707.562 ns |   479.6857 ns |   533.1694 ns |  24,542.358 ns | 0.1221 | 0.0610 |    5149 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,668.611 ns |    30.2017 ns |    26.7730 ns |   2,664.341 ns | 0.0610 | 0.0458 |    2422 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |   8,208.213 ns |    55.8447 ns |    46.6329 ns |   8,190.649 ns |      - |      - |    3400 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  23,314.256 ns |   280.1953 ns |   262.0948 ns |  23,302.130 ns | 0.1221 | 0.0610 |    4647 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     627.692 ns |     4.2378 ns |     3.7567 ns |     627.076 ns | 0.0238 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   4,322.087 ns |    23.8932 ns |    19.9519 ns |   4,322.835 ns | 0.0458 |      - |    1760 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 124,951.347 ns |   857.5917 ns |   716.1279 ns | 124,740.320 ns | 0.2441 |      - |   12982 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   7,040.989 ns |    30.0359 ns |    25.0813 ns |   7,041.266 ns | 0.1221 | 0.0610 |    6648 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |   8,351.528 ns |   146.2050 ns |   122.0878 ns |   8,329.892 ns | 0.0610 |      - |    3902 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       4.833 ns |     0.0770 ns |     0.0720 ns |       4.830 ns | 0.0006 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       5.357 ns |     0.0358 ns |     0.0334 ns |       5.363 ns | 0.0006 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       5.685 ns |     0.0979 ns |     0.0916 ns |       5.683 ns | 0.0006 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 | 116,414.498 ns | 2,218.3919 ns | 2,278.1252 ns | 115,150.537 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,107.021 ns |    58.2893 ns |    62.3689 ns |   3,106.281 ns | 0.0610 | 0.0458 |    2415 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |   7,753.726 ns |    95.8887 ns |    89.6943 ns |   7,753.784 ns |      - |      - |    3888 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 135,030.141 ns | 1,473.9886 ns | 1,306.6525 ns | 134,524.438 ns |      - |      - |    7264 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   3,700.541 ns |    51.9188 ns |    43.3545 ns |   3,681.599 ns | 0.1221 | 0.1068 |    3358 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |   7,934.172 ns |    71.7054 ns |    67.0733 ns |   7,907.642 ns | 0.1221 | 0.0610 |    4635 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  13,800.428 ns |   154.9084 ns |   144.9015 ns |  13,773.953 ns |      - |      - |    5128 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   2,949.162 ns |    58.0286 ns |    62.0899 ns |   2,971.972 ns | 0.0076 | 0.0038 |    2418 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |   9,416.040 ns |    69.9175 ns |    58.3843 ns |   9,407.378 ns |      - |      - |    3400 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  13,040.626 ns |   186.6891 ns |   165.4950 ns |  13,049.727 ns |      - |      - |    4624 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     692.520 ns |     4.8238 ns |     4.0281 ns |     691.270 ns | 0.0029 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   5,219.955 ns |    49.3721 ns |    46.1827 ns |   5,196.156 ns |      - |      - |    1760 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  | 114,243.823 ns | 1,100.3106 ns |   975.3967 ns | 113,814.612 ns |      - |      - |   12960 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   7,845.259 ns |   116.4968 ns |   134.1579 ns |   7,787.973 ns |      - |      - |    6744 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |   9,848.621 ns |   106.4002 ns |    99.5268 ns |   9,809.807 ns | 0.0153 |      - |    3911 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       4.569 ns |     0.0300 ns |     0.0266 ns |       4.563 ns | 0.0001 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.907 ns |     0.0415 ns |     0.0388 ns |       4.895 ns | 0.0001 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       4.522 ns |     0.0487 ns |     0.0455 ns |       4.497 ns | 0.0001 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  | 101,224.316 ns | 1,250.2738 ns | 1,108.3351 ns | 100,936.536 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   3,376.790 ns |    55.9730 ns |    49.6186 ns |   3,376.908 ns | 0.0076 |      - |    2411 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |   8,943.535 ns |   175.9048 ns |   308.0832 ns |   8,791.656 ns |      - |      - |    4304 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 119,862.510 ns | 1,902.4406 ns | 1,779.5441 ns | 120,039.722 ns |      - |      - |    7261 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   3,985.612 ns |    26.3613 ns |    22.0129 ns |   3,983.834 ns | 0.0076 |      - |    3349 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |   8,849.354 ns |    81.8078 ns |    76.5231 ns |   8,847.919 ns |      - |      - |    5032 B |
