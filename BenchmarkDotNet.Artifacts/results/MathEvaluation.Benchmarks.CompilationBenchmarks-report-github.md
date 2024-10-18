```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.403
  [Host]   : .NET 6.0.35 (6.0.3524.45918), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.35 (6.0.3524.45918), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean           | Error       | StdDev      | Gen0   | Gen1   | Gen2   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |---------------:|------------:|------------:|-------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |  15,176.552 ns | 257.3184 ns | 240.6958 ns | 0.3967 | 0.1831 |      - |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 |  35,845.244 ns | 404.6195 ns | 378.4813 ns | 0.6714 | 0.3052 |      - |    8919 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 6.0 | .NET 6.0 |  14,920.835 ns | 208.9740 ns | 185.2500 ns | 0.3662 | 0.1831 | 0.0153 |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 6.0 | .NET 6.0 |  25,273.058 ns | 254.6441 ns | 225.7354 ns | 0.4272 | 0.2136 |      - |    5392 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 |  99,531.152 ns | 567.0599 ns | 502.6838 ns | 0.6104 | 0.2441 |      - |    9092 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  86,846.239 ns | 765.2832 ns | 715.8463 ns | 0.4883 | 0.2441 |      - |    6838 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 6.0 | .NET 6.0 |       3.222 ns |   0.0259 ns |   0.0216 ns | 0.0019 |      - |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 6.0 | .NET 6.0 |       3.135 ns |   0.0276 ns |   0.0230 ns | 0.0019 |      - |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |  74,037.199 ns | 448.4599 ns | 397.5480 ns | 0.3662 | 0.1221 |      - |    5683 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 |  37,172.991 ns | 258.6947 ns | 241.9831 ns | 0.6104 | 0.3052 |      - |    8063 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  92,404.747 ns | 621.9347 ns | 581.7581 ns | 0.4883 | 0.2441 |      - |    7197 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 |  66,249.732 ns | 447.1234 ns | 418.2395 ns | 0.6104 | 0.2441 |      - |    8510 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  13,457.831 ns | 123.8281 ns | 115.8289 ns | 0.3967 | 0.3815 |      - |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  29,498.074 ns | 200.4612 ns | 177.7036 ns | 0.6104 | 0.4883 |      - |    8231 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |  13,171.918 ns |  74.8048 ns |  66.3125 ns | 0.3662 | 0.3510 |      - |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  20,863.286 ns |  89.3353 ns |  83.5643 ns | 0.4272 | 0.3967 |      - |    5440 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |  94,345.635 ns | 992.9555 ns | 880.2292 ns | 0.4883 | 0.2441 |      - |    9136 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  79,188.520 ns | 395.6972 ns | 350.7753 ns | 0.4883 | 0.2441 |      - |    6886 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       2.981 ns |   0.0278 ns |   0.0217 ns | 0.0019 |      - |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       2.959 ns |   0.0281 ns |   0.0249 ns | 0.0019 |      - |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |  96,168.293 ns | 377.6030 ns | 334.7352 ns | 0.3662 | 0.2441 |      - |    5684 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  32,203.231 ns | 307.3480 ns | 287.4935 ns | 0.6104 | 0.4883 |      - |    7967 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 111,269.899 ns | 741.2004 ns | 657.0549 ns | 0.4883 | 0.2441 |      - |    7193 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  58,387.107 ns | 132.1551 ns | 110.3555 ns | 0.6104 | 0.4883 |      - |    8510 B |
