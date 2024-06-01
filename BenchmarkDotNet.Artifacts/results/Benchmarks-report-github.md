```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.300
  [Host]   : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                                                                                        | Job      | Runtime  | Mean        | Error     | StdDev    | Allocated |
|---------------------------------------------------------------------------------------------------------------------------------------------- |--------- |--------- |------------:|----------:|----------:|----------:|
| &#39;MathEvaluator.Evaluate(&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;)&#39;                                                         | .NET 6.0 | .NET 6.0 | 534.9801 ns | 9.3057 ns | 8.7045 ns |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 6.0 | .NET 6.0 | 280.7042 ns | 1.3352 ns | 1.2489 ns |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 6.0 | .NET 6.0 |   0.0000 ns | 0.0000 ns | 0.0000 ns |         - |
| &#39;MathEvaluator.Evaluate(&quot;sin(30°) + cos(60°)&quot;)&#39;                                                                                               | .NET 6.0 | .NET 6.0 | 235.1063 ns | 0.9277 ns | 0.8678 ns |         - |
| &#39;MathEvaluator.Evaluate(&quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;)&#39;                                                         | .NET 8.0 | .NET 8.0 | 476.0524 ns | 8.7085 ns | 8.1460 ns |         - |
| &#39;Parse(&quot;22888.32&quot;) * Parse(&quot;30&quot;) / Parse(&quot;323.34&quot;) / Parse(&quot;.5&quot;) - Parse(&quot;-1&quot;) / (Parse(&quot;2&quot;) + Parse(&quot;22888.32&quot;)) * Parse(&quot;4&quot;) - Parse(&quot;6&quot;))&#39; | .NET 8.0 | .NET 8.0 | 286.9004 ns | 1.8452 ns | 1.6357 ns |         - |
| &#39;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6)&#39;                                                                                  | .NET 8.0 | .NET 8.0 |   0.0347 ns | 0.0227 ns | 0.0189 ns |         - |
| &#39;MathEvaluator.Evaluate(&quot;sin(30°) + cos(60°)&quot;)&#39;                                                                                               | .NET 8.0 | .NET 8.0 | 181.3583 ns | 3.0046 ns | 2.8105 ns |         - |
