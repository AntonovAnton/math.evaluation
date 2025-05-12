using MathEvaluation.Compilation;
using MathEvaluation.Context;
using System;

namespace MathEvaluation;

/// <inheritdoc />
public sealed class FastMathExpression : MathExpression
{
    /// <summary>Initializes a new instance of the <see cref="FastMathExpression" /> class.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="context">The math context.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <param name="compiler">The specified expression compiler. If null, the <see cref="FastMathExpressionCompiler">FastMathExpressionCompiler</see> will be used.</param>
    /// <inheritdoc />
    public FastMathExpression(string mathString, IMathContext? context = null, IFormatProvider? provider = null,
        IExpressionCompiler? compiler = null)
        : base(mathString, context, provider, compiler ?? new FastMathExpressionCompiler())
    {
    }
}