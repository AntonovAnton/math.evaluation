using MathEvaluation.Compilation;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MathEvaluation;

public partial class MathExpression
{
    private const string ParameterName = "args";

    /// <summary>Gets the expression tree.</summary>
    /// <value>The expression tree.</value>
    // ReSharper disable once MemberCanBePrivate.Global
    public Expression? ExpressionTree { get; private set; }

    internal ParameterExpression? ParameterExpression { get; set; }
    internal ExpressionStatements? Statements { get; set; }

    /// <summary>Compiles the <see cref="MathString">math expression string</see>.</summary>
    /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="MathExpressionException" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<TResult> Compile<TResult>()
        where TResult : struct, INumberBase<TResult>
    {
        try
        {
            Statements = null;
            ExpressionTree = Build<TResult>(null);

            if (Statements != null)
                ExpressionTree = Statements.BuildBlock(ExpressionTree);

            var lambda = Expression.Lambda<Func<TResult>>(ExpressionTree);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, null);
        }
    }

    /// <inheritdoc cref="Compile{T}()" />
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <typeparam name="T">The type of the parameters.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException">parameters</exception>
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, TResult> Compile<T, TResult>(T parameters)
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(parameters);

        try
        {
            Statements = null;
            ParameterExpression = Expression.Parameter(typeof(T), ParameterName);
            ExpressionTree = Build<TResult>(new MathParameters(parameters));

            if (Statements != null)
                ExpressionTree = Statements.BuildBlock(ExpressionTree);

            var lambda = ParameterExpression == null
                ? Expression.Lambda<Func<T, TResult>>(ExpressionTree)
                : Expression.Lambda<Func<T, TResult>>(ExpressionTree, ParameterExpression);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, parameters);
        }
    }

    /// <summary>
    ///     Compiles the <see cref="MathString">math expression string</see> that can contain async functions.
    ///     The async functions are awaited one after another before the rest of the math expression is calculated.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the task.</typeparam>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="MathExpressionException" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<ValueTask<TResult>> CompileAsyncFunc<TResult>()
        where TResult : struct, INumberBase<TResult>
    {
        try
        {
            Statements = null;
            ExpressionTree = Build<TResult>(null);
            ExpressionTree = (Statements ?? new()).BuildAsyncBlock<TResult>(ExpressionTree);

            var lambda = Expression.Lambda<Func<ValueTask<TResult>>>(ExpressionTree);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, null);
        }
    }

    /// <inheritdoc cref="CompileAsyncFunc{TResult}()" />
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <typeparam name="T">The type of the parameters.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the task.</typeparam>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException">parameters</exception>
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, ValueTask<TResult>> CompileAsyncFunc<T, TResult>(T parameters)
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(parameters);

        try
        {
            Statements = null;
            ParameterExpression = Expression.Parameter(typeof(T), ParameterName);
            ExpressionTree = Build<TResult>(new MathParameters(parameters));
            ExpressionTree = (Statements ?? new()).BuildAsyncBlock<TResult>(ExpressionTree);

            var lambda = ParameterExpression == null
                ? Expression.Lambda<Func<T, ValueTask<TResult>>>(ExpressionTree)
                : Expression.Lambda<Func<T, ValueTask<TResult>>>(ExpressionTree, ParameterExpression);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, parameters);
        }
    }

    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    internal Expression Build<TResult>(MathParameters? parameters, int depth = 0)
        where TResult : struct, INumberBase<TResult>
    {
        if (!RuntimeFeature.IsDynamicCodeSupported)
        {
            throw new PlatformNotSupportedException(
                "Dynamic compilation is not supported in this runtime (e.g. NativeAOT).");
        }

        Parameters = parameters;
        _evaluatingStep = 0;

        var i = 0;
        var expression = Build<TResult>(ref i, depth, null, null);

        if (_evaluatingStep == 0)
            RaiseEvaluatingStep(0, i, expression);

        return expression;
    }

    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    internal Expression Build<TResult>(ref int i, int depth, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
        where TResult : struct, INumberBase<TResult>
    {
        if ((++depth & (NestingDepthProbeInterval - 1)) == 1)
            EnsureNestingDepth(depth, i);

        var span = MathString.AsSpan();
        Expression expression = Expression.Constant(default(TResult));

        var start = i;
        while (span.Length > i)
        {
            if ((separator.HasValue && IsParamSeparator(separator.Value, start, i)) ||
                (closingSymbol.HasValue && span[i] == closingSymbol.Value))
            {
                if (expression.IsDefault<TResult>())
                    MathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return expression;
            }

            if (char.IsAsciiDigit(span[i]) || span[i] == _decimalSeparator || //the real part of a number.
                (typeof(TResult) == typeof(Complex) &&
                 span[i] is 'i' && (span.Length == i + 1 || !char.IsLetterOrDigit(span[i + 1])))) //the imaginary part of a complex number.
            {
                if (isOperand)
                    return Build<TResult>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Function);

                var tokenPosition = i;
                var value = span.ParseNumber<TResult>(_numberFormat, ref i);
                expression = Expression.Constant(value);

                if (value is Complex c && c.Imaginary != 0.0)
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
                    var right = Build<TResult>(ref i, depth, null, Constants.DefaultClosingSymbol);
                    MathString.ThrowExceptionIfNotClosed(Constants.DefaultClosingSymbol, tokenPosition, ref i);
                    if (isOperand)
                        return right;

                    right = BuildExponentiation<TResult>(tokenPosition, ref i, depth, separator, closingSymbol, right);
                    expression = BuildMultiplyIfLeftNotDefault<TResult>(expression, right);

                    if (expression != right)
                        RaiseEvaluatingStep(start, i, expression);
                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || (precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsWhiteSpace(start, i)))
                        return expression;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    right = Build<TResult>(ref i, depth, separator, closingSymbol, p, isOperand);
                    expression = MathCompatibleOperator.Build<TResult>(OperatorType.Add, expression, right);

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
                    right = Build<TResult>(ref i, depth, separator, closingSymbol, p, isOperand);

                    //it keeps sign of the part of the complex number, correct sign is important in complex analysis.
                    if (right is ConstantExpression { Value: Complex value })
                    {
                        if (isWhiteSpace && span[numberPosition..i].IsComplexNumberPart(_numberFormat, out var isImaginaryPart))
                            expression = Expression.Constant(isImaginaryPart ? Complex.Conjugate(value) : new Complex(-value.Real, value.Imaginary));
                        else
                            expression = MathCompatibleOperator.Build<TResult>(OperatorType.Subtract, expression, right);
                    }
                    else
                    {
                        var operatorType = isWhiteSpace ? OperatorType.Negate : OperatorType.Subtract;
                        expression = MathCompatibleOperator.Build<TResult>(operatorType, expression, right);
                    }

                    RaiseEvaluatingStep(start, i, expression);
                    if (isOperand)
                        return expression;

                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build<TResult>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = MathCompatibleOperator.Build<TResult>(OperatorType.Multiply, expression, right);

                    RaiseEvaluatingStep(start, i, expression);
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return expression;

                    i++;
                    right = Build<TResult>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    expression = MathCompatibleOperator.Build<TResult>(OperatorType.Divide, expression, right);

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

                    expression = entity.Build<TResult>(this, start, ref i, depth, separator, closingSymbol, expression);

                    if (isOperand)
                        return expression;

                    break;
            }
        }

        if (expression.IsDefault<TResult>())
            MathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return expression.Reduce();
    }

    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    internal static Expression BuildMultiplyIfLeftNotDefault<TResult>(Expression left, Expression right)
        where TResult : struct, INumberBase<TResult>
    {
        return left.IsDefault<TResult>()
            ? right
            : MathCompatibleOperator.Build<TResult>(OperatorType.Multiply, left, right);
    }

    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    internal Expression BuildOperand<TResult>(ref int i, int depth, char? separator, char? closingSymbol)
        where TResult : struct, INumberBase<TResult>
    {
        var start = i;
        var expression = Build<TResult>(ref i, depth, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (expression.IsDefault<TResult>())
            MathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return expression;
    }

    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    internal Expression BuildExponentiation<TResult>(int start, ref int i, int depth, char? separator, char? closingSymbol, Expression left)
        where TResult : struct, INumberBase<TResult>
    {
        MathString.SkipWhiteSpace(ref i);
        if (MathString.Length <= i)
            return left;

        var entity = FirstMathEntity(MathString.AsSpan(i));
        if (entity is not { Precedence: >= (int)EvalPrecedence.Exponentiation })
            return left;

        //an operand operator that processes the left operand recurses back here without calling Build,
        //for example '2!!!!', so this cycle has to probe the call stack on its own.
        if ((++depth & (NestingDepthProbeInterval - 1)) == 1)
            EnsureNestingDepth(depth, i);

        return entity.Build<TResult>(this, start, ref i, depth, separator, closingSymbol, left);
    }
}