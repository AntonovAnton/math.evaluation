```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4249/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |  21,621.283 ns |   432.3001 ns |   779.5262 ns | 0.4883 | 0.2441 |    6319 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 |  36,991.048 ns |   694.6759 ns |   682.2647 ns | 0.6714 | 0.3052 |    8919 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 6.0 | .NET 6.0 |  25,113.862 ns |   479.0103 ns |   512.5356 ns | 0.5493 | 0.2747 |    7048 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 6.0 | .NET 6.0 |  26,783.351 ns |   531.1424 ns |   690.6352 ns | 0.4272 | 0.2136 |    5392 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 | 102,096.461 ns | 1,930.8384 ns | 1,806.1074 ns | 0.7324 | 0.3662 |    9288 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  89,962.640 ns | 1,710.4040 ns | 1,901.1095 ns | 0.4883 | 0.2441 |    6838 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 6.0 | .NET 6.0 |       2.978 ns |     0.0811 ns |     0.0759 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 6.0 | .NET 6.0 |       2.707 ns |     0.0772 ns |     0.0826 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |  78,277.008 ns | 1,243.2715 ns | 1,102.1278 ns | 0.3662 | 0.1221 |    6098 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 |  39,106.573 ns |   777.7102 ns |   864.4228 ns | 0.6104 | 0.3052 |    8063 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  96,905.549 ns | 1,888.1332 ns | 2,318.7972 ns | 0.4883 | 0.2441 |    7212 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 |  68,334.575 ns | 1,355.3005 ns | 2,301.4031 ns | 0.6104 | 0.2441 |    8510 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  17,562.366 ns |   123.6890 ns |   115.6987 ns | 0.4883 | 0.4578 |    6319 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  29,378.627 ns |   315.7658 ns |   295.3675 ns | 0.6104 | 0.4883 |    8231 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |  20,464.586 ns |    84.9375 ns |    70.9266 ns | 0.5493 | 0.4883 |    7096 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  20,960.880 ns |   132.8135 ns |   124.2338 ns | 0.4272 | 0.3967 |    5440 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |  96,676.328 ns | 1,276.7964 ns | 1,194.3161 ns | 0.7324 | 0.4883 |    9333 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  82,374.993 ns |   416.8497 ns |   389.9215 ns | 0.4883 | 0.2441 |    6886 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       2.690 ns |     0.0443 ns |     0.0415 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       2.532 ns |     0.0182 ns |     0.0162 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |  99,942.552 ns |   529.4654 ns |   495.2623 ns | 0.2441 |      - |    6092 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  32,332.343 ns |   295.0342 ns |   275.9752 ns | 0.6104 | 0.4883 |    7967 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 113,262.664 ns |   497.6357 ns |   465.4887 ns | 0.4883 | 0.2441 |    7213 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  60,655.757 ns |   460.5054 ns |   430.7571 ns | 0.6104 | 0.4883 |    8510 B |
