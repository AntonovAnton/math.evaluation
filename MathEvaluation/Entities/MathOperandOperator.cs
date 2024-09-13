using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator that performs an action on one math operand. 
/// For example, degrees, factorial, decrement, increment, or negation.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperandOperator<T> : MathEntity
    where T : struct, IConvertible
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.OperandUnaryOperator;

    /// <summary>
    /// Gets a value indicating whether this instance is processing left operand.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is processing left operand; otherwise, <c>false</c>.
    /// </value>
    public bool IsProcessingLeft { get; } = false;

    /// <summary>
    /// Gets a value indicating whether this instance is processing right operand.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is processing right operand; otherwise, <c>false</c>.
    /// </value>
    public bool IsProcessingRight => !IsProcessingLeft;

    /// <summary>Initializes a new instance of the <see cref="MathOperandOperator{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="isProcessingLeft">
    ///   <c>true</c> if this instance is processing left operand; otherwise, <c>false</c>.
    /// </param>
    /// <exception cref="ArgumentNullException"/>
    public MathOperandOperator(string? key, Func<T, T> fn, bool isProcessingLeft = false)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        IsProcessingLeft = isProcessingLeft;
    }

    /// <inheritdoc/>
    public override double Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, ref i, separator, closingSymbol, (decimal)value);

        i += Key.Length;

        T result;
        if (IsProcessingLeft)
            result = Fn(value is T v ? v : (T)Convert.ChangeType(value, typeof(T)));
        else
        {
            var right = mathExpression.EvaluateOperand(ref i, separator, closingSymbol);
            result = Fn(right is T r ? r : (T)Convert.ChangeType(right, typeof(T)));
        }

        return mathExpression.EvaluateExponentiation(ref i, separator, closingSymbol, Convert.ToDouble(result));
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, ref i, separator, closingSymbol, (double)value);

        i += Key.Length;

        T result;
        if (IsProcessingLeft)
            result = Fn(value is T v ? v : (T)Convert.ChangeType(value, typeof(T)));
        else
        {
            var right = mathExpression.EvaluateOperandDecimal(ref i, separator, closingSymbol);
            result = Fn(right is T r ? r : (T)Convert.ChangeType(right, typeof(T)));
        }

        return mathExpression.EvaluateExponentiationDecimal(ref i, separator, closingSymbol, Convert.ToDecimal(result));
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;

        Expression result;
        if (IsProcessingLeft)
        {
            left = left.Type != typeof(T) ? Expression.Convert(left, typeof(T)) : left;
            result = Expression.Invoke(Expression.Constant(Fn), left);
        }
        else
        {
            var right = mathExpression.Build<T>(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
            result = Expression.Invoke(Expression.Constant(Fn), right);
        }

        result = result.Type != typeof(TResult) ? Expression.Convert(result, typeof(TResult)) : result;
        return mathExpression.BuildExponentiation<TResult>(ref i, separator, closingSymbol, result);
    }
}
