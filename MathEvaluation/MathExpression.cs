using MathEvaluation.Compilation;
using MathEvaluation.Context;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MathEvaluation;

/// <summary>
///     Evaluates and compiles mathematical expressions from strings.
/// </summary>
public partial class MathExpression : IDisposable
{
    /// <summary>
    ///     Probes the call stack on every Nth level of the parsing recursion.
    ///     Must be a power of two greater than one: the check is '(depth &amp; (N - 1)) == 1', which never matches
    ///     when N is 1, because 'depth &amp; 0' is always 0, so the guard would be silently disabled.
    ///     It matches at the depth of 1, 1 + N, 1 + 2N, and so on, so the very first level is probed immediately.
    ///     Probing the first level matters only when the caller is already close to exhausting the call stack,
    ///     for example when it evaluates a math expression string from inside its own deep recursion. Otherwise, the
    ///     reserve that the runtime keeps below the stack limit, measured as 110-130KB on 64-bit, is much bigger
    ///     than what N levels consume, so the first probe could be deferred without a risk.
    ///     Do not raise N without re-measuring: the most expensive level, the one of a Complex or decimal expression
    ///     variable, costs about 2.1KB in a release build and about 3KB in a debug one, so N = 16 keeps at least
    ///     a 2.7x margin against the reserve, while N = 32 would drop it to 1.4x.
    /// </summary>
    private const int NestingDepthProbeInterval = 16;

    private readonly NumberFormatInfo _numberFormat;
    private readonly char _decimalSeparator;

    private int _evaluatingStep;

    /// <summary>Gets the math expression string.</summary>
    /// <value>The math expression string.</value>
    public string MathString { get; }

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="MathContext" /> class.</value>
    public MathContext? Context { get; }

    /// <summary>Gets the specified format provider.</summary>
    /// <value>The specified format provider.</value>
    public IFormatProvider? Provider { get; }

    /// <summary> Gets the expression compiler used to compile and evaluate expressions.</summary>
    /// <value>The expression compiler.</value>
    // ReSharper disable once MemberCanBePrivate.Global
    public IExpressionCompiler? Compiler { get; }

    internal MathParameters? Parameters { get; private set; }

    /// <summary>Initializes a new instance of the <see cref="MathExpression" /> class.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="context">The math context.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <param name="compiler">The specified expression compiler. If null, the <see cref="LambdaExpression.Compile()" /> method will be used.</param>
    /// <exception cref="System.ArgumentNullException">mathString</exception>
    /// <exception cref="System.ArgumentException">Expression string is empty or white space. - mathString</exception>
    public MathExpression(string mathString, MathContext? context = null, IFormatProvider? provider = null,
        IExpressionCompiler? compiler = null)
    {
        ArgumentNullException.ThrowIfNull(mathString);

        if (string.IsNullOrWhiteSpace(mathString))
            throw new ArgumentException("Expression string is empty or white space.", nameof(mathString));

        MathString = mathString;
        Context = context;
        Compiler = compiler;

        // Parsers use CurrentCulture when provider is null, so do not set number info for current culture to avoid unnecessary overhead.
        _numberFormat = NumberFormatInfo.GetInstance(provider);
        Provider = _numberFormat;

        _decimalSeparator = _numberFormat.NumberDecimalSeparator.Length > 0 ? _numberFormat.NumberDecimalSeparator[0] : '.';
    }

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    /// <exception cref="NotSupportedException">parameters</exception>
    public double Evaluate(object? parameters = null)
        => Evaluate(parameters != null ? new MathParameters(parameters) : null);

    /// <inheritdoc cref="Evaluate{TResult}(MathParameters?)" />
    public double Evaluate(MathParameters? parameters)
        => Evaluate<double>(parameters);

    /// <inheritdoc cref="Evaluate{TResult}(MathParameters?)" />
    /// <exception cref="NotSupportedException">parameters</exception>
    public TResult Evaluate<TResult>(object? parameters = null)
        where TResult : struct, INumberBase<TResult>
        => Evaluate<TResult>(parameters != null ? new MathParameters(parameters) : null);

    /// <summary>Evaluates the <see cref="MathString">math expression string</see>.</summary>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <returns>Value of the math expression.</returns>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <exception cref="MathExpressionException" />
    public TResult Evaluate<TResult>(MathParameters? parameters)
        where TResult : struct, INumberBase<TResult>
    {
        try
        {
            return Evaluate<TResult>(parameters, 0);
        }
        catch (Exception ex)
        {
            throw CreateException(ex, parameters);
        }
    }

    /// <inheritdoc cref="Evaluate{TResult}(MathParameters?)" />
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <param name="depth">
    ///     The recursion depth that the parsing starts from. It isn't zero when the math expression string
    ///     is a variable that is evaluated as a part of another math expression string.
    /// </param>
    internal TResult Evaluate<TResult>(MathParameters? parameters, int depth)
        where TResult : struct, INumberBase<TResult>
    {
        Parameters = parameters;
        _evaluatingStep = 0;

        var i = 0;
        var value = Evaluate<TResult>(ref i, depth, null, null);

        if (_evaluatingStep == 0)
            OnEvaluating(0, i, value);

        return value;
    }

    /// <summary>
    ///     Occurs when the <see cref="MathString">math string</see> is evaluating and triggers at each step of the evaluation.
    /// </summary>
    public event EventHandler<EvaluatingEventArgs>? Evaluating;

    /// <summary> Converts to string.</summary>
    /// <returns>
    ///     A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
        => $"{nameof(MathString)}: \"{MathString}\", {nameof(Context)}: {Context?.ToString() ?? "null"}, {nameof(Provider)}: {Provider?.ToString() ?? "null"}";

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
#pragma warning disable CA1816
    void IDisposable.Dispose()
#pragma warning restore CA1816
    {
        Evaluating = null;
        Parameters = null;
        ParameterExpression = null;
        ExpressionTree = null;
    }

    internal TResult Evaluate<TResult>(ref int i, int depth, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
        where TResult : struct, INumberBase<TResult>
    {
        if ((++depth & (NestingDepthProbeInterval - 1)) == 1)
            EnsureNestingDepth(depth, i);

        var span = MathString.AsSpan();
        var value = default(TResult);

        var start = i;
        while (span.Length > i)
        {
            if ((separator.HasValue && IsParamSeparator(separator.Value, start, i)) ||
                (closingSymbol.HasValue && span[i] == closingSymbol.Value))
            {
                if (value == default)
                    MathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return value;
            }

            if (char.IsAsciiDigit(span[i]) || span[i] == _decimalSeparator || //the real part of a number.
                (typeof(TResult) == typeof(Complex) &&
                 span[i] is 'i' && (span.Length == i + 1 || !char.IsLetterOrDigit(span[i + 1])))) //the imaginary part of a complex number.
            {
                if (isOperand)
                    return Evaluate<TResult>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Function);

                var tokenPosition = i;
                value = span.ParseNumber<TResult>(_numberFormat, ref i);

                if (value is Complex c && c.Imaginary != 0.0)
                    OnEvaluating(tokenPosition, i, value);
                continue;
            }

            switch (span[i])
            {
                case Constants.DefaultOpeningSymbol:
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var tokenPosition = i;
                    i++;
                    var result = Evaluate<TResult>(ref i, depth, null, Constants.DefaultClosingSymbol);
                    MathString.ThrowExceptionIfNotClosed(Constants.DefaultClosingSymbol, tokenPosition, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiation(tokenPosition, ref i, depth, separator, closingSymbol, result);
                    value = value == default ? result : value * result;

                    if (value != result)
                        OnEvaluating(start, i, value, skipNaN: true);
                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || (precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsWhiteSpace(start, i)))
                        return value;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    value += Evaluate<TResult>(ref i, depth, separator, closingSymbol, p, isOperand);

                    OnEvaluating(start, i, value);
                    if (isOperand)
                        return value;

                    break;
                case '-' when span.Length == i + 1 || span[i + 1] != '-':
                    var isWhiteSpace = MathString.IsWhiteSpace(start, i);
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !isWhiteSpace)
                        return value;

                    i++;
                    var numberPosition = i;

                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    result = Evaluate<TResult>(ref i, depth, separator, closingSymbol, p, isOperand);

                    if (result is Complex c)
                    {
                        //it keeps sign of the part of the complex number, correct sign is important in complex analysis.
                        if (isWhiteSpace && span[numberPosition..i].IsComplexNumberPart(_numberFormat, out var isImaginaryPart))
                            value = TResult.CreateChecked(isImaginaryPart ? Complex.Conjugate(c) : new Complex(-c.Real, c.Imaginary));
                        else
                            value -= result;
                    }
                    else
                        value = isWhiteSpace ? -result : value - result; //it keeps sign

                    OnEvaluating(start, i, value);
                    if (isOperand)
                        return value;

                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= Evaluate<TResult>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic);

                    OnEvaluating(start, i, value);
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= Evaluate<TResult>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic);

                    OnEvaluating(start, i, value);
                    break;
                default:
                    if (char.IsWhiteSpace(span[i]))
                    {
                        i++;
                        break;
                    }

                    var entity = FirstMathEntity(span[i..]);
                    if (entity == null)
                    {
                        if (span.TryParseCurrency(_numberFormat, ref i))
                            break;

                        throw CreateExceptionInvalidToken(span, i);
                    }

                    //highest precedence is evaluating first
                    if (precedence >= entity.Precedence)
                        return value;

                    value = entity.Evaluate(this, start, ref i, depth, separator, closingSymbol, value);

                    if (isOperand)
                        return value;

                    break;
            }
        }

        if (value == default)
            MathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return value;
    }

    internal TResult EvaluateOperand<TResult>(ref int i, int depth, char? separator, char? closingSymbol)
        where TResult : struct, INumberBase<TResult>
    {
        var start = i;
        var value = Evaluate<TResult>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (value == default)
            MathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return value;
    }

    internal TResult EvaluateExponentiation<TResult>(int start, ref int i, int depth, char? separator, char? closingSymbol, TResult value)
        where TResult : struct, INumberBase<TResult>
    {
        MathString.SkipWhiteSpace(ref i);
        if (MathString.Length <= i)
            return value;

        var entity = FirstMathEntity(MathString.AsSpan(i));
        if (entity is not { Precedence: >= (int)EvalPrecedence.Exponentiation })
            return value;

        //an operand operator that processes the left operand recurses back here without calling Evaluate,
        //for example '2!!!!', so this cycle has to probe the call stack on its own.
        if ((++depth & (NestingDepthProbeInterval - 1)) == 1)
            EnsureNestingDepth(depth, i);

        return entity.Evaluate(this, start, ref i, depth, separator, closingSymbol, value);
    }

    internal void OnEvaluating<T>(int start, int i, T value, string? mathString = null, bool? isCompleted = null, bool skipNaN = false)
    {
        if (Evaluating == null)
            return;

        if (skipNaN && (value is double.NaN ||
                        value is float.NaN ||
                        value is Half h && Half.IsNaN(h) ||
                        value is Complex c && (double.IsNaN(c.Real) || double.IsNaN(c.Imaginary))))
        {
            return;
        }

        mathString ??= MathString;
        _evaluatingStep++;
        Evaluating.Invoke(this, new EvaluatingEventArgs(mathString, start, i - 1, _evaluatingStep, value!, isCompleted));
    }

    /// <summary>
    ///     Throws the <see cref="MathExpressionException" /> if the call stack is running out,
    ///     so a too deeply nested math expression string is reported instead of terminating the process
    ///     with an uncatchable <see cref="StackOverflowException" />.
    /// </summary>
    /// <param name="depth">The current recursion depth of the parsing.</param>
    /// <param name="i">The current char index.</param>
    /// <exception cref="MathExpressionException" />
    [MethodImpl(MethodImplOptions.NoInlining)] //the throwing path must not take space in the recursive frame
    private static void EnsureNestingDepth(int depth, int i)
    {
        if (RuntimeHelpers.TryEnsureSufficientExecutionStack())
            return;

        throw new MathExpressionException(
            $"The math expression string is nested too deeply, the nesting depth {depth} doesn't fit into the call stack of the current thread. " +
            "Simplify the math expression string, or evaluate it on a thread that has a bigger stack size.",
            i) { NestingDepth = depth };
    }

    private MathExpressionException CreateException(Exception ex, object? parameters)
    {
        ex = ex is not MathExpressionException ? new MathExpressionException(ex.Message, ex) : ex;
        ex.Data["mathString"] = MathString;
        ex.Data["context"] = Context;
        ex.Data["provider"] = Provider;
        ex.Data["compiler"] = Compiler;
        ex.Data[nameof(parameters)] = parameters;

        if (ex is MathExpressionException { NestingDepth: >= 0 } depthEx)
            ex.Data["nestingDepth"] = depthEx.NestingDepth;

        return (MathExpressionException)ex;
    }

    private static MathExpressionException CreateExceptionInvalidToken(ReadOnlySpan<char> span, int invalidTokenPosition)
    {
        var i = invalidTokenPosition;
        const string values = "(0123456789.,٫+-*/ \t\n\r";
        var end = span[i..].IndexOfAny(values) + i;
        var unknownSubstring = end > i ? span[i..end] : span[i..];

        return new MathExpressionException($"'{unknownSubstring}' is not recognizable, maybe setting the appropriate MathContext could help.", i);
    }

    private IMathEntity? FirstMathEntity(ReadOnlySpan<char> mathString)
    {
        var p = Parameters?.FirstMathEntity(mathString);
        if (p == null)
        {
            return Context?.FirstMathEntity(mathString);
        }

        var c = Context?.FirstMathEntity(mathString);
        if (c != null)
        {
            //parameter and context entity are found so compare them to get the longest key it says what entity is found
            return c.Key.Length > p.Key.Length ? c : p;
        }

        return p;
    }

    private bool IsParamSeparator(char separator, int start, int i)
        => MathString[i] == separator && (_decimalSeparator != separator || !MathString.IsWhiteSpace(start, i));
}