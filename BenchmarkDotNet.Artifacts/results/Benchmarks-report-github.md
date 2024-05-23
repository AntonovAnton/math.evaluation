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
| &#39;MathEvaluator.Evaluate(&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;)&#39;                                                         | .NET 6.0 | .NET 6.0 | 478.6224 ns | 2.5991 ns | 2.4312 ns | 477.4653 ns |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 6.0 | .NET 6.0 | 288.1724 ns | 4.2854 ns | 3.7989 ns | 287.7514 ns |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 6.0 | .NET 6.0 |   0.0014 ns | 0.0016 ns | 0.0015 ns |   0.0006 ns |         - |
| &#39;MathEvaluator.Evaluate(&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;)&#39;                                                         | .NET 8.0 | .NET 8.0 | 446.6391 ns | 1.6492 ns | 1.5426 ns | 446.7024 ns |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 8.0 | .NET 8.0 | 305.3880 ns | 4.0496 ns | 3.5899 ns | 305.3351 ns |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 8.0 | .NET 8.0 |   0.0046 ns | 0.0067 ns | 0.0060 ns |   0.0019 ns |         - |
