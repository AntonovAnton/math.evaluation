using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace MathEvaluation.Extensions;

internal static class TypeExtensions
{
    private static readonly Type NumberBaseInterfaceType = typeof(INumberBase<>);

    // Thread-safe cache mapping Types to their readable properties
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyInfo>> _cache = new();

    extension(Type type)
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