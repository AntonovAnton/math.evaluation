using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
///     The base programming math context supports floor division '//', uses '**' notation for exponentiation, and '%' for
///     modulo operation.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DecimalProgrammingMathContext : ProgrammingMathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalProgrammingMathContext" /> class.</summary>
    public DecimalProgrammingMathContext()
        : base()
    {
        BindConstant(1m, "true");
        BindConstant(1m, "True");
        BindConstant(1m, "TRUE");

        BindConstant(0m, "false");
        BindConstant(0m, "False");
        BindConstant(0m, "FALSE");

        static decimal floorDivisionFn(decimal left, decimal right) => Math.Floor(left / right);

        BindOperator(floorDivisionFn, "//");

        static decimal iifFn(decimal[] args) => args[0] != default
            ? args.Length > 1 ? args[1] : 1m
            : args.Length > 2
                ? args[2]
                : args.Length > 3
                    ? throw new ArgumentOutOfRangeException(nameof(args), "Count of args > 3")
                    : 0m;

        BindFunction(iifFn, "iif");
        BindFunction(iifFn, "Iif");
        BindFunction(iifFn, "IIF");
    }
}