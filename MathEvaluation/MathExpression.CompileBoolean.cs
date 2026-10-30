using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile()" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<bool> CompileBoolean()
    {
        try
        {
            Statements = null;
            // double because expression can have not boolean logic inside
            ExpressionTree = Build<double>(null);
            ExpressionTree = ConvertToBoolean(ExpressionTree);

            if (Statements != null)
                ExpressionTree = Statements.BuildBlock(ExpressionTree);

            var lambda = Expression.Lambda<Func<bool>>(ExpressionTree);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, null);
        }
    }

    /// <inheritdoc cref="Compile{T}(T)" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, bool> CompileBoolean<T>(T parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        try
        {
            Statements = null;
            // double because expression can have not boolean logic inside
            ParameterExpression = Expression.Parameter(typeof(T), ParameterName);
            ExpressionTree = Build<double>(new MathParameters(parameters));
            ExpressionTree = ConvertToBoolean(ExpressionTree);

            if (Statements != null)
                ExpressionTree = Statements.BuildBlock(ExpressionTree);

            var lambda = ParameterExpression == null
                ? Expression.Lambda<Func<T, bool>>(ExpressionTree)
                : Expression.Lambda<Func<T, bool>>(ExpressionTree, ParameterExpression);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, parameters);
        }
    }

    /// <inheritdoc cref="CompileAsyncFunc{TResult}()" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<ValueTask<bool>> CompileBooleanAsyncFunc()
    {
        try
        {
            Statements = null;
            // double because expression can have not boolean logic inside
            ExpressionTree = Build<double>(null);
            ExpressionTree = ConvertToBoolean(ExpressionTree);
            ExpressionTree = (Statements ?? new()).BuildAsyncBlock<bool>(ExpressionTree);

            var lambda = Expression.Lambda<Func<ValueTask<bool>>>(ExpressionTree);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, null);
        }
    }

    /// <inheritdoc cref="CompileAsyncFunc{T, TResult}(T)" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, ValueTask<bool>> CompileBooleanAsyncFunc<T>(T parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        try
        {
            Statements = null;
            // double because expression can have not boolean logic inside
            ParameterExpression = Expression.Parameter(typeof(T), ParameterName);
            ExpressionTree = Build<double>(new MathParameters(parameters));
            ExpressionTree = ConvertToBoolean(ExpressionTree);
            ExpressionTree = (Statements ?? new()).BuildAsyncBlock<bool>(ExpressionTree);

            var lambda = ParameterExpression == null
                ? Expression.Lambda<Func<T, ValueTask<bool>>>(ExpressionTree)
                : Expression.Lambda<Func<T, ValueTask<bool>>>(ExpressionTree, ParameterExpression);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, parameters);
        }
    }

    private static Expression ConvertToBoolean(Expression expression)
    {
        // Avoid unnecessary conversion for boolean expressions
        if (expression.NodeType == ExpressionType.Convert &&
            expression is UnaryExpression unaryExpression &&
            unaryExpression.Operand.Type == typeof(bool))
        {
            return unaryExpression.Operand;
        }
        else
        {
            return Expression.NotEqual(expression, Expression.Constant(0.0));
        }
    }
}