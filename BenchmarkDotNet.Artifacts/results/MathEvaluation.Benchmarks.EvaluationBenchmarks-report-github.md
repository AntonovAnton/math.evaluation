```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8737/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.301
  [Host]    : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4


```
| Method                                                                       | Job       | Runtime   | Mean       | Error    | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------- |---------- |---------- |-----------:|---------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   506.6 ns |  3.21 ns |  2.85 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 10.0 | .NET 10.0 | 5,763.5 ns | 12.98 ns | 11.51 ns | 0.0763 |    2896 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   341.2 ns |  6.45 ns |  5.72 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 10.0 | .NET 10.0 | 4,996.1 ns | 13.72 ns | 12.83 ns | 0.0687 |    2688 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   364.3 ns |  1.76 ns |  1.56 ns | 0.0196 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 10.0 | .NET 10.0 | 4,281.1 ns | 10.55 ns |  9.36 ns | 0.0534 |    2184 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   476.0 ns |  2.89 ns |  2.70 ns | 0.0238 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 | 5,167.5 ns | 73.13 ns | 64.83 ns | 0.0534 |    2228 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 10.0 | .NET 10.0 |   879.5 ns |  9.07 ns |  8.49 ns | 0.0238 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 10.0 | .NET 10.0 | 6,592.0 ns | 65.81 ns | 61.56 ns | 0.0916 |    3488 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   664.5 ns | 13.31 ns | 16.83 ns |      - |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0  | .NET 8.0  | 6,729.4 ns | 16.27 ns | 14.43 ns | 0.0076 |    2896 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   390.9 ns |  4.66 ns |  4.36 ns |      - |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0  | .NET 8.0  | 5,735.4 ns | 51.77 ns | 45.90 ns | 0.0076 |    2800 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   429.1 ns |  4.92 ns |  4.60 ns | 0.0029 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0  | .NET 8.0  | 5,069.1 ns | 86.43 ns | 80.85 ns | 0.0076 |    2296 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   582.3 ns |  4.71 ns |  3.93 ns | 0.0029 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  | 5,982.1 ns | 44.49 ns | 41.61 ns | 0.0076 |    2228 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0  | .NET 8.0  |   974.5 ns |  5.18 ns |  4.84 ns | 0.0019 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0  | .NET 8.0  | 7,578.4 ns | 79.43 ns | 66.33 ns |      - |    3488 B |
