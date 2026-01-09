```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                                                         | Job       | Runtime   | Mean       | Error    | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------------------------- |---------- |---------- |-----------:|---------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                   | .NET 10.0 | .NET 10.0 |   470.5 ns |  1.03 ns |  0.91 ns | 0.0029 |     112 B |
| &#39;MathEvaluator (Evaluate&lt;float&gt;): &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   506.4 ns |  0.99 ns |  0.93 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                           | .NET 10.0 | .NET 10.0 | 6,870.7 ns | 22.45 ns | 21.00 ns | 0.1144 |    4464 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                       | .NET 10.0 | .NET 10.0 |   341.1 ns |  0.97 ns |  0.91 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                               | .NET 10.0 | .NET 10.0 | 5,558.4 ns | 19.46 ns | 18.20 ns | 0.0992 |    3752 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                             | .NET 10.0 | .NET 10.0 |   351.8 ns |  1.16 ns |  1.03 ns | 0.0196 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                     | .NET 10.0 | .NET 10.0 | 4,523.0 ns | 13.89 ns | 12.31 ns | 0.0687 |    2688 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                     | .NET 10.0 | .NET 10.0 |   462.6 ns |  2.42 ns |  2.14 ns | 0.0238 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                             | .NET 10.0 | .NET 10.0 | 5,088.9 ns | 16.34 ns | 15.29 ns | 0.0610 |    2368 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   792.4 ns |  2.85 ns |  2.52 ns | 0.0238 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                                           | .NET 10.0 | .NET 10.0 | 7,112.7 ns | 21.32 ns | 19.95 ns | 0.1221 |    4640 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                   | .NET 8.0  | .NET 8.0  |   564.8 ns |  5.45 ns |  4.83 ns |      - |     112 B |
| &#39;MathEvaluator (Evaluate&lt;float&gt;): &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   619.1 ns |  3.12 ns |  2.77 ns |      - |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;                           | .NET 8.0  | .NET 8.0  | 8,130.1 ns | 41.99 ns | 37.22 ns | 0.0153 |    4496 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                       | .NET 8.0  | .NET 8.0  |   379.6 ns |  1.97 ns |  1.75 ns |      - |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                                               | .NET 8.0  | .NET 8.0  | 6,412.3 ns | 24.83 ns | 23.23 ns | 0.0153 |    3784 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                                             | .NET 8.0  | .NET 8.0  |   399.5 ns |  2.43 ns |  2.27 ns | 0.0029 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                                     | .NET 8.0  | .NET 8.0  | 5,374.0 ns | 17.58 ns | 16.44 ns | 0.0076 |    2720 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                                     | .NET 8.0  | .NET 8.0  |   521.6 ns |  6.19 ns |  5.79 ns | 0.0029 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                                             | .NET 8.0  | .NET 8.0  | 5,828.5 ns | 23.98 ns | 22.43 ns | 0.0076 |    2400 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   881.1 ns |  3.03 ns |  2.84 ns | 0.0029 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                                           | .NET 8.0  | .NET 8.0  | 8,183.9 ns | 33.91 ns | 31.72 ns | 0.0153 |    4672 B |
