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
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |    509.4 ns |  1.85 ns |  1.73 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 10.0 | .NET 10.0 |  7,443.8 ns | 28.70 ns | 23.97 ns | 0.2747 |    8744 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |    346.9 ns |  0.79 ns |  0.70 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 10.0 | .NET 10.0 | 10,324.4 ns | 19.57 ns | 17.35 ns | 0.7629 |   23864 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |    344.9 ns |  0.94 ns |  0.83 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 10.0 | .NET 10.0 |  9,611.4 ns | 29.37 ns | 26.03 ns | 0.7477 |   23312 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |    441.0 ns |  1.32 ns |  1.24 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 | 11,643.1 ns | 29.87 ns | 26.48 ns | 0.9003 |   28464 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 10.0 | .NET 10.0 |    831.6 ns |  3.36 ns |  2.98 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 10.0 | .NET 10.0 | 11,777.8 ns | 99.36 ns | 82.97 ns | 0.7782 |   24560 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |    659.4 ns |  5.20 ns |  4.61 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0  | .NET 8.0  |  9,168.4 ns | 41.77 ns | 32.61 ns | 0.2747 |    9008 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |    412.4 ns |  6.43 ns |  6.88 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0  | .NET 8.0  | 13,144.1 ns | 35.48 ns | 31.46 ns | 0.7935 |   24728 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |    434.0 ns |  1.14 ns |  0.95 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0  | .NET 8.0  | 11,859.7 ns | 24.40 ns | 19.05 ns | 0.7629 |   24176 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |    551.9 ns |  1.86 ns |  1.65 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  | 14,449.1 ns | 78.33 ns | 73.27 ns | 0.9460 |   29528 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0  | .NET 8.0  |    973.6 ns |  4.01 ns |  3.75 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0  | .NET 8.0  | 14,412.9 ns | 44.83 ns | 41.94 ns | 0.8087 |   25424 B |
