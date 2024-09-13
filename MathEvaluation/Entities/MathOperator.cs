using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator processes the left and right expressions.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperator<T> : MathEntity
    where T : struct, IConvertible
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
    public override double Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, ref i, separator, closingSymbol, (decimal)value);

        i += Key.Length;
        var right = mathExpression.Evaluate(ref i, separator, closingSymbol, Precedence);
        return Convert.ToDouble(Fn(
            value is T v ? v : (T)Convert.ChangeType(value, typeof(T)),
            right is T r ? r : (T)Convert.ChangeType(right, typeof(T))));
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, ref i, separator, closingSymbol, (double)value);

        i += Key.Length;
        var right = mathExpression.EvaluateDecimal(ref i, separator, closingSymbol, Precedence);
        return Convert.ToDecimal(Fn(
            value is T v ? v : (T)Convert.ChangeType(value, typeof(T)),
            right is T r ? r : (T)Convert.ChangeType(right, typeof(T))));
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;
        var right = mathExpression.Build<T>(ref i, separator, closingSymbol, Precedence);
        return Build<TResult>(left, right);
    }

    private Expression Build<TResult>(Expression left, Expression right)
    {
        if (_binaryOperatorType.HasValue)
        {
            if (_binaryOperatorType.Value is ExpressionType.AndAlso or ExpressionType.OrElse)
            {
                left = ConvertToBoolean(left);
                right = ConvertToBoolean(right);
            }

            if (_binaryOperatorType.Value is ExpressionType.And or ExpressionType.Or or ExpressionType.ExclusiveOr)
            {
                left = ConvertToLong(left);
                right = ConvertToLong(right);
            }

            // if logical negation operation (NOT right)
            var expression = _binaryOperatorType.Value is ExpressionType.Not
                ? Expression.Not(ConvertToBoolean(right)).Reduce()
                : Expression.MakeBinary(_binaryOperatorType.Value, left, right).Reduce();

            if (typeof(TResult) == expression.Type)
                return expression;

            return typeof(TResult) == typeof(decimal) && expression.Type == typeof(bool)
                ? Expression.Condition(expression, Expression.Constant(1.0m), Expression.Constant(0.0m))
                : Expression.Convert(expression, typeof(TResult)).Reduce();
        }

        left = left.Type != typeof(T) ? Expression.Convert(left, typeof(T)) : left;
        Expression result = Expression.Invoke(Expression.Constant(Fn), left, right);
        result = result.Type != typeof(TResult) ? Expression.Convert(result, typeof(TResult)) : result;
        return result;
    }

    private static Expression ConvertToBoolean(Expression expression)
    {
        if (expression.Type != typeof(bool))
            expression = Expression.NotEqual(expression, typeof(T) == typeof(decimal)
                ? Expression.Constant(0.0m)
                : Expression.Constant(0.0));

        return expression;
    }

    private static Expression ConvertToLong(Expression expression)
    {
        if (expression.Type != typeof(bool))
            expression = Expression.Convert(expression, typeof(long));

        return expression;
    }
}
