using MathEvaluation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The function with multiple parameters, so opening, separator, and closing symbol are required.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathFunction<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T[], T> Fn { get; }

    /// <summary>Gets the opening symbol.</summary>
    /// <value>The opening symbol.</value>
    public char OpeningSymbol { get; }

    /// <summary>Gets the parameters separator.</summary>
    /// <value>The parameter separator.</value>
    public char Separator { get; }

    /// <summary>Gets the closing symbol.</summary>
    /// <value>The closing symbol.</value>
    public char ClosingSymbol { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Initializes a new instance of the <see cref="MathFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public MathFunction(string? key, Func<T[], T> fn, char openingSymbol, char separator, char closingSymbol)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Separator = separator;
        OpeningSymbol = openingSymbol;
        ClosingSymbol = closingSymbol;
    }

    /// <inheritdoc />
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (decimal)value);

        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol, tokenPosition, ref i);

        var args = new List<T>();
        while (mathExpression.MathString.Length > i)
        {
            var arg = mathExpression.Evaluate(ref i, Separator, ClosingSymbol);
            args.Add(arg is T a ? a : (T)ChangeType(arg, typeof(T)));

            if (mathExpression.MathString[i] == Separator)
            {
                i++; //other param
                continue;
            }

            break;
        }

        mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol, tokenPosition, ref i);

        var fnResult = Fn([.. args]);
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
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (double)value);

        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol, tokenPosition, ref i);

        var args = new List<T>();
        while (mathExpression.MathString.Length > i)
        {
            var arg = mathExpression.EvaluateDecimal(ref i, Separator, ClosingSymbol);
            args.Add(arg is T a ? a : (T)ChangeType(arg, typeof(T)));

            if (mathExpression.MathString[i] == Separator)
            {
                i++; //other param
                continue;
            }

            break;
        }

        mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol, tokenPosition, ref i);

        var fnResult = Fn([.. args]);
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
        mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol, tokenPosition, ref i);

        var args = new List<T>();
        while (mathExpression.MathString.Length > i)
        {
            var arg = mathExpression.EvaluateComplex(ref i, Separator, ClosingSymbol);
            args.Add(arg is T a ? a : (T)ChangeType(arg, typeof(T)));

            if (mathExpression.MathString[i] == Separator)
            {
                i++; //other param
                continue;
            }

            break;
        }

        mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol, tokenPosition, ref i);

        var fnResult = Fn([.. args]);
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
        mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol, tokenPosition, ref i);

        var args = new List<Expression>();
        while (mathExpression.MathString.Length > i)
        {
            var arg = mathExpression.Build<T>(ref i, Separator, ClosingSymbol);
            args.Add(arg);

            if (mathExpression.MathString[i] == Separator)
            {
                i++; //other param
                continue;
            }

            break;
        }

        mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol, tokenPosition, ref i);

        Expression right = Expression.Invoke(Expression.Constant(Fn), Expression.NewArrayInit(typeof(T), args));
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = BuildConvert<TResult>(right);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}