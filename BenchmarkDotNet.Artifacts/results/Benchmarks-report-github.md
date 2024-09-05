```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.4112/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]   : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                   | Job      | Runtime  | Mean          | Error        | StdDev       | Gen0   | Gen1   | Allocated |
|------------------------------------------------------------------------- |--------- |--------- |--------------:|-------------:|-------------:|-------:|-------:|----------:|
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39; | .NET 6.0 | .NET 6.0 |     669.78 ns |     3.047 ns |     2.850 ns |      - |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.Evaluate(_scientificContext)&#39;                   | .NET 6.0 | .NET 6.0 |     476.85 ns |     2.375 ns |     2.105 ns |      - |      - |         - |
| &#39;&quot;sin(a) + cos(b)&quot;.Evaluate(_scientificContext, new { a, b })&#39;           | .NET 6.0 | .NET 6.0 |     747.90 ns |     5.437 ns |     5.086 ns | 0.0572 |      - |     728 B |
| &#39;&quot;sin(a) + cos(b)&quot;.Compile(new { a, b }, _scientificContext)&#39;            | .NET 6.0 | .NET 6.0 | 165,265.02 ns | 3,278.968 ns | 5,828.362 ns | 0.9766 | 0.4883 |   13925 B |
| &#39;compiled sin(a) + cos(b): fn(new { a, b })&#39;                             | .NET 6.0 | .NET 6.0 |      24.26 ns |     0.189 ns |     0.176 ns | 0.0044 |      - |      56 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39; | .NET 8.0 | .NET 8.0 |     565.68 ns |     5.249 ns |     4.653 ns |      - |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.Evaluate(_scientificContext)&#39;                   | .NET 8.0 | .NET 8.0 |     407.17 ns |     1.402 ns |     1.311 ns |      - |      - |         - |
| &#39;&quot;sin(a) + cos(b)&quot;.Evaluate(_scientificContext, new { a, b })&#39;           | .NET 8.0 | .NET 8.0 |     502.43 ns |     2.711 ns |     2.404 ns | 0.0572 |      - |     728 B |
| &#39;&quot;sin(a) + cos(b)&quot;.Compile(new { a, b }, _scientificContext)&#39;            | .NET 8.0 | .NET 8.0 | 144,108.94 ns |   676.229 ns |   599.459 ns | 0.9766 | 0.7324 |   13941 B |
| &#39;compiled sin(a) + cos(b): fn(new { a, b })&#39;                             | .NET 8.0 | .NET 8.0 |      21.90 ns |     0.455 ns |     0.623 ns | 0.0044 |      - |      56 B |
