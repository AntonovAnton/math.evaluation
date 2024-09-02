using System;

namespace MathEvaluation.Extensions;

internal static class TypeExtensions
{
    public static bool IsConvertibleToDouble(this Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.SByte or TypeCode.Byte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32
            or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single or TypeCode.Double
            or TypeCode.Decimal or TypeCode.Boolean => true,
        _ => false
    };

    public static bool IsDecimal(this Type type) => Type.GetTypeCode(type) == TypeCode.Decimal;
}
