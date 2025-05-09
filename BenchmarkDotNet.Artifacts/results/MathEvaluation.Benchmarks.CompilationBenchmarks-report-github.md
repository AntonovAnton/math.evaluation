```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                              | Job      | Runtime  | Mean           | Error         | StdDev      | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0 | .NET 8.0 |  13,720.633 ns |    61.1338 ns |  54.1935 ns | 0.3967 | 0.3815 |    5135 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   3,205.099 ns |    26.0400 ns |  24.3578 ns | 0.2098 | 0.2060 |    2648 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0 | .NET 8.0 |  30,230.079 ns |   164.7474 ns | 137.5715 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0 | .NET 8.0 |  12,928.894 ns |    57.1999 ns |  53.5048 ns | 0.3662 | 0.3510 |    4632 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |   2,803.918 ns |    42.7674 ns |  40.0047 ns | 0.1602 | 0.1564 |    2024 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0 | .NET 8.0 |  20,801.028 ns |   112.6751 ns | 105.3963 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0 | .NET 8.0 |  95,105.977 ns |   444.5049 ns | 415.7902 ns | 0.4883 | 0.2441 |    9168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   7,149.900 ns |   109.8346 ns |  97.3655 ns | 0.3967 | 0.3662 |    5303 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 |  80,309.798 ns |   261.4414 ns | 244.5524 ns | 0.4883 | 0.2441 |    6910 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0 | .NET 8.0 |       2.794 ns |     0.0263 ns |   0.0220 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       3.600 ns |     0.0211 ns |   0.0176 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0 | .NET 8.0 |       2.712 ns |     0.0146 ns |   0.0129 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0 | .NET 8.0 |  97,660.151 ns | 1,066.9636 ns | 998.0384 ns | 0.3662 | 0.2441 |    5692 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   3,957.297 ns |    21.6560 ns |  19.1975 ns | 0.2136 | 0.2060 |    2759 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0 | .NET 8.0 |  32,734.252 ns |   158.5208 ns | 140.5246 ns | 0.6104 | 0.4883 |    7816 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 | 113,155.700 ns |   653.5563 ns | 611.3370 ns | 0.4883 | 0.2441 |    7221 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   4,708.957 ns |    52.1752 ns |  46.2520 ns | 0.2899 | 0.2823 |    3696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0 | .NET 8.0 |  58,321.455 ns |   710.0546 ns | 592.9277 ns | 0.6104 | 0.4883 |    8358 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 9.0 | .NET 9.0 |  13,457.440 ns |    87.6374 ns |  81.9761 ns | 0.3967 | 0.3815 |    5135 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |   3,102.565 ns |    17.9515 ns |  15.9135 ns | 0.2098 | 0.2060 |    2648 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 9.0 | .NET 9.0 |  27,876.751 ns |   173.0254 ns | 153.3825 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 9.0 | .NET 9.0 |  13,164.021 ns |    67.8299 ns |  63.4481 ns | 0.3662 | 0.3510 |    4632 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 9.0 | .NET 9.0 |   2,693.913 ns |    40.4083 ns |  35.8209 ns | 0.1526 | 0.1373 |    2023 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 9.0 | .NET 9.0 |  20,442.261 ns |   101.8197 ns |  95.2422 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 9.0 | .NET 9.0 |  94,585.615 ns |   533.0424 ns | 498.6082 ns | 0.4883 | 0.2441 |    9165 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |   6,809.283 ns |   133.9283 ns | 164.4760 ns | 0.3967 | 0.3662 |    5303 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 |  75,438.363 ns |   415.3600 ns | 388.5280 ns | 0.4883 | 0.3662 |    6909 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 9.0 | .NET 9.0 |       2.360 ns |     0.0641 ns |   0.0630 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 9.0 | .NET 9.0 |       3.597 ns |     0.0265 ns |   0.0207 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 9.0 | .NET 9.0 |       2.256 ns |     0.0205 ns |   0.0182 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 9.0 | .NET 9.0 |  87,027.797 ns |   356.6514 ns | 333.6120 ns | 0.3662 | 0.2441 |    5692 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |   3,903.547 ns |    28.3757 ns |  23.6950 ns | 0.2136 | 0.2060 |    2759 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 9.0 | .NET 9.0 |  30,669.928 ns |   151.9122 ns | 134.6662 ns | 0.6104 | 0.4883 |    7720 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 | 104,253.221 ns |   472.8507 ns | 419.1698 ns | 0.4883 | 0.2441 |    7221 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |   4,575.992 ns |    76.2122 ns |  90.7252 ns | 0.2899 | 0.2747 |    3696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 9.0 | .NET 9.0 |  56,671.080 ns |   188.7002 ns | 176.5103 ns | 0.6104 | 0.4883 |    8374 B |
