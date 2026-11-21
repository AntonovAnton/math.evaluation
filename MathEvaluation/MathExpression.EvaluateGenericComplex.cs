#if NET11_0_OR_GREATER
namespace MathEvaluation;

public partial class MathExpression
{
    /// <summary>The method that evaluates the generic complex number, it's <see cref="EvaluateComplex{T}(ref int, int, char?, char?, int, bool)" />.</summary>
    private delegate TResult EvaluateFn<TResult>(MathExpression mathExpression, ref int i, int depth, char? separator, char? closingSymbol,
        int precedence, bool isOperand);

    /// <summary>
    ///     The <see cref="EvaluateFn{TResult}" /> of the <see cref="Complex{T}" /> type that is set by <see cref="EvaluateComplex{T}(MathParameters?)" />.
    ///     The math entities only know the <see cref="Complex{T}" /> type of the result, so <see cref="Evaluate{TResult}(ref int, int, char?, char?, int, bool)" />
    ///     forwards to it, it doesn't require reflection.
    /// </summary>
    private object? _complexEvaluate;

    /// <summary>
    ///     The <see cref="ComplexOperators{TResult}" /> of the <see cref="Complex{T}" /> type that are set by
    ///     <see cref="EvaluateComplex{T}(MathParameters?)" /> and <see cref="CompileComplex{T}()" />.
    /// </summary>
    private object? _complexOperators;

    /// <inheritdoc cref="EvaluateComplex{T}(MathParameters?)" />
    public Complex<T> EvaluateComplex<T>()
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
        => EvaluateComplex<T>((MathParameters?)null);

    /// <inheritdoc cref="EvaluateComplex{T}(MathParameters?)" />
    /// <exception cref="NotSupportedException">parameters</exception>
    [RequiresUnreferencedCode("Binding object as parameters requires reflection which may access trimmed members")]
    public Complex<T> EvaluateComplex<T>(object parameters)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
        => EvaluateComplex<T>(new MathParameters(parameters));

    /// <inheritdoc cref="EvaluateComplex{T}(MathParameters?)" />
    public Complex<T> EvaluateComplex<T>(IDictionary<string, Complex<T>> parameters)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return EvaluateComplex<T>(mathParameters);
    }

    /// <inheritdoc cref="EvaluateComplex{T}(MathParameters?)" />
    public Complex<T> EvaluateComplex<T>(IDictionary<string, object> parameters)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        var mathParameters = new MathParameters();
        mathParameters.Bind(parameters);
        return EvaluateComplex<T>(mathParameters);
    }

    /// <summary>
    ///     Evaluates the <see cref="MathString">math expression string</see> as a generic <see cref="Complex{T}">complex</see> number (.NET 11+),
    ///     for example <c>Complex&lt;Decimal64&gt;</c>. The real and imaginary parts keep the precision of <typeparamref name="T" />.
    /// </summary>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <typeparam name="T">The type of the real and imaginary parts.</typeparam>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="MathExpressionException" />
    public Complex<T> EvaluateComplex<T>(MathParameters? parameters)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        _complexEvaluate = ComplexFns<T>.Evaluate;
        _complexOperators = ComplexFns<T>.Operators;
        return Evaluate<Complex<T>>(parameters);
    }

    internal Complex<T> EvaluateComplex<T>(ref int i, int depth, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        if ((++depth & (NestingDepthProbeInterval - 1)) == 1)
            EnsureNestingDepth(depth, i);

        var span = MathString.AsSpan();
        var value = default(Complex<T>);

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

            if (char.IsAsciiDigit(span[i]) || span[i] == _decimalSeparator || //the real part of a complex number.
                (span[i] is 'i' && (span.Length == i + 1 || !char.IsLetterOrDigit(span[i + 1])))) //the imaginary part of a complex number.
            {
                if (isOperand)
                    return EvaluateComplex<T>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Function);

                var tokenPosition = i;
                value = span.ParseComplexNumber<T>(_numberFormat, ref i);

                if (value.Imaginary != T.Zero)
                    RaiseEvaluatingStep(tokenPosition, i, value);

                continue;
            }

            switch (span[i])
            {
                case Constants.DefaultOpeningSymbol:
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var tokenPosition = i;
                    i++;
                    var result = EvaluateComplex<T>(ref i, depth, null, Constants.DefaultClosingSymbol);
                    MathString.ThrowExceptionIfNotClosed(Constants.DefaultClosingSymbol, tokenPosition, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiation(tokenPosition, ref i, depth, separator, closingSymbol, result);
                    value = value == default ? result : value * result;

                    if (value != result)
                        RaiseEvaluatingStepIfNotNaN(start, i, value);

                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || (precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsWhiteSpace(start, i)))
                        return value;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    value += EvaluateComplex<T>(ref i, depth, separator, closingSymbol, p, isOperand);

                    RaiseEvaluatingStep(start, i, value);
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
                    result = EvaluateComplex<T>(ref i, depth, separator, closingSymbol, p, isOperand);

                    //it keeps sign of the part of the complex number, correct sign is important in complex analysis.
                    if (isWhiteSpace && span[numberPosition..i].IsComplexNumberPart(_numberFormat, out var isImaginaryPart))
                        value = isImaginaryPart ? Complex<T>.Conjugate(result) : new Complex<T>(-result.Real, result.Imaginary);
                    else
                        value -= result;

                    RaiseEvaluatingStep(start, i, value);
                    if (isOperand)
                        return value;

                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= EvaluateComplex<T>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic);

                    RaiseEvaluatingStep(start, i, value);
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= EvaluateComplex<T>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic);

                    RaiseEvaluatingStep(start, i, value);
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

    /// <summary>Copies the generic complex number methods of the parent math expression, e.g. for an expression-defined variable.</summary>
    /// <param name="parent">The parent math expression.</param>
    internal void InheritComplexFns(MathExpression parent)
    {
        _complexEvaluate = parent._complexEvaluate;
        _complexBuild = parent._complexBuild;
        _complexOperators = parent._complexOperators;
    }

    /// <summary>
    ///     Gets the operators of the generic complex number that generic math doesn't define, e.g. the power,
    ///     if <typeparamref name="TResult" /> is the <see cref="Complex{T}" /> type of <see cref="EvaluateComplex{T}(MathParameters?)" />
    ///     or <see cref="CompileComplex{T}()" />; otherwise, <c>null</c>.
    /// </summary>
    internal ComplexOperators<TResult>? GetComplexOperators<TResult>()
        => _complexOperators as ComplexOperators<TResult>;

    /// <summary>
    ///     Throws the exception if the <typeparamref name="TResult" /> type is a generic <see cref="Complex{T}" /> type
    ///     that is evaluated or compiled without <see cref="EvaluateComplex{T}(MathParameters?)" /> or <see cref="CompileComplex{T}()" />.
    /// </summary>
    /// <exception cref="NotSupportedException" />
    private static void ThrowIfGenericComplex<TResult>(bool hasComplexFn, string methodName)
    {
        if (!hasComplexFn && typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(Complex<>))
        {
            var t = typeof(TResult).GetGenericArguments()[0].Name;
            throw new NotSupportedException($"Use the {methodName}<{t}> method for the Complex<{t}> type.");
        }
    }

    private static class ComplexFns<T>
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        internal static readonly EvaluateFn<Complex<T>> Evaluate =
            static (MathExpression mathExpression, ref int i, int depth, char? separator, char? closingSymbol, int precedence, bool isOperand)
                => mathExpression.EvaluateComplex<T>(ref i, depth, separator, closingSymbol, precedence, isOperand);

        internal static readonly ComplexOperators<Complex<T>> Operators = new(
            Complex<T>.Pow,
            static (left, right) => new Complex<T>(GetReal(left) % GetReal(right), T.Zero));

        /// <summary>Gets the real part, the imaginary part has to be zero.</summary>
        /// <exception cref="InvalidCastException">The imaginary part isn't zero.</exception>
        private static T GetReal(Complex<T> value)
            => value.Imaginary == T.Zero
                ? value.Real
                : throw new InvalidCastException($"Cannot convert the Complex<{typeof(T).Name}> number to a {typeof(T).Name}, value = {value}.");
    }
}

/// <summary>
///     The operators of the generic complex number that generic math doesn't define,
///     so the C# compatible operators, e.g. '^', '**', or 'mod', calculate them in the precision of the parts.
/// </summary>
/// <param name="Pow">Raises a complex number to the power.</param>
/// <param name="Modulo">Divides the real parts and returns the remainder, the imaginary parts have to be zero.</param>
/// <typeparam name="TResult">The <see cref="Complex{T}" /> type.</typeparam>
internal sealed record ComplexOperators<TResult>(Func<TResult, TResult, TResult> Pow, Func<TResult, TResult, TResult> Modulo);
#endif
