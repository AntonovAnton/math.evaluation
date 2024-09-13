using System;
using System.Linq.Expressions;
using MathEvaluation.Parameters;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile()"/>
    public Func<bool> CompileBoolean()
    {
        try
        {
            var i = 0;
            ExpressionTree = Build<double>(ref i, null, null);
            ExpressionTree = Expression.NotEqual(ExpressionTree, Expression.Constant(default(double)));

            return Expression.Lambda<Func<bool>>(ExpressionTree).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, null);
        }
    }

    /// <inheritdoc cref="Compile{T}(T)"/>
    public Func<T, bool> CompileBoolean<T>(T parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        _parameters = new MathParameters(parameters);
        ParameterExpression = Expression.Parameter(typeof(T), nameof(parameters));

        try
        {
            var i = 0;
            ExpressionTree = Build<double>(ref i, null, null);
            ExpressionTree = Expression.NotEqual(ExpressionTree, Expression.Constant(default(double)));

            return Expression.Lambda<Func<T, bool>>(ExpressionTree, ParameterExpression).Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }
}
