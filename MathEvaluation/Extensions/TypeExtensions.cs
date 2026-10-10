using System;
using System.Linq;
using System.Numerics;

namespace MathEvaluation.Extensions;

internal static class TypeExtensions
{
    private static readonly Type NumberBaseInterfaceType = typeof(INumberBase<>);

    extension(Type type)
    {
        /// <summary>Determines whether the specified type is a number base type.</summary>
        public bool IsNumberBaseType()
        {
            var interfaces = type.GetInterfaces();
            return interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == NumberBaseInterfaceType);
        }
    }
}