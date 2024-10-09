```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4249/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean           | Error         | StdDev        | Median         | Gen0   | Gen1   | Gen2   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|---------------:|-------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |  15,032.091 ns |   161.3128 ns |   142.9996 ns |  14,979.684 ns | 0.3967 | 0.1984 | 0.0153 |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 |  36,458.860 ns |   206.2827 ns |   161.0520 ns |  36,436.853 ns | 0.6714 | 0.3052 |      - |    8919 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 6.0 | .NET 6.0 |  16,646.587 ns |   330.8435 ns |   786.2846 ns |  16,706.882 ns | 0.3662 | 0.1831 | 0.0153 |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 6.0 | .NET 6.0 |  26,856.595 ns |   536.3212 ns |   910.7142 ns |  26,442.636 ns | 0.4272 | 0.2136 |      - |    5392 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 | 105,314.407 ns | 1,155.7731 ns | 1,081.1109 ns | 105,225.549 ns | 0.6104 | 0.2441 |      - |    9092 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  92,545.790 ns | 1,849.7510 ns | 2,055.9934 ns |  92,928.931 ns | 0.4883 | 0.2441 |      - |    6838 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 6.0 | .NET 6.0 |       3.070 ns |     0.0393 ns |     0.0349 ns |       3.062 ns | 0.0019 |      - |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 6.0 | .NET 6.0 |       2.967 ns |     0.0375 ns |     0.0332 ns |       2.961 ns | 0.0019 |      - |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |  76,173.123 ns |   484.3979 ns |   453.1061 ns |  76,291.577 ns | 0.3662 | 0.1221 |      - |    5683 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 |  39,122.680 ns |   782.0428 ns |   768.0706 ns |  38,854.819 ns | 0.6104 | 0.3052 |      - |    8063 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  94,166.071 ns |   558.0049 ns |   521.9581 ns |  94,154.932 ns | 0.4883 | 0.2441 |      - |    7197 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 |  68,532.528 ns |   552.2548 ns |   516.5795 ns |  68,641.602 ns | 0.6104 | 0.2441 |      - |    8510 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  14,112.013 ns |   276.6931 ns |   307.5436 ns |  14,115.544 ns | 0.3967 | 0.3815 |      - |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  29,156.579 ns |   207.4928 ns |   183.9370 ns |  29,154.022 ns | 0.6104 | 0.4883 |      - |    8231 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |  13,013.848 ns |    66.7061 ns |    62.3969 ns |  13,010.188 ns | 0.3662 | 0.3510 |      - |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  21,008.652 ns |   182.6876 ns |   161.9478 ns |  20,993.329 ns | 0.4272 | 0.3967 |      - |    5440 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |  93,112.742 ns | 1,504.2720 ns | 1,333.4979 ns |  92,679.419 ns | 0.4883 | 0.2441 |      - |    9136 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  79,929.857 ns |   533.3944 ns |   498.9375 ns |  80,066.577 ns | 0.4883 | 0.2441 |      - |    6886 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       2.739 ns |     0.0789 ns |     0.0775 ns |       2.737 ns | 0.0019 |      - |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       2.585 ns |     0.0744 ns |     0.0696 ns |       2.565 ns | 0.0019 |      - |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |  95,694.280 ns | 1,158.4613 ns |   904.4504 ns |  95,989.893 ns | 0.3662 | 0.2441 |      - |    5684 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  32,196.764 ns |   237.5285 ns |   222.1843 ns |  32,215.540 ns | 0.6104 | 0.4883 |      - |    7967 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 109,528.494 ns |   852.4475 ns |   797.3799 ns | 109,669.165 ns | 0.4883 | 0.2441 |      - |    7197 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  57,624.708 ns |   469.5967 ns |   416.2853 ns |  57,461.798 ns | 0.6104 | 0.4883 |      - |    8510 B |
