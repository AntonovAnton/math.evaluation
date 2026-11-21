#if NET11_0_OR_GREATER
namespace MathEvaluation;

public partial class MathExpression
{
    /// <summary>The method that builds the generic complex number expression, it's <see cref="BuildComplex{T}(ref int, int, char?, char?, int, bool)" />.</summary>
    /// <typeparam name="TResult">The <see cref="Complex{T}" /> type of the expression.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    private delegate Expression BuildFn<TResult>(MathExpression mathExpression, ref int i, int depth, char? separator, char? closingSymbol,
        int precedence, bool isOperand);

    /// <summary>
    ///     The <see cref="BuildFn{TResult}" /> of the <see cref="Complex{T}" /> type that is set by <see cref="CompileComplex{T}()" />.
    ///     The math entities only know the <see cref="Complex{T}" /> type of the result, so <see cref="Build{TResult}(ref int, int, char?, char?, int, bool)" />
    ///     forwards to it, it doesn't require reflection.
    /// </summary>
    private object? _complexBuild;

    /// <summary>
    ///     Compiles the <see cref="MathString">math expression string</see> that returns a generic <see cref="Complex{T}">complex</see> number (.NET 11+),
    ///     for example <c>Complex&lt;Decimal64&gt;</c>. The real and imaginary parts keep the precision of <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the real and imaginary parts.</typeparam>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="MathExpressionException" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<Complex<T>> CompileComplex<T>()
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        _complexBuild = new BuildFn<Complex<T>>(BuildComplexFn<T>);
        _complexOperators = ComplexFns<T>.Operators;
        return Compile<Complex<T>>();
    }

    /// <inheritdoc cref="CompileComplex{T}()" />
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <typeparam name="T">The type of the real and imaginary parts.</typeparam>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException">parameters</exception>
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<TParameters, Complex<T>> CompileComplex<TParameters, T>(TParameters parameters)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        _complexBuild = new BuildFn<Complex<T>>(BuildComplexFn<T>);
        _complexOperators = ComplexFns<T>.Operators;
        return Compile<TParameters, Complex<T>>(parameters);
    }

    /// <inheritdoc cref="CompileAsyncFunc{TResult}()" />
    /// <typeparam name="T">The type of the real and imaginary parts.</typeparam>
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<CancellationToken, ValueTask<Complex<T>>> CompileComplexAsyncFunc<T>()
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        _complexBuild = new BuildFn<Complex<T>>(BuildComplexFn<T>);
        _complexOperators = ComplexFns<T>.Operators;
        return CompileAsyncFunc<Complex<T>>();
    }

    /// <inheritdoc cref="CompileAsyncFunc{T, TResult}(T)" />
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <typeparam name="T">The type of the real and imaginary parts.</typeparam>
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<TParameters, CancellationToken, ValueTask<Complex<T>>> CompileComplexAsyncFunc<TParameters, T>(TParameters parameters)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        _complexBuild = new BuildFn<Complex<T>>(BuildComplexFn<T>);
        _complexOperators = ComplexFns<T>.Operators;
        return CompileAsyncFunc<TParameters, Complex<T>>(parameters);
    }

    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    internal Expression BuildComplex<T>(ref int i, int depth, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
    {
        if ((++depth & (NestingDepthProbeInterval - 1)) == 1)
            EnsureNestingDepth(depth, i);

        var span = MathString.AsSpan();
        Expression expression = Expression.Constant(default(Complex<T>));

        var start = i;
        while (span.Length > i)
        {
            if ((separator.HasValue && IsParamSeparator(separator.Value, start, i)) ||
                (closingSymbol.HasValue && span[i] == closingSymbol.Value))
            {
                if (expression.IsDefault<Complex<T>>())
                    MathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return expression;
            }

            if (char.IsAsciiDigit(span[i]) || span[i] == _decimalSeparator || //the real part of a complex number.
                (span[i] is 'i' && (span.Length == i + 1 || !char.IsLetterOrDigit(span[i + 1])))) //the imaginary part of a complex number.
            {
                if (isOperand)
                    return BuildComplex<T>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Function);

                var tokenPosition = i;
                var value = span.ParseComplexNumber<T>(_numberFormat, ref i);
                expression = Expression.Constant(value);

                if (value.Imaginary != T.Zero)
                    RaiseEvaluatingStep(tokenPosition, i, expression);

                continue;
            }

            switch (span[i])
            {
                case Constants.DefaultOpeningSymbol:
                    if (precedence >= (int)EvalPrecedence.Function)
                        return expression;

                    var tokenPosition = i;
                    i++;
                    var right = BuildComplex<T>(ref i, depth, null, Constants.DefaultClosingSymbol);
                    MathString.ThrowExceptionIfNotClosed(Constants.DefaultClosingSymbol, tokenPosition, ref i);
                    if (isOperand)
                        return right;

                    right = BuildExponentiation<Complex<T>>(tokenPosition, ref i, depth, separator, closingSymbol, right);
                    expression = BuildMultiplyIfLeftNotDefault<Complex<T>>(expression, right);

                    if (expression != right)
                        RaiseEvaluatingStep(start, i, expression);

                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || (precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsWhiteSpace(start, i)))
                        return expression;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = BuildComplex<T>(ref i, depth, separator, closingSymbol, p, isOperand);
                    expression = MathCompatibleOperator.Build<Complex<T>>(OperatorType.Add, expression, right);

                    RaiseEvaluatingStep(start, i, expression);
                    if (isOperand)
                        return expression;

                    break;
                case '-' when span.Length == i + 1 || span[i + 1] != '-':
                    var isWhiteSpace = MathString.IsWhiteSpace(start, i);
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !isWhiteSpace)
                        return expression;

                    i++;
                    var numberPosition = i;

                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = BuildComplex<T>(ref i, depth, separator, closingSymbol, p, isOperand);

                    //it keeps sign of the part of the complex number, correct sign is important in complex analysis.
                    if (right is ConstantExpression { Value: Complex<T> value })
                    {
                        if (isWhiteSpace && span[numberPosition..i].IsComplexNumberPart(_numberFormat, out var isImaginaryPart))
                            expression = Expression.Constant(isImaginaryPart ? Complex<T>.Conjugate(value) : new Complex<T>(-value.Real, value.Imaginary));
                        else
                            expression = MathCompatibleOperator.Build<Complex<T>>(OperatorType.Subtract, expression, right);
                    }
                    else
                    {
                        var operatorType = isWhiteSpace ? OperatorType.Negate : OperatorType.Subtract;
                        expression = MathCompatibleOperator.Build<Complex<T>>(operatorType, expression, right);
                    }

                    RaiseEvaluatingStep(start, i, expression);
                    if (isOperand)
                        return expression;

                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = BuildComplex<T>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = MathCompatibleOperator.Build<Complex<T>>(OperatorType.Multiply, expression, right);

                    RaiseEvaluatingStep(start, i, expression);
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = BuildComplex<T>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = MathCompatibleOperator.Build<Complex<T>>(OperatorType.Divide, expression, right);

                    RaiseEvaluatingStep(start, i, expression);
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
                        return expression;

                    expression = entity.Build<Complex<T>>(this, start, ref i, depth, separator, closingSymbol, expression);

                    if (isOperand)
                        return expression;

                    break;
            }
        }

        if (expression.IsDefault<Complex<T>>())
            MathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return expression.Reduce();
    }

    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    private static Expression BuildComplexFn<T>(MathExpression mathExpression, ref int i, int depth, char? separator, char? closingSymbol,
        int precedence, bool isOperand)
        where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
        => mathExpression.BuildComplex<T>(ref i, depth, separator, closingSymbol, precedence, isOperand);
}
#endif
