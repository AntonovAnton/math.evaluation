```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9711)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]         : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0      : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0       : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  NativeAOT 10.0 : .NET 10.0.14, X64 NativeAOT x86-64-v4
  NativeAOT 8.0  : .NET 8.0.33, X64 NativeAOT x86-64-v4


```
| Method                                                                       | Job            | Runtime        | Mean        | Error     | StdDev    | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------------- |--------------- |------------:|----------:|----------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0      | .NET 10.0      |    521.4 ns |   1.86 ns |   1.65 ns | 0.0029 |     104 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 10.0      | .NET 10.0      |  7,687.5 ns |  32.09 ns |  30.01 ns | 0.2747 |    8744 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0      | .NET 10.0      |    351.1 ns |   0.99 ns |   0.92 ns | 0.0033 |     104 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 10.0      | .NET 10.0      | 10,688.4 ns |  34.62 ns |  30.69 ns | 0.7629 |   23864 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0      | .NET 10.0      |    354.5 ns |   0.83 ns |   0.69 ns | 0.0229 |     728 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 10.0      | .NET 10.0      |  9,649.5 ns |  28.41 ns |  26.58 ns | 0.7477 |   23312 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0      | .NET 10.0      |    437.8 ns |   0.66 ns |   0.59 ns | 0.0281 |     888 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 10.0      | .NET 10.0      | 11,973.1 ns |  41.99 ns |  37.23 ns | 0.9003 |   28464 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 10.0      | .NET 10.0      |    880.9 ns |  17.51 ns |  20.85 ns | 0.0277 |     888 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 10.0      | .NET 10.0      | 12,296.9 ns | 132.08 ns | 123.55 ns | 0.7782 |   24560 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0       | .NET 8.0       |    675.0 ns |   8.97 ns |   8.39 ns | 0.0029 |     104 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0       | .NET 8.0       |  9,424.5 ns |  19.20 ns |  17.96 ns | 0.2747 |    9008 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0       | .NET 8.0       |    415.1 ns |   1.22 ns |   1.08 ns | 0.0033 |     104 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0       | .NET 8.0       | 13,391.2 ns |  64.82 ns |  57.46 ns | 0.7935 |   24728 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0       | .NET 8.0       |    431.4 ns |   0.92 ns |   0.86 ns | 0.0234 |     728 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0       | .NET 8.0       | 12,293.2 ns |  31.78 ns |  28.17 ns | 0.7629 |   24176 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0       | .NET 8.0       |    581.9 ns |   2.22 ns |   1.97 ns | 0.0277 |     888 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0       | .NET 8.0       | 14,630.2 ns |  28.11 ns |  26.29 ns | 0.9460 |   29528 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0       | .NET 8.0       |  1,016.5 ns |   2.37 ns |   2.10 ns | 0.0267 |     888 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0       | .NET 8.0       | 15,030.3 ns |  33.84 ns |  31.66 ns | 0.8087 |   25424 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | NativeAOT 10.0 | NativeAOT 10.0 |    787.0 ns |   2.35 ns |   2.08 ns | 0.0029 |     104 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | NativeAOT 10.0 | NativeAOT 10.0 | 11,003.3 ns |  77.94 ns |  69.09 ns | 0.2747 |    8984 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | NativeAOT 10.0 | NativeAOT 10.0 |    451.6 ns |   1.88 ns |   1.67 ns | 0.0033 |     104 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | NativeAOT 10.0 | NativeAOT 10.0 | 16,176.0 ns |  60.74 ns |  50.72 ns | 0.7629 |   24680 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | NativeAOT 10.0 | NativeAOT 10.0 |    459.3 ns |   0.84 ns |   0.79 ns | 0.0234 |     728 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | NativeAOT 10.0 | NativeAOT 10.0 | 14,771.6 ns |  29.98 ns |  26.57 ns | 0.7629 |   24128 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | NativeAOT 10.0 | NativeAOT 10.0 |    618.7 ns |   1.99 ns |   1.87 ns | 0.0277 |     888 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | NativeAOT 10.0 | NativeAOT 10.0 | 18,386.8 ns |  56.01 ns |  52.39 ns | 0.9155 |   29472 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | NativeAOT 10.0 | NativeAOT 10.0 |  1,078.8 ns |   3.13 ns |   2.93 ns | 0.0267 |     888 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | NativeAOT 10.0 | NativeAOT 10.0 | 18,329.3 ns |  57.62 ns |  48.11 ns | 0.7935 |   25376 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | NativeAOT 8.0  | NativeAOT 8.0  |    730.8 ns |   2.02 ns |   1.89 ns | 0.0029 |     104 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | NativeAOT 8.0  | NativeAOT 8.0  | 11,195.5 ns |  25.54 ns |  22.64 ns | 0.2747 |    8976 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | NativeAOT 8.0  | NativeAOT 8.0  |    416.8 ns |   1.52 ns |   1.42 ns | 0.0033 |     104 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | NativeAOT 8.0  | NativeAOT 8.0  | 16,538.0 ns |  62.13 ns |  58.12 ns | 0.7629 |   24648 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | NativeAOT 8.0  | NativeAOT 8.0  |    453.6 ns |   1.19 ns |   1.11 ns | 0.0229 |     728 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | NativeAOT 8.0  | NativeAOT 8.0  | 15,379.5 ns |  43.65 ns |  40.83 ns | 0.7629 |   24096 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | NativeAOT 8.0  | NativeAOT 8.0  |    597.2 ns |   3.13 ns |   2.78 ns | 0.0277 |     888 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | NativeAOT 8.0  | NativeAOT 8.0  | 18,873.8 ns |  58.98 ns |  55.17 ns | 0.9155 |   29432 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | NativeAOT 8.0  | NativeAOT 8.0  |  1,032.8 ns |   5.16 ns |   4.83 ns | 0.0267 |     888 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | NativeAOT 8.0  | NativeAOT 8.0  | 19,141.9 ns |  52.23 ns |  46.30 ns | 0.7935 |   25344 B |
