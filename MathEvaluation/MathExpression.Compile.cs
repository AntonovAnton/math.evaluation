using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <summary>Gets the expression tree.</summary>
    /// <value>The expression tree.</value>
    public Expression? ExpressionTree { get; private set; }

    internal ParameterExpression? ParameterExpression { get; private set; }

    /// <inheritdoc cref="Compile{TResult}()" />
    public Func<double> Compile()
        => Compile<double>();

    /// <inheritdoc cref="Compile{T, TResult}(T)" />
    public Func<T, double> Compile<T>(T parameters)
        => Compile<T, double>(parameters);

    internal Expression Build<TResult>(ref int i, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
        where TResult : struct
    {
        var span = MathString.AsSpan();
        Expression expression = Expression.Constant(default(TResult));

        var start = i;
        while (span.Length > i)
        {
            if ((separator.HasValue && IsParamSeparator(separator.Value, start, i)) ||
                (closingSymbol.HasValue && span[i] == closingSymbol.Value))
            {
                if (expression.IsDefault())
                    MathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return expression;
            }

            if (span[i] is >= '0' and <= '9' || span[i] == _decimalSeparator || //the real part of a number.
                (typeof(TResult) == typeof(Complex) &&
                 span[i] is 'i' && (span.Length == i + 1 || !char.IsLetterOrDigit(span[i + 1])))) //the imaginary part of a complex number.
            {
                if (isOperand)
                    return Build<TResult>(ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                var tokenPosition = i;
                var value = span.ParseNumber<TResult>(_numberFormat, ref i);
                expression = Expression.Constant(value);

                if (value is Complex c && c.Imaginary != default)
                    OnEvaluating(tokenPosition, i, expression);
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

                    right = BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
                    expression = BuildMultiplyIfLeftNotDefault<TResult>(expression, right);

                    if (expression != right)
                        OnEvaluating(start, i, expression);
                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || (precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsMeaningless(start, i)))
                        return expression;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = Build<TResult>(ref i, separator, closingSymbol, p, isOperand);
                    expression = MathCompatibleOperator.Build<TResult>(OperatorType.Add, expression, right);

                    OnEvaluating(start, i, expression);
                    if (isOperand)
                        return expression;

                    break;
                case '-' when span.Length == i + 1 || span[i + 1] != '-':
                    var isMeaningless = MathString.IsMeaningless(start, i);
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !isMeaningless)
                        return expression;

                    i++;
                    var numberPosition = i;

                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = Build<TResult>(ref i, separator, closingSymbol, p, isOperand);

                    //it keeps sign of the part of the complex number, correct sign is important in complex analysis.
                    if (right is ConstantExpression { Value: Complex value })
                    {
                        if (isMeaningless && span[numberPosition..i].IsComplexNumberPart(_numberFormat, out var isImaginaryPart))
                            expression = Expression.Constant(isImaginaryPart ? Complex.Conjugate(value) : new Complex(-value.Real, value.Imaginary));
                        else
                            expression = MathCompatibleOperator.Build<TResult>(OperatorType.Subtract, expression, right);
                    }
                    else
                    {
                        var operatorType = isMeaningless ? OperatorType.Negate : OperatorType.Subtract;
                        expression = MathCompatibleOperator.Build<TResult>(operatorType, expression, right);
                    }

                    OnEvaluating(start, i, expression);
                    if (isOperand)
                        return expression;

                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build<TResult>(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = MathCompatibleOperator.Build<TResult>(OperatorType.Multiply, expression, right);

                    OnEvaluating(start, i, expression);
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build<TResult>(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = MathCompatibleOperator.Build<TResult>(OperatorType.Divide, expression, right);

                    OnEvaluating(start, i, expression);
                    break;
                case ' ' or '\t' or '\n' or '\r': //whitespace, tab, LF, or CR
                    i++;
                    break;
                default:
                    var entity = FirstMathEntity(span[i..]);
                    if (entity == null)
                    {
                        if (span.TryParseCurrency(_numberFormat, ref i))
                            break;

                        throw CreateExceptionInvalidToken(span, i);
                    }

                    //highest precedence is evaluating first
                    if (precedence >= entity.Precedence)
                        return expression;

                    expression = entity.Build<TResult>(this, start, ref i, separator, closingSymbol, expression);

                    if (isOperand)
                        return expression;

                    break;
            }
        }

        if (expression.IsDefault())
            MathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return expression.Reduce();
    }

    internal static Expression BuildMultiplyIfLeftNotDefault<TResult>(Expression left, Expression right)
    {
        if (left.IsDefault())
            return right;

        if (left is ConstantExpression)
            return MathCompatibleOperator.Build<TResult>(OperatorType.Multiply, left, right);

        var equalToDefaultExpr = Expression.Equal(left, Expression.Default(left.Type)).Reduce();
        return Expression.Condition(equalToDefaultExpr, right, Expression.Multiply(left, right).Reduce());
    }

    internal Expression BuildOperand<TResult>(ref int i, char? separator, char? closingSymbol)
        where TResult : struct
    {
        var start = i;
        var expression = Build<TResult>(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (expression.IsDefault())
            MathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return expression;
    }

    internal Expression BuildExponentiation<TResult>(int start, ref int i, char? separator, char? closingSymbol, Expression left)
        where TResult : struct
    {
        MathString.SkipMeaningless(ref i);
        if (MathString.Length <= i)
            return left;

        var entity = FirstMathEntity(MathString.AsSpan(i));
        return entity is { Precedence: >= (int)EvalPrecedence.Exponentiation }
            ? entity.Build<TResult>(this, start, ref i, separator, closingSymbol, left)
            : left;
    }

    /// <summary>Compiles the <see cref="MathString">math expression string</see>.</summary>
    /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="MathExpressionException" />
    private Func<TResult> Compile<TResult>()
        where TResult : struct
    {
        _evaluatingStep = 0;

        try
        {
            var i = 0;
            ExpressionTree = Build<TResult>(ref i, null, null);

            if (_evaluatingStep == 0)
                OnEvaluating(0, i, ExpressionTree);

            return Expression.Lambda<Func<TResult>>(ExpressionTree).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, null);
        }
    }

    /// <inheritdoc cref="Compile{T}()" />
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException">parameters</exception>
    private Func<T, TResult> Compile<T, TResult>(T parameters)
        where TResult : struct
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        const string parameterName = "p";
        _parameters = new MathParameters(parameters);
        ParameterExpression = Expression.Parameter(typeof(T), parameterName);
        _evaluatingStep = 0;

        try
        {
            var i = 0;
            ExpressionTree = Build<TResult>(ref i, null, null);

            if (_evaluatingStep == 0)
                OnEvaluating(0, i, ExpressionTree);

            return Expression.Lambda<Func<T, TResult>>(ExpressionTree, ParameterExpression).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }
}