using System;
using System.Linq.Expressions;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile()" />
    public Func<bool> CompileBoolean()
    {
        try
        {
            // double because expression can have not boolean logic inside
            ExpressionTree = Build<double>();
            ExpressionTree = ConvertToBoolean(ExpressionTree);

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
        try
        {
            // double because expression can have not boolean logic inside
            ExpressionTree = Build<T, double>(parameters);
            ExpressionTree = ConvertToBoolean(ExpressionTree);

            if (ExpressionVariables.Count > 0)
            {
                ExpressionStatements.Add(ExpressionTree);
                ExpressionTree = Expression.Block(ExpressionVariables.Values, ExpressionStatements);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(ExpressionTree, ParameterExpression);
            ExpressionTree = lambda;

            return Compiler?.Compile(lambda) ?? lambda.Compile();
        }
        catch (Exception ex)
        {
            throw CreateException(ex, parameters);
        }
    }

    private Expression ConvertToBoolean(Expression expression)
    {
        // Avoid unnecessary conversion for boolean expressions
        if (expression.NodeType == ExpressionType.Convert &&
            expression is UnaryExpression unaryExpression &&
            unaryExpression.Operand?.Type == typeof(bool))
        {
            return unaryExpression.Operand;
        }
        else
        {
            return Expression.NotEqual(expression, Expression.Constant(default(double)));
        }
    }
}