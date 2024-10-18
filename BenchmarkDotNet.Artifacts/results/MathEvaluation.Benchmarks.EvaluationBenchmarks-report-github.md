```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.403
  [Host]   : .NET 6.0.35 (6.0.3524.45918), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.35 (6.0.3524.45918), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean       | Error     | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |-----------:|----------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |   687.0 ns |   3.73 ns |  3.49 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 | 8,698.2 ns |  32.23 ns | 30.14 ns | 0.3967 |    5160 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |   437.7 ns |   1.47 ns |  1.38 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 | 6,929.2 ns |  25.73 ns | 22.81 ns | 0.2899 |    3688 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |   480.9 ns |   1.59 ns |  1.49 ns | 0.0553 |     696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 | 5,628.4 ns |  30.15 ns | 28.20 ns | 0.1984 |    2496 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 |   601.8 ns |   2.78 ns |  2.32 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 | 6,465.0 ns |  19.00 ns | 17.77 ns | 0.1678 |    2168 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 6.0 | .NET 6.0 |   977.5 ns |   3.33 ns |  2.78 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 6.0 | .NET 6.0 | 8,696.6 ns | 101.79 ns | 95.22 ns | 0.3967 |    5104 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   601.3 ns |   1.86 ns |  1.65 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 | 6,674.4 ns |  21.31 ns | 19.94 ns | 0.3510 |    4472 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   335.5 ns |   2.82 ns |  2.63 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 | 5,497.3 ns |  22.88 ns | 20.29 ns | 0.2823 |    3592 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   350.9 ns |   3.33 ns |  2.95 ns | 0.0553 |     696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 | 4,650.0 ns |  23.53 ns | 22.01 ns | 0.1984 |    2496 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   408.4 ns |   0.96 ns |  0.75 ns | 0.0672 |     848 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 5,095.5 ns |  16.65 ns | 13.90 ns | 0.1678 |    2168 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0 | .NET 8.0 |   734.0 ns |   4.48 ns |  3.97 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0 | .NET 8.0 | 6,241.7 ns |  31.41 ns | 29.38 ns | 0.3738 |    4704 B |
