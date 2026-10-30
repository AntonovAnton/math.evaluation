using System;
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
    }
}