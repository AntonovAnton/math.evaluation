using System.Collections.Concurrent;
using System.Reflection;

namespace MathEvaluation.Extensions;

internal static class TypeExtensions
{
    // Thread-safe cache mapping Types to their readable properties
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyInfo>> _cache = new();
    private static readonly Type NumberBaseInterfaceType = typeof(INumberBase<>);

    /// <summary>Determines whether the specified type is a number base type.</summary>
    public static bool IsNumberBaseType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] this Type type)
    {
        // loops are used instead of lambdas, since a lambda inside the annotated extension is reported as accessed via reflection (IL2111)
        foreach (var i in type.GetInterfaces())
        {
            if (i.IsGenericType && i.GetGenericTypeDefinition() == NumberBaseInterfaceType)
                return true;
        }

        return false;
    }

    /// <summary>Gets the readable properties of the specified type, using a thread-safe cache.</summary>
    public static IReadOnlyList<PropertyInfo> GetReadableProperties(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
        this Type type)
    {
        if (_cache.TryGetValue(type, out var properties))
            return properties;

        var readableProperties = new List<PropertyInfo>();
        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (property.CanRead)
                readableProperties.Add(property);
        }

        return _cache.GetOrAdd(type, readableProperties);
    }

    /// <summary>Creates the exception for the type of property that can't be bound, with a hint if the type is a function.</summary>
    /// <param name="type">The type.</param>
    /// <param name="key">The key.</param>
    public static NotSupportedException CreateNotSupportedException(this Type type, string key)
    {
        if (!type.IsGenericType || type.FullName?.StartsWith("System.Func") != true)
            return new NotSupportedException($"{type} isn't supported for '{key}'.");

        var returnType = type.GetGenericArguments()[^1];
        var returnTypeDefinition = returnType.IsGenericType ? returnType.GetGenericTypeDefinition() : null;

        // a Task<T> function is a natural mistake, so it says how to wrap it
        if (returnTypeDefinition == typeof(Task<>))
        {
            return new NotSupportedException(
                $"{type} isn't supported for '{key}', async functions must return ValueTask<T> and take a CancellationToken as the last parameter, " +
                "you can wrap the function: (x, cancellationToken) => new ValueTask<T>(f(x)).");
        }

        if (returnTypeDefinition == typeof(ValueTask<>))
        {
            var arguments = type.GetGenericArguments();
            if (arguments.Length < 2 || arguments[^2] != typeof(CancellationToken))
            {
                return new NotSupportedException(
                    $"{type} isn't supported for '{key}', async functions must take a CancellationToken as the last parameter, " +
                    "for example: (x, cancellationToken) => f(x).");
            }

            return new NotSupportedException(
                $"{type} isn't supported for '{key}', the types of the arguments and of the result must be the same, " +
                "you can use Func<T[], CancellationToken, ValueTask<T>> instead.");
        }

        return new NotSupportedException($"{type} isn't supported for '{key}', you can use Func<T[], T> instead.");
    }
}