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
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39; | .NET 6.0 | .NET 6.0 |     675.61 ns |     2.683 ns |     2.241 ns | 0.0057 |      - |      80 B |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.Evaluate(null, _scientificContext)&#39;             | .NET 6.0 | .NET 6.0 |     475.96 ns |     1.641 ns |     1.535 ns | 0.0057 |      - |      80 B |
| &#39;&quot;sin(a) + cos(b)&quot;.Evaluate(_scientificContext, new { a, b })&#39;           | .NET 6.0 | .NET 6.0 |     746.20 ns |     3.914 ns |     3.269 ns | 0.0639 |      - |     808 B |
| &#39;&quot;sin(a) + cos(b)&quot;.Compile(new { a, b }, _scientificContext)&#39;            | .NET 6.0 | .NET 6.0 | 148,837.28 ns | 1,355.571 ns | 1,268.002 ns | 0.9766 | 0.4883 |   13223 B |
| &#39;compiled &quot;sin(a) + cos(b)&quot;: fn(new { a, b })&#39;                           | .NET 6.0 | .NET 6.0 |      23.84 ns |     0.181 ns |     0.169 ns | 0.0044 |      - |      56 B |
| &#39;&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;.Evaluate()&#39; | .NET 8.0 | .NET 8.0 |     583.45 ns |     7.771 ns |     7.269 ns | 0.0057 |      - |      80 B |
| &#39;&quot;sin(pi/6) + cos(pi/3)&quot;.Evaluate(null, _scientificContext)&#39;             | .NET 8.0 | .NET 8.0 |     385.22 ns |     1.398 ns |     1.308 ns | 0.0062 |      - |      80 B |
| &#39;&quot;sin(a) + cos(b)&quot;.Evaluate(_scientificContext, new { a, b })&#39;           | .NET 8.0 | .NET 8.0 |     491.38 ns |     1.869 ns |     1.748 ns | 0.0639 |      - |     808 B |
| &#39;&quot;sin(a) + cos(b)&quot;.Compile(new { a, b }, _scientificContext)&#39;            | .NET 8.0 | .NET 8.0 | 141,232.67 ns | 1,354.140 ns | 1,266.664 ns | 0.9766 | 0.7324 |   13238 B |
| &#39;compiled &quot;sin(a) + cos(b)&quot;: fn(new { a, b })&#39;                           | .NET 8.0 | .NET 8.0 |      21.74 ns |     0.352 ns |     0.312 ns | 0.0044 |      - |      56 B |
