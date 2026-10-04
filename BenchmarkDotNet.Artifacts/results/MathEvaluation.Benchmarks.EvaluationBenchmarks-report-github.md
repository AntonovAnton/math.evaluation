```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9550/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4


```
| Method                                                                       | Job       | Runtime   | Mean        | Error    | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------- |---------- |---------- |------------:|---------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |    527.5 ns |  1.63 ns |  1.28 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 10.0 | .NET 10.0 |  7,425.2 ns | 15.15 ns | 14.17 ns | 0.2747 |    8744 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |    347.2 ns |  0.64 ns |  0.57 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 10.0 | .NET 10.0 | 10,402.7 ns | 21.79 ns | 19.32 ns | 0.7629 |   23864 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |    342.7 ns |  1.02 ns |  0.95 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 10.0 | .NET 10.0 |  9,579.7 ns | 22.98 ns | 20.37 ns | 0.7477 |   23312 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |    434.6 ns |  1.03 ns |  0.91 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 | 11,720.6 ns | 39.16 ns | 36.63 ns | 0.9003 |   28464 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 10.0 | .NET 10.0 |    827.7 ns |  1.57 ns |  1.47 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 10.0 | .NET 10.0 | 11,836.5 ns | 28.61 ns | 26.76 ns | 0.7782 |   24560 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |    648.7 ns |  2.95 ns |  2.76 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0  | .NET 8.0  |  9,222.8 ns | 19.41 ns | 16.21 ns | 0.2747 |    9008 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |    399.3 ns |  0.70 ns |  0.62 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0  | .NET 8.0  | 12,938.1 ns | 30.13 ns | 28.18 ns | 0.7935 |   24728 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |    427.2 ns |  5.17 ns |  4.84 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0  | .NET 8.0  | 12,067.9 ns | 60.93 ns | 56.99 ns | 0.7629 |   24176 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |    550.3 ns |  0.97 ns |  0.81 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  | 14,895.4 ns | 78.38 ns | 69.48 ns | 0.9460 |   29528 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0  | .NET 8.0  |    960.6 ns |  2.42 ns |  2.15 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0  | .NET 8.0  | 14,303.8 ns | 31.93 ns | 29.87 ns | 0.8087 |   25424 B |
