using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Extensions;

internal static class ExpressionExtensions
{
#if NET8_0_OR_GREATER

    internal static bool IsDefault<T>(this Expression expression)
        where T : struct, INumberBase<T>
        => expression is ConstantExpression c &&
           (c.Value is T t ? t == T.Zero : Convert.ToDouble(c.Value) == default);
#else

    internal static bool IsDefault<T>(this Expression expression)
        where T : struct
        => expression is ConstantExpression c &&
           (c.Value is Complex complex ? complex == default : Convert.ToDouble(c.Value) == default);

#endif
}