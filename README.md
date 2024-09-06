# Math Expression Evaluator in .NET
[![NuGet Downloads](https://img.shields.io/nuget/dt/MathEvaluator?style=for-the-badge)](https://www.nuget.org/packages/MathEvaluator/)
[![NuGet Version](https://img.shields.io/nuget/v/MathEvaluator?style=for-the-badge)](https://www.nuget.org/packages/MathEvaluator/)
## Overview
MathEvaluator is a .NET library that allows you to evaluate and compile any mathematical expressions from a string dynamically.

## Features
- Supports different mathematical contexts, such as scientific, programming, and other custom contexts.
- Evaluates to double, boolean, or decimal.
- Compiles a math expression string into executable code and produces a delegate that represents the math expression.
- Provides variable support within math expressions.
- Extensible with custom functions and operators.
- Fast and comprehensive. More than 1300 tests are passed, including complex math expressions (for example, -3^4sin(-π/2) or sin-3/cos1).

## Articles
[Evaluating Boolean Expressions. Comparison with NCalc.](https://medium.com/@AntonAntonov88/evaluate-boolean-expression-from-string-in-c-net-af80e08453ea)

## Installation

    dotnet add package MathEvaluator

Alternatively, you can install the package using the NuGet Package Manager Console:

    Install-Package MathEvaluator

## Perfomance
This math expression evaluator is designed for exceptional performance by leveraging modern .NET features and best practices, which is why it targets .NET Standard 2.1 or higher. 

This high-performance evaluator stands out due to its use of `ReadOnlySpan<char>`, avoidance of regular expressions, and reliance on static methods. These design choices collectively ensure minimal memory allocation, fast parsing, and efficient execution.

The evaluator uses recursive method calls to handle mathematical operations based on operator precedence and rules, an operator with highest precedence is evaluating first. This approach avoids the overhead associated with stack or queue data structures.

The evaluator uses a prefix tree, also known as a trie (pronounced "try"), for efficient searching of variables, operators, and functions by their keys (names) when providing a specific mathematical context or adding custom variables, operators, and functions is required.

Let's compare, for example, performance of calculating the mathematical expression:

    22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6

Below are the results of the comparison with the NCalc library: 

| Method                                                                   | Job      | Runtime  | Mean       | Error    | StdDev   | Gen0   | Allocated |
|------------------------------------------------------------------------- |--------- |--------- |-----------:|---------:|---------:|-------:|----------:|
| MathEvaluator | .NET 6.0 | .NET 6.0 |     670.33 ns |     3.304 ns |     3.091 ns | 0.0057 |      80 B |
| NCalc | .NET 6.0 | .NET 6.0 | 8,901.1 ns | 62.27 ns | 52.00 ns | 0.2747 |    3464 B |
| MathEvaluator | .NET 8.0 | .NET 8.0 |     547.54 ns |     1.283 ns |     1.072 ns | 0.0057 |      80 B |
| NCalc | .NET 8.0 | .NET 8.0 | 8,499.7 ns | 40.09 ns | 37.50 ns | 0.2594 |    3440 B |

## Compilation
Added in version [2.0.0](https://github.com/AntonovAnton/math.evaluation/releases/tag/2.0.0)

By using compilation, you can convert any mathematical expression string into a delegate, such as Func\<T, TResult> or Func\<TResult>, which significantly improves performance when evaluating the expression. 
However, since compilation takes time, it is beneficial to compile the expression beforehand if you plan to evaluate it multiple times, especially for 300 or more iterations. Refer to the [benchmarks](https://github.com/AntonovAnton/math.evaluation/blob/main/BenchmarkDotNet.Artifacts/results/Benchmarks-report-github.md) for detailed performance insights.

## How to use
Examples of using string extentions:

    "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".Evaluate();

    "22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6".EvaluateDecimal();

    "$22,888.32 * 30 / 323.34 / .5 - - 1 / (2 + $22,888.32) * 4 - 6".Evaluate(null, new CultureInfo("en-US"));

    "22’888.32 CHF * 30 / 323.34 / .5 - - 1 / (2 + 22’888.32 CHF) * 4 - 6".EvaluateDecimal(null, new CultureInfo("de-CH"));

    "ln(1/-0.5 + √(1/(0.5^2) + 1))".Evaluate(new ScientificMathContext());
    
    "ln(1/-0,5 + √(1/(0,5^2) + 1))".Evaluate(new ScientificMathContext(), new CultureInfo("fr"));
    
    "ln(1/-0.5 + √(1/(0.5^2) + 1))".EvaluateDecimal(new DecimalScientificMathContext());
    
    "ln(1/-0,5 + √(1/(0,5^2) + 1))".EvaluateDecimal(new DecimalScientificMathContext(), new CultureInfo("fr"));
    
    "4 % 3".Evaluate(new ProgrammingMathContext());
    
    "4 mod 3".EvaluateDecimal(new DecimalScientificMathContext());

    "4 <> 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01".EvaluateBoolean(new ProgrammingMathContext());

    "¬⊥∧⊤∨¬⊤⇒¬⊤".EvaluateBoolean(new ScientificMathContext());

Examples of using an instance of the MathExpression class:
        
    new MathExpression("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6").Evaluate();

    new MathExpression("22888.32 * 30 / 323.34 / .5 - -1 / (2 + 22888.32) * 4 - 6").EvaluateDecimal();

    new MathExpression("$22,888.32 * 30 / 323.34 / .5 - - 1 / (2 + $22,888.32) * 4 - 6", null, new CultureInfo("en-US")).Evaluate();

    new MathExpression("22’888.32 CHF * 30 / 323.34 / .5 - - 1 / (2 + 22’888.32 CHF) * 4 - 6", null, new CultureInfo("de-CH")).EvaluateDecimal();
    
    new MathExpression("ln(1/-0.5 + √(1/(0.5^2) + 1))", new ScientificMathContext()).Evaluate();
    
    new MathExpression("ln(1/-0,5 + √(1/(0,5^2) + 1))", new ScientificMathContext(), new CultureInfo("fr")).Evaluate();
    
    new MathExpression("ln(1/-0.5 + √(1/(0.5^2) + 1))", new DecimalScientificMathContext()).EvaluateDecimal();
    
    new MathExpression("ln(1/-0,5 + √(1/(0,5^2) + 1))", new DecimalScientificMathContext(), new CultureInfo("fr")).EvaluateDecimal();
    
    new MathExpression("4 % 3", new ProgrammingMathContext()).Evaluate();
    
    new MathExpression("4 mod 3", new DecimalScientificMathContext()).EvaluateDecimal();

    new MathExpression("4 <> 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", new ProgrammingMathContext()).EvaluateBoolean();

    new MathExpression("¬⊥∧⊤∨¬⊤⇒¬⊤", new ScientificMathContext()).EvaluateBoolean();

Examples of passing custom variables and functions as parameters:
        
    var x1 = 0.5;
    var x2 = -0.5;
    var sqrt = Math.Sqrt;
    Func<double, double> ln = Math.Log;

    var value1 = "ln(1/-x1 + sqrt(1/(x2*x2) + 1))"
        .Evaluate(new { x1, x2, sqrt, ln });

    var parameters = new MathParameters();
    parameters.BindVariable(0.5, "x1");
    parameters.BindVariable(-0.5, "x2");
    parameters.BindFunction(Math.Sqrt);
    parameters.BindFunction(d => Math.Log(d), "ln");

    var value2 = "ln(1/-x1 + Math.Sqrt(1/(x2*x2) + 1))"
        .Evaluate(parameters);

Example of using custom context:

    var context = new MathContext();
    context.BindFunction(Math.Sqrt);
    context.BindFunction(d => Math.Log(d), "ln");

    "ln(1/-x1 + Math.Sqrt(1/(x2*x2) + 1))"
        .Evaluate(new { x1 = 0.5, x2 = -0.5 }, context);

Examples of compilation:

    
    Func<decimal> fn1 = "22’888.32 CHF * 30 / 323.34 / .5 - - 1 / (2 + 22’888.32 CHF) * 4 - 6"
        .CompileDecimal(null, new CultureInfo("de-CH"));

    var value1 = fn1();

    var fn2 = "ln(1/x1 + √(1/(x2*x2) + 1))"
        .Compile(new { x1 = 0.0, x2 = 0.0 }, new ScientificMathContext());

    var value2 = fn2(new { x1 = -0.5, x2 = 0.5 });


## Supported math functions, operators, and constants

#### When no mathematical context is specified:
|          | Notation | Precedence |
|--------- |--------- | --------- |
| Addition | + | 0 |
| Subtraction, Negativity | - | 0 |
| Multiplication  | * | 100 |
| Division  | / | 100 |
| Parentheses  | ( ) | 200 |
| Currency symbol  | depends on culture info | |

#### Programming Math Context (using ProgrammingMathContext class):
|          | Notation | Precedence |
|--------- |--------- |--------- |
| Addition | + | 0 |
| Subtraction, Negativity | - | 0 |
| Multiplication  | * | 100 |
| Division  | / | 100 |
| Parentheses  | ( ) | 200 |
| Currency symbol  | depends on culture info | |
| Exponentiation | ** | 400 |
| Modulus | % | 100 |
| Floor Division  | // | 100 |
| Logical constants  | true, false, True, False, TRUE, FALSE | 300 |
| Equality  | = | -100 |
| Inequality  | \<> | -100 |
| Less than  | \< | -100 |
| Greater than  | > | -100 |
| Less than or equal  | \<= | -100 |
| Greater than or equal  | >= | -100 |
| Logical negation  | not, Not, NOT | -200 |
| Logical AND  | and, And, AND | -300 |
| Logical exclusive OR  | xor, Xor, XOR | -400 |
| Logical OR  | or, Or, OR | -500 |

#### Scientific Math Context (using ScientificMathContext class):

|          | Notation | Precedence |
|--------- |--------- | --------- |
| Addition | + | 0 |
| Subtraction, Negativity | - | 0 |
| Multiplication  | *, ×, or · | 100 |
| Division  | / or ÷ | 100 |
| Parentheses | ( ) | 200 |
| Currency symbol  | depends on culture info | |
| Exponentiation | ^ | 400 |
| Modulus | mod, Mod, MOD, modulo, Modulo, or MODULO | 100 |
| Floor Division  | // | 100 |
| Absolute  | \| \|, abs, Abs, ABS | 200 |
| Ceiling | ⌈ ⌉ | 200 |
| Floor | ⌊ ⌋ | 200 |
| Square root, cube root, fourth root | √, ∛, ∜ | 200 |
| Natural logarithmic base | e | 300 |
| Natural logarithm | ln, Ln, LN | 200 |
| Common logarithm (base 10) | log, Log, LOG | 200 |
| Factorial | ! | 500 |
| Infinity | ∞ | 300 |
| Logical constants  | true, false, True, False, TRUE, FALSE, T, F, ⊤, ⊥ | 300 |
| Equality  | = | -100 |
| Inequality  | ≠ | -100 |
| Less than  | \< | -100 |
| Greater than  | > | -100 |
| Less than or equal  | ≤, ⪯ | -100 |
| Greater than or equal  | ≥, ⪰ | -100 |
| Logical negation  | ¬, not, Not, NOT | 500 for ¬, -200 |
| Logical AND  | ∧, and, And, AND | -300 |
| Logical exclusive OR  | ⊕, xor, Xor, XOR | -400 |
| Logical OR  | ∨, or, Or, OR | -500 |
| Logical implication  | →, ⇒, ←, ⟸ | -800 |
| Logical biconditional equivalence  | ↔, ⇔ | -900 |
| Logical biconditional inequivalence  | ↮, ⇎ | -900 |
| Logical equivalence  | ≡ | -1000 |
| Logical inequivalence  | ≢ | -1000 |
| Degree | ° | 500 |
| Pi constant | π, pi, Pi, PI | 300 |
| Tau constant | τ | 300 |
| Sine | sin, Sin, SIN | 200 |
| Cosine | cos, Cos, COS | 200 |
| Tangent | tan, Tan, TAN | 200 |
| Secant | sec, Sec, SEC | 200 |
| Cosecant | csc, Csc, CSC | 200 |
| Cotangent | cot, Cot, COT | 200 |
| Hyperbolic sine | sinh, Sinh, SINH | 200 |
| Hyperbolic cosine | cosh, Cosh, COSH | 200 |
| Hyperbolic tangent | tanh, Tanh, TANH | 200 |
| Hyperbolic secant | sech, Sech, SECH | 200 |
| Hyperbolic cosecant | csch, Csch, CSCH | 200 |
| Hyperbolic cotangent | coth, Coth, COTH | 200 |
| Inverse sine | arcsin, Arcsin, ARCSIN, sin\^-1, Sin\^-1, SIN\^-1 | 200 |
| Inverse cosine | arccos, Arccos, ARCCOS, cos\^-1, Cos\^-1, COS\^-1 | 200 |
| Inverse tangent | arctan, Arctan, ARCTAN, tan\^-1, Tan\^-1, TAN\^-1 | 200 |
| Inverse secant | arcsec, Arcsec, ARCSEC, sec\^-1, Sec\^-1, SEC\^-1 | 200 |
| Inverse cosecant | arccsc, Arccsc, ARCCSC, csc\^-1, Csc\^-1, CSC\^-1 | 200 |
| Inverse cotangent | arccot, Arccot, ARCCOT, cot\^-1, Cot\^-1, COT\^-1 | 200 |
| Inverse Hyperbolic sine | arsinh, Arsinh, ARSINH, sinh\^-1, Sinh\^-1, SINH\^-1 | 200 |
| Inverse Hyperbolic cosine | arcosh, Arcosh, ARCOSH, cosh\^-1, Cosh\^-1, COSH\^-1 | 200 |
| Inverse Hyperbolic tangent | artanh, Artanh, ARTANH, tanh\^-1, Tanh\^-1, TANH\^-1 | 200 |
| Inverse Hyperbolic secant | arsech, Arsech, ARSECH, sech\^-1, Sech\^-1, SECH\^-1 | 200 |
| Inverse Hyperbolic cosecant | arcsch, Arcsch, ARCSCH, csch\^-1, Csch\^-1, CSCH\^-1 | 200 |
| Inverse Hyperbolic cotangent | arcoth, Arcoth, ARCOTH, coth\^-1, Coth\^-1, COTH\^-1 | 200 |

#### How to evaluate C# math string expression
DotNetStandartMathContext is the .NET Standart 2.1 programming math context supports all constants and functions provided by the System.Math class, and supports equlity, comparision, logical boolean operators.

Example of evaluating C# expression:

    "-2 * Math.Log(1/0.5f + Math.Sqrt(1/Math.Pow(0.5d, 2) + 1L))".Evaluate(new DotNetStandartMathContext());

*NOTE: More math functions could be added to the math expression evaluator based on user needs.*

## Contributing
Contributions are welcome! Please fork the repository and submit pull requests for any enhancements or bug fixes.
If you enjoy my work and find it valuable, please consider becoming my [sponsor on GitHub](https://github.com/sponsors/AntonovAnton). Your support will enable me to share more open-source code. Together, we can make a positive impact in the developer community!

## License
This project is licensed under the Apache License, Version 2.0 - see the [LICENSE](https://github.com/AntonovAnton/math.evaluation?tab=License-1-ov-file) file for details.

## Contact
If you have any questions or suggestions, feel free to open an issue or contact me directly.
