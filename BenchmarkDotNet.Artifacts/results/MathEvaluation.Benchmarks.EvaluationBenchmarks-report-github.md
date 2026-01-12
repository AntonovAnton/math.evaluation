```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.101
  [Host]    : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v4


```
| Method                                                                       | Job       | Runtime   | Mean       | Error    | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------- |---------- |---------- |-----------:|---------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0 | .NET 10.0 |   465.8 ns |  1.75 ns |  1.55 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 10.0 | .NET 10.0 | 6,894.8 ns | 18.02 ns | 16.85 ns | 0.1144 |    4464 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0 | .NET 10.0 |   335.2 ns |  0.39 ns |  0.32 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 10.0 | .NET 10.0 | 5,593.9 ns | 22.72 ns | 21.25 ns | 0.0992 |    3752 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 |   362.0 ns |  0.94 ns |  0.84 ns | 0.0196 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 10.0 | .NET 10.0 | 4,509.0 ns | 18.00 ns | 15.03 ns | 0.0687 |    2688 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0 | .NET 10.0 |   482.8 ns |  3.20 ns |  2.99 ns | 0.0238 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 10.0 | .NET 10.0 | 5,007.7 ns | 12.46 ns | 11.04 ns | 0.0610 |    2368 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 10.0 | .NET 10.0 |   821.2 ns |  2.17 ns |  2.03 ns | 0.0238 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 10.0 | .NET 10.0 | 7,080.1 ns | 29.03 ns | 24.24 ns | 0.1221 |    4640 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0  | .NET 8.0  |   603.5 ns |  4.31 ns |  3.82 ns |      - |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0  | .NET 8.0  | 8,087.4 ns | 23.51 ns | 19.63 ns | 0.0153 |    4496 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0  | .NET 8.0  |   396.5 ns |  1.18 ns |  1.04 ns |      - |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0  | .NET 8.0  | 6,383.5 ns | 15.02 ns | 14.05 ns | 0.0153 |    3784 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  |   435.9 ns |  1.53 ns |  1.36 ns | 0.0029 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0  | .NET 8.0  | 5,308.7 ns | 14.06 ns | 13.16 ns | 0.0076 |    2720 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0  | .NET 8.0  |   564.3 ns |  3.10 ns |  2.75 ns | 0.0029 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0  | .NET 8.0  | 5,757.8 ns | 20.85 ns | 18.48 ns | 0.0076 |    2400 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0  | .NET 8.0  |   913.0 ns |  1.90 ns |  1.78 ns | 0.0029 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0  | .NET 8.0  | 8,165.4 ns | 69.39 ns | 61.51 ns | 0.0153 |    4672 B |
