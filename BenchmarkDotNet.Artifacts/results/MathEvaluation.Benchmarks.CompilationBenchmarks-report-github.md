```

BenchmarkDotNet v0.15.4, Windows 11 (10.0.26100.6725/24H2/2024Update/HudsonValley)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.305
  [Host]   : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v4
  .NET 8.0 : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v4
  .NET 9.0 : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0 | .NET 8.0 |  13,607.047 ns |   136.8098 ns |   121.2783 ns | 0.3967 | 0.3815 |    5151 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   3,182.349 ns |    62.9799 ns |    77.3450 ns | 0.1907 | 0.1869 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0 | .NET 8.0 |   9,435.548 ns |   152.8457 ns |   142.9720 ns | 0.3204 | 0.3052 |    4056 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0 | .NET 8.0 |  14,052.214 ns |   279.4030 ns |   601.4444 ns | 0.3662 | 0.3510 |    4648 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |     590.624 ns |    11.5563 ns |    11.8674 ns | 0.0706 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0 | .NET 8.0 |   3,760.356 ns |    16.5616 ns |    13.8297 ns | 0.1297 |      - |    1664 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0 | .NET 8.0 |  94,513.954 ns |   440.1072 ns |   390.1435 ns | 0.4883 | 0.2441 |    8961 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   7,066.511 ns |    54.9675 ns |    51.4166 ns | 0.3586 | 0.3510 |    4504 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 |   8,288.365 ns |    20.0719 ns |    17.7932 ns | 0.2899 | 0.2747 |    3815 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0 | .NET 8.0 |       2.774 ns |     0.0087 ns |     0.0073 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       3.182 ns |     0.0216 ns |     0.0191 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0 | .NET 8.0 |       2.998 ns |     0.0196 ns |     0.0164 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0 | .NET 8.0 |  98,469.912 ns |   436.6455 ns |   340.9041 ns | 0.3662 | 0.2441 |    5707 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   3,790.700 ns |    19.0057 ns |    14.8384 ns | 0.1907 | 0.1869 |    2416 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0 | .NET 8.0 |   8,434.588 ns |    43.8017 ns |    36.5764 ns | 0.3052 | 0.2441 |    4501 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 | 114,493.619 ns |   529.6701 ns |   469.5387 ns | 0.4883 | 0.2441 |    7348 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   4,533.409 ns |    65.6965 ns |    61.4525 ns | 0.2594 | 0.2518 |    3312 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0 | .NET 8.0 |   8,491.982 ns |    84.2885 ns |    74.7195 ns | 0.3662 | 0.3052 |    4895 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 9.0 | .NET 9.0 |  13,839.190 ns |    56.5734 ns |    47.2414 ns | 0.3967 | 0.3662 |    5151 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |   2,983.985 ns |    15.6671 ns |    13.8884 ns | 0.1907 | 0.1869 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 9.0 | .NET 9.0 |   8,452.014 ns |    63.2376 ns |    52.8062 ns | 0.3204 | 0.2899 |    4024 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 9.0 | .NET 9.0 |  14,075.166 ns |   279.3360 ns |   503.7005 ns | 0.3662 | 0.3357 |    4648 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 9.0 | .NET 9.0 |     560.633 ns |    10.2374 ns |    16.5315 ns | 0.0706 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 9.0 | .NET 9.0 |   3,396.435 ns |    64.2837 ns |    60.1311 ns | 0.1297 |      - |    1632 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 9.0 | .NET 9.0 |  95,099.175 ns |   857.9586 ns |   802.5350 ns | 0.4883 | 0.2441 |    8961 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |   6,394.234 ns |    77.5717 ns |    64.7759 ns | 0.3052 | 0.2441 |    4501 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 |   8,246.813 ns |   163.4815 ns |   194.6131 ns | 0.2899 | 0.2747 |    3783 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 9.0 | .NET 9.0 |       2.535 ns |     0.0727 ns |     0.0680 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 9.0 | .NET 9.0 |       3.407 ns |     0.0297 ns |     0.0248 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 9.0 | .NET 9.0 |       3.332 ns |     0.0268 ns |     0.0224 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 9.0 | .NET 9.0 |  86,964.123 ns |   263.1364 ns |   246.1380 ns | 0.3662 | 0.2441 |    5707 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |   3,638.133 ns |    17.5919 ns |    15.5947 ns | 0.1907 | 0.1869 |    2416 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 9.0 | .NET 9.0 |   8,456.303 ns |   164.6458 ns |   208.2243 ns | 0.3357 | 0.3204 |    4375 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 | 105,550.910 ns | 1,271.7566 ns | 1,127.3791 ns | 0.4883 | 0.2441 |    7236 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |   4,458.288 ns |    79.1682 ns |    74.0540 ns | 0.2594 | 0.2518 |    3312 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 9.0 | .NET 9.0 |   8,049.932 ns |   152.2549 ns |   142.4194 ns | 0.3662 | 0.3052 |    4767 B |
