#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Runtime.CompilerServices
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
    {
        public string ParameterName { get; } = parameterName;
    }
}
