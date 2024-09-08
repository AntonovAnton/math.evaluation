```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4112/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |  19,959.936 ns |   398.2559 ns |   353.0435 ns | 0.4578 | 0.2136 |    5911 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 |  36,077.338 ns |   600.2674 ns |   561.4905 ns | 0.6714 | 0.3052 |    8919 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 6.0 | .NET 6.0 |  24,240.317 ns |   476.7464 ns |   445.9489 ns | 0.5188 | 0.2441 |    6616 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 6.0 | .NET 6.0 |  25,856.245 ns |   205.9693 ns |   182.5865 ns | 0.3967 | 0.1831 |    5215 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 |  98,030.027 ns |   697.8857 ns |   618.6575 ns | 0.6104 | 0.2441 |    8829 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  82,356.410 ns | 1,093.3762 ns | 1,022.7447 ns | 0.4883 | 0.2441 |    6662 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 6.0 | .NET 6.0 |       4.134 ns |     0.0377 ns |     0.0352 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 6.0 | .NET 6.0 |       3.681 ns |     0.0259 ns |     0.0216 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |  75,715.492 ns | 1,103.8160 ns | 1,032.5102 ns | 0.3662 | 0.1221 |    5707 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 |  36,853.378 ns |   161.9074 ns |   151.4483 ns | 0.6104 | 0.3052 |    8063 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  92,095.243 ns |   464.2458 ns |   387.6663 ns | 0.4883 | 0.2441 |    6901 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 |  66,622.287 ns |   717.6596 ns |   636.1865 ns | 0.6104 | 0.2441 |    8510 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  17,895.342 ns |   161.2193 ns |   150.8047 ns | 0.4578 | 0.4272 |    5911 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  29,967.506 ns |   197.6627 ns |   184.8939 ns | 0.6104 | 0.4883 |    8231 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |  20,550.904 ns |   160.1718 ns |   141.9881 ns | 0.4883 | 0.4272 |    6662 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  21,895.307 ns |   426.7413 ns |   399.1741 ns | 0.3967 | 0.3662 |    5263 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |  93,004.774 ns |   921.0943 ns |   861.5922 ns | 0.4883 | 0.2441 |    8873 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  76,588.903 ns |   403.0938 ns |   377.0542 ns | 0.4883 | 0.2441 |    6710 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       3.209 ns |     0.0315 ns |     0.0294 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       3.325 ns |     0.0298 ns |     0.0264 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |  98,711.475 ns | 1,045.8360 ns |   927.1064 ns | 0.2441 |      - |    5701 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  32,097.432 ns |   173.8600 ns |   154.1224 ns | 0.6104 | 0.4883 |    7967 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 112,895.921 ns |   776.2890 ns |   726.1412 ns | 0.4883 | 0.2441 |    6902 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  58,549.463 ns |   322.8634 ns |   286.2100 ns | 0.6104 | 0.4883 |    8510 B |
