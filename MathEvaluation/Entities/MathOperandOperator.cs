using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator that performs an action on one math operand. 
/// For example, degrees, factorial, decrement, increment, or negation.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperandOperator<T> : MathEntity
    where T : struct
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
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (decimal)value);

        i += Key.Length;

        T result;
        if (IsProcessingLeft)
            result = Fn(value is T v ? v : (T)ChangeType(value, typeof(T)));
        else
        {
            start = i - Key.Length; //tokenPosition
            var right = mathExpression.EvaluateOperand(ref i, separator, closingSymbol);
            result = Fn(right is T v ? v : (T)ChangeType(right, typeof(T)));
        }

        mathExpression.OnEvaluating(start, i, result);

        var dResult = ConvertToDouble(result);
        return mathExpression.EvaluateExponentiation(start, ref i, separator, closingSymbol, dResult);
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (double)value);

        i += Key.Length;

        T result;
        if (IsProcessingLeft)
            result = Fn(value is T v ? v : (T)ChangeType(value, typeof(T)));
        else
        {
            start = i - Key.Length; //tokenPosition
            var right = mathExpression.EvaluateOperandDecimal(ref i, separator, closingSymbol);
            result = Fn(right is T v ? v : (T)ChangeType(right, typeof(T)));
        }

        mathExpression.OnEvaluating(start, i, result);

        var dResult = ConvertToDecimal(result);
        return mathExpression.EvaluateExponentiationDecimal(start, ref i, separator, closingSymbol, dResult);
    }

    /// <inheritdoc/>
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        i += Key.Length;

        T result;
        if (IsProcessingLeft)
            result = Fn(value is T v ? v : (T)ChangeType(value, typeof(T)));
        else
        {
            start = i - Key.Length; //tokenPosition
            var right = mathExpression.EvaluateOperandComplex(ref i, separator, closingSymbol);
            result = Fn(right is T v ? v : (T)ChangeType(right, typeof(T)));
        }

        mathExpression.OnEvaluating(start, i, result);

        var dResult = result is Complex r ? r : ConvertToDouble(result);
        return mathExpression.EvaluateExponentiationComplex(start, ref i, separator, closingSymbol, dResult);
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;

        Expression result;
        if (IsProcessingLeft)
        {
            left = BuildConvert<T>(left);
            result = Expression.Invoke(Expression.Constant(Fn), left);
        }
        else
        {
            start = i - Key.Length; //tokenPosition
            var right = mathExpression.Build<T>(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
            result = Expression.Invoke(Expression.Constant(Fn), right);
        }

        mathExpression.OnEvaluating(start, i, result);

        result = BuildConvert<TResult>(result);
        return mathExpression.BuildExponentiation<TResult>(start, ref i, separator, closingSymbol, result);
    }
}
