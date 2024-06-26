```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3737/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.300
  [Host]   : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                                                                        | Job      | Runtime  | Mean        | Error     | StdDev    | Gen0   | Allocated |
|---------------------------------------------------------------------------------------------------------------------------------------------- |--------- |--------- |------------:|----------:|----------:|-------:|----------:|
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                                                                      | .NET 6.0 | .NET 6.0 | 520.3146 ns | 0.7470 ns | 0.6238 ns |      - |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 6.0 | .NET 6.0 | 282.8661 ns | 2.5246 ns | 2.3615 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 6.0 | .NET 6.0 |   0.0000 ns | 0.0000 ns | 0.0000 ns |      - |         - |
| &#39;&quot;sin(PI/6) + cos(PI/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                                                                           | .NET 6.0 | .NET 6.0 | 381.8164 ns | 0.9134 ns | 0.8544 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).Bind(new { a, b }).Evaluate()&#39;                                                              | .NET 6.0 | .NET 6.0 | 543.1023 ns | 1.7507 ns | 1.6376 ns | 0.0172 |     216 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                                                                      | .NET 8.0 | .NET 8.0 | 485.8830 ns | 1.3023 ns | 1.2181 ns |      - |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 8.0 | .NET 8.0 | 289.7929 ns | 2.2501 ns | 1.9946 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 8.0 | .NET 8.0 |   0.0122 ns | 0.0062 ns | 0.0055 ns |      - |         - |
| &#39;&quot;sin(PI/6) + cos(PI/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                                                                           | .NET 8.0 | .NET 8.0 | 278.9461 ns | 0.3623 ns | 0.3025 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).Bind(new { a, b }).Evaluate()&#39;                                                              | .NET 8.0 | .NET 8.0 | 269.3729 ns | 1.6761 ns | 1.4859 ns | 0.0172 |     216 B |
