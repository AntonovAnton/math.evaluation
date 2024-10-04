using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator processes the left and right math operands.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperandsOperator<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperandsOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="fn">The function.</param>
    /// <param name="precedece">The operator precedence.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathOperandsOperator(string? key, Func<T, T, T> fn, int precedece)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Precedence = precedece;
    }

    /// <inheritdoc/>
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (decimal)value);

        i += Key.Length;
        var right = mathExpression.EvaluateOperand(ref i, separator, closingSymbol);
        right = mathExpression.EvaluateExponentiation(start, ref i, separator, closingSymbol, right);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        var dResult = ConvertToDouble(result);
        return dResult;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (double)value);

        i += Key.Length;
        var right = mathExpression.EvaluateOperandDecimal(ref i, separator, closingSymbol);
        right = mathExpression.EvaluateExponentiationDecimal(start, ref i, separator, closingSymbol, right);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        var dResult = ConvertToDecimal(result);
        return dResult;
    }

    /// <inheritdoc/>
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        if (typeof(T) != typeof(Complex))
        {
            if (value.Imaginary == default)
                return (Complex)Evaluate(mathExpression, start, ref i, separator, closingSymbol, value.Real);

            throw new NotSupportedException(NotComplexErrorMessage);
        }

        i += Key.Length;
        var right = mathExpression.EvaluateOperandComplex(ref i, separator, closingSymbol);
        right = mathExpression.EvaluateExponentiationComplex(start, ref i, separator, closingSymbol, right);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        var dResult = result is Complex r ? r : ConvertToDouble(result);
        return dResult;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;

        left = left.Type != typeof(T) ? Expression.Convert(left, typeof(T)) : left;

        var right = mathExpression.BuildOperand<T>(ref i, separator, closingSymbol);
        right = mathExpression.BuildExponentiation<T>(start, ref i, separator, closingSymbol, right);

        Expression result = Expression.Invoke(Expression.Constant(Fn), left, right);
        result = result.Type != typeof(TResult) ? Expression.Convert(result, typeof(TResult)) : result;

        mathExpression.OnEvaluating(start, i, result);
        return result;
    }
}