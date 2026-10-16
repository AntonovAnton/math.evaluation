```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.401
  [Host]         : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v4
  .NET 10.0      : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0       : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v4
  NativeAOT 10.0 : .NET 10.0.12, X64 NativeAOT x86-64-v4
  NativeAOT 8.0  : .NET 8.0.31, X64 NativeAOT x86-64-v4


```
| Method                                                                       | Job            | Runtime        | Mean        | Error     | StdDev    | Median      | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------------- |--------------- |------------:|----------:|----------:|------------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0      | .NET 10.0      |    510.1 ns |   2.67 ns |   2.36 ns |    509.7 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 10.0      | .NET 10.0      |  7,579.3 ns |  15.92 ns |  13.29 ns |  7,577.8 ns | 0.2747 |    8744 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0      | .NET 10.0      |    349.3 ns |   0.78 ns |   0.69 ns |    349.3 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 10.0      | .NET 10.0      | 10,645.5 ns |  24.28 ns |  21.52 ns | 10,648.0 ns | 0.7629 |   23864 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0      | .NET 10.0      |    346.3 ns |   0.94 ns |   0.88 ns |    346.1 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 10.0      | .NET 10.0      |  9,673.5 ns |  32.12 ns |  30.04 ns |  9,664.5 ns | 0.7477 |   23312 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0      | .NET 10.0      |    435.7 ns |   1.00 ns |   0.83 ns |    435.7 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 10.0      | .NET 10.0      | 11,612.2 ns |  30.40 ns |  28.43 ns | 11,608.6 ns | 0.9003 |   28464 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 10.0      | .NET 10.0      |    842.2 ns |   1.60 ns |   1.34 ns |    842.1 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 10.0      | .NET 10.0      | 11,894.6 ns |  33.04 ns |  30.90 ns | 11,889.1 ns | 0.7782 |   24560 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0       | .NET 8.0       |    639.0 ns |   2.34 ns |   1.95 ns |    638.4 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0       | .NET 8.0       |  9,253.3 ns |  30.36 ns |  28.40 ns |  9,255.5 ns | 0.2747 |    9008 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0       | .NET 8.0       |    413.9 ns |   3.17 ns |   2.96 ns |    413.4 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0       | .NET 8.0       | 12,755.2 ns |  29.38 ns |  27.48 ns | 12,749.6 ns | 0.7935 |   24728 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0       | .NET 8.0       |    444.3 ns |   2.88 ns |   2.69 ns |    443.9 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0       | .NET 8.0       | 12,053.7 ns |  57.92 ns |  48.36 ns | 12,058.6 ns | 0.7629 |   24176 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0       | .NET 8.0       |    550.7 ns |   8.63 ns |  11.52 ns |    547.0 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0       | .NET 8.0       | 15,109.0 ns | 285.18 ns | 305.14 ns | 15,104.7 ns | 0.9460 |   29528 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0       | .NET 8.0       |    980.6 ns |  15.27 ns |  14.28 ns |    976.5 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0       | .NET 8.0       | 14,472.2 ns |  35.37 ns |  31.36 ns | 14,468.2 ns | 0.8087 |   25424 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | NativeAOT 10.0 | NativeAOT 10.0 |    797.9 ns |   1.21 ns |   1.13 ns |    797.9 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | NativeAOT 10.0 | NativeAOT 10.0 | 11,006.4 ns |  28.03 ns |  26.21 ns | 10,996.7 ns | 0.2747 |    8984 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | NativeAOT 10.0 | NativeAOT 10.0 |    450.4 ns |   0.62 ns |   0.55 ns |    450.2 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | NativeAOT 10.0 | NativeAOT 10.0 | 16,445.4 ns |  25.34 ns |  21.16 ns | 16,445.8 ns | 0.7629 |   24680 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | NativeAOT 10.0 | NativeAOT 10.0 |    454.7 ns |   1.01 ns |   0.95 ns |    454.7 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | NativeAOT 10.0 | NativeAOT 10.0 | 15,478.2 ns |  54.03 ns |  50.54 ns | 15,485.7 ns | 0.7629 |   24128 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | NativeAOT 10.0 | NativeAOT 10.0 |    615.2 ns |   0.76 ns |   0.63 ns |    615.2 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | NativeAOT 10.0 | NativeAOT 10.0 | 18,664.1 ns |  52.00 ns |  48.64 ns | 18,658.9 ns | 0.9155 |   29472 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | NativeAOT 10.0 | NativeAOT 10.0 |  1,060.2 ns |   2.81 ns |   2.63 ns |  1,061.3 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | NativeAOT 10.0 | NativeAOT 10.0 | 18,557.2 ns |  52.38 ns |  43.74 ns | 18,564.5 ns | 0.7935 |   25376 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | NativeAOT 8.0  | NativeAOT 8.0  |    753.3 ns |   1.12 ns |   0.94 ns |    753.1 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | NativeAOT 8.0  | NativeAOT 8.0  | 11,028.2 ns |  53.58 ns |  44.74 ns | 11,012.6 ns | 0.2747 |    8976 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | NativeAOT 8.0  | NativeAOT 8.0  |    439.3 ns |   8.32 ns |  19.29 ns |    429.8 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | NativeAOT 8.0  | NativeAOT 8.0  | 16,244.3 ns |  76.07 ns |  67.44 ns | 16,255.9 ns | 0.7629 |   24648 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | NativeAOT 8.0  | NativeAOT 8.0  |    453.8 ns |   3.75 ns |   3.51 ns |    453.6 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | NativeAOT 8.0  | NativeAOT 8.0  | 14,854.1 ns |  67.09 ns |  52.38 ns | 14,853.7 ns | 0.7629 |   24096 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | NativeAOT 8.0  | NativeAOT 8.0  |    588.6 ns |   6.00 ns |   5.31 ns |    586.8 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | NativeAOT 8.0  | NativeAOT 8.0  | 18,321.5 ns |  80.39 ns |  67.13 ns | 18,308.0 ns | 0.9155 |   29432 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | NativeAOT 8.0  | NativeAOT 8.0  |  1,022.4 ns |   8.37 ns |   7.83 ns |  1,023.5 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | NativeAOT 8.0  | NativeAOT 8.0  | 18,284.7 ns | 164.23 ns | 145.58 ns | 18,237.8 ns | 0.7935 |   25344 B |
