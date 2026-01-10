using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathEvaluation.Extensions;

internal static class TypeExtensions
{
    private static readonly HashSet<Type> NumberBaseTypes =
    [
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(sbyte),
        typeof(byte),
        typeof(Complex),
        typeof(Half),
        typeof(BigInteger),
        typeof(Int128),
        typeof(UInt128),
        typeof(IntPtr),
        typeof(UIntPtr)
    ];

    private static readonly Type NumberBaseInterfaceType = typeof(INumberBase<>);

    /// <summary>Determines whether the specified type is a number base type.</summary>
    public static bool IsNumberBaseType(this Type type)
    {
        if (NumberBaseTypes.Contains(type))
            return true;

        var interfaces = type.GetInterfaces();
        foreach (var i in interfaces)
        {
            if (i.IsGenericType && i.GetGenericTypeDefinition() == NumberBaseInterfaceType)
                return true;
        }

        return false;
    }

    public static bool IsBooleanType(this Type type)
        => type == typeof(bool);
}