```

BenchmarkDotNet v0.15.4, Windows 11 (10.0.26100.6725/24H2/2024Update/HudsonValley)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.305
  [Host]   : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v4
  .NET 8.0 : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v4
  .NET 9.0 : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v4


```
| Method                                                                       | Job      | Runtime  | Mean       | Error     | StdDev    | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |-----------:|----------:|----------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   626.6 ns |  11.73 ns |  10.97 ns | 0.0086 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 | 6,119.4 ns | 120.04 ns | 123.27 ns | 0.3662 |    4608 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   362.7 ns |   6.24 ns |   5.84 ns | 0.0086 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 | 5,133.9 ns |  72.63 ns |  67.94 ns | 0.2899 |    3728 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   334.4 ns |   1.13 ns |   0.88 ns | 0.0587 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 | 4,011.6 ns |  69.86 ns |  65.34 ns | 0.2060 |    2632 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   497.6 ns |   9.91 ns |  22.37 ns | 0.0706 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 4,217.7 ns |  22.62 ns |  20.05 ns | 0.1831 |    2304 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0 | .NET 8.0 |   815.0 ns |   3.32 ns |   3.11 ns | 0.0706 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0 | .NET 8.0 | 6,232.8 ns |  21.91 ns |  19.42 ns | 0.3738 |    4776 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |   545.9 ns |   1.96 ns |   1.64 ns | 0.0086 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 9.0 | .NET 9.0 | 5,454.2 ns |  21.29 ns |  17.77 ns | 0.3586 |    4576 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |   343.4 ns |   4.63 ns |   4.10 ns | 0.0086 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 9.0 | .NET 9.0 | 4,467.8 ns |  18.65 ns |  15.57 ns | 0.2899 |    3696 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |   324.8 ns |   1.07 ns |   0.95 ns | 0.0587 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 9.0 | .NET 9.0 | 3,590.8 ns |   7.72 ns |   6.45 ns | 0.2060 |    2600 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |   447.3 ns |   3.38 ns |   3.16 ns | 0.0710 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 | 3,838.7 ns |  34.15 ns |  31.95 ns | 0.1755 |    2272 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 9.0 | .NET 9.0 |   795.2 ns |  15.75 ns |  13.96 ns | 0.0706 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 9.0 | .NET 9.0 | 5,723.7 ns |  65.11 ns |  60.90 ns | 0.3738 |    4744 B |
