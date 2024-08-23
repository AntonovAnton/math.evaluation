using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

using MathEvaluation.Context;

namespace MathEvaluation;

public class MathExpression
{
    private static readonly Expression DoubleZero = Expression.Constant(0.0);

    private List<MathVariable<double>> _variables = new();
    private MathContextTrie _variablesTrie = new();

    private ParameterExpression _parameter = Expression.Parameter(typeof(double[]), "args");
    private Func<double[], double>? _func;

    /// <summary>Gets the math expression string.</summary>
    /// <value>The math expression string.</value>
    public string MathString { get; }

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext" /> interface.</value>
    public IMathContext? Context { get; }

    /// <summary>Gets the specified format provider.</summary>
    /// <value>The specified format provider.</value>
    public IFormatProvider Provider { get; }

    /// <summary>Gets the expression tree.</summary>
    /// <value>The expression tree.</value>
    public Expression? ExpressionTree { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MathExpression"/> class.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="context">The context.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <exception cref="System.ArgumentException">Expression is empty or white space. - expression</exception>
    public MathExpression(string expression, IMathContext? context = null, IFormatProvider? provider = null)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException("Expression is empty or white space.", nameof(expression));

        MathString = expression;
        Context = context;
        Provider = provider ?? CultureInfo.CurrentCulture;

        _func = null;
    }

    /// <summary>Sets the variable.</summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public void SetVariable(double value, string key)
    {
        var index = _variables.FindIndex(v => v.Key == key);
        if (index == -1)
            _variables.Add(new MathVariable<double>(key.ToString(), value));
        else
            _variables[index].SetValue(value);
    }

    /// <summary>Compiles the <see cref="MathString">math expression</see>.</summary>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public void Compile()
    {
        if (_func != null)
            return;

        try
        {
            foreach (var variable in _variables)
                _variablesTrie.AddMathEntity(variable);

            var i = 0;
            ExpressionTree = Build(MathString, ref i, null, null, (int)EvalPrecedence.Unknown, DoubleZero);

            _func = Expression.Lambda<Func<double[], double>>(ExpressionTree, _parameter).Compile();
        }
        catch (Exception ex)
        {
            ex.Data[nameof(MathString)] = MathString;
            ex.Data[nameof(Context)] = Context;
            ex.Data[nameof(Provider)] = Provider;
            throw;
        }
    }

    public double Evaluate()
    {
        if (_func == null)
            throw new InvalidOperationException();

        var args = _variables.Select(v => v.Value).ToArray();

        return _func(args);
    }

    private Expression Build(ReadOnlySpan<char> str, ref int i, char? separator, char? closingSymbol, int precedence, Expression expr)
    {
        var start = i;
        while (str.Length > i)
        {
            if ((closingSymbol.HasValue && str[i] == closingSymbol.Value) ||
                (separator.HasValue && str[i] == separator.Value))
                return expr;

            switch (str[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return expr;

                    i++;
                    var right = Build(str, ref i, null, ')', (int)EvalPrecedence.Unknown, DoubleZero);
                    i++;
                    right = BuildExponentiation(str, ref i, separator, closingSymbol, right);
                    var left = expr is ConstantExpression c && (double)c.Value == 0.0 ? Expression.Constant(1.0) : expr;
                    expr = Expression.Multiply(left, right);
                    break;
                case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                    expr = Expression.Constant(MathEvaluator.GetNumber(str, Provider, ref i, separator, closingSymbol));
                    break;
                case '+':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !str[start..i].IsWhiteSpace())
                        return expr;

                    i++;
                    right = Build(str, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, DoubleZero);
                    expr = Expression.Add(expr, right);
                    break;
                case '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !str[start..i].IsWhiteSpace())
                        return expr;

                    i++;
                    while (str.Length > i && str[i] is ' ' or '\n' or '\r')
                        i++;

                    //two negatives should combine to make a positive
                    if (str.Length > i && str[i] is '-')
                    {
                        i++;
                        right = Build(str, ref i, separator, closingSymbol,
                                precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, DoubleZero);
                        expr = Expression.Add(expr, right);
                    }
                    else
                    {
                        right = Build(str, ref i, separator, closingSymbol,
                                precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, DoubleZero);
                        expr = Expression.Subtract(expr, right);
                    }

                    break;
                case '*' when str.Length == i + 1 || str[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expr;

                    i++;
                    right = Build(str, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, DoubleZero);
                    expr = Expression.Multiply(expr, right);
                    break;
                case '/' when str.Length == i + 1 || str[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expr;

                    i++;
                    right = Build(str, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, DoubleZero);
                    expr = Expression.Divide(expr, right);
                    break;
                case ' ' or '\n' or '\r': //space or LF or CR
                    i++;
                    break;
                default:
                    var entity = Context?.FirstMathEntity(str[i..]);

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return expr;

                    expr = BuildFuncOrVar(str, ref i, separator, closingSymbol, precedence, expr, false, entity);
                    break;
            }
        }

        if (expr is ConstantExpression && (double)((ConstantExpression)expr).Value == 0.0 && str[start..i].IsWhiteSpace())
            return Expression.Constant(double.NaN);

        return expr;
    }

    private Expression BuildOperand(ReadOnlySpan<char> str, ref int i, char? separator, char? closingSymbol)
    {
        while (str.Length > i && str[i] is ' ')
            i++;

        switch (str[i])
        {
            case '(':
                {
                    i++;
                    var expr = Build(str, ref i, null, ')', (int)EvalPrecedence.Unknown, DoubleZero);
                    i++;
                    return expr;
                }
            case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                return Build(str, ref i, separator, closingSymbol, (int)EvalPrecedence.Function, DoubleZero);
            case '-':
                i++;
                return Expression.Negate(BuildOperand(str, ref i, separator, closingSymbol));
            default:
                return BuildFuncOrVar(str, ref i, separator, closingSymbol, (int)EvalPrecedence.Function, DoubleZero, true);
        }
    }

    private Expression BuildFuncOrVar(ReadOnlySpan<char> str, ref int i, char? separator, char? closingSymbol, int precedence, Expression expr, bool isOperand, IMathEntity? entity = null)
    {
        entity ??= Context?.FirstMathEntity(str[i..]);
        if (entity?.Precedence < precedence)
            return expr;

        if (entity != null && TryBuildContext(str, entity, ref i, separator, closingSymbol, ref expr, isOperand))
            return expr;

        entity = _variablesTrie.FirstMathEntity(str[i..]);
        if (entity is MathVariable<double> mathVariable)
        {
            i += entity.Key.Length;
            var index = _variables.IndexOf(mathVariable);
            var right = Expression.ArrayAccess(_parameter, Expression.Constant(index));
            var left = expr == DoubleZero ? Expression.Constant(1.0) : expr;
            return Expression.Multiply(left, right);
        }

        if (MathEvaluator.TryParseCurrencySymbol(str, Provider, ref i))
            return expr;

        var end = str[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? str[i..end] : str[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private Expression BuildExponentiation(ReadOnlySpan<char> str, ref int i, char? separator, char? closingSymbol, Expression expr, IMathEntity? entity = null)
    {
        while (str.Length > i && str[i] is ' ')
            i++;

        entity ??= (str.Length > i ? Context?.FirstMathEntity(str[i..]) : null);
        switch (entity)
        {
            case MathOperandConverter<double> mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    Expression<Func<double, double>> lambda = arg => fn(arg);
                    Expression result;
                    if (mathConverter.IsConvertingLeftOperand)
                        result = Expression.Invoke(lambda, expr);
                    else
                    {
                        var right = Build(str, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, DoubleZero);
                        result = Expression.Invoke(lambda, right);
                    }

                    expr = BuildExponentiation(str, ref i, separator, closingSymbol, result);
                    break;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    Expression<Func<double, double, double>> lambda = (arg1, arg2) => fn(arg1, arg2);
                    var rightOperand = BuildOperand(str, ref i, separator, closingSymbol);
                    rightOperand = BuildExponentiation(str, ref i, separator, closingSymbol, rightOperand);
                    expr = Expression.Invoke(lambda, expr, rightOperand);
                    break;
                }
        }

        return expr;
    }

    private bool TryBuildContext(ReadOnlySpan<char> str, IMathEntity entity,
        ref int i, char? separator, char? closingSymbol, ref Expression expr, bool isOperand)
    {
        switch (entity)
        {
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var right = Expression.Constant(mathVariable.Value);
                    var left = expr == DoubleZero ? Expression.Constant(1.0) : expr;
                    expr = Expression.Multiply(left, right);
                    return true;
                }
            case MathVariableFunction<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var right = Expression.Constant(mathVariable.GetValue());
                    var left = expr == DoubleZero ? Expression.Constant(1.0) : expr;
                    expr = Expression.Multiply(left, right);
                    return true;
                }
            case MathOperandConverter<double> mathConverter:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double>> lambda = arg => mathConverter.Fn(arg);
                    if (mathConverter.IsConvertingLeftOperand)
                        expr = Expression.Invoke(lambda, expr);
                    else
                    {
                        var right = Build(str, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, DoubleZero);
                        expr = Expression.Invoke(lambda, right);
                    }
                    return true;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double, double>> lambda = (arg1, arg2) => mathOperator.Fn(arg1, arg2);
                    var rightOperand = BuildOperand(str, ref i, separator, closingSymbol);
                    rightOperand = BuildExponentiation(str, ref i, separator, closingSymbol, rightOperand);
                    expr = Expression.Invoke(lambda, expr, rightOperand);
                    return true;
                }
            default:
                return false;
        }
    }
}