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
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 6.0 | .NET 6.0 | 689.2 ns | 2.20 ns | 2.06 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 6.0 | .NET 6.0 | 484.2 ns | 5.80 ns | 4.84 ns | 0.0019 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 6.0 | .NET 6.0 | 759.5 ns | 2.56 ns | 2.40 ns | 0.0601 |     760 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39;                 | .NET 8.0 | .NET 8.0 | 593.2 ns | 1.86 ns | 1.74 ns |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.SetContext(_scientificContext).Evaluate()&#39;                      | .NET 8.0 | .NET 8.0 | 394.6 ns | 1.53 ns | 1.35 ns | 0.0024 |      32 B |
| &#39;&quot;sin(a) + cos(b)&quot;.SetContext(_scientificContext).BindVariable(new { a, b }).Evaluate()&#39; | .NET 8.0 | .NET 8.0 | 508.0 ns | 2.48 ns | 2.20 ns | 0.0601 |     760 B |
