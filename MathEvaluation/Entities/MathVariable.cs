using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace MathEvaluation.Entities;

/// <summary>
///     The math variable uses as a parameter.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathVariable<T> : MathEntity
    where T : struct, INumberBase<T>
{
    private readonly bool _isDictionaryItem;
    private readonly T _variableValue;

    /// <summary>
    ///     The math variable uses as a parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public MathVariable(string? key, T value, bool isDictionaryItem = false) : base(key)
    {
        _isDictionaryItem = isDictionaryItem;
        _variableValue = value;
    }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Variable;

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
    {
        var tokenPosition = i;
        i += Key.Length;

        var result = ConvertNumber<T, TResult>(_variableValue);
        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result)
            mathExpression.OnEvaluating(start, i, value, skipNaN: true);

        return value;
    }

    /// <inheritdoc />
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;

        Expression right;
        if (_isDictionaryItem)
        {
            var parameterType = mathExpression.ParameterExpression!.Type;

            // Try to get the default indexer property
            var indexerProperty = parameterType.GetProperties()
                .FirstOrDefault(p => p.GetIndexParameters().Length > 0 && p.GetIndexParameters()[0].ParameterType == typeof(string));

            if (indexerProperty != null)
            {
                // Use the indexer if available
                var keyExpression = Expression.Constant(Key, typeof(string));
                right = Expression.MakeIndex(mathExpression.ParameterExpression!, indexerProperty, [keyExpression]);
            }
            else
            {
                // Fallback: Try to use TryGetValue or ContainsKey/get_Item pattern
                var dictionaryInterface = parameterType.GetInterfaces()
                    .FirstOrDefault(i
                        => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>) && i.GetGenericArguments()[0] == typeof(string));

                MethodInfo? tryGetValueMethod;
                Type? dictionaryValueType = null;

                const string methodName = "TryGetValue";
                if (dictionaryInterface != null)
                {
                    dictionaryValueType = dictionaryInterface.GetGenericArguments()[1];
                    tryGetValueMethod = dictionaryInterface.GetMethod(methodName, [typeof(string), dictionaryValueType.MakeByRefType()]);
                }
                else
                {
                    tryGetValueMethod = parameterType.GetMethod(methodName, [typeof(string), typeof(T).MakeByRefType()]);
                }

                if (tryGetValueMethod != null)
                {
                    // Build expression for TryGetValue
                    var valueType = dictionaryValueType ?? typeof(T);
                    var valueVariable = Expression.Variable(valueType, "value");
                    var keyExpression = Expression.Constant(Key, typeof(string));

                    Expression parameterExpression;
                    if (dictionaryInterface != null)
                    {
                        parameterExpression = Expression.Convert(mathExpression.ParameterExpression!, dictionaryInterface);
                    }
                    else
                    {
                        parameterExpression = mathExpression.ParameterExpression!;
                    }

                    var tryGetValueCall = Expression.Call(
                        parameterExpression,
                        tryGetValueMethod,
                        keyExpression,
                        valueVariable
                    );

                    Expression resultExpression = valueVariable;

                    // If dictionary value type is different from T, add conversion
                    if (valueType != typeof(T))
                    {
                        resultExpression = Expression.Convert(valueVariable, typeof(T));
                    }

                    right = Expression.Block([valueVariable], tryGetValueCall, resultExpression);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"The parameter type '{parameterType.Name}' does not support dictionary-like access with string keys.");
                }
            }
        }
        else
        {
            right = Expression.Property(mathExpression.ParameterExpression!, Key);
        }

        right = BuildConvert<TResult>(right);
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}