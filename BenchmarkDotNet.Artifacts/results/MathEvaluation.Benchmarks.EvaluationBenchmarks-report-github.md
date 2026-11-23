```

BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.10075)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
Memory: 31.77 GB Total, 15.97 GB Available
.NET SDK 11.0.100
  [Host]     : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4
  Job-WNYEYB : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  Job-ZJTCNP : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4
  Job-KJBBEV : .NET 10.0.15, X64 NativeAOT x86-64-v4

IterationCount=10  WarmupCount=3  

```
| Method                                                       | Runtime        | Mean       | Error   | StdDev  | Gen0   | Allocated |
|------------------------------------------------------------- |--------------- |-----------:|--------:|--------:|-------:|----------:|
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                     | .NET 10.0      |   349.0 ns | 2.97 ns | 1.97 ns | 0.0033 |     112 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                           | .NET 10.0      |   348.8 ns | 2.34 ns | 1.55 ns | 0.0234 |     736 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                   | .NET 10.0      |   452.1 ns | 3.42 ns | 2.26 ns | 0.0286 |     896 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39; | .NET 10.0      |   855.4 ns | 3.73 ns | 2.22 ns | 0.0286 |     896 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                     | .NET 11.0      |   346.9 ns | 1.70 ns | 0.89 ns | 0.0043 |     136 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                           | .NET 11.0      |   361.3 ns | 1.52 ns | 1.00 ns | 0.0243 |     760 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                   | .NET 11.0      |   457.1 ns | 3.76 ns | 2.49 ns | 0.0291 |     920 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39; | .NET 11.0      |   825.4 ns | 7.45 ns | 4.93 ns | 0.0286 |     920 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                     | NativeAOT 10.0 |   446.3 ns | 2.30 ns | 1.52 ns | 0.0033 |     112 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                           | NativeAOT 10.0 |   464.9 ns | 3.02 ns | 1.58 ns | 0.0234 |     736 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                   | NativeAOT 10.0 |   620.3 ns | 7.39 ns | 4.89 ns | 0.0286 |     896 B |
| &#39;MathEvaluator: &quot;A != B &amp;&amp; !C ^ -2.9 &gt;= -12.9 + 0.1 / 0.01&quot;&#39; | NativeAOT 10.0 | 1,084.9 ns | 4.74 ns | 3.13 ns | 0.0286 |     896 B |
