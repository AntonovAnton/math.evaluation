```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean       | Error     | StdDev    | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |-----------:|----------:|----------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |   636.3 ns |   9.62 ns |   8.53 ns | 0.0076 |      96 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 | 6,554.2 ns | 126.61 ns | 155.49 ns | 0.3510 |    4496 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |   354.1 ns |   1.42 ns |   1.26 ns | 0.0076 |      96 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 | 5,247.8 ns |  98.68 ns |  96.92 ns | 0.2823 |    3616 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |   358.4 ns |   7.21 ns |   8.01 ns | 0.0572 |     720 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 | 4,026.0 ns |  51.86 ns |  48.51 ns | 0.1984 |    2520 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |   474.2 ns |   3.49 ns |   3.27 ns | 0.0696 |     880 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 4,155.9 ns |  45.59 ns |  40.41 ns | 0.1678 |    2192 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0 | .NET 8.0 |   836.0 ns |  16.54 ns |  17.70 ns | 0.0696 |     880 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0 | .NET 8.0 | 5,912.3 ns |  28.84 ns |  24.08 ns | 0.3738 |    4728 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |   565.6 ns |   1.50 ns |   1.25 ns | 0.0076 |      96 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 9.0 | .NET 9.0 | 5,558.4 ns |  26.89 ns |  25.15 ns | 0.3510 |    4496 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |   344.5 ns |   3.31 ns |   3.09 ns | 0.0076 |      96 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 9.0 | .NET 9.0 | 4,509.8 ns |  34.32 ns |  32.10 ns | 0.2823 |    3616 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |   322.6 ns |   2.05 ns |   1.82 ns | 0.0572 |     720 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 9.0 | .NET 9.0 | 3,500.6 ns |   7.90 ns |   6.59 ns | 0.1984 |    2520 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |   425.2 ns |   8.39 ns |   7.44 ns | 0.0701 |     880 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 | 3,732.0 ns |  14.80 ns |  13.85 ns | 0.1717 |    2192 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 9.0 | .NET 9.0 |   782.3 ns |   4.10 ns |   3.43 ns | 0.0696 |     880 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 9.0 | .NET 9.0 | 5,431.0 ns |  15.89 ns |  14.09 ns | 0.3738 |    4728 B |
