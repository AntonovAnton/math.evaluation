```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4652/24H2/2024Update/HudsonValley)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.302
  [Host]   : .NET 8.0.18 (8.0.1825.31117), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.18 (8.0.1825.31117), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                              | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0 | .NET 8.0 |  14,527.173 ns |   272.2460 ns |   291.3002 ns | 0.3967 | 0.3662 |    5151 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   3,026.418 ns |    45.1166 ns |    37.6744 ns | 0.1907 | 0.1869 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0 | .NET 8.0 |   9,204.492 ns |   180.4199 ns |   221.5718 ns | 0.3052 | 0.2899 |    3951 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0 | .NET 8.0 |  13,685.492 ns |   216.5182 ns |   231.6720 ns | 0.3662 | 0.3510 |    4648 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |     636.478 ns |     9.8371 ns |     8.2145 ns | 0.0706 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0 | .NET 8.0 |   3,768.484 ns |    36.5079 ns |    32.3633 ns | 0.1221 |      - |    1560 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0 | .NET 8.0 |  94,596.503 ns |   530.7728 ns |   443.2193 ns | 0.4883 | 0.2441 |    8953 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   7,180.145 ns |   140.3868 ns |   167.1205 ns | 0.3357 | 0.3052 |    4503 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 |   8,785.997 ns |   173.9761 ns |   238.1405 ns | 0.2899 | 0.2747 |    3712 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0 | .NET 8.0 |       2.760 ns |     0.0747 ns |     0.0623 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       3.156 ns |     0.0874 ns |     0.0818 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0 | .NET 8.0 |       3.390 ns |     0.0914 ns |     0.2118 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0 | .NET 8.0 | 102,553.949 ns | 1,588.4412 ns | 1,485.8289 ns | 0.3662 | 0.2441 |    5707 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   4,123.207 ns |    82.4134 ns |   184.3294 ns | 0.1907 | 0.1831 |    2416 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0 | .NET 8.0 |   8,624.898 ns |   163.8384 ns |   136.8125 ns | 0.3052 | 0.2441 |    4397 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 | 114,588.981 ns | 1,550.9773 ns | 1,374.9009 ns | 0.4883 | 0.2441 |    7228 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   4,727.067 ns |    93.3143 ns |   139.6685 ns | 0.2594 | 0.2518 |    3312 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0 | .NET 8.0 |   8,544.144 ns |    70.0948 ns |    54.7254 ns | 0.3662 | 0.3052 |    4791 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 9.0 | .NET 9.0 |  13,953.649 ns |   266.5651 ns |   261.8026 ns | 0.3967 | 0.3815 |    5151 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |   3,100.755 ns |    61.7783 ns |   148.0168 ns | 0.1907 | 0.1869 |    2424 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 9.0 | .NET 9.0 |   8,432.044 ns |    57.7356 ns |    48.2119 ns | 0.3052 | 0.2899 |    3951 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 9.0 | .NET 9.0 |  13,160.850 ns |   149.7980 ns |   132.7920 ns | 0.3662 | 0.3357 |    4648 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 9.0 | .NET 9.0 |     555.827 ns |     8.9250 ns |     7.9118 ns | 0.0706 |      - |     896 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 9.0 | .NET 9.0 |   3,375.077 ns |    24.4784 ns |    20.4406 ns | 0.1221 |      - |    1560 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 9.0 | .NET 9.0 |  94,416.035 ns |   963.8299 ns |   854.4101 ns | 0.4883 | 0.2441 |    9033 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |   6,855.317 ns |   136.7288 ns |   348.0186 ns | 0.3052 | 0.2441 |    4501 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 |   7,840.709 ns |   154.0190 ns |   144.0695 ns | 0.2899 | 0.2747 |    3712 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 9.0 | .NET 9.0 |       2.694 ns |     0.0764 ns |     0.0750 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 9.0 | .NET 9.0 |       3.540 ns |     0.0466 ns |     0.0436 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 9.0 | .NET 9.0 |       3.514 ns |     0.0492 ns |     0.0460 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 9.0 | .NET 9.0 |  89,489.059 ns |   992.1341 ns |   879.5010 ns | 0.3662 | 0.2441 |    5707 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |   3,866.386 ns |    75.3339 ns |    73.9880 ns | 0.1907 | 0.1869 |    2416 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 9.0 | .NET 9.0 |   8,360.452 ns |   159.2098 ns |   207.0178 ns | 0.3357 | 0.3204 |    4304 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 | 106,849.171 ns | 2,073.3927 ns | 2,304.5705 ns | 0.4883 | 0.2441 |    7228 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |   4,442.019 ns |    61.8002 ns |    57.8080 ns | 0.2594 | 0.2518 |    3312 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 9.0 | .NET 9.0 |   7,969.489 ns |    39.3250 ns |    32.8381 ns | 0.3662 | 0.3052 |    4696 B |
