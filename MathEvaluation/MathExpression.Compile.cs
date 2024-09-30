using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System;
using System.Linq.Expressions;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <summary>Gets the expression tree.</summary>
    /// <value>The expression tree.</value>
    public Expression? ExpressionTree { get; private set; }

    internal ParameterExpression? ParameterExpression { get; private set; }

    /// <inheritdoc cref="Compile{TResult}()"/>
    public Func<double> Compile()
        => Compile<double>();

    /// <inheritdoc cref="Compile{T, TResult}(T)"/>
    public Func<T, double> Compile<T>(T parameters)
        => Compile<T, double>(parameters);

    internal Expression Build<TResult>(ref int i, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
        where TResult : struct, IConvertible
    {
        var span = MathString.AsSpan();
        Expression expression = Expression.Constant(default(TResult));

        var start = i;
        while (span.Length > i)
        {
            if (separator.HasValue && IsParamSeparator(separator.Value, start, i) ||
                closingSymbol.HasValue && span[i] == closingSymbol.Value)
            {
                if (expression.IsZero())
                    MathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return expression;
            }

            if (span[i] is >= '0' and <= '9' || span[i] == _decimalSeparator) //number
            {
                if (isOperand)
                    return Build<TResult>(ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                expression = Expression.Constant(span.ParseNumber<TResult>(_numberFormat, ref i));
                continue;
            }

            switch (span[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return expression;

                    var tokenPosition = i;
                    i++;
                    var right = Build<TResult>(ref i, null, ')');
                    MathString.ThrowExceptionIfNotClosed(')', tokenPosition, ref i);
                    if (isOperand)
                        return right;

                    right = BuildExponentiation<TResult>(ref i, separator, closingSymbol, right);
                    expression = expression.IsZero() ? right : Expression.Multiply(expression, right).Reduce();
                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsMeaningless(start, i))
                        return expression;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = Build<TResult>(ref i, separator, closingSymbol, p, isOperand);
                    expression = Expression.Add(expression, right).Reduce();
                    if (isOperand)
                        return expression;
                    break;
                case '-' when span.Length == i + 1 || span[i + 1] != '-':
                    if (precedence >=(int)EvalPrecedence.LowestBasic && !MathString.IsMeaningless(start, i))
                        return expression;

                    var isNegativity = start == i;
                    i++;
                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = Build<TResult>(ref i, separator, closingSymbol, p, isOperand);
                    expression = isNegativity ? Expression.Negate(right) : Expression.Subtract(expression, right); //it keeps sign
                    expression = expression.Reduce();
                    if (isOperand)
                        return expression;
                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build<TResult>(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = Expression.Multiply(expression, right).Reduce();
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build<TResult>(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = Expression.Divide(expression, right).Reduce();
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = FirstMathEntity(span[i..]);
                    if (entity == null && span.TryParseCurrency(_numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return expression;

                    if (entity != null)
                        expression = entity.Build<TResult>(this, ref i, separator, closingSymbol, expression);
                    else
                        MathString.ThrowExceptionInvalidToken(i);

                    if (isOperand)
                        return expression;
                    break;
            }
        }

        if (expression.IsZero())
            MathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return expression.Reduce();
    }

    internal Expression BuildOperand<TResult>(ref int i, char? separator, char? closingSymbol)
        where TResult : struct, IConvertible
    {
        var start = i;
        var expression = Build<TResult>(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (expression.IsZero())
            MathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return expression;
    }

    internal Expression BuildExponentiation<TResult>(ref int i, char? separator, char? closingSymbol, Expression left)
        where TResult : struct, IConvertible
    {
        MathString.SkipMeaningless(ref i);
        if (MathString.Length <= i)
            return left;

        var entity = FirstMathEntity(MathString.AsSpan(i));
        if (entity != null && entity.Precedence >= (int)EvalPrecedence.Exponentiation)
            return entity.Build<TResult>(this, ref i, separator, closingSymbol, left);

        return left;
    }

    /// <summary>Compiles the <see cref="MathString">math expression string</see>.</summary>
    /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="MathExpressionException"/>
    private Func<TResult> Compile<TResult>()
        where TResult : struct, IConvertible
    {
        try
        {
            var i = 0;
            ExpressionTree = Build<TResult>(ref i, null, null);

            return Expression.Lambda<Func<TResult>>(ExpressionTree).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, null);
        }
    }

    /// <inheritdoc cref="Compile{T}()"/>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException">parameters</exception>
    private Func<T, TResult> Compile<T, TResult>(T parameters)
        where TResult : struct, IConvertible
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        _parameters = new MathParameters(parameters);
        ParameterExpression = Expression.Parameter(typeof(T), nameof(parameters));

        try
        {
            var i = 0;
            ExpressionTree = Build<TResult>(ref i, null, null);

            return Expression.Lambda<Func<T, TResult>>(ExpressionTree, ParameterExpression).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }
}