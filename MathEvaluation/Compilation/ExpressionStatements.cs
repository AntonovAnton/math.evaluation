using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MathEvaluation.Compilation;

/// <summary>
///     The ordered statements (variable assignments and awaits) that are executed before the math expression itself.
///     The insertion order is the dependency order: an entity builds its arguments (or a nested expression variable)
///     before it adds its own statement, so every statement references only statements that were added before it.
///     For example, an expression variable that contains an async function is assigned after the function is awaited.
/// </summary>
internal sealed class ExpressionStatements
{
#if NET9_0_OR_GREATER 
    private readonly OrderedDictionary<string, Statement> _statements = [];
#else
    private readonly System.Collections.Specialized.OrderedDictionary _statements = new(StringComparer.Ordinal);
#endif

    /// <summary>Gets a value indicating whether there are statements to await.</summary>
    public bool HasAwaits { get; private set; }

    /// <summary>Gets the statements in the order of execution.</summary>
#if NET9_0_OR_GREATER
    private IEnumerable<Statement> Statements => _statements.Values;
#else
    private IEnumerable<Statement> Statements => _statements.Values.Cast<Statement>();
#endif

    /// <summary>Gets the variable that is assigned by the statement with the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="variable">The variable.</param>
    /// <returns><c>true</c> if the variable is found; otherwise, <c>false</c>.</returns>
    public bool TryGetVariable(string key, [NotNullWhen(true)] out ParameterExpression? variable)
    {
#if NET9_0_OR_GREATER
        _statements.TryGetValue(key, out var statement);
#else
        var statement = (Statement?)_statements[key];
#endif
        variable = statement?.Variable;
        return variable != null;
    }

    /// <summary>Adds the variable assignment (like: 'x = a + b').</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value expression.</param>
    /// <returns>The assigned variable.</returns>
    public ParameterExpression AddAssignment(string key, Expression value)
    {
        var variable = Expression.Variable(value.Type, key);
        _statements.Add(key, new Statement(variable, value, false));
        return variable;
    }

    /// <summary>Adds the statement that awaits a task (like: 'await f(a, b)') before the next statements.</summary>
    /// <param name="notation">The notation of the function call in the math expression string (like: 'f(a, b)').</param>
    /// <param name="task">The expression that returns a <see cref="ValueTask{TResult}" />.</param>
    /// <returns>The variable of the task result.</returns>
    public ParameterExpression AddAwait(string notation, Expression task)
    {
        // the notation contains the function brackets, so it can't match a variable key,
        // but the same call can be used several times and every call is awaited, so it gets the number of the statement
        var key = Contains(notation) ? $"{notation}#{_statements.Count}" : notation;

        // the result of the task is passed as a parameter of the continuation, since a ValueTask can be awaited only once
        var variable = Expression.Parameter(task.Type.GetGenericArguments()[0], key);
        _statements.Add(key, new Statement(variable, task, true));
        HasAwaits = true;
        return variable;
    }

    /// <summary>Builds the block that executes the statements and then the body.</summary>
    /// <param name="body">The math expression.</param>
    /// <returns>The block expression.</returns>
    /// <exception cref="NotSupportedException">There are statements to await.</exception>
    public Expression BuildBlock(Expression body)
    {
        if (HasAwaits)
            throw new NotSupportedException("Async functions are supported only by the CompileAsyncFunc method.");

        if (_statements.Count == 0)
            return body;

        List<Expression> expressions = [.. Statements.Select(s => Expression.Assign(s.Variable, s.Value)), body];
        return Expression.Block(Statements.Select(s => s.Variable), expressions);
    }

    /// <summary>
    ///     Builds the block that executes the statements and then the body, it awaits each task before the next statements,
    ///     so the rest of the statements are built as a continuation that gets the result of the task as a parameter.
    /// </summary>
    /// <param name="body">The math expression.</param>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <returns>The block expression that returns a <see cref="ValueTask{TResult}" />.</returns>
    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    public Expression BuildAsyncBlock<TResult>(Expression body)
        where TResult : struct
    {
        var then = typeof(AsyncContinuation).GetMethod(nameof(AsyncContinuation.Then))!;
        body = Expression.New(typeof(ValueTask<TResult>).GetConstructor([typeof(TResult)])!, body);

        foreach (var s in Statements.Reverse())
        {
            body = s.IsAwait
                ? Expression.Call(then.MakeGenericMethod(s.Variable.Type, typeof(TResult)), s.Value, Expression.Lambda(body, s.Variable))
                : Expression.Block([s.Variable], Expression.Assign(s.Variable, s.Value), body);
        }

        return body;
    }

#if NET9_0_OR_GREATER
    private bool Contains(string key) => _statements.ContainsKey(key);
#else
    private bool Contains(string key) => _statements.Contains(key);
#endif

    /// <summary>The statement that assigns the value, or the result of the awaited task, to the variable.</summary>
    private sealed record Statement(ParameterExpression Variable, Expression Value, bool IsAwait);
}
