```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                              | Job      | Runtime  | Mean           | Error         | StdDev        | Median         | Gen0   | Gen1   | Allocated |
|---------------------------------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|---------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0 | .NET 8.0 |  14,059.138 ns |   171.0523 ns |   151.6334 ns |  14,015.468 ns | 0.3967 | 0.3815 |    5135 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   3,227.777 ns |    56.5669 ns |   122.9718 ns |   3,174.352 ns | 0.2098 | 0.2060 |    2648 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0 | .NET 8.0 |  30,795.247 ns |   501.4591 ns |   469.0651 ns |  30,622.900 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0 | .NET 8.0 |  12,935.927 ns |   230.8438 ns |   265.8401 ns |  12,823.959 ns | 0.3662 | 0.3510 |    4632 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |   2,897.210 ns |    45.8110 ns |    42.8516 ns |   2,902.579 ns | 0.1602 | 0.1564 |    2024 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0 | .NET 8.0 |  21,264.781 ns |   421.6760 ns |   485.6027 ns |  21,259.679 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0 | .NET 8.0 |  96,436.932 ns | 1,294.0062 ns | 1,147.1028 ns |  96,024.927 ns | 0.4883 | 0.2441 |    9168 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   7,500.935 ns |   147.8781 ns |   284.9108 ns |   7,436.749 ns | 0.3967 | 0.3662 |    5303 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 |  81,659.245 ns | 1,526.1875 ns | 1,427.5967 ns |  81,639.819 ns | 0.4883 | 0.2441 |    6906 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0 | .NET 8.0 |       3.078 ns |     0.0845 ns |     0.1502 ns |       3.050 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       3.949 ns |     0.1028 ns |     0.1689 ns |       3.908 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0 | .NET 8.0 |       2.937 ns |     0.0170 ns |     0.0151 ns |       2.940 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0 | .NET 8.0 |  97,347.887 ns |   382.2538 ns |   357.5605 ns |  97,249.902 ns | 0.3662 | 0.2441 |    5692 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   4,039.733 ns |    23.0100 ns |    21.5236 ns |   4,042.417 ns | 0.2136 | 0.2060 |    2759 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0 | .NET 8.0 |  32,743.351 ns |   195.1642 ns |   191.6773 ns |  32,700.348 ns | 0.6104 | 0.4883 |    7816 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0 | .NET 8.0 | 112,867.651 ns |   603.4256 ns |   564.4447 ns | 112,698.511 ns | 0.4883 | 0.2441 |    7221 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   4,693.218 ns |    78.5644 ns |    73.4892 ns |   4,694.620 ns | 0.2899 | 0.2747 |    3696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0 | .NET 8.0 |  58,233.852 ns |   295.7064 ns |   276.6039 ns |  58,141.479 ns | 0.6104 | 0.4883 |    8358 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 9.0 | .NET 9.0 |  13,767.676 ns |   177.4325 ns |   165.9705 ns |  13,688.690 ns | 0.3967 | 0.3815 |    5135 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |   3,107.982 ns |    21.4199 ns |    18.9882 ns |   3,109.130 ns | 0.2098 | 0.2060 |    2648 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 9.0 | .NET 9.0 |  27,478.266 ns |   187.8955 ns |   175.7576 ns |  27,446.509 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 9.0 | .NET 9.0 |  13,030.720 ns |    58.7885 ns |    52.1145 ns |  13,035.394 ns | 0.3662 | 0.3510 |    4632 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 9.0 | .NET 9.0 |   2,737.820 ns |    31.8866 ns |    29.8268 ns |   2,741.227 ns | 0.1602 | 0.1564 |    2024 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 9.0 | .NET 9.0 |  20,787.768 ns |    98.8075 ns |    82.5087 ns |  20,782.886 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 9.0 | .NET 9.0 |  95,167.989 ns |   457.2731 ns |   357.0088 ns |  95,391.797 ns | 0.7324 | 0.4883 |    9288 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |   6,680.258 ns |    73.8377 ns |    65.4552 ns |   6,660.245 ns | 0.3967 | 0.3662 |    5303 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 |  75,437.552 ns |   554.6154 ns |   463.1290 ns |  75,469.763 ns | 0.4883 | 0.3662 |    6909 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 9.0 | .NET 9.0 |       2.470 ns |     0.0110 ns |     0.0103 ns |       2.470 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 9.0 | .NET 9.0 |       3.776 ns |     0.0240 ns |     0.0213 ns |       3.778 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 9.0 | .NET 9.0 |       2.463 ns |     0.0129 ns |     0.0121 ns |       2.465 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 9.0 | .NET 9.0 |  87,314.820 ns |   360.1526 ns |   336.8869 ns |  87,159.839 ns | 0.3662 | 0.2441 |    5692 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |   3,799.682 ns |    24.1546 ns |    21.4124 ns |   3,800.966 ns | 0.2174 | 0.2136 |    2760 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 9.0 | .NET 9.0 |  31,072.230 ns |   173.4314 ns |   162.2278 ns |  31,049.512 ns | 0.6104 | 0.4883 |    7720 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 9.0 | .NET 9.0 | 104,300.142 ns |   498.8245 ns |   466.6008 ns | 104,323.462 ns | 0.4883 | 0.2441 |    7217 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |   4,500.496 ns |    73.5729 ns |    68.8201 ns |   4,520.026 ns | 0.2899 | 0.2823 |    3696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 9.0 | .NET 9.0 |  56,663.210 ns |   592.4641 ns |   554.1913 ns |  56,465.137 ns | 0.6104 | 0.4883 |    8262 B |
