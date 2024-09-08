using System;
using System.Collections.Generic;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(object?)"/>
    public decimal EvaluateDecimal(object? parameters = null)
        => EvaluateDecimal(parameters != null ? new MathParameters(parameters) : null);

    /// <inheritdoc cref="Evaluate(IMathParameters?)"/>
    public decimal EvaluateDecimal(IMathParameters? parameters)
    {
        _parameters = parameters;

        try
        {
            var defaultValue = new EvalValue(0.0m);
            var i = 0;
            return Evaluate(MathString, ref i, defaultValue, null, null);
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }

    private bool TryEvaluateEntityDecimal(ReadOnlySpan<char> mathString, IMathEntity entity, ref int i,
        char? separator, char? closingSymbol, ref EvalValue value)
    {
        var defaultValue = new EvalValue(0.0m);
        var start = i;
        switch (entity)
        {
            case MathConstant<decimal> constant:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, constant.Value);
                    value = value.IsDefault ? result : value * result;
                    return true;
                }
            case MathVariable<decimal> variable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, variable.Value);
                    value = value.IsDefault ? result : value * result;
                    return true;
                }
            case MathOperandOperator<decimal> op:
                {
                    i += entity.Key.Length;
                    var result = op.IsProcessingLeft
                        ? op.Fn(value)
                        : op.Fn(EvaluateOperand(mathString, ref i, defaultValue, separator, closingSymbol));
                    value = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<decimal> op:
                {
                    i += entity.Key.Length;
                    var right = EvaluateOperand(mathString, ref i, defaultValue, separator, closingSymbol);
                    right = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, right);
                    value = op.Fn(value, right);
                    return true;
                }
            case MathOperator<decimal> op:
                {
                    i += entity.Key.Length;
                    var right = Evaluate(mathString, ref i, defaultValue, separator, closingSymbol, op.Precedence);
                    value = op.Fn(value, right);
                    return true;
                }
            case MathGetValueFunction<decimal> func:
                {
                    i += entity.Key.Length;
                    mathString.SkipParenthesis(ref i);

                    var result = func.Fn();
                    result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, func.Fn());
                    value = value.IsDefault ? result : value * result;
                    return true;
                }
            case MathUnaryFunction<decimal> func:
                {
                    i += entity.Key.Length;
                    if (func.OpeningSymbol.HasValue)
                        mathString.ThrowExceptionIfNotOpened(func.OpeningSymbol.Value, start, ref i);

                    var arg = func.ClosingSymbol.HasValue
                        ? Evaluate(mathString, ref i, defaultValue, null, func.ClosingSymbol)
                        : EvaluateOperand(mathString, ref i, defaultValue, separator, closingSymbol);

                    if (func.ClosingSymbol.HasValue)
                        mathString.ThrowExceptionIfNotClosed(func.ClosingSymbol.Value, start, ref i);

                    var result = func.Fn(arg);
                    result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, result);
                    value = value.IsDefault ? result : value * result;
                    return true;
                }
            case MathFunction<decimal> func:
                {
                    i += entity.Key.Length;
                    mathString.ThrowExceptionIfNotOpened(func.OpeningSymbol, start, ref i);

                    var args = new List<decimal>();
                    while (mathString.Length > i)
                    {
                        var arg = Evaluate(mathString, ref i, defaultValue, func.Separator, func.ClosingSymbol);
                        args.Add(arg);

                        if (mathString[i] == func.Separator)
                        {
                            i++; //other param
                            continue;
                        }
                        break;
                    }

                    mathString.ThrowExceptionIfNotClosed(func.ClosingSymbol, start, ref i);

                    var result = func.Fn([.. args]);
                    result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, result);
                    value = value.IsDefault ? result : value * result;
                    return true;
                }
            default:
                return false;
        }
    }
}