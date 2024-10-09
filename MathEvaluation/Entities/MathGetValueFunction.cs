using MathEvaluation.Extensions;
using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
/// The getting value function.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathGetValueFunction<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the getting value function.</summary>
    /// <value>The getting value function.</value>
    public Func<T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Initializes a new instance of the <see cref="MathGetValueFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The getting value function.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathGetValueFunction(string? key, Func<T> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }

    /// <inheritdoc/>
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var result = Fn();
        mathExpression.OnEvaluating(tokenPosition, i, result);

        var dResult = ConvertToDouble(result);
        dResult = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, dResult);
        value = value == default ? dResult : value * dResult;

        if (value != dResult && !double.IsNaN(value))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var result = Fn();
        mathExpression.OnEvaluating(tokenPosition, i, result);

        var dResult = ConvertToDecimal(result);
        dResult = mathExpression.EvaluateExponentiationDecimal(tokenPosition, ref i, separator, closingSymbol, dResult);
        value = value == default ? dResult : value * dResult;

        if (value != dResult)
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc/>
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var result = Fn();
        mathExpression.OnEvaluating(tokenPosition, i, result);

        var dResult = result is Complex r ? r : ConvertToDouble(result);
        dResult = mathExpression.EvaluateExponentiationComplex(tokenPosition, ref i, separator, closingSymbol, dResult);
        value = value == default ? dResult : value * dResult;

        if (value != dResult && !double.IsNaN(value.Real) && !double.IsNaN(value.Imaginary))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        Expression right = Expression.Invoke(Expression.Constant(Fn));
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = BuildConvert<TResult>(right);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultipyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}
