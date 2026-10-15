using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace MathEvaluation.Extensions;

#pragma warning disable IL2111 // Method with parameters or return value with 'DynamicallyAccessedMembers' is accessed via reflection
#pragma warning disable IL2070 // 'this' argument does not satisfy 'DynamicallyAccessedMembers' in call to 'Type.GetProperties(BindingFlags)'

internal static class TypeExtensions
{
    private static readonly Type NumberBaseInterfaceType = typeof(INumberBase<>);

    // Thread-safe cache mapping Types to their readable properties
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyInfo>> _cache = new();

    extension([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicProperties)] Type type)
    {
        /// <summary>Determines whether the specified type is a number base type.</summary>
        public bool IsNumberBaseType()
        {
            var interfaces = type.GetInterfaces();
            return interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == NumberBaseInterfaceType);
        }

        /// <summary>Gets the readable properties of the specified type, using a thread-safe cache.</summary>
        public IReadOnlyList<PropertyInfo> GetReadableProperties()
        {
            return _cache.GetOrAdd(type, t =>
                [.. t.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead)]
            );
        }
    }
}

#pragma warning restore IL2070
#pragma warning restore IL2111