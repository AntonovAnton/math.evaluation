```

BenchmarkDotNet v0.15.7, Windows 11 (10.0.26200.7171/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error       | StdDev      | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|------------:|------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  19,870.882 ns | 117.6298 ns |  98.2262 ns | 0.1221 | 0.0610 |    5146 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,710.289 ns |  12.0420 ns |  10.6749 ns | 0.0763 | 0.0725 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |   8,699.110 ns |  37.3588 ns |  31.1963 ns | 0.1221 | 0.0916 |    4023 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  20,351.890 ns |  69.5324 ns |  61.6387 ns | 0.1221 | 0.0916 |    4644 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     513.441 ns |   1.9051 ns |   1.5909 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   3,997.265 ns |  14.3706 ns |  12.0001 ns | 0.0458 |      - |    1632 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 104,090.356 ns | 223.6066 ns | 198.2214 ns | 0.2441 |      - |    9013 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   5,727.942 ns |  34.7859 ns |  32.5388 ns | 0.1221 | 0.0916 |    4548 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |   8,014.444 ns |  38.3573 ns |  32.0301 ns | 0.1068 | 0.0916 |    3781 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       3.269 ns |   0.0407 ns |   0.0380 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       4.234 ns |   0.0360 ns |   0.0337 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       4.124 ns |   0.0342 ns |   0.0303 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 |  99,609.002 ns | 393.8914 ns | 368.4462 ns |      - |      - |    5688 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,387.732 ns |  13.9932 ns |  11.6849 ns | 0.0763 | 0.0725 |    2416 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |   8,356.421 ns |  26.6659 ns |  24.9433 ns | 0.1221 | 0.0916 |    4373 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 116,476.578 ns | 812.0552 ns | 719.8657 ns |      - |      - |    7264 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   3,843.428 ns |  19.3182 ns |  16.1316 ns | 0.1068 | 0.0763 |    3358 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |   7,825.131 ns |  78.1631 ns |  73.1138 ns | 0.1526 | 0.1221 |    4765 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  13,196.382 ns |  72.6571 ns |  67.9635 ns | 0.1526 | 0.1373 |    5150 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   2,988.276 ns |  12.6766 ns |  10.5855 ns | 0.0763 | 0.0725 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |  10,110.245 ns |  48.8555 ns |  43.3092 ns | 0.1221 | 0.1068 |    4055 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  13,057.076 ns |  50.4617 ns |  44.7330 ns | 0.1373 | 0.1221 |    4646 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     636.046 ns |   2.2799 ns |   2.0211 ns | 0.0286 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   4,606.727 ns |  12.4004 ns |  10.9927 ns | 0.0534 |      - |    1664 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  |  96,545.301 ns | 262.0117 ns | 218.7916 ns | 0.2441 |      - |    9013 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   6,752.309 ns |  27.0507 ns |  22.5885 ns | 0.1450 | 0.1373 |    4552 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |   9,250.201 ns |  49.5511 ns |  43.9258 ns | 0.1068 | 0.0916 |    3813 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       3.692 ns |   0.0207 ns |   0.0183 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.056 ns |   0.0228 ns |   0.0202 ns | 0.0008 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       4.048 ns |   0.0098 ns |   0.0082 ns | 0.0008 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  |  98,468.031 ns | 135.6459 ns | 120.2465 ns | 0.1221 |      - |    5704 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   3,890.331 ns |  14.8354 ns |  11.5825 ns | 0.0763 | 0.0725 |    2416 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |   9,522.422 ns |  45.1167 ns |  42.2022 ns | 0.1221 | 0.0916 |    4500 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 113,631.663 ns | 246.0412 ns | 218.1091 ns |      - |      - |    7261 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   4,478.205 ns |  16.4291 ns |  15.3678 ns | 0.1068 | 0.0992 |    3360 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |   9,503.792 ns |  86.5286 ns |  80.9389 ns | 0.1526 | 0.1373 |    5007 B |
