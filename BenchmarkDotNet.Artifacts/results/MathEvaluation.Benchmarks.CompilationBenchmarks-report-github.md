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
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  23,714.491 ns |  74.3857 ns |  62.1154 ns | 0.1221 | 0.0610 |    5146 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,813.061 ns |  12.7333 ns |  11.2878 ns | 0.0763 | 0.0725 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |  10,395.089 ns |  35.7141 ns |  31.6596 ns | 0.2747 | 0.2441 |    9103 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  23,071.042 ns |  83.1475 ns |  77.7762 ns | 0.1221 | 0.0610 |    4644 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     578.497 ns |   1.0117 ns |   0.8969 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   6,190.994 ns |  17.1918 ns |  15.2401 ns | 0.2365 |      - |    7440 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 120,964.087 ns | 333.1082 ns | 295.2917 ns | 0.2441 |      - |   12982 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   6,939.235 ns | 103.0353 ns |  96.3792 ns | 0.2136 | 0.1831 |    6653 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |  15,399.115 ns |  39.2404 ns |  34.7856 ns | 0.9155 | 0.8545 |   30103 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       3.352 ns |   0.0096 ns |   0.0075 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       3.769 ns |   0.0147 ns |   0.0123 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       3.986 ns |   0.0087 ns |   0.0068 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 | 110,209.166 ns | 282.4262 ns | 250.3635 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,539.434 ns |  47.5313 ns |  42.1353 ns | 0.0763 | 0.0610 |    2413 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |  14,124.071 ns |  49.4198 ns |  46.2273 ns | 0.7935 | 0.7324 |   24896 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 129,782.347 ns | 241.3975 ns | 213.9926 ns |      - |      - |    7264 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   4,002.066 ns |  28.2159 ns |  25.0126 ns | 0.1068 | 0.0916 |    3359 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |  13,952.622 ns |  78.4607 ns |  73.3922 ns | 0.7324 | 0.6104 |   25621 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  14,141.599 ns |  80.1235 ns |  74.9475 ns | 0.1526 | 0.1221 |    5150 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   3,109.853 ns |  12.2346 ns |  11.4443 ns | 0.0763 | 0.0725 |    2423 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |  12,395.092 ns |  46.9044 ns |  43.8744 ns | 0.2441 | 0.1831 |    9363 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  13,045.650 ns |  67.3099 ns |  62.9617 ns | 0.1221 | 0.0916 |    4644 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     701.552 ns |   1.7262 ns |   1.6147 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   7,337.453 ns |  13.6061 ns |  12.0614 ns | 0.2441 |      - |    7704 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  | 110,125.531 ns | 285.6209 ns | 267.1699 ns | 0.2441 |      - |   12982 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   8,045.719 ns |  33.2261 ns |  31.0797 ns | 0.2136 | 0.1831 |    6656 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |  18,975.814 ns | 283.9109 ns | 251.6796 ns | 1.0071 | 0.9766 |   31168 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       3.651 ns |   0.0585 ns |   0.0519 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.484 ns |   0.1050 ns |   0.0982 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       3.801 ns |   0.0302 ns |   0.0268 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  |  96,773.354 ns | 660.8678 ns | 585.8421 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   3,858.521 ns |  15.9916 ns |  14.1762 ns | 0.0763 | 0.0725 |    2416 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |  18,044.369 ns | 290.7697 ns | 242.8059 ns | 0.7324 | 0.6104 |   26125 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 112,614.853 ns | 383.7718 ns | 320.4668 ns | 0.2441 |      - |    7400 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   4,767.546 ns |  49.1259 ns |  45.9524 ns | 0.1068 | 0.0992 |    3360 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |  18,175.802 ns | 132.2652 ns | 110.4474 ns | 0.8545 | 0.7324 |   26856 B |
