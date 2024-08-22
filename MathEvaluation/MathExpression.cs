using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

using LinqExpression = System.Linq.Expressions.Expression;

using MathEvaluation.Context;

namespace MathEvaluation;

public class MathExpression
{
    private static readonly LinqExpression DoubleZero = LinqExpression.Constant(0.0);

    private MathContextTrie _variables = new();
    private Func<double> _func;

    /// <summary>Gets the math expression.</summary>
    /// <value>The math expression.</value>
    public string Expression { get; }

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext" /> interface.</value>
    public IMathContext? Context { get; }

    /// <summary>Gets the specified format provider.</summary>
    /// <value>The specified format provider.</value>
    public IFormatProvider Provider { get; }

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

        Expression = expression;
        Context = context;
        Provider = provider ?? CultureInfo.CurrentCulture;

        _func = () => double.NaN;
    }

    /// <summary>Sets the variable.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void SetVariable<T>(string key, T value)
        where T : struct
    {
        _variables.AddMathEntity(new MathVariable<T>(key, value));
    }

    /// <summary>Compiles the <see cref="Expression">math expression</see>.</summary>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public void Compile()
    {
        try
        {

            var i = 0;
            var expression = Build(Expression, Context, Provider, ref i, null, null, (int)EvalPrecedence.Unknown, DoubleZero);

            _func = LinqExpression.Lambda<Func<double>>(expression).Compile();
        }
        catch (Exception ex)
        {
            ex.Data[nameof(Expression)] = Expression;
            ex.Data[nameof(Context)] = Context;
            ex.Data[nameof(Provider)] = Provider;
            throw;
        }
    }

    public double Evaluate()
    {
        return _func();
    }

    private static LinqExpression Build(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, LinqExpression value)
    {
        var start = i;
        while (expression.Length > i)
        {
            if ((closingSymbol.HasValue && expression[i] == closingSymbol.Value) ||
                (separator.HasValue && expression[i] == separator.Value))
                return value;

            switch (expression[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    i++;
                    var right = Build(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown, DoubleZero);
                    i++;
                    right = BuildExponentiation(expression, context, provider, ref i, separator, closingSymbol, right);
                    var left = value is ConstantExpression c && (double)c.Value == 0.0 ? LinqExpression.Constant(1.0) : value;
                    value = LinqExpression.Multiply(left, right);
                    break;
                case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                    value = LinqExpression.Constant(MathEvaluator.GetNumber(expression, provider, ref i, separator, closingSymbol));
                    break;
                case '+':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !expression[start..i].IsWhiteSpace())
                        return value;

                    i++;
                    right = Build(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, DoubleZero);
                    value = LinqExpression.Add(value, right);
                    break;
                case '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !expression[start..i].IsWhiteSpace())
                        return value;

                    i++;
                    while (expression.Length > i && expression[i] is ' ' or '\n' or '\r')
                        i++;

                    //two negatives should combine to make a positive
                    if (expression.Length > i && expression[i] is '-')
                    {
                        i++;
                        right = Build(expression, context, provider, ref i, separator, closingSymbol,
                                precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, DoubleZero);
                        value = LinqExpression.Add(value, right);
                    }
                    else
                    {
                        right = Build(expression, context, provider, ref i, separator, closingSymbol,
                                precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, DoubleZero);
                        value = LinqExpression.Subtract(value, right);
                    }

                    break;
                case '*' when expression.Length == i + 1 || expression[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    right = Build(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, DoubleZero);
                    value = LinqExpression.Multiply(value, right);
                    break;
                case '/' when expression.Length == i + 1 || expression[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    right = Build(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, DoubleZero);
                    value = LinqExpression.Divide(value, right);
                    break;
                case ' ' or '\n' or '\r': //space or LF or CR
                    i++;
                    break;
                default:
                    var entity = context?.FirstMathEntity(expression[i..]);

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = BuildFuncOrVar(expression, context, provider, ref i, separator, closingSymbol, precedence, value, false, entity);
                    break;
            }
        }

        if (value is ConstantExpression && (double)((ConstantExpression)value).Value == 0.0 && expression[start..i].IsWhiteSpace())
            return LinqExpression.Constant(double.NaN);

        return value;
    }

    private static LinqExpression BuildOperand(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        switch (expression[i])
        {
            case '(':
                {
                    i++;
                    var value = Build(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown, DoubleZero);
                    i++;
                    return value;
                }
            case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                return Build(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Function, DoubleZero);
            case '-':
                i++;
                return LinqExpression.Negate(BuildOperand(expression, context, provider, ref i, separator, closingSymbol));
            default:
                return BuildFuncOrVar(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Function, DoubleZero, true);
        }
    }

    private static LinqExpression BuildFuncOrVar(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, LinqExpression value, bool isOperand, IMathEntity? entity = null)
    {
        entity ??= context?.FirstMathEntity(expression[i..]);
        if (entity?.Precedence < precedence)
            return value;

        if (entity != null && TryBuildContext(expression, context!, entity, provider, ref i, separator, closingSymbol, ref value, isOperand))
            return value;

        if (MathEvaluator.TryParseCurrencySymbol(expression, provider, ref i))
            return value;

        var end = expression[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static LinqExpression BuildExponentiation(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, LinqExpression value, IMathEntity? entity = null)
    {
        while (expression.Length > i && expression[i] is ' ')
            i++;

        entity ??= (expression.Length > i ? context?.FirstMathEntity(expression[i..]) : null);
        switch (entity)
        {
            case MathOperandConverter<double> mathConverter:
                {
                    i += entity.Key.Length;
                    var fn = mathConverter.Fn;
                    Expression<Func<double, double>> lambda = arg => fn(arg);
                    LinqExpression result;
                    if (mathConverter.IsConvertingLeftOperand)
                        result = LinqExpression.Invoke(lambda, value);
                    else
                    {
                        var right = Build(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, DoubleZero);
                        result = LinqExpression.Invoke(lambda, right);
                    }

                    value = BuildExponentiation(expression, context, provider, ref i, separator, closingSymbol, result);
                    break;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    Expression<Func<double, double, double>> lambda = (arg1, arg2) => fn(arg1, arg2);
                    var rightOperand = BuildOperand(expression, context, provider, ref i, separator, closingSymbol);
                    rightOperand = BuildExponentiation(expression, context, provider, ref i, separator, closingSymbol, rightOperand);
                    value = LinqExpression.Invoke(lambda, value, rightOperand);
                    break;
                }
        }

        return value;
    }

    private static bool TryBuildContext(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, ref LinqExpression value, bool isOperand)
    {
        switch (entity)
        {
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var right = LinqExpression.Constant(mathVariable.Value);
                    var left = value is ConstantExpression c && (double)c.Value == 0.0 ? LinqExpression.Constant(1.0) : value;
                    value = LinqExpression.Multiply(left, right);
                    return true;
                }
            case MathVariableFunction<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var right = LinqExpression.Constant(mathVariable.GetValue());
                    var left = value is ConstantExpression c && (double)c.Value == 0.0 ? LinqExpression.Constant(1.0) : value;
                    value = LinqExpression.Multiply(left, right);
                    return true;
                }
            case MathOperandConverter<double> mathConverter:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double>> lambda = arg => mathConverter.Fn(arg);
                    if (mathConverter.IsConvertingLeftOperand)
                        value = LinqExpression.Invoke(lambda, value);
                    else
                    {
                        var right = Build(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, DoubleZero);
                        value = LinqExpression.Invoke(lambda, right);
                    }
                    return true;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    Expression<Func<double, double, double>> lambda = (arg1, arg2) => mathOperator.Fn(arg1, arg2);
                    var rightOperand = BuildOperand(expression, context, provider, ref i, separator, closingSymbol);
                    rightOperand = BuildExponentiation(expression, context, provider, ref i, separator, closingSymbol, rightOperand);
                    value = LinqExpression.Invoke(lambda, value, rightOperand);
                    return true;
                }
            default:
                return false;
        }
    }
}