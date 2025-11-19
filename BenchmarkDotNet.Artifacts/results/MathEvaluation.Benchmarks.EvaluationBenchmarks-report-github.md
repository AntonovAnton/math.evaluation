```

BenchmarkDotNet v0.15.7, Windows 11 (10.0.26200.7171/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                                       | Job       | Runtime   | Mean       | Error    | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------- |---------- |---------- |-----------:|---------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   465.7 ns |  1.14 ns |  1.07 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 10.0 | .NET 10.0 | 6,236.1 ns | 26.36 ns | 23.36 ns | 0.1373 |    4480 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   320.6 ns |  2.17 ns |  2.03 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 10.0 | .NET 10.0 | 5,292.6 ns | 23.26 ns | 21.76 ns | 0.1144 |    3672 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   317.5 ns |  2.56 ns |  2.40 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 10.0 | .NET 10.0 | 4,275.0 ns | 16.45 ns | 15.39 ns | 0.0763 |    2576 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   411.1 ns |  2.15 ns |  2.01 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 | 4,623.2 ns | 11.75 ns | 10.42 ns | 0.0687 |    2272 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 10.0 | .NET 10.0 |   729.6 ns |  1.82 ns |  1.52 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 10.0 | .NET 10.0 | 6,396.8 ns | 37.13 ns | 34.74 ns | 0.1450 |    4696 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   591.8 ns |  3.14 ns |  2.94 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0  | .NET 8.0  | 7,225.6 ns | 91.16 ns | 85.27 ns | 0.1450 |    4512 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   354.0 ns |  1.10 ns |  1.03 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0  | .NET 8.0  | 6,185.8 ns | 12.01 ns | 11.23 ns | 0.1144 |    3704 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   373.4 ns |  5.25 ns |  4.91 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0  | .NET 8.0  | 4,954.0 ns | 11.80 ns | 11.04 ns | 0.0763 |    2608 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   506.8 ns |  2.15 ns |  1.91 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  | 5,431.4 ns | 12.29 ns | 11.49 ns | 0.0687 |    2304 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0  | .NET 8.0  |   874.2 ns |  7.22 ns |  6.76 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0  | .NET 8.0  | 7,546.2 ns | 15.32 ns | 13.58 ns | 0.1450 |    4728 B |
