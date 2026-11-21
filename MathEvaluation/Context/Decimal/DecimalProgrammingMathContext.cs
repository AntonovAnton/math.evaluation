﻿#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
///     The programming math context for <see cref="decimal" /> numbers supports floor division '//', uses '**' notation for exponentiation,
///     and '%' for modulo operation. The constants, the floor division, the modulo, and the iif function are calculated in decimal.
/// </summary>
/// <seealso cref="MathEvaluation.Context.ProgrammingMathContext{T}" />
public class DecimalProgrammingMathContext : ProgrammingMathContext<decimal>
{
    /// <summary>Initializes a new instance of the <see cref="DecimalProgrammingMathContext" /> class.</summary>
    public DecimalProgrammingMathContext()
    {
    }
}
