```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4112/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean       | Error    | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |-----------:|---------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |   689.2 ns |  2.70 ns |  2.40 ns | 0.0057 |      80 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 | 8,525.2 ns | 54.92 ns | 51.38 ns | 0.3967 |    5160 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |   504.7 ns |  0.74 ns |  0.61 ns | 0.0057 |      80 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 | 6,815.7 ns | 36.15 ns | 32.05 ns | 0.2899 |    3688 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |   512.7 ns |  1.94 ns |  1.72 ns | 0.0544 |     688 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 | 5,540.5 ns | 23.10 ns | 21.61 ns | 0.1984 |    2496 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 |   734.8 ns |  2.68 ns |  2.51 ns | 0.0668 |     840 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 | 6,044.2 ns | 87.15 ns | 81.52 ns | 0.1755 |    2288 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 6.0 | .NET 6.0 | 1,082.8 ns |  2.68 ns |  2.24 ns | 0.0668 |     840 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 6.0 | .NET 6.0 | 8,310.4 ns | 24.11 ns | 22.55 ns | 0.3815 |    4936 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   599.9 ns |  6.63 ns |  6.20 ns | 0.0057 |      80 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 | 6,378.6 ns | 76.27 ns | 71.35 ns | 0.3510 |    4472 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   387.3 ns |  2.20 ns |  2.06 ns | 0.0062 |      80 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 | 5,221.6 ns | 36.71 ns | 32.54 ns | 0.2823 |    3592 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   413.7 ns |  4.66 ns |  4.36 ns | 0.0548 |     688 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 | 4,313.9 ns | 36.02 ns | 33.69 ns | 0.1984 |    2496 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   524.1 ns |  2.97 ns |  2.63 ns | 0.0668 |     840 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 4,745.3 ns | 35.06 ns | 32.80 ns | 0.1755 |    2288 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0 | .NET 8.0 |   843.6 ns |  6.54 ns |  5.79 ns | 0.0668 |     840 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0 | .NET 8.0 | 6,132.9 ns | 25.84 ns | 22.91 ns | 0.3586 |    4536 B |
