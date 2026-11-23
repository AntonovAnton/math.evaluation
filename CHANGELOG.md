# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [4.0.0] - 2026-11-22

### Added

- **.NET 11 target.** `MathEvaluator` and `MathEvaluator.FastExpressionCompiler` now multi-target .NET 8, .NET 9, .NET 10, and .NET 11.
- **.NET 11 numeric types** (available on `net11.0` only):
  - IEEE 754 decimal floating-point types `Decimal32`, `Decimal64`, `Decimal128`, and `BFloat16` work with the generic `Evaluate<T>()` / `Compile<T>()` APIs.
  - Generic `Complex<T>` numbers (`Complex<double>`, `Complex<float>`, `Complex<Half>`, `Complex<BFloat16>`, `Complex<Decimal32>`, `Complex<Decimal64>`, `Complex<Decimal128>`):
    - `EvaluateComplex<T>()` on `MathExpression` and on `string`, with overloads for no parameters, an object, `IDictionary<string, Complex<T>>`, `IDictionary<string, object>`, and `MathParameters`.
    - `CompileComplex<TParameters, T>()` and `CompileComplexAsyncFunc<TParameters, T>()`.
  - Variables, dictionary items, and context bindings of all these types are recognized by `MathParameters` and `MathContext`.
  - `DotNetMathContext` binds the members of these types in C# syntax, e.g. `Decimal64.Pi`, `Decimal128.MaxValue`, `BFloat16.Sqrt(x)`, `Decimal64.Quantize(x, y)`, `default(Decimal32)`, and the matching `Complex<T>` members.
- **Generic math contexts:**
  - `ScientificMathContext<T>` calculates scientific functions and constants in the precision of any IEEE 754 floating-point type (`double`, `float`, `Half`, and on .NET 11 `BFloat16`, `Decimal32`, `Decimal64`, `Decimal128`).
  - `ProgrammingMathContext<T>` calculates constants, floor division `//`, modulo `%`, and `iif` in the precision of any `IFloatingPoint<T>` type, including `decimal`.
  - `ComplexScientificMathContext<T>` is the scientific context for generic `Complex<T>` numbers (.NET 11+).
- **FastExpressionCompiler async compilation:** `CompileAsyncFuncFast`, `CompileDecimalAsyncFuncFast`, `CompileBooleanAsyncFuncFast`, and `CompileComplexAsyncFuncFast` string extensions compile expressions with async functions into a `Func<T, CancellationToken, ValueTask<TResult>>` delegate (#130).
- **FastExpressionCompiler generic complex compilation (.NET 11+):** `CompileComplexFast<TParameters, T>()` and `CompileComplexAsyncFuncFast<TParameters, T>()` string extensions compile expressions that return a generic `Complex<T>`.

### Changed

- **Breaking:** `ScientificMathContext` and `ProgrammingMathContext` now inherit from `ScientificMathContext<double>` and `ProgrammingMathContext<double>`. Their behavior is unchanged, but code that relies on the exact base type (for example, reflection or `GetType().BaseType` checks) is affected.
- **Breaking:** `DotNetStandardMathContext` now binds C# literal suffixes, `default(...)` values, and `MinValue`/`MaxValue` constants with their actual types. Before, it converted them to `double`. For example:
  - `1f` is now a `float`, `1L` a `long`, and `1u` a `uint`.
  - `int.MaxValue` is now an `int`, and `decimal.MaxValue` a `decimal`.

  Expressions that mix these constants can produce different result types or precision than in 3.x.
- `DecimalProgrammingMathContext` now inherits from `ProgrammingMathContext<decimal>`.
- `MathEvaluator.FastExpressionCompiler` now depends on `MathEvaluator` 4.0.0.
- `EvaluatingStep` events are no longer raised for NaN constant values, and they now also apply to generic complex values.

### Deprecated

- `DecimalProgrammingMathContext`: use `ProgrammingMathContext<decimal>` instead.

[4.0.0]: https://github.com/AntonovAnton/math.evaluation/compare/v3.8.0...v4.0.0
