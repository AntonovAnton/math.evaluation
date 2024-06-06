```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.300
  [Host]   : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                                                                        | Job      | Runtime  | Mean        | Error     | StdDev    | Median      | Allocated |
|---------------------------------------------------------------------------------------------------------------------------------------------- |--------- |--------- |------------:|----------:|----------:|------------:|----------:|
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                                                                      | .NET 6.0 | .NET 6.0 | 513.1914 ns | 2.7362 ns | 2.5594 ns | 512.9876 ns |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 6.0 | .NET 6.0 | 284.2267 ns | 4.9923 ns | 4.6698 ns | 283.2525 ns |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 6.0 | .NET 6.0 |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |         - |
| &#39;&quot;sin(PI/6) + cos(PI/3)&quot;.Evaluate()&#39;                                                                                                          | .NET 6.0 | .NET 6.0 | 272.4613 ns | 0.4705 ns | 0.4171 ns | 272.4932 ns |         - |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                                                                      | .NET 8.0 | .NET 8.0 | 480.9547 ns | 2.0864 ns | 1.9516 ns | 480.6810 ns |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 8.0 | .NET 8.0 | 297.4222 ns | 2.8234 ns | 2.6410 ns | 296.6249 ns |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 8.0 | .NET 8.0 |   0.0245 ns | 0.0175 ns | 0.0164 ns |   0.0304 ns |         - |
| &#39;&quot;sin(PI/6) + cos(PI/3)&quot;.Evaluate()&#39;                                                                                                          | .NET 8.0 | .NET 8.0 | 189.3298 ns | 0.4161 ns | 0.3688 ns | 189.3148 ns |         - |
