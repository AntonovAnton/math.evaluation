```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3810/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.300
  [Host]   : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                                                                        | Job      | Runtime  | Mean        | Error     | StdDev    | Median      | Gen0   | Allocated |
|---------------------------------------------------------------------------------------------------------------------------------------------- |--------- |--------- |------------:|----------:|----------:|------------:|-------:|----------:|
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                                                                      | .NET 6.0 | .NET 6.0 | 563.4503 ns | 5.0652 ns | 4.4901 ns | 562.8201 ns |      - |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 6.0 | .NET 6.0 | 292.8199 ns | 3.7397 ns | 3.4981 ns | 291.5516 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 6.0 | .NET 6.0 |   0.0011 ns | 0.0036 ns | 0.0030 ns |   0.0000 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                                                                           | .NET 6.0 | .NET 6.0 | 433.1629 ns | 1.7593 ns | 1.6456 ns | 433.1928 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39;                                                      | .NET 6.0 | .NET 6.0 | 602.6214 ns | 4.9370 ns | 4.3765 ns | 601.5659 ns | 0.0172 |     216 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                                                                      | .NET 8.0 | .NET 8.0 | 520.0653 ns | 4.9260 ns | 4.3668 ns | 519.3810 ns |      - |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 8.0 | .NET 8.0 | 281.6485 ns | 1.6160 ns | 1.2617 ns | 281.2305 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 8.0 | .NET 8.0 |   0.0025 ns | 0.0035 ns | 0.0031 ns |   0.0015 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                                                                           | .NET 8.0 | .NET 8.0 | 348.9866 ns | 4.3167 ns | 4.0379 ns | 347.0173 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39;                                                      | .NET 8.0 | .NET 8.0 | 377.0083 ns | 1.9630 ns | 1.7402 ns | 377.2319 ns | 0.0172 |     216 B |
