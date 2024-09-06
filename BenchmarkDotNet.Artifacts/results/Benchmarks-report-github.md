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
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39; | .NET 6.0 | .NET 6.0 |     659.71 ns |    12.349 ns |    10.947 ns |      - |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.Evaluate(_scientificContext)&#39;                   | .NET 6.0 | .NET 6.0 |     491.93 ns |     3.026 ns |     2.831 ns |      - |      - |         - |
| &#39;&quot;sin(a) + cos(b)&quot;.Evaluate(_scientificContext, new { a, b })&#39;           | .NET 6.0 | .NET 6.0 |     752.84 ns |     3.395 ns |     3.175 ns | 0.0572 |      - |     728 B |
| &#39;&quot;sin(a) + cos(b)&quot;.Compile(new { a, b }, _scientificContext)&#39;            | .NET 6.0 | .NET 6.0 | 151,604.82 ns | 2,981.369 ns | 3,061.647 ns | 0.9766 | 0.4883 |   13215 B |
| &#39;compiled sin(a) + cos(b): fn(new { a, b })&#39;                             | .NET 6.0 | .NET 6.0 |      25.09 ns |     0.205 ns |     0.192 ns | 0.0044 |      - |      56 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39; | .NET 8.0 | .NET 8.0 |     553.27 ns |     0.916 ns |     0.857 ns |      - |      - |         - |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.Evaluate(_scientificContext)&#39;                   | .NET 8.0 | .NET 8.0 |     393.73 ns |     1.923 ns |     1.606 ns |      - |      - |         - |
| &#39;&quot;sin(a) + cos(b)&quot;.Evaluate(_scientificContext, new { a, b })&#39;           | .NET 8.0 | .NET 8.0 |     489.19 ns |     2.323 ns |     2.059 ns | 0.0572 |      - |     728 B |
| &#39;&quot;sin(a) + cos(b)&quot;.Compile(new { a, b }, _scientificContext)&#39;            | .NET 8.0 | .NET 8.0 | 141,059.00 ns | 1,330.752 ns | 1,179.677 ns | 0.9766 | 0.7324 |   13231 B |
| &#39;compiled sin(a) + cos(b): fn(new { a, b })&#39;                             | .NET 8.0 | .NET 8.0 |      21.14 ns |     0.108 ns |     0.096 ns | 0.0044 |      - |      56 B |
