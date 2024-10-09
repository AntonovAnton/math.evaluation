```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4249/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean       | Error    | StdDev    | Median     | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |-----------:|---------:|----------:|-----------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |   674.1 ns |  2.21 ns |   1.84 ns |   673.7 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 | 8,721.4 ns | 41.61 ns |  38.92 ns | 8,722.3 ns | 0.3967 |    5160 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |   440.2 ns |  1.16 ns |   1.03 ns |   439.8 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 | 6,857.0 ns | 34.87 ns |  30.91 ns | 6,856.3 ns | 0.2899 |    3688 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |   464.2 ns |  2.01 ns |   1.78 ns |   463.8 ns | 0.0553 |     696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 | 5,718.6 ns | 65.98 ns |  58.49 ns | 5,701.9 ns | 0.1984 |    2496 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 |   601.5 ns |  2.96 ns |   2.63 ns |   600.4 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 | 6,164.9 ns | 37.58 ns |  35.15 ns | 6,159.9 ns | 0.1678 |    2168 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 6.0 | .NET 6.0 | 1,008.2 ns |  8.73 ns |   8.17 ns | 1,004.9 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 6.0 | .NET 6.0 | 8,506.2 ns | 75.43 ns |  70.56 ns | 8,511.5 ns | 0.3967 |    5104 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   597.2 ns |  6.65 ns |   6.22 ns |   596.9 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 | 6,689.5 ns | 75.87 ns |  70.97 ns | 6,659.2 ns | 0.3510 |    4472 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   343.5 ns |  1.42 ns |   1.26 ns |   343.4 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 | 5,442.0 ns | 20.57 ns |  16.06 ns | 5,444.9 ns | 0.2823 |    3592 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   337.2 ns |  2.01 ns |   1.88 ns |   337.4 ns | 0.0553 |     696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 | 4,642.0 ns | 92.34 ns | 161.72 ns | 4,559.0 ns | 0.1984 |    2496 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   425.7 ns |  1.91 ns |   1.70 ns |   425.0 ns | 0.0672 |     848 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 4,956.1 ns | 16.12 ns |  13.46 ns | 4,957.0 ns | 0.1678 |    2168 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0 | .NET 8.0 |   732.6 ns |  4.09 ns |   3.62 ns |   732.1 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0 | .NET 8.0 | 6,115.6 ns | 24.51 ns |  22.92 ns | 6,111.2 ns | 0.3738 |    4704 B |
