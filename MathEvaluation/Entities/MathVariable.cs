﻿using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The math variable uses as a parameter.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathVariable<T>(string? key, T value) : MathEntity(key)
    where T : struct
{
    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Variable;

    /// <summary>Gets the value.</summary>
    /// <value>The value.</value>
    public T Value { get; } = value;

    /// <inheritdoc/>
    public override Expression ToExpression()
    {
        return Expression.Constant(Value);
    }
}
