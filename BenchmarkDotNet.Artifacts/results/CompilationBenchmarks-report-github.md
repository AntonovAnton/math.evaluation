```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4169/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |  19,938.791 ns |   166.2195 ns |   138.8008 ns | 0.4883 | 0.2441 |    6439 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 |  36,920.265 ns |   713.5946 ns |   927.8745 ns | 0.6714 | 0.3052 |    8919 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 6.0 | .NET 6.0 |  24,230.694 ns |   163.0016 ns |   144.4967 ns | 0.5493 | 0.2747 |    7040 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 6.0 | .NET 6.0 |  26,380.959 ns |   381.7797 ns |   338.4377 ns | 0.3967 | 0.1831 |    5215 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 | 100,971.944 ns | 1,651.1639 ns | 1,544.4997 ns | 0.7324 | 0.3662 |    9256 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  83,576.104 ns |   948.3688 ns |   887.1048 ns | 0.4883 | 0.2441 |    6662 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 6.0 | .NET 6.0 |       2.936 ns |     0.0271 ns |     0.0253 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 6.0 | .NET 6.0 |       3.556 ns |     0.0905 ns |     0.1006 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |  78,986.703 ns | 1,479.4636 ns | 1,453.0311 ns | 0.3662 | 0.1221 |    6090 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 |  40,382.823 ns |   793.0421 ns | 1,058.6884 ns | 0.6104 | 0.3052 |    8063 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |  97,566.627 ns | 1,686.6910 ns | 1,577.7318 ns | 0.4883 | 0.2441 |    7189 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 |  70,452.834 ns | 1,368.8036 ns | 1,576.3163 ns | 0.6104 | 0.2441 |    8510 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  18,447.447 ns |   252.3220 ns |   223.6769 ns | 0.4883 | 0.4578 |    6439 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  32,740.270 ns |   638.7286 ns | 1,013.0913 ns | 0.6104 | 0.4883 |    8231 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |  23,697.547 ns |   464.6382 ns |   604.1610 ns | 0.5493 | 0.4883 |    7088 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  24,406.666 ns |   475.4540 ns |   547.5336 ns | 0.3967 | 0.3662 |    5263 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |  97,830.970 ns | 1,544.3126 ns | 1,444.5510 ns | 0.7324 | 0.4883 |    9304 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  78,474.557 ns |   972.4896 ns |   909.6674 ns | 0.4883 | 0.2441 |    6710 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       2.830 ns |     0.0815 ns |     0.1737 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       3.307 ns |     0.0829 ns |     0.1453 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 | 105,298.874 ns | 2,079.5129 ns | 4,006.5153 ns | 0.2441 |      - |    6084 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  34,885.354 ns |   226.5149 ns |   211.8822 ns | 0.6104 | 0.4883 |    7967 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 114,326.405 ns |   640.1140 ns |   598.7630 ns | 0.4883 | 0.2441 |    7185 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  60,732.883 ns |   256.0143 ns |   239.4759 ns | 0.6104 | 0.4883 |    8510 B |
