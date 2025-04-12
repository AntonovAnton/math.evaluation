```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3775)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                                                                       | Job      | Runtime  | Mean           | Error         | StdDev        | Gen0   | Gen1   | Allocated |
|----------------------------------------------------------------------------- |--------- |--------- |---------------:|--------------:|--------------:|-------:|-------:|----------:|
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 8.0 | .NET 8.0 |  13,709.471 ns |    42.0334 ns |    35.0998 ns | 0.3967 | 0.3815 |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 8.0 | .NET 8.0 |  28,696.420 ns |   148.0533 ns |   131.2454 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 8.0 | .NET 8.0 |  13,401.056 ns |   241.6633 ns |   226.0520 ns | 0.3662 | 0.3510 |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 8.0 | .NET 8.0 |  20,173.380 ns |   138.3304 ns |   129.3944 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 8.0 | .NET 8.0 |  93,932.524 ns |   413.8340 ns |   387.1006 ns | 0.4883 | 0.2441 |    9160 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 |  79,165.538 ns |   425.2119 ns |   376.9393 ns | 0.4883 | 0.2441 |    6910 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 8.0 | .NET 8.0 |       2.837 ns |     0.0642 ns |     0.0601 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 8.0 | .NET 8.0 |       2.713 ns |     0.0150 ns |     0.0133 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 8.0 | .NET 8.0 |  96,829.521 ns |   432.1877 ns |   404.2686 ns | 0.3662 | 0.2441 |    5684 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 8.0 | .NET 8.0 |  31,217.977 ns |   229.6837 ns |   203.6086 ns | 0.6104 | 0.4883 |    7816 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 8.0 | .NET 8.0 | 112,240.522 ns |   596.4471 ns |   498.0603 ns | 0.4883 | 0.2441 |    7213 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 8.0 | .NET 8.0 |  57,241.605 ns |   239.6463 ns |   224.1653 ns | 0.6104 | 0.4883 |    8358 B |
| &#39;MathEvaluator: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39; | .NET 9.0 | .NET 9.0 |  14,003.951 ns |   188.1768 ns |   176.0207 ns | 0.3967 | 0.3815 |    5127 B |
| &#39;NCalc: &quot;22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6&quot;&#39;         | .NET 9.0 | .NET 9.0 |  26,580.140 ns |   282.3336 ns |   264.0950 ns | 0.6104 | 0.4883 |    8255 B |
| &#39;MathEvaluator: &quot;true or not false and (true or false)&quot;&#39;                     | .NET 9.0 | .NET 9.0 |  13,192.323 ns |   199.6831 ns |   186.7837 ns | 0.3662 | 0.3510 |    4624 B |
| &#39;NCalc: &quot;true or not false and (true or false)&quot;&#39;                             | .NET 9.0 | .NET 9.0 |  20,622.011 ns |   343.3554 ns |   304.3756 ns | 0.4272 | 0.3967 |    5464 B |
| &#39;MathEvaluator: &quot;A or not B and (C or B)&quot;&#39;                                   | .NET 9.0 | .NET 9.0 |  93,489.894 ns |   581.6380 ns |   544.0645 ns | 0.4883 | 0.2441 |    9160 B |
| &#39;NCalc: &quot;A or not B and (C or B)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 |  74,099.280 ns |   348.5820 ns |   291.0818 ns | 0.4883 | 0.3662 |    6909 B |
| &#39;MathEvaluator: fn(new BooleanVariables { A = a, B = b, C = c })&#39;            | .NET 9.0 | .NET 9.0 |       2.355 ns |     0.0385 ns |     0.0300 ns | 0.0019 |      - |      24 B |
| &#39;NCalc: fn(new BooleanVariables { A = a, B = b, C = c })&#39;                    | .NET 9.0 | .NET 9.0 |       2.306 ns |     0.0458 ns |     0.0357 ns | 0.0019 |      - |      24 B |
| &#39;MathEvaluator: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                     | .NET 9.0 | .NET 9.0 |  87,283.394 ns | 1,507.6249 ns | 1,410.2333 ns | 0.3662 | 0.2441 |    5684 B |
| &#39;NCalc: &quot;Sin(pi/6) + Cos(pi/3)&quot;&#39;                                             | .NET 9.0 | .NET 9.0 |  30,634.501 ns |   590.6905 ns |   680.2401 ns | 0.6104 | 0.4883 |    7720 B |
| &#39;MathEvaluator: &quot;Sin(a) + Cos(b)&quot;&#39;                                           | .NET 9.0 | .NET 9.0 | 104,248.288 ns | 1,187.1435 ns | 1,052.3718 ns | 0.4883 | 0.2441 |    7213 B |
| &#39;NCalc: &quot;Sin(a) + Cos(b)&quot;&#39;                                                   | .NET 9.0 | .NET 9.0 |  54,684.249 ns |   291.6524 ns |   258.5423 ns | 0.6104 | 0.4883 |    8262 B |
