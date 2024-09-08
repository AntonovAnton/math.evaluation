using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator processes the left and right expressions.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperator<T> : MathEntity
    where T : struct
{
    private readonly ExpressionType? _binaryOperatorType;

    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="fn">The function.</param>
    /// <param name="precedece">The operator precedence.</param>
    /// <param name="binaryOperatorType">The specified expression type of the operator allows improve performance if it matches a C# binary operator.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathOperator(string? key, Func<T, T, T> fn, int precedece = (int)EvalPrecedence.Basic, ExpressionType? binaryOperatorType = null)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Precedence = precedece;

        _binaryOperatorType = binaryOperatorType;
    }

    /// <inheritdoc/>
    public override Expression BuildExpression()
    {
        return Expression.Constant(Fn);
    }

    /// <inheritdoc cref="BuildExpression()"/>
    public Expression BuildExpression(Expression left, Expression right)
    {
        if (_binaryOperatorType.HasValue)
        {
            if (_binaryOperatorType.Value is ExpressionType.AndAlso or ExpressionType.OrElse)
            {
                if (left.Type != typeof(bool))
                    left = Expression.NotEqual(left, typeof(T) == typeof(decimal) ? Expression.Constant(0.0m) : Expression.Constant(0.0));
            }

            if (_binaryOperatorType.Value is ExpressionType.AndAlso or ExpressionType.OrElse or ExpressionType.Not)
            {
                if (right.Type != typeof(bool))
                    right = Expression.NotEqual(right, typeof(T) == typeof(decimal) ? Expression.Constant(0.0m) : Expression.Constant(0.0));
            }

            if (_binaryOperatorType.Value is ExpressionType.And or ExpressionType.Or or ExpressionType.ExclusiveOr)
            {
                if (left.Type != typeof(bool))
                    left = Expression.Convert(left, typeof(long));

                if (right.Type != typeof(bool))
                    right = Expression.Convert(right, typeof(long));
            }


            Expression expression = _binaryOperatorType.Value is ExpressionType.Not
                ? expression = Expression.Not(right).Reduce()
                : expression = Expression.MakeBinary(_binaryOperatorType.Value, left, right).Reduce();

            if (expression.Type != typeof(T))
            {
                expression = typeof(T) == typeof(decimal) && expression.Type == typeof(bool)
                    ? Expression.Condition(expression, Expression.Constant(1.0m), Expression.Constant(0.0m))
                    : expression = Expression.Convert(expression, typeof(T)).Reduce();
            }

            return expression;
        }

        return Expression.Invoke(BuildExpression(), left, right);
    }
}
