```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3593/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.300
  [Host]   : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                                                                        | Job      | Runtime  | Mean        | Error     | StdDev    | Median      | Allocated |
|---------------------------------------------------------------------------------------------------------------------------------------------- |--------- |--------- |------------:|----------:|----------:|------------:|----------:|
| &#39;MathEvaluator.Evaluate(&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;)&#39;                                                         | .NET 6.0 | .NET 6.0 | 511.5319 ns | 3.5275 ns | 3.1271 ns | 510.8746 ns |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 6.0 | .NET 6.0 | 288.3204 ns | 2.3382 ns | 2.1872 ns | 288.2687 ns |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 6.0 | .NET 6.0 |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |         - |
| &#39;MathEvaluator.Evaluate(&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;)&#39;                                                         | .NET 8.0 | .NET 8.0 | 456.3397 ns | 2.2320 ns | 2.0879 ns | 456.6247 ns |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 8.0 | .NET 8.0 | 302.1510 ns | 2.7469 ns | 2.5694 ns | 302.5246 ns |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 8.0 | .NET 8.0 |   0.0001 ns | 0.0002 ns | 0.0002 ns |   0.0000 ns |         - |
