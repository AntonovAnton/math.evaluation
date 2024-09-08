using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;

namespace MathEvaluation;

public partial class MathExpression
{
    private static readonly Expression DoubleZero = Expression.Constant(0.0);

    private ParameterExpression? _parameterExpression;

    /// <summary>Gets the expression tree.</summary>
    /// <value>The expression tree.</value>
    public Expression? ExpressionTree { get; private set; }

    /// <summary>Compiles the <see cref="MathString">math expression string</see>.</summary>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="MathExpressionException"/>
    public Func<double> Compile()
    {
        try
        {
            var i = 0;
            ExpressionTree = Build(MathString, ref i, null, null);

            return Expression.Lambda<Func<double>>(ExpressionTree).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, null);
        }
    }

    /// <inheritdoc cref="Compile()"/>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException">parameters</exception>
    public Func<T, double> Compile<T>(T parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        _parameters = new MathParameters(parameters);
        _parameterExpression = Expression.Parameter(typeof(T), nameof(parameters));

        try
        {
            var i = 0;
            ExpressionTree = Build(MathString, ref i, null, null);

            return Expression.Lambda<Func<T, double>>(ExpressionTree, _parameterExpression).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }

    private Expression Build(ReadOnlySpan<char> mathString, ref int i,
        char? separator, char? closingSymbol, int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
    {
        Expression expression = DoubleZero;
        var start = i;
        while (mathString.Length > i)
        {
            if (separator.HasValue && mathString.IsParamsSeparator(start, i, separator.Value, _decimalSeparator) ||
                closingSymbol.HasValue && mathString[i] == closingSymbol.Value)
            {
                if (expression == DoubleZero)
                    mathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return expression;
            }

            if (mathString[i] is >= '0' and <= '9' || mathString[i] == _decimalSeparator) //number
            {
                if (isOperand)
                    return Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                expression = Expression.Constant(mathString.ParseNumber(_numberFormat, ref i));
                continue;
            }

            switch (mathString[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return expression;

                    var startParenthesis = i;
                    i++;
                    var right = Build(mathString, ref i, null, ')');
                    mathString.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return right;

                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right).Reduce();
                    break;
                case '+' when mathString.Length == i + 1 || mathString[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return expression;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = Build(mathString, ref i, separator, closingSymbol, p, isOperand);
                    expression = Expression.Add(expression, right).Reduce();
                    if (isOperand)
                        return expression;
                    break;
                case '-' when mathString.Length == i + 1 || mathString[i + 1] != '-':
                    if (precedence >=(int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return expression;

                    var isNegativity = start == i;
                    i++;
                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = Build(mathString, ref i, separator, closingSymbol, p, isOperand);
                    expression = isNegativity ? Expression.Negate(right) : Expression.Subtract(expression, right); //it keeps sign
                    expression = expression.Reduce();
                    if (isOperand)
                        return expression;
                    break;
                case '*' when mathString.Length == i + 1 || mathString[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = Expression.Multiply(expression, right).Reduce();
                    break;
                case '/' when mathString.Length == i + 1 || mathString[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = Expression.Divide(expression, right).Reduce();
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = FirstMathEntity(mathString[i..]);
                    if (entity == null && mathString.TryParseCurrency(_numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return expression;

                    expression = BuildMathEntity(mathString, ref i, separator, closingSymbol, precedence, expression, entity);
                    if (isOperand)
                        return expression;
                    break;
            }
        }

        if (expression == DoubleZero)
            mathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return expression.Reduce();
    }

    private Expression BuildOperand(ReadOnlySpan<char> mathString, ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var expression = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (expression == DoubleZero)
            mathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return expression;
    }

    private Expression BuildMathEntity(ReadOnlySpan<char> mathString, ref int i,
        char? separator, char? closingSymbol, int precedence, Expression expression, IMathEntity? entity, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryBuildEntity(mathString, entity, ref i, separator, closingSymbol, ref expression))
                return expression;

            var decimalExpression = expression == DoubleZero ? DecimalZero : Expression.Convert(expression, typeof(decimal)).Reduce();
            if (TryBuildEntityDecimal(mathString, entity, ref i, separator, closingSymbol, ref decimalExpression))
                return Expression.Convert(decimalExpression, typeof(double)).Reduce();
        }

        if (throwError)
            mathString.ThrowExceptionInvalidToken(i);

        return expression;
    }

    private Expression BuildExponentiation(ReadOnlySpan<char> mathString, ref int i,
        char? separator, char? closingSymbol, Expression expression)
    {
        mathString.SkipMeaningless(ref i);
        if (mathString.Length <= i)
            return expression;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = FirstMathEntity(mathString[i..]);
        return BuildMathEntity(mathString, ref i, separator, closingSymbol, precedence, expression, entity, false);
    }

    private bool TryBuildEntity(ReadOnlySpan<char> mathString, IMathEntity entity, ref int i,
        char? separator, char? closingSymbol, ref Expression expression)
    {
        var start = i;
        switch (entity)
        {
            case MathConstant<double> constant:
                {
                    i += entity.Key.Length;
                    var right = constant.BuildExpression();
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right).Reduce();
                    return true;
                }
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var right = mathVariable.BuildExpression(_parameterExpression!);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right).Reduce();
                    return true;
                }
            case MathOperandOperator<double> op:
                {
                    i += entity.Key.Length;
                    if (op.IsProcessingLeft)
                        expression = op.BuildExpression(expression);
                    else
                    {
                        var right = Build(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                        expression = op.BuildExpression(right);
                    }
                    expression = BuildExponentiation(mathString, ref i, separator, closingSymbol, expression);
                    return true;
                }
            case MathOperandsOperator<double> op:
                {
                    i += entity.Key.Length;
                    var right = BuildOperand(mathString, ref i, separator, closingSymbol);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = op.BuildExpression(expression, right);
                    return true;
                }
            case MathOperator<double> op:
                {
                    i += entity.Key.Length;
                    var right = Build(mathString, ref i, separator, closingSymbol, op.Precedence);
                    expression = op.BuildExpression(expression, right);
                    return true;
                }
            case MathGetValueFunction<double> func:
                {
                    i += entity.Key.Length;
                    mathString.SkipParenthesis(ref i);

                    var right = func.BuildExpression();
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right).Reduce();
                    return true;
                }
            case MathUnaryFunction<double> func:
                {
                    i += entity.Key.Length;
                    if (func.OpeningSymbol.HasValue)
                        mathString.ThrowExceptionIfNotOpened(func.OpeningSymbol.Value, start, ref i);

                    var arg = func.ClosingSymbol.HasValue
                        ? Build(mathString, ref i, null, func.ClosingSymbol)
                        : BuildOperand(mathString, ref i, separator, closingSymbol);

                    if (func.ClosingSymbol.HasValue)
                        mathString.ThrowExceptionIfNotClosed(func.ClosingSymbol.Value, start, ref i);

                    var right = func.BuildExpression(arg);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right).Reduce();
                    return true;
                }
            case MathFunction<double> func:
                {
                    i += entity.Key.Length;
                    mathString.ThrowExceptionIfNotOpened(func.OpeningSymbol, start, ref i);

                    var args = new List<Expression>();
                    while (mathString.Length > i)
                    {
                        var arg = Build(mathString, ref i, func.Separator, func.ClosingSymbol);
                        args.Add(arg);

                        if (mathString[i] == func.Separator)
                        {
                            i++; //other param
                            continue;
                        }
                        break;
                    }

                    mathString.ThrowExceptionIfNotClosed(func.ClosingSymbol, start, ref i);

                    var right = func.BuildExpression(args);
                    right = BuildExponentiation(mathString, ref i, separator, closingSymbol, right);
                    expression = expression == DoubleZero ? right : Expression.Multiply(expression, right).Reduce();
                    return true;
                }
            default:
                return false;
        }
    }
}