```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |  15,706.412 ns |   293.5587 ns |   274.5950 ns | 0.3967 | 0.1831 |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 |  37,497.083 ns |   717.9330 ns |   797.9806 ns | 0.6714 | 0.3052 |    8919 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 6.0 | .NET 6.0 |  16,205.590 ns |   320.3479 ns |   405.1375 ns | 0.3662 | 0.1831 |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 6.0 | .NET 6.0 |  26,695.286 ns |   314.8632 ns |   279.1181 ns | 0.4272 | 0.2136 |    5392 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 | 102,120.099 ns | 1,399.6251 ns | 1,168.7504 ns | 0.6104 | 0.2441 |    9092 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  89,973.983 ns | 1,435.5364 ns | 1,342.8016 ns | 0.4883 | 0.2441 |    6838 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 6.0 | .NET 6.0 |       2.969 ns |     0.0516 ns |     0.0483 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 6.0 | .NET 6.0 |       2.873 ns |     0.0780 ns |     0.0730 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |  76,717.016 ns |   237.2181 ns |   221.8940 ns | 0.3662 | 0.1221 |    5683 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 |  39,198.571 ns |   289.6283 ns |   226.1227 ns | 0.6104 | 0.3052 |    8063 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  94,491.298 ns |   431.1265 ns |   360.0101 ns | 0.4883 | 0.2441 |    7197 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 |  70,590.610 ns | 1,385.6001 ns | 1,296.0912 ns | 0.6104 | 0.2441 |    8510 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  14,834.608 ns |   291.7561 ns |   272.9089 ns | 0.3967 | 0.3662 |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  31,079.342 ns |   616.9309 ns |   605.9086 ns | 0.6104 | 0.4883 |    8231 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |  15,202.500 ns |   303.6168 ns |   298.1923 ns | 0.3662 | 0.3357 |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  22,969.240 ns |   303.1965 ns |   283.6102 ns | 0.4272 | 0.3967 |    5440 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |  95,344.228 ns | 1,259.4879 ns | 1,116.5032 ns | 0.4883 | 0.2441 |    9136 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  83,610.477 ns | 1,629.6416 ns | 1,524.3678 ns | 0.4883 | 0.2441 |    6886 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       3.361 ns |     0.0599 ns |     0.0500 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       3.310 ns |     0.0763 ns |     0.0713 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |  98,521.537 ns | 1,750.0002 ns | 1,636.9513 ns | 0.3662 | 0.2441 |    5684 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  34,681.006 ns |   411.0152 ns |   364.3543 ns | 0.6104 | 0.4883 |    7967 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 115,757.847 ns | 1,253.7040 ns | 1,172.7155 ns | 0.4883 | 0.2441 |    7197 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  61,955.998 ns |   772.0599 ns |   722.1853 ns | 0.6104 | 0.4883 |    8510 B |
