```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4112/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                   | Job      | Runtime  | Mean     | Error   | StdDev  | Gen0   | Allocated |
|----------------------------------------------------------------------------------------- |--------- |--------- |---------:|--------:|--------:|-------:|----------:|
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 6.0 | .NET 6.0 | 684.9 ns | 3.00 ns | 2.81 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 6.0 | .NET 6.0 | 466.7 ns | 2.44 ns | 2.16 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 6.0 | .NET 6.0 | 758.2 ns | 3.04 ns | 2.69 ns | 0.0601 |     760 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 8.0 | .NET 8.0 | 563.2 ns | 3.18 ns | 2.65 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 8.0 | .NET 8.0 | 414.8 ns | 0.63 ns | 0.59 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 8.0 | .NET 8.0 | 515.3 ns | 8.43 ns | 7.89 ns | 0.0601 |     760 B |
