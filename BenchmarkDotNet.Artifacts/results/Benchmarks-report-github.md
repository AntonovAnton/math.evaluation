```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3958/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.300
  [Host]   : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                   | Job      | Runtime  | Mean        | Error     | StdDev    | Median      | Gen0   | Allocated |
|----------------------------------------------------------------------------------------- |--------- |--------- |------------:|----------:|----------:|------------:|-------:|----------:|
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 6.0 | .NET 6.0 | 573.0969 ns | 3.0739 ns | 2.8753 ns | 573.8610 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                             | .NET 6.0 | .NET 6.0 |   0.0171 ns | 0.0025 ns | 0.0023 ns |   0.0178 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 6.0 | .NET 6.0 | 439.9444 ns | 1.8046 ns | 1.5998 ns | 440.1495 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 6.0 | .NET 6.0 | 612.8856 ns | 3.1173 ns | 2.9159 ns | 612.7366 ns | 0.0172 |     216 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 8.0 | .NET 8.0 | 530.9920 ns | 3.0343 ns | 2.8383 ns | 531.4642 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                             | .NET 8.0 | .NET 8.0 |   0.0006 ns | 0.0017 ns | 0.0014 ns |   0.0000 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 8.0 | .NET 8.0 | 362.0977 ns | 2.0836 ns | 1.8471 ns | 362.6830 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 8.0 | .NET 8.0 | 341.6049 ns | 1.5579 ns | 1.3009 ns | 341.5832 ns | 0.0172 |     216 B |
