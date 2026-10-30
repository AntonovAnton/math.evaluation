using MathEvaluation.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading.Tasks;

namespace MathEvaluation.Entities;

/// <summary>
///     The async function with multiple parameters, so opening, separator, and closing symbol are required.
///     It is supported only by compiling the math expression into an async delegate.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathAsyncFunction<T> : MathEntity
    where T : struct, INumberBase<T>
{
    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    private Func<T[], ValueTask<T>> Fn { get; }

    /// <summary>Gets the opening symbol.</summary>
    /// <value>The opening symbol.</value>
    private char OpeningSymbol { get; }

    /// <summary>Gets the parameters separator.</summary>
    /// <value>The parameter separator.</value>
    private char Separator { get; }

    /// <summary>Gets the closing symbol.</summary>
    /// <value>The closing symbol.</value>
    private char ClosingSymbol { get; }

    /// <summary>Initializes a new instance of the <see cref="MathAsyncFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public MathAsyncFunction(string? key, Func<T[], ValueTask<T>> fn, char openingSymbol, char separator, char closingSymbol)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Separator = separator;
        OpeningSymbol = openingSymbol;
        ClosingSymbol = closingSymbol;
    }

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        TResult value)
        => throw new NotSupportedException($"The async function '{Key}' can't be evaluated synchronously, use the CompileAsyncFunc method instead.");

    /// <inheritdoc />
    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol, tokenPosition, ref i);

        var args = new List<Expression>();
        while (mathExpression.MathString.Length > i)
        {
            var arg = mathExpression.Build<T>(ref i, depth, Separator, ClosingSymbol);
            args.Add(arg);

            if (mathExpression.MathString[i] == Separator)
            {
                i++; //other param
                continue;
            }

            break;
        }

        mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol, tokenPosition, ref i);

        // Await the task before the math expression (like: 'await f(a, b)') and use its result
        var task = Expression.Invoke(Expression.Constant(Fn), Expression.NewArrayInit(typeof(T), args));
        mathExpression.Statements ??= new();
        Expression right = mathExpression.Statements.AddAwait(mathExpression.MathString[tokenPosition..i], task);
        mathExpression.RaiseEvaluatingStep(tokenPosition, i, right);

        right = BuildConvert<TResult>(right);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, depth, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.RaiseEvaluatingStep(start, i, expression);

        return expression;
    }
}
