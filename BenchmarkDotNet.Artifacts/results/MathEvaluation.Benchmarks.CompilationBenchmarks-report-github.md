```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9711)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4


```
| Method                                                                                              | Job       | Runtime   | Mean           | Error         | StdDev        | Gen0   | Gen1   | Gen2   | Allocated |
|---------------------------------------------------------------------------------------------------- |---------- |---------- |---------------:|--------------:|--------------:|-------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 10.0 | .NET 10.0 |  24,349.262 ns |    82.6132 ns |    77.2764 ns | 0.1221 | 0.0610 |      - |    5137 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   2,842.359 ns |    16.6582 ns |    14.7671 ns | 0.0763 | 0.0725 |      - |    2416 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 10.0 | .NET 10.0 |  10,647.086 ns |    55.9017 ns |    52.2905 ns | 0.2747 | 0.2441 |      - |    9103 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 10.0 | .NET 10.0 |  23,481.065 ns |   100.3709 ns |    93.8870 ns | 0.1221 | 0.0610 |      - |    4636 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 10.0 | .NET 10.0 |     599.074 ns |     1.4072 ns |     1.2474 ns | 0.0277 |      - |      - |     888 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 10.0 | .NET 10.0 |   6,185.530 ns |     6.8693 ns |     6.0894 ns | 0.2365 |      - |      - |    7440 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 10.0 | .NET 10.0 | 122,995.299 ns |   698.1722 ns |   653.0707 ns | 0.2441 |      - |      - |   12862 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   7,082.671 ns |    30.1168 ns |    28.1713 ns | 0.1831 | 0.1526 |      - |    6605 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 |  16,230.184 ns |    90.5940 ns |    80.3092 ns | 0.9766 | 0.9155 |      - |   30096 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 10.0 | .NET 10.0 |       3.374 ns |     0.0115 ns |     0.0096 ns | 0.0008 |      - |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 10.0 | .NET 10.0 |       3.767 ns |     0.0188 ns |     0.0167 ns | 0.0008 |      - |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 10.0 | .NET 10.0 |       4.005 ns |     0.0093 ns |     0.0078 ns | 0.0008 |      - |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 10.0 | .NET 10.0 | 112,324.159 ns |   586.8427 ns |   520.2208 ns |      - |      - |      - |    5680 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   3,516.507 ns |    59.6856 ns |    52.9097 ns | 0.0916 | 0.0610 | 0.0153 |    2404 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 10.0 | .NET 10.0 |  14,266.668 ns |    41.9678 ns |    37.2034 ns | 0.7935 | 0.7324 |      - |   24896 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 10.0 | .NET 10.0 | 132,407.788 ns |   577.8460 ns |   512.2454 ns |      - |      - |      - |    7160 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   3,877.428 ns |    34.3661 ns |    30.4646 ns | 0.0916 | 0.0763 |      - |    3253 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 10.0 | .NET 10.0 |  13,944.218 ns |    93.7612 ns |    87.7043 ns | 0.7935 | 0.7324 |      - |   25623 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                        | .NET 8.0  | .NET 8.0  |  14,001.998 ns |    99.9762 ns |    78.0548 ns | 0.1526 | 0.1221 |      - |    5142 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   3,191.160 ns |    14.6696 ns |    13.0042 ns | 0.0763 | 0.0725 |      - |    2416 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                                | .NET 8.0  | .NET 8.0  |  13,207.056 ns |    76.2968 ns |    71.3681 ns | 0.2441 | 0.1831 |      - |    9364 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                                            | .NET 8.0  | .NET 8.0  |  12,984.119 ns |    68.9406 ns |    64.4871 ns | 0.1221 | 0.0916 |      - |    4636 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0  | .NET 8.0  |     738.039 ns |     5.1181 ns |     4.7875 ns | 0.0277 |      - |      - |     888 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                                                    | .NET 8.0  | .NET 8.0  |   7,847.130 ns |    40.8808 ns |    38.2399 ns | 0.2441 |      - |      - |    7704 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                          | .NET 8.0  | .NET 8.0  | 111,877.033 ns |   430.3955 ns |   402.5923 ns | 0.2441 |      - |      - |   12862 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   8,387.016 ns |    54.5721 ns |    51.0468 ns | 0.1831 | 0.0916 |      - |    6605 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  |  20,339.376 ns |    54.1233 ns |    45.1954 ns | 1.0071 | 0.9766 |      - |   31160 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                   | .NET 8.0  | .NET 8.0  |       3.698 ns |     0.0125 ns |     0.0104 ns | 0.0008 |      - |      - |      24 B |
| &#39;MathEvaluator.FastExpressionCompiler: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0  | .NET 8.0  |       4.365 ns |     0.0612 ns |     0.0543 ns | 0.0008 |      - |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                                           | .NET 8.0  | .NET 8.0  |       3.794 ns |     0.0084 ns |     0.0070 ns | 0.0008 |      - |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                            | .NET 8.0  | .NET 8.0  |  99,575.585 ns | 1,915.2655 ns | 2,422.1978 ns |      - |      - |      - |    5680 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   3,822.646 ns |    13.0548 ns |    12.2115 ns | 0.0763 | 0.0725 |      - |    2408 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                                    | .NET 8.0  | .NET 8.0  |  18,042.669 ns |    75.7350 ns |    63.2421 ns | 0.7324 | 0.6104 |      - |   26125 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                  | .NET 8.0  | .NET 8.0  | 113,719.141 ns |   386.8492 ns |   361.8590 ns |      - |      - |      - |    7160 B |
| &#39;MathEvaluator.FastExpressionCompiler: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   4,497.179 ns |    20.1000 ns |    17.8181 ns | 0.0916 | 0.0763 |      - |    3253 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                          | .NET 8.0  | .NET 8.0  |  18,379.553 ns |   312.6742 ns |   360.0761 ns | 0.8545 | 0.7324 |      - |   26856 B |
