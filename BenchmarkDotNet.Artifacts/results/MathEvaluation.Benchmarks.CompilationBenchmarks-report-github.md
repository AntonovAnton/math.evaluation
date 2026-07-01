```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8737/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.301
  [Host]    : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  24,323.172 ns |   482.4133 ns |   555.5479 ns | 0.1831 | 0.1221 |    5148 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,729.452 ns |    54.2083 ns |    81.1365 ns | 0.0610 | 0.0458 |    2422 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |   8,343.993 ns |   164.7749 ns |   154.1305 ns |      - |      - |    3400 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  22,803.855 ns |   174.9491 ns |   163.6475 ns | 0.1221 | 0.0610 |    4643 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     630.613 ns |     8.5362 ns |     7.9848 ns | 0.0238 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   4,411.112 ns |    87.9112 ns |    86.3406 ns | 0.0458 |      - |    1760 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 124,736.275 ns | 2,080.7656 ns | 2,043.5901 ns | 0.2441 |      - |   12985 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   6,850.031 ns |    64.5519 ns |    53.9038 ns | 0.1221 | 0.0610 |    6648 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |   8,098.861 ns |    96.5482 ns |    90.3112 ns | 0.0610 |      - |    3902 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       4.452 ns |     0.0630 ns |     0.0558 ns | 0.0006 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       4.584 ns |     0.1175 ns |     0.3463 ns | 0.0011 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       5.300 ns |     0.0843 ns |     0.0789 ns | 0.0006 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 | 112,103.507 ns | 1,084.6996 ns |   961.5579 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,047.008 ns |    38.6019 ns |    36.1083 ns | 0.0610 | 0.0458 |    2414 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |   7,805.041 ns |    52.4839 ns |    49.0935 ns | 0.0610 |      - |    3902 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 131,391.800 ns | 1,164.1476 ns | 1,088.9444 ns |      - |      - |    7264 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   3,750.705 ns |    39.8323 ns |    37.2591 ns | 0.0916 | 0.0763 |    3471 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |   7,816.414 ns |    80.9105 ns |    75.6838 ns | 0.1221 | 0.0610 |    4639 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  13,899.902 ns |   225.4145 ns |   199.8241 ns |      - |      - |    5128 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   3,033.592 ns |    60.4531 ns |    56.5478 ns | 0.0076 | 0.0038 |    2418 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |   9,431.571 ns |    60.7470 ns |    56.8228 ns |      - |      - |    3400 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  13,091.859 ns |   120.3913 ns |   106.7238 ns |      - |      - |    4624 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     688.933 ns |     5.2847 ns |     4.9433 ns | 0.0029 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   5,200.008 ns |    43.7928 ns |    40.9638 ns |      - |      - |    1760 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  | 113,802.005 ns | 2,262.4241 ns | 2,222.0030 ns |      - |      - |   12968 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   7,806.492 ns |   121.4998 ns |   113.6510 ns |      - |      - |    6632 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |   9,925.622 ns |   156.0971 ns |   146.0133 ns | 0.0153 |      - |    3911 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       4.401 ns |     0.0446 ns |     0.0395 ns | 0.0001 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.615 ns |     0.0452 ns |     0.0400 ns | 0.0001 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       4.214 ns |     0.0703 ns |     0.0587 ns | 0.0001 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  |  99,387.022 ns | 1,092.9946 ns | 1,022.3878 ns |      - |      - |    5685 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   3,240.743 ns |    26.2691 ns |    21.9359 ns | 0.0076 |      - |    2411 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |   9,018.706 ns |   112.7574 ns |   105.4734 ns |      - |      - |    4304 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 120,130.936 ns | 2,158.8460 ns | 3,361.0633 ns |      - |      - |    7264 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   3,888.879 ns |    65.9480 ns |    55.0696 ns |      - |      - |    3336 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |   9,069.135 ns |   158.3432 ns |   194.4597 ns |      - |      - |    5032 B |
