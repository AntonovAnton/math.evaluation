using MathEvaluation.Extensions;
using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The getting value function.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathGetValueFunction<T> : MathEntity
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
    /// <exception cref="ArgumentNullException" />
    public MathGetValueFunction(string? key, Func<T> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }

    /// <inheritdoc />
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var fnResult = Fn();
        mathExpression.OnEvaluating(tokenPosition, i, fnResult);

        var result = ConvertToDouble(fnResult);
        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc />
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var fnResult = Fn();
        mathExpression.OnEvaluating(tokenPosition, i, fnResult);

        var result = ConvertToDecimal(fnResult);
        result = mathExpression.EvaluateExponentiationDecimal(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result)
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc />
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var fnResult = Fn();
        mathExpression.OnEvaluating(tokenPosition, i, fnResult);

        var result = fnResult is Complex r ? r : ConvertToDouble(fnResult);
        result = mathExpression.EvaluateExponentiationComplex(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value.Real) && !double.IsNaN(value.Imaginary))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc />
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        Expression right = Expression.Invoke(Expression.Constant(Fn));
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = BuildConvert<TResult>(right);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}