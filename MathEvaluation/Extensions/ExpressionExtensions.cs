using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Extensions;

internal static class ExpressionExtensions
{
    internal static bool IsDefault(this Expression expression)
        => expression is ConstantExpression c &&
           (c.Value is Complex complex ? complex == default : Convert.ToDouble(c.Value) == default);
}