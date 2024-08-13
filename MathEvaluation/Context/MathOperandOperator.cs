using System;

namespace MathEvaluation.Context;

/// <summary>
/// The math operator processes the left and right math operands.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="MathEvaluation.Context.MathOperator&lt;T&gt;" />
public class MathOperandOperator<T>(string? key, Func<T, T, T> fn, int precedece)
    : MathOperator<T>(key, fn, precedece)
    where T : struct
{
}