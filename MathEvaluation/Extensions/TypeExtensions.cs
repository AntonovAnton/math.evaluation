using System;
using System.Collections.Generic;
using System.Linq;
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

    extension(Type type)
    {
        /// <summary>Determines whether the specified type is a number base type.</summary>
        public bool IsNumberBaseType()
        {
            if (NumberBaseTypes.Contains(type))
                return true;

            var interfaces = type.GetInterfaces();
            return interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == NumberBaseInterfaceType);
        }

        public bool IsBooleanType()
            => type == typeof(bool);
    }
}