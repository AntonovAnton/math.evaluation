```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean       | Error     | StdDev    | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |-----------:|----------:|----------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 6.0 | .NET 6.0 |   676.4 ns |   6.12 ns |   5.11 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 6.0 | .NET 6.0 | 8,896.8 ns | 177.23 ns | 296.11 ns | 0.3967 |    5160 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 6.0 | .NET 6.0 |   455.9 ns |   7.35 ns |   6.14 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 6.0 | .NET 6.0 | 7,117.2 ns | 129.85 ns | 213.35 ns | 0.2899 |    3688 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 |   483.4 ns |   9.29 ns |  11.41 ns | 0.0553 |     696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 6.0 | .NET 6.0 | 5,770.4 ns | 109.51 ns | 117.17 ns | 0.1984 |    2496 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 6.0 | .NET 6.0 |   606.1 ns |  11.24 ns |  10.51 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 6.0 | .NET 6.0 | 6,470.7 ns |  71.42 ns |  59.64 ns | 0.1678 |    2168 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 6.0 | .NET 6.0 |   996.7 ns |  15.32 ns |  14.33 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 6.0 | .NET 6.0 | 8,949.9 ns | 146.50 ns | 137.04 ns | 0.3967 |    5104 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   640.3 ns |  12.46 ns |  13.85 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 | 6,544.9 ns |  91.04 ns |  85.16 ns | 0.3510 |    4472 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   344.2 ns |   5.32 ns |   4.71 ns | 0.0067 |      88 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 | 5,629.6 ns |  95.78 ns |  84.91 ns | 0.2823 |    3592 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   381.7 ns |   7.59 ns |  13.88 ns | 0.0553 |     696 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 | 4,776.5 ns |  81.00 ns |  75.77 ns | 0.1984 |    2496 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   426.1 ns |   3.50 ns |   2.92 ns | 0.0672 |     848 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 5,162.3 ns |  65.44 ns |  61.21 ns | 0.1678 |    2168 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0 | .NET 8.0 |   768.2 ns |  15.13 ns |  26.11 ns | 0.0668 |     848 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0 | .NET 8.0 | 6,391.8 ns |  88.81 ns |  83.07 ns | 0.3738 |    4704 B |
