using MathEvaluation.Context;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System;
using System.Globalization;

namespace MathEvaluation;

/// <summary>
/// Evaluates and compiles mathematical expressions from strings.
/// </summary>
public partial class MathExpression : IDisposable
{
    private readonly NumberFormatInfo? _numberFormat;
    private readonly char _decimalSeparator;

    private IMathParameters? _parameters;
    private int _evaluatingStep = 0;

    /// <summary>Gets the math expression string.</summary>
    /// <value>The math expression string.</value>
    public string MathString { get; }

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext"/> interface.</value>
    public IMathContext? Context { get; }

    /// <summary>Gets the specified format provider.</summary>
    /// <value>The specified format provider.</value>
    public IFormatProvider? Provider { get; }

    /// <summary>Initializes a new instance of the <see cref="MathExpression" /> class.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="context">The math context.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <exception cref="System.ArgumentNullException">mathString</exception>
    /// <exception cref="System.ArgumentException">Expression string is empty or white space. - mathString</exception>
    public MathExpression(string mathString, IMathContext? context = null, IFormatProvider? provider = null)
    {
        if (mathString == null)
            throw new ArgumentNullException(nameof(mathString));

        if (string.IsNullOrWhiteSpace(mathString))
            throw new ArgumentException("Expression string is empty or white space.", nameof(mathString));

        MathString = mathString;
        Context = context;
        Provider = provider;

        _numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;
        _decimalSeparator = _numberFormat?.NumberDecimalSeparator?.Length > 0 ? _numberFormat.NumberDecimalSeparator[0] : '.';
    }

    /// <inheritdoc cref="Evaluate(IMathParameters?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public double Evaluate(object? parameters = null)
        => Evaluate(parameters != null ? new MathParameters(parameters) : null);

    /// <summary>Evaluates the <see cref="MathString">math expression string</see>.</summary>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="MathExpressionException"/>
    public double Evaluate(IMathParameters? parameters)
    {
        _parameters = parameters;
        _evaluatingStep = 0;

        try
        {
            var i = 0;
            var value = Evaluate(ref i, null, null);

            if (_evaluatingStep == 0)
                OnEvaluating(0, i, value);

            return value;
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }

    /// <summary>
    /// Occurs when the <see cref="MathString">math string</see> is evaluating and triggers at each step of the evaluation.
    /// </summary>
    public event EventHandler<EvaluatingEventArgs>? Evaluating;

    internal double Evaluate(ref int i, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
    {
        var span = MathString.AsSpan();
        var value = default(double);

        var start = i;
        while (span.Length > i)
        {
            if (separator.HasValue && IsParamSeparator(separator.Value, start, i) ||
                closingSymbol.HasValue && span[i] == closingSymbol.Value)
            {
                if (value == default)
                    MathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return value;
            }

            if (span[i] is >= '0' and <= '9' || span[i] == _decimalSeparator) //number
            {
                if (isOperand)
                    return Evaluate(ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = span.ParseNumber(_numberFormat, ref i);
                continue;
            }

            switch (span[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var tokenPosition = i;
                    i++;
                    var result = Evaluate(ref i, null, ')');
                    MathString.ThrowExceptionIfNotClosed(')', tokenPosition, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;

                    if (value != result && !double.IsNaN(value))
                        OnEvaluating(start, i, value);
                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsMeaningless(start, i))
                        return value;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    value += Evaluate(ref i, separator, closingSymbol, p, isOperand);

                    OnEvaluating(start, i, value);
                    if (isOperand)
                        return value;
                    break;
                case '-' when span.Length == i + 1 || span[i + 1] != '-':
                    var isMeaningless = MathString.IsMeaningless(start, i);
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !isMeaningless)
                        return value;

                    i++;
                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    result = Evaluate(ref i, separator, closingSymbol, p, isOperand);
                    value = isMeaningless ? -result : value - result; //it keeps sign

                    OnEvaluating(start, i, value);
                    if (isOperand)
                        return value;
                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= Evaluate(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);

                    OnEvaluating(start, i, value);
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= Evaluate(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);

                    OnEvaluating(start, i, value);
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
                        return value;

                    if (entity != null)
                        value = entity.Evaluate(this, start, ref i, separator, closingSymbol, value);
                    else
                        MathString.ThrowExceptionInvalidToken(i);

                    if (isOperand)
                        return value;
                    break;
            }
        }

        if (value == default)
            MathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return value;
    }

    /// <summary> Converts to string. </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
        => $"{nameof(MathString)}: \"{MathString}\", {nameof(Context)}: {Context?.ToString() ?? "null"}, {nameof(Provider)}: {Provider?.ToString() ?? "null"}";

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Evaluating = null;
        _parameters = null;
        ParameterExpression = null;
        ExpressionTree = null;
    }

    internal double EvaluateOperand(ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = Evaluate(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (value == default)
            MathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return value;
    }

    internal double EvaluateExponentiation(int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        MathString.SkipMeaningless(ref i);
        if (MathString.Length <= i)
            return value;

        var entity = FirstMathEntity(MathString.AsSpan(i));
        if (entity != null && entity.Precedence >= (int)EvalPrecedence.Exponentiation)
            return entity.Evaluate(this, start, ref i, separator, closingSymbol, value);

        return value;
    }

    internal void OnEvaluating<T>(int start, int i, T value)
    {
        if (Evaluating != null)
        {
            _evaluatingStep++;
            Evaluating.Invoke(this, new EvaluatingEventArgs(MathString, start, i - 1, _evaluatingStep, value!));
        }
    }

    private static MathExpressionException CreateException(Exception ex,
        string mathString, IMathContext? context, IFormatProvider? provider, object? parameters)
    {
        ex = ex is not MathExpressionException ? new MathExpressionException(ex.Message, ex) : ex;
        ex.Data[nameof(mathString)] = mathString;
        ex.Data[nameof(context)] = context;
        ex.Data[nameof(provider)] = provider;
        ex.Data[nameof(parameters)] = parameters;
        throw ex;
    }

    private IMathEntity? FirstMathEntity(ReadOnlySpan<char> mathString)
        => Context?.FirstMathEntity(mathString) ?? _parameters?.FirstMathEntity(mathString);

    private bool IsParamSeparator(char separator, int start, int i)
        => MathString[i] == separator && (_decimalSeparator != separator || !MathString.IsMeaningless(start, i));
}