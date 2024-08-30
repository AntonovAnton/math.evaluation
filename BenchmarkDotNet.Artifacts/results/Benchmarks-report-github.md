```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4112/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                   | Job      | Runtime  | Mean        | Error     | StdDev     | Median      | Gen0   | Allocated |
|----------------------------------------------------------------------------------------- |--------- |--------- |------------:|----------:|-----------:|------------:|-------:|----------:|
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 6.0 | .NET 6.0 | 689.9295 ns | 2.8689 ns |  2.5432 ns | 689.6423 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                             | .NET 6.0 | .NET 6.0 |   0.0000 ns | 0.0000 ns |  0.0000 ns |   0.0000 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 6.0 | .NET 6.0 | 480.7071 ns | 5.4809 ns |  4.8587 ns | 478.9223 ns | 0.0019 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 6.0 | .NET 6.0 | 620.5838 ns | 2.1326 ns |  1.8905 ns | 620.7232 ns | 0.0172 |     216 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 8.0 | .NET 8.0 | 574.8492 ns | 2.6266 ns |  2.4569 ns | 575.0673 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                             | .NET 8.0 | .NET 8.0 |   0.0007 ns | 0.0015 ns |  0.0012 ns |   0.0000 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 8.0 | .NET 8.0 | 394.8733 ns | 0.9340 ns |  0.8279 ns | 394.8194 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 8.0 | .NET 8.0 | 399.5120 ns | 7.9095 ns | 10.5590 ns | 399.6573 ns | 0.0172 |     216 B |
