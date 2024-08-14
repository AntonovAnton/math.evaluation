# Math Expression Evaluator in .NET
[![NuGet Downloads](https://img.shields.io/nuget/dt/MathEvaluator?style=for-the-badge)](https://www.nuget.org/packages/MathEvaluator/)
[![NuGet Version](https://img.shields.io/nuget/v/MathEvaluator?style=for-the-badge)](https://www.nuget.org/packages/MathEvaluator/)
## Overview
MathEvaluator is a .NET library that allows you to evaluate any mathematical expressions from a string dynamically.

## Features
- Supports different mathematical contexts, such as scientific, programming, and other custom contexts.
- Evaluates to double, boolean, or decimal.
- Provides variable support within expressions.
- Extensible with custom functions.
- Fast and comprehensive. More than 1000 tests are passed, including complex math expressions (for example, -3^4sin(-π/2) or sin-3/cos1).

## Installation

    dotnet add package MathEvaluator

Alternatively, you can install the package using the NuGet Package Manager Console:

    Install-Package MathEvaluator

## Perfomance
This math expression evaluator is designed for exceptional performance by leveraging modern .NET features and best practices, which is why it targets .NET Standard 2.1 or higher. 

This high-performance evaluator stands out due to its use of `ReadOnlySpan<char>`, avoidance of regular expressions, and reliance on static methods. These design choices collectively ensure minimal memory allocation, fast parsing, and efficient execution.

The evaluator uses recursive method calls to handle mathematical operations based on operator precedence and rules. This approach avoids the overhead associated with stack or queue data structures.

The evaluator uses a prefix tree, also known as a trie (pronounced "try"), for efficient searching of variables and functions by their keys (names) when providing a specific mathematical context or adding custom variables and functions is required.

Let's compare, for example, performance of calculating the mathematical expression:

    22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6

Below are the results of the comparison with the NCalc library: 

| Method                                                                   | Job      | Runtime  | Mean       | Error    | StdDev   | Gen0   | Allocated |
|------------------------------------------------------------------------- |--------- |--------- |-----------:|---------:|---------:|-------:|----------:|
| MathEvaluator | .NET 6.0 | .NET 6.0 |   616.9 ns |  2.67 ns |  2.23 ns |      - |         - |
| NCalc | .NET 6.0 | .NET 6.0 | 8,901.1 ns | 62.27 ns | 52.00 ns | 0.2747 |    3464 B |
| MathEvaluator | .NET 8.0 | .NET 8.0 |   518.0 ns |  2.20 ns |  1.95 ns |      - |         - |
| NCalc | .NET 8.0 | .NET 8.0 | 8,499.7 ns | 40.09 ns | 37.50 ns | 0.2594 |    3440 B |

## How to use
Examples of using string extentions:

    "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".Evaluate();

    "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".EvaluateDecimal();

    "$22,888.32 * 30 / 323.34 / .5 - - 1 / (2 + $22,888.32) * 4 - 6".Evaluate(new CultureInfo("en-US"));

    "22’888.32 CHF * 30 / 323.34 / .5 - - 1 / (2 + 22’888.32 CHF) * 4 - 6".EvaluateDecimal(new CultureInfo("de-CH"));

    "ln[1/-0.5 + √(1/(0.5^2) + 1)]".Evaluate(new ScientificMathContext());
    
    "ln[1/-0,5 + √(1/(0,5^2) + 1)]".Evaluate(new ScientificMathContext(), new CultureInfo("fr"));
    
    "ln[1/-0.5 + √(1/(0.5^2) + 1)]".EvaluateDecimal(new DecimalScientificMathContext());
    
    "ln[1/-0,5 + √(1/(0,5^2) + 1)]".EvaluateDecimal(new DecimalScientificMathContext(), new CultureInfo("fr"));
    
    "4 % 3".Evaluate(new ProgrammingMathContext());
    
    "4 mod 3".EvaluateDecimal(new DecimalScientificMathContext());

    "4 <> 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01".EvaluateBoolean(new ProgrammingMathContext());

Examples of using static methods:
        
    MathEvaluator.Evaluate("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6");

    MathEvaluator.EvaluateDecimal("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6");

    MathEvaluator.Evaluate("$22,888.32 * 30 / 323.34 / .5 - - 1 / (2 + $22,888.32) * 4 - 6", new CultureInfo("en-US"));

    MathEvaluator.EvaluateDecimal("22’888.32 CHF * 30 / 323.34 / .5 - - 1 / (2 + 22’888.32 CHF) * 4 - 6", new CultureInfo("de-CH"));
    
    MathEvaluator.Evaluate("ln[1/-0.5 + √(1/(0.5^2) + 1)]", new ScientificMathContext());
    
    MathEvaluator.Evaluate("ln[1/-0,5 + √(1/(0,5^2) + 1)]", new ScientificMathContext(), new CultureInfo("fr"));
    
    MathEvaluator.EvaluateDecimal("ln[1/-0.5 + √(1/(0.5^2) + 1)]", new DecimalScientificMathContext());
    
    MathEvaluator.EvaluateDecimal("ln[1/-0,5 + √(1/(0,5^2) + 1)]", new DecimalScientificMathContext(), new CultureInfo("fr"));
    
    MathEvaluator.Evaluate("4 % 3", new ProgrammingMathContext());
    
    MathEvaluator.EvaluateDecimal("4 mod 3", new DecimalScientificMathContext());

    MathEvaluator.EvaluateBoolean("4 <> 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", new ProgrammingMathContext());

Examples of using an instance of the MathEvaluator class:
        
    new MathEvaluator("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6").Evaluate();

    new MathEvaluator("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6").EvaluateDecimal();

    new MathEvaluator("$22,888.32 * 30 / 323.34 / .5 - - 1 / (2 + $22,888.32) * 4 - 6").Evaluate(new CultureInfo("en-US"));

    new MathEvaluator("22’888.32 CHF * 30 / 323.34 / .5 - - 1 / (2 + 22’888.32 CHF) * 4 - 6").EvaluateDecimal(new CultureInfo("de-CH"));
    
    new MathEvaluator("ln[1/-0.5 + √(1/(0.5^2) + 1)]", new ScientificMathContext()).Evaluate();
    
    new MathEvaluator("ln[1/-0,5 + √(1/(0,5^2) + 1)]", new ScientificMathContext()).Evaluate(new CultureInfo("fr"));
    
    new MathEvaluator("ln[1/-0.5 + √(1/(0.5^2) + 1)]", new DecimalScientificMathContext()).EvaluateDecimal();
    
    new MathEvaluator("ln[1/-0,5 + √(1/(0,5^2) + 1)]", new DecimalScientificMathContext()).EvaluateDecimal(new CultureInfo("fr"));
    
    new MathEvaluator("4 % 3", new ProgrammingMathContext()).Evaluate();
    
    new MathEvaluator("4 mod 3", new DecimalScientificMathContext()).EvaluateDecimal();

    new MathEvaluator("4 <> 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", new ProgrammingMathContext()).EvaluateBoolean();

Examples of using custom variables or functions:
        
    var x1 = 0.5;
    var x2 = -0.5;
    var sqrt = Math.Sqrt;
    Func<double, double> ln = Math.Log;

    var value1 = "ln(1/-x1 + sqrt(1/(x2*x2) + 1))"
        .Bind(new { x1, x2, sqrt, ln })
        .Evaluate();

    var value2 = "ln(1/-x1 + Math.Sqrt(1/(x2*x2) + 1))"
        .BindVariable(0.5, "x1")
        .BindVariable(-0.5, "x2")
        .BindFunction(Math.Sqrt)
        .BindFunction(Math.Log, "ln")
        .Evaluate();

Example of using custom context:

    var context = new MathContext();
    context.BindVariable(0.5, "x1");
    context.BindVariable(-0.5, "x2");
    context.BindFunction(Math.Sqrt);
    context.BindFunction(Math.Log, "ln");

    "ln(1/-x1 + Math.Sqrt(1/(x2*x2) + 1))"
        .SetContext(context)
        .Evaluate();

## Supported math functions, operators, and constants

|          | Notation |
|--------- |--------- |
| Addition | + |
| Subtraction, Negativity | - |
| Multiplication  | * |
| Division  | / |
| Parentheses  | ( ) |
| Currency symbol  | depends on culture info |

#### Programming Math Context (using ProgrammingMathContext class):
|          | Notation |
|--------- |--------- |
| Addition | + |
| Subtraction, Negativity | - |
| Multiplication  | * |
| Division  | / |
| Parentheses  | ( ) |
| Currency symbol  | depends on culture info |
| Exponentiation | ** |
| Modulus | % |
| Floor Division  | // |
| Logical constants  | true, false, True, False, TRUE, FALSE |
| Equality  | = |
| Inequality  | \<> |
| Less than  | \< |
| Greater than  | > |
| Less than or equal  | \<= |
| Greater than or equal  | >= |
| Logical negation  | not, Not, NOT |
| Logical AND  | and, And, AND |
| Logical exclusive OR  | xor, Xor, XOR |
| Logical OR  | or, Or, OR |

#### Scientific Math Context (using ScientificMathContext class):

|          | Notation |
|--------- |--------- |
| Addition | + |
| Subtraction, Negativity | - |
| Multiplication  | *, ×, or · |
| Division  | / or ÷ |
| Parentheses, Square brackets | ( ) or [ ] |
| Currency symbol  | depends on culture info |
| Exponentiation | ^ |
| Modulus | mod, Mod, MOD, modulo, Modulo, or MODULO |
| Floor Division  | // |
| Absolute  | \| \|, abs, Abs, ABS |
| Ceiling | ⌈ ⌉ |
| Floor | ⌊ ⌋ |
| Square root, cube root, fourth root | √, ∛, ∜ |
| Natural logarithmic base | e |
| Natural logarithm | ln, Ln, LN |
| Common logarithm (base 10) | log, Log, LOG |
| Factorial | ! |
| Infinity | ∞ |
| Degree | ° |
| Pi constant | π, pi, Pi, PI |
| Tau constant | τ |
| Sine | sin, Sin, SIN |
| Cosine | cos, Cos, COS |
| Tangent | tan, Tan, TAN |
| Secant | sec, Sec, SEC |
| Cosecant | csc, Csc, CSC |
| Cotangent | cot, Cot, COT |
| Hyperbolic sine | sinh, Sinh, SINH |
| Hyperbolic cosine | cosh, Cosh, COSH |
| Hyperbolic tangent | tanh, Tanh, TANH |
| Hyperbolic secant | sech, Sech, SECH |
| Hyperbolic cosecant | csch, Csch, CSCH |
| Hyperbolic cotangent | coth, Coth, COTH |
| Inverse sine | arcsin, Arcsin, ARCSIN, sin\^-1, Sin\^-1, SIN\^-1 |
| Inverse cosine | arccos, Arccos, ARCCOS, cos\^-1, Cos\^-1, COS\^-1 |
| Inverse tangent | arctan, Arctan, ARCTAN, tan\^-1, Tan\^-1, TAN\^-1 |
| Inverse secant | arcsec, Arcsec, ARCSEC, sec\^-1, Sec\^-1, SEC\^-1 |
| Inverse cosecant | arccsc, Arccsc, ARCCSC, csc\^-1, Csc\^-1, CSC\^-1 |
| Inverse cotangent | arccot, Arccot, ARCCOT, cot\^-1, Cot\^-1, COT\^-1 |
| Inverse Hyperbolic sine | arsinh, Arsinh, ARSINH, sinh\^-1, Sinh\^-1, SINH\^-1 |
| Inverse Hyperbolic cosine | arcosh, Arcosh, ARCOSH, cosh\^-1, Cosh\^-1, COSH\^-1 |
| Inverse Hyperbolic tangent | artanh, Artanh, ARTANH, tanh\^-1, Tanh\^-1, TANH\^-1 |
| Inverse Hyperbolic secant | arsech, Arsech, ARSECH, sech\^-1, Sech\^-1, SECH\^-1 |
| Inverse Hyperbolic cosecant | arcsch, Arcsch, ARCSCH, csch\^-1, Csch\^-1, CSCH\^-1 |
| Inverse Hyperbolic cotangent | arcoth, Arcoth, ARCOTH, coth\^-1, Coth\^-1, COTH\^-1 |

#### How to evaluate C# math string expression
DotNetStandartMathContext is the .NET Standart 2.1 programming math context supports all constants and functions provided by the System.Math class, and supports equlity, comparision, logical boolean operators.

Example of evaluating C# expression:

    "-2 * Math.Log(1/0.5f + Math.Sqrt(1/Math.Pow(0.5d, 2) + 1L)".Evaluate(new DotNetStandartMathContext());

*NOTE: More math functions could be added to the math expression evaluator based on user needs.*

## Contributing
Contributions are welcome! Please fork the repository and submit pull requests for any enhancements or bug fixes.
If you enjoy my work and find it valuable, please consider becoming my [sponsor on GitHub](https://github.com/sponsors/AntonovAnton). Your support will enable me to share more open-source code. Together, we can make a positive impact in the developer community!

## License
This project is licensed under the Apache License, Version 2.0 - see the [LICENSE](https://github.com/AntonovAnton/math.evaluation?tab=License-1-ov-file) file for details.

## Contact
If you have any questions or suggestions, feel free to open an issue or contact me directly.