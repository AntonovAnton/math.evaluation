using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Extensions;

internal static class ExpressionExtensions
{
    extension(Expression expression)
    {
        internal bool IsDefault<T>()
            where T : struct, INumberBase<T>
            => expression is ConstantExpression c &&
               // ReSharper disable once CompareOfFloatsByEqualityOperator
               (c.Value is T t ? t == T.Zero : Convert.ToDouble(c.Value) == 0.0);

        internal Expression AssignVariables(IReadOnlyCollection<BinaryExpression> assignmentExpressions)
        {
            if (assignmentExpressions.Count == 0)
            {
                return expression;
            }

            var variables = assignmentExpressions.Select(e => e.Left).Cast<ParameterExpression>();
            List<Expression> expressions = [.. assignmentExpressions, expression];
            return Expression.Block(variables, expressions);
        }
    }
}