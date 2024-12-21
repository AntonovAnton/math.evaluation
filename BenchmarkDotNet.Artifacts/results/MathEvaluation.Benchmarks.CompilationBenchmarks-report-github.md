```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2605)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.101
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  13,462.745 ns |    38.9057 ns |    32.4880 ns | 0.3967 | 0.3815 |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  30,755.682 ns |   230.9866 ns |   204.7636 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |  13,194.486 ns |   159.6657 ns |   141.5395 ns | 0.3662 | 0.3510 |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  21,510.943 ns |   140.9135 ns |   124.9162 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |  94,109.788 ns |   345.6492 ns |   306.4091 ns | 0.4883 | 0.2441 |    9136 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  80,739.289 ns |   430.2243 ns |   402.4321 ns | 0.4883 | 0.2441 |    6910 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       2.804 ns |     0.0237 ns |     0.0210 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       2.785 ns |     0.0535 ns |     0.0474 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |  96,930.879 ns |   462.9301 ns |   410.3755 ns | 0.2441 |      - |    5677 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  33,228.497 ns |   177.7926 ns |   157.6085 ns | 0.6104 | 0.4883 |    7928 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 113,596.297 ns |   782.0415 ns |   731.5221 ns | 0.4883 | 0.2441 |    7197 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  58,660.946 ns |   309.1381 ns |   289.1679 ns | 0.6104 | 0.4883 |    8358 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |  13,612.925 ns |    73.0014 ns |    68.2856 ns | 0.3967 | 0.3815 |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 9.0 | .NET 9.0 |  28,000.249 ns |   194.3222 ns |   172.2616 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 9.0 | .NET 9.0 |  13,182.939 ns |    87.5438 ns |    68.3484 ns | 0.3662 | 0.3510 |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 9.0 | .NET 9.0 |  21,347.308 ns |   144.6080 ns |   135.2665 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |  95,381.278 ns |   451.9385 ns |   422.7436 ns | 0.4883 | 0.2441 |    9133 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |  76,483.894 ns |   431.1805 ns |   360.0552 ns | 0.4883 | 0.2441 |    6909 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 9.0 | .NET 9.0 |       2.725 ns |     0.0155 ns |     0.0130 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 9.0 | .NET 9.0 |       2.736 ns |     0.0320 ns |     0.0267 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |  87,172.635 ns |   388.0211 ns |   343.9706 ns | 0.3662 | 0.2441 |    5684 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 9.0 | .NET 9.0 |  30,832.714 ns |   160.1077 ns |   149.7648 ns | 0.6104 | 0.4883 |    7720 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 | 105,029.481 ns |   654.9600 ns |   546.9213 ns | 0.4883 | 0.2441 |    7197 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 9.0 | .NET 9.0 |  57,656.365 ns | 1,097.9097 ns | 1,174.7511 ns | 0.6104 | 0.4883 |    8262 B |
