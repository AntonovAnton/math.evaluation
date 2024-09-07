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
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |  19,375.918 ns |   152.2788 ns |   142.4417 ns | 0.4578 | 0.2136 |    5911 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 |  36,530.747 ns |   710.2549 ns |   664.3729 ns | 0.6714 | 0.3052 |    8919 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 6.0 | .NET 6.0 | 203,433.239 ns | 1,801.5710 ns | 1,685.1907 ns | 1.4648 | 0.7324 |   19191 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 6.0 | .NET 6.0 |  26,121.954 ns |   222.5361 ns |   185.8278 ns | 0.3967 | 0.1831 |    5215 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 | 246,453.034 ns | 2,214.8807 ns | 1,963.4340 ns | 1.4648 | 0.4883 |   21309 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  82,900.978 ns | 1,195.4576 ns | 1,059.7421 ns | 0.4883 | 0.2441 |    6662 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 6.0 | .NET 6.0 |      12.726 ns |     0.2267 ns |     0.2120 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 6.0 | .NET 6.0 |       3.123 ns |     0.0663 ns |     0.0620 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 | 115,960.396 ns | 1,179.9914 ns | 1,103.7647 ns | 0.7324 | 0.2441 |   11364 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 |  37,362.869 ns |   417.0988 ns |   348.2964 ns | 0.6104 | 0.3052 |    8063 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 | 129,985.936 ns | 1,138.3989 ns | 1,064.8590 ns | 0.9766 | 0.4883 |   12552 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 |  66,280.888 ns |   552.3252 ns |   516.6453 ns | 0.6104 | 0.2441 |    8510 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  17,248.048 ns |   172.7821 ns |   144.2809 ns | 0.4578 | 0.4272 |    5911 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  29,190.403 ns |   290.4000 ns |   242.4972 ns | 0.6104 | 0.4883 |    8231 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 | 190,491.061 ns |   507.0693 ns |   423.4258 ns | 1.4648 | 0.9766 |   19184 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  20,867.185 ns |   120.6479 ns |   112.8541 ns | 0.3967 | 0.3662 |    5263 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 | 228,124.697 ns | 1,981.8600 ns | 1,853.8331 ns | 1.4648 | 0.9766 |   21310 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  74,064.520 ns | 1,092.4956 ns |   968.4689 ns | 0.4883 | 0.3662 |    6710 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |      12.098 ns |     0.0560 ns |     0.0468 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       3.005 ns |     0.0336 ns |     0.0298 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 | 112,920.396 ns | 2,192.0705 ns | 2,152.9064 ns | 0.7324 | 0.4883 |   11364 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  31,995.693 ns |   239.5800 ns |   224.1033 ns | 0.6104 | 0.4883 |    7967 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 123,777.485 ns | 1,414.8871 ns | 1,323.4862 ns | 0.9766 | 0.7324 |   12552 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  57,445.363 ns |   339.0292 ns |   300.5405 ns | 0.6104 | 0.4883 |    8510 B |
