using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

using LinqExpression = System.Linq.Expressions.Expression;

using MathEvaluation.Context;

namespace MathEvaluation;

public class MathExpression
{
    private Func<double> _func;

    /// <summary>Gets the math expression.</summary>
    /// <value>The math expression.</value>
    public string Expression { get; }

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext" /> interface.</value>
    public IMathContext? Context { get; }

    /// <summary>
    /// Gets the specified format provider.
    /// </summary>
    /// <value>
    /// The specified format provider.
    /// </value>
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

    /// <summary>Compiles the <see cref="Expression">math expression</see>.</summary>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public void Compile()
    {
        try
        {

            var i = 0;
            var expression = Compile(Expression, Context, Provider, ref i, null, null, (int)EvalPrecedence.Unknown, LinqExpression.Constant(0.0));

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

    private static LinqExpression Compile(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
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
                    var right = Compile(expression, context, provider, ref i, null, ')', (int)EvalPrecedence.Unknown, LinqExpression.Constant(0.0));
                    var left = value is ConstantExpression c && (double)c.Value == 0.0 ? LinqExpression.Constant(1.0) : value;
                    value = LinqExpression.Multiply(left, right);
                    i++;
                    break;
                case > '0' and <= '9' or '.' or '0' or ',' or '٫':
                    value = LinqExpression.Constant(GetNumber(expression, provider, ref i, separator, closingSymbol));
                    break;
                case '+':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && start != i && !expression[start..i].IsWhiteSpace())
                        return value;

                    i++;
                    right = Compile(expression, context, provider, ref i, separator, closingSymbol,
                            precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, LinqExpression.Constant(0.0));
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
                        right = Compile(expression, context, provider, ref i, separator, closingSymbol,
                                precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, LinqExpression.Constant(0.0));
                        value = LinqExpression.Add(value, right);
                    }
                    else
                    {
                        right = Compile(expression, context, provider, ref i, separator, closingSymbol,
                                precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic, LinqExpression.Constant(0.0));
                        value = LinqExpression.Subtract(value, right);
                    }

                    break;
                case '*' when expression.Length == i + 1 || expression[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    right = Compile(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, LinqExpression.Constant(0.0));
                    value = LinqExpression.Multiply(value, right);
                    break;
                case '/' when expression.Length == i + 1 || expression[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    right = Compile(expression, context, provider, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, LinqExpression.Constant(0.0));
                    value = LinqExpression.Divide(value, right);
                    break;
                case ' ' or '\n' or '\r': //space or LF or CR
                    i++;
                    break;
                default:
                    //var entity = context?.FirstMathEntity(expression[i..]);

                    ////highest precedence is evaluating first
                    //if (precedence >= entity?.Precedence)
                    //    return value;

                    //value = CompileFuncOrVar(expression, context, provider, ref i, separator, closingSymbol, precedence, value, false, entity);
                    break;
            }
        }

        if (value is ConstantExpression && (double)((ConstantExpression)value).Value == 0.0 && expression[start..i].IsWhiteSpace())
            return LinqExpression.Constant(double.NaN);

        return value;
    }

    private static double CompileFuncOrVar(ReadOnlySpan<char> expression, IMathContext? context, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, int precedence, double value, bool isOperand, IMathEntity? entity = null)
    {
        entity ??= context?.FirstMathEntity(expression[i..]);
        if (entity?.Precedence < precedence)
            return value;

        if (entity != null && TryCompileContext(expression, context!, entity, provider, ref i, separator, closingSymbol, ref value, isOperand))
            return value;

        if (TryCompileCurrencySymbol(expression, provider, ref i))
            return value;

        var end = expression[i..].IndexOfAny("([ |") + i;
        var unknownSubstring = end > i ? expression[i..end] : expression[i..];

        throw new NotSupportedException($"'{unknownSubstring.ToString()}' isn't supported.");
    }

    private static bool TryCompileContext(ReadOnlySpan<char> expression, IMathContext context, IMathEntity entity, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol, ref double value, bool isOperand)
    {
        switch (entity)
        {
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = mathVariable.Value;
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            case MathVariableFunction<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = mathVariable.GetValue();
                    value = (value == 0 ? 1 : value) * result;
                    return true;
                }
            default:
                return false;
        }
    }

    private static bool TryCompileCurrencySymbol(ReadOnlySpan<char> expression, IFormatProvider provider,
        ref int i)
    {
        var currencySymbol = NumberFormatInfo.GetInstance(provider).CurrencySymbol;
        if (!expression[i..].StartsWith(currencySymbol))
            return false;

        i += currencySymbol.Length;
        return true;
    }

    private static double GetNumber(ReadOnlySpan<char> expression, IFormatProvider provider,
        ref int i, char? separator, char? closingSymbol)
    {
        var str = GetNumberString(expression, ref i, separator, closingSymbol);
        return double.Parse(str, NumberStyles.Number | NumberStyles.AllowExponent, provider);
    }

    private static ReadOnlySpan<char> GetNumberString(ReadOnlySpan<char> expression, ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        i++;
        while (expression.Length > i)
            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '\u202f' or '\u00a0' or '٫' or '’' or '٬' or '⹁' &&
                (!separator.HasValue || expression[i] != separator.Value) &&
                (!closingSymbol.HasValue || expression[i] != closingSymbol.Value))
            {
                i++;
            }
            else
            {
                //an exponential notation number
                if (expression[i] is 'e' or 'E')
                {
                    i++;
                    if (expression.Length > i && expression[i] is '-' or '+')
                        i++;
                }
                else
                {
                    break;
                }
            }

        //if the last symbol is 'e' it's the natural logarithmic base constant
        if (expression[i - 1] is 'e')
            i--;

        return expression[start..i];
    }
}