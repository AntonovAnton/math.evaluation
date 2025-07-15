using MathEvaluation.Parameters;
using System;
using System.Linq.Expressions;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile()" />
    public Func<bool> CompileBoolean()
    {
        _evaluatingStep = 0;

        try
        {
            var i = 0;
            ExpressionTree = Build<double>(ref i, null, null);

            if (_evaluatingStep == 0)
                OnEvaluating(0, i, ExpressionTree);

            ExpressionTree = Expression.NotEqual(ExpressionTree, Expression.Constant(default(double)));

            var lambda = Expression.Lambda<Func<bool>>(ExpressionTree);
            ExpressionTree =  lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, null);
        }
    }

    /// <inheritdoc cref="Compile{T}(T)" />
    public Func<T, bool> CompileBoolean<T>(T parameters)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        _parameters = new MathParameters(parameters);
        ParameterExpression = Expression.Parameter(typeof(T), nameof(parameters));
        _evaluatingStep = 0;

        try
        {
            var i = 0;
            ExpressionTree = Build<double>(ref i, null, null);

            if (_evaluatingStep == 0)
                OnEvaluating(0, i, ExpressionTree);

            ExpressionTree = Expression.NotEqual(ExpressionTree, Expression.Constant(default(double)));

            var lambda = Expression.Lambda<Func<T, bool>>(ExpressionTree, ParameterExpression);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, parameters);
        }
    }
}