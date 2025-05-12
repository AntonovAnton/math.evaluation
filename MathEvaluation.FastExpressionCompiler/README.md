<img src="../logo.png" alt="logo" style="width:64px;height:64px;"/>

# Fast Math Expression Compiler in .NET

NuGet packages:

- MathEvaluator [![NuGet Version](https://img.shields.io/nuget/v/MathEvaluator)](https://www.nuget.org/packages/MathEvaluator) [![NuGet Downloads](https://img.shields.io/nuget/dt/MathEvaluator)](https://www.nuget.org/packages/MathEvaluator)
- MathEvaluator.FastExpressionCompiler [![NuGet Version](https://img.shields.io/nuget/v/MathEvaluator.FastExpressionCompiler)](https://www.nuget.org/packages/MathEvaluator.FastExpressionCompiler) [![NuGet Downloads](https://img.shields.io/nuget/dt/MathEvaluator.FastExpressionCompiler)](https://www.nuget.org/packages/MathEvaluator.FastExpressionCompiler)

MathEvaluator.FastExpressionCompiler is an extension of the [MathEvaluator](https://nuget.org/packages/MathEvaluator) library that uses the [FastExpressionCompiler](https://github.com/dadhi/FastExpressionCompiler) library to provide high-performance compilation of mathematical expressions.

Compilation of expressions is up to 10-40x faster than the built-in .NET `LambdaExpression.Compile()` method. This library includes all features of the MathEvaluator library. For more details, refer to the [MathEvaluator GitHub repository](https://github.com/AntonovAnton/math.evaluation?tab=readme-ov-file).

## How to use
Examples of using string extentions:

    using MathEvaluation.Extensions;
    
    // Compile a double expression
    var compiled = "x * y + z".CompileFast(new { x = 0, y = 0, z = 0 });
    double result = compiled(new { x = 5, y = 6, z = 7 }); // Result: 47

    // Compile a decimal expression
    var compiledDecimal = "x / y".CompileDecimalFast(new { x = 0m, y = 0m });
    decimal decimalResult = compiledDecimal(new { x = 20m, y = 4m }); // Result: 5

    // Compile a boolean expression
    var compiledBoolean = "x > y".CompileBooleanFast(new { x = 0, y = 0 });
    bool booleanResult = compiledBoolean(new { x = 2, y = 3 }); // Result: false

    // Compile a complex expression
    var compiledComplex = "x + y * i".CompileComplexFast(new { x = 0, y = 0 });
    Complex complexResult = compiledComplex(new { x = 3, y = 4 }); // Result: 3 + 4i

Example of compilation with a Dictionary as a parameter (Added in version [2.3.0](https://github.com/AntonovAnton/math.evaluation/releases/tag/2.3.0)):

    var dict = new Dictionary<string, double>();
    dict.Add("x1", 3.0);
    dict.Add("x2", 2.0);

    var fn = "x1 + Math.Sin(x2) * 0.5"
        .CompileFast(dict, new DotNetStandardMathContext());

    var value = fn(dict);
    Console.WriteLine(value); // 3.4546487...

### FastMathExpression class
The `FastMathExpression` class is a high-performance alternative to `MathExpression`. It uses `FastMathExpressionCompiler` by default for compiling expressions. Examples of using an instance of the `FastMathExpression` class:
        
    var dict = new Dictionary<string, double>();
    dict.Add("x1", 3.0);
    dict.Add("x2", 2.0);

    var fn = new FastMathExpression("x1 + Math.Sin(x2) * 0.5", new DotNetStandardMathContext())
        .Compile(dict);

    var value = fn(dict);
    Console.WriteLine(value); // 3.4546487...


### FastMathExpressionCompiler class
The `FastMathExpressionCompiler` class implements the `IExpressionCompiler` interface (Added in version [2.3.1](https://github.com/AntonovAnton/math.evaluation/releases/tag/2.3.1)) and uses the [FastExpressionCompiler](https://github.com/dadhi/FastExpressionCompiler) to compile LINQ expressions into delegates.

## Performance
By leveraging `FastExpressionCompiler`, this library significantly reduces the time required to compile mathematical expressions. This makes it ideal for scenarios where expressions need to be compiled and executed repeatedly.

## Contributing
Contributions are welcome! Please fork the repository and submit pull requests for any enhancements or bug fixes.

Looking to localize your project? Check out [l10n.dev](https://l10n.dev), an AI-powered localization service. [Translate JSON](https://l10n.dev/ws/translate-json) files while preserving format, keys, and placeholders. Supports 165 languages with an easy-to-use API and UI. Get started for free!

## License
This project is licensed under the Apache License, Version 2.0 - see the [LICENSE](https://github.com/AntonovAnton/math.evaluation?tab=License-1-ov-file) file for details.

## Contact
If you have any questions or suggestions, feel free to open an issue or contact me directly.
