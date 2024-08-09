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
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 6.0 | .NET 6.0 | 584.6453 ns | 5.4133 ns | 5.0636 ns | 584.0519 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                             | .NET 6.0 | .NET 6.0 |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 6.0 | .NET 6.0 | 420.5880 ns | 2.2123 ns | 2.0693 ns | 420.6436 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 6.0 | .NET 6.0 | 584.7521 ns | 3.3462 ns | 3.1300 ns | 584.1420 ns | 0.0172 |     216 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 8.0 | .NET 8.0 | 537.2132 ns | 2.8375 ns | 2.6542 ns | 537.4217 ns |      - |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                             | .NET 8.0 | .NET 8.0 |   0.0006 ns | 0.0011 ns | 0.0011 ns |   0.0001 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 8.0 | .NET 8.0 | 355.9826 ns | 1.0049 ns | 0.7846 ns | 356.0945 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 8.0 | .NET 8.0 | 329.0986 ns | 3.9050 ns | 3.4617 ns | 328.3522 ns | 0.0172 |     216 B |
