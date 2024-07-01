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
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                                                                      | .NET 6.0 | .NET 6.0 | 558.3867 ns | 7.1290 ns | 6.6684 ns | 560.0690 ns |      - |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 6.0 | .NET 6.0 | 283.0453 ns | 3.8417 ns | 3.4056 ns | 282.1919 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 6.0 | .NET 6.0 |   0.0010 ns | 0.0022 ns | 0.0021 ns |   0.0000 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                                                                           | .NET 6.0 | .NET 6.0 | 391.2251 ns | 0.7430 ns | 0.6587 ns | 391.2073 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).Bind(new { a, b }).Evaluate()&#39;                                                              | .NET 6.0 | .NET 6.0 | 545.8640 ns | 2.6155 ns | 2.3185 ns | 545.7301 ns | 0.0172 |     216 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                                                                      | .NET 8.0 | .NET 8.0 | 480.5093 ns | 2.0361 ns | 1.9045 ns | 480.2014 ns |      - |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 8.0 | .NET 8.0 | 278.4982 ns | 2.6591 ns | 2.4873 ns | 278.5138 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 8.0 | .NET 8.0 |   0.0025 ns | 0.0028 ns | 0.0024 ns |   0.0022 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                                                                           | .NET 8.0 | .NET 8.0 | 321.6251 ns | 1.0223 ns | 0.9563 ns | 321.5304 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).Bind(new { a, b }).Evaluate()&#39;                                                              | .NET 8.0 | .NET 8.0 | 325.6741 ns | 2.8513 ns | 2.6671 ns | 324.4514 ns | 0.0172 |     216 B |
