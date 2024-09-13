using System;
using System.Linq.Expressions;

namespace MathEvaluation.Extensions;

internal static class ExpressionExtensions
{
    internal static bool IsZero(this Expression expression)
        => expression is ConstantExpression c && Convert.ToDouble(c.Value) == 0.0;
}
