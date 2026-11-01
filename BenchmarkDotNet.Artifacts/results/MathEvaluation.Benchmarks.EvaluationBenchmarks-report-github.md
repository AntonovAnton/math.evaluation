```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9818)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.403
  [Host]         : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 10.0      : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0       : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  NativeAOT 10.0 : .NET 10.0.14, X64 NativeAOT x86-64-v4
  NativeAOT 8.0  : .NET 8.0.33, X64 NativeAOT x86-64-v4


```
| Method                                                                       | Job            | Runtime        | Mean       | Error    | StdDev   | Gen0   | Allocated |
|----------------------------------------------------------------------------- |--------------- |--------------- |-----------:|---------:|---------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 10.0      | .NET 10.0      |   517.1 ns |  2.98 ns |  2.64 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 10.0      | .NET 10.0      | 3,584.9 ns |  8.19 ns |  7.66 ns | 0.1144 |    3592 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 10.0      | .NET 10.0      |   351.4 ns |  1.20 ns |  1.06 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 10.0      | .NET 10.0      | 2,920.2 ns | 12.63 ns | 11.20 ns | 0.1030 |    3328 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 10.0      | .NET 10.0      |   378.2 ns |  7.12 ns |  7.31 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 10.0      | .NET 10.0      | 2,601.3 ns | 18.88 ns | 17.66 ns | 0.0877 |    2776 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 10.0      | .NET 10.0      |   464.5 ns |  7.88 ns |  7.37 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 10.0      | .NET 10.0      | 2,807.8 ns | 16.54 ns | 15.47 ns | 0.0839 |    2672 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 10.0      | .NET 10.0      |   863.2 ns |  4.20 ns |  3.51 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 10.0      | .NET 10.0      | 4,021.8 ns | 26.25 ns | 24.55 ns | 0.1221 |    4024 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0       | .NET 8.0       |   643.5 ns |  5.56 ns |  5.20 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0       | .NET 8.0       | 4,455.0 ns | 20.82 ns | 18.45 ns | 0.1144 |    3592 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0       | .NET 8.0       |   416.8 ns |  3.28 ns |  2.91 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0       | .NET 8.0       | 3,379.6 ns | 24.34 ns | 21.58 ns | 0.1030 |    3328 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0       | .NET 8.0       |   457.0 ns |  3.21 ns |  3.00 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0       | .NET 8.0       | 3,037.0 ns | 15.43 ns | 13.68 ns | 0.0877 |    2776 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0       | .NET 8.0       |   587.8 ns |  6.11 ns |  5.71 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0       | .NET 8.0       | 3,593.9 ns | 14.34 ns | 12.71 ns | 0.0839 |    2672 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | .NET 8.0       | .NET 8.0       | 1,054.2 ns | 13.77 ns | 12.20 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | .NET 8.0       | .NET 8.0       | 4,735.8 ns | 29.31 ns | 24.48 ns | 0.1221 |    4024 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | NativeAOT 10.0 | NativeAOT 10.0 |   806.9 ns |  3.11 ns |  2.91 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | NativeAOT 10.0 | NativeAOT 10.0 | 6,412.2 ns | 73.75 ns | 65.38 ns | 0.1144 |    3592 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | NativeAOT 10.0 | NativeAOT 10.0 |   448.5 ns |  2.97 ns |  2.78 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | NativeAOT 10.0 | NativeAOT 10.0 | 5,461.0 ns | 84.12 ns | 78.69 ns | 0.1068 |    3328 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | NativeAOT 10.0 | NativeAOT 10.0 |   480.2 ns |  7.33 ns |  6.85 ns | 0.0229 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | NativeAOT 10.0 | NativeAOT 10.0 | 4,673.8 ns | 67.76 ns | 60.07 ns | 0.0839 |    2776 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | NativeAOT 10.0 | NativeAOT 10.0 |   649.3 ns |  2.89 ns |  2.70 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | NativeAOT 10.0 | NativeAOT 10.0 | 5,343.1 ns | 22.16 ns | 18.50 ns | 0.0839 |    2672 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | NativeAOT 10.0 | NativeAOT 10.0 | 1,132.0 ns |  9.08 ns |  8.05 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | NativeAOT 10.0 | NativeAOT 10.0 | 6,834.0 ns | 96.75 ns | 90.50 ns | 0.1221 |    4024 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | NativeAOT 8.0  | NativeAOT 8.0  |   771.4 ns | 12.30 ns | 11.50 ns | 0.0029 |     112 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | NativeAOT 8.0  | NativeAOT 8.0  | 7,298.8 ns | 69.21 ns | 64.74 ns | 0.1144 |    3592 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | NativeAOT 8.0  | NativeAOT 8.0  |   434.3 ns |  6.08 ns |  5.69 ns | 0.0033 |     112 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | NativeAOT 8.0  | NativeAOT 8.0  | 5,830.0 ns | 31.19 ns | 27.65 ns | 0.0992 |    3328 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | NativeAOT 8.0  | NativeAOT 8.0  |   465.8 ns |  3.66 ns |  3.42 ns | 0.0234 |     736 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | NativeAOT 8.0  | NativeAOT 8.0  | 5,113.4 ns | 35.82 ns | 31.76 ns | 0.0839 |    2776 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | NativeAOT 8.0  | NativeAOT 8.0  |   605.9 ns |  9.02 ns |  8.44 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | NativeAOT 8.0  | NativeAOT 8.0  | 5,739.8 ns | 55.90 ns | 49.55 ns | 0.0839 |    2672 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                 | NativeAOT 8.0  | NativeAOT 8.0  | 1,049.4 ns | 11.16 ns | 10.44 ns | 0.0286 |     896 B |
| &#39;NCalc: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39;                         | NativeAOT 8.0  | NativeAOT 8.0  | 7,489.9 ns | 71.67 ns | 67.04 ns | 0.1221 |    4024 B |
