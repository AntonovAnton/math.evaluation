```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2605)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.101
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean       | Error    | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |-----------:|---------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   608.8 ns |  1.77 ns |  1.48 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 | 6,262.8 ns | 47.27 ns | 44.22 ns | 0.3510 |    4496 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   339.9 ns |  0.75 ns |  0.66 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 | 5,107.1 ns | 35.14 ns | 32.87 ns | 0.2823 |    3616 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   356.3 ns |  1.85 ns |  1.64 ns | 0.0553 |     696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 | 4,079.7 ns | 44.64 ns | 39.57 ns | 0.1984 |    2520 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   404.6 ns |  2.54 ns |  2.12 ns | 0.0672 |     848 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 4,171.4 ns | 13.31 ns | 11.11 ns | 0.1678 |    2192 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0 | .NET 8.0 |   732.9 ns | 10.86 ns | 10.16 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0 | .NET 8.0 | 6,072.3 ns | 22.83 ns | 21.35 ns | 0.3738 |    4728 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |   526.4 ns |  2.81 ns |  2.63 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 9.0 | .NET 9.0 | 5,643.3 ns | 29.76 ns | 27.84 ns | 0.3510 |    4496 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |   322.0 ns |  1.76 ns |  1.47 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 9.0 | .NET 9.0 | 4,714.3 ns | 22.31 ns | 18.63 ns | 0.2823 |    3616 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |   314.8 ns |  1.84 ns |  1.63 ns | 0.0553 |     696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 9.0 | .NET 9.0 | 3,576.2 ns | 32.18 ns | 30.10 ns | 0.1984 |    2520 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |   404.4 ns |  1.74 ns |  1.54 ns | 0.0672 |     848 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 | 3,764.9 ns | 23.00 ns | 21.52 ns | 0.1678 |    2192 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 9.0 | .NET 9.0 |   698.2 ns |  5.14 ns |  4.81 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 9.0 | .NET 9.0 | 5,484.3 ns | 32.07 ns | 30.00 ns | 0.3738 |    4728 B |
