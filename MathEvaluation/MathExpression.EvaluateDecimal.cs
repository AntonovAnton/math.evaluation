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
            var i = 0;
            return EvaluateDecimal(MathString, ref i, null, null, (int)EvalPrecedence.Unknown);
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }

    private decimal EvaluateDecimal(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand = false, decimal value = default)
    {
        var start = i;
        while (mathString.Length > i)
        {
            if (separator.HasValue && mathString.IsParamsSeparator(start, i, separator.Value, _decimalSeparator) ||
                closingSymbol.HasValue && mathString[i] == closingSymbol.Value)
            {
                if (value == default)
                    mathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return value;
            }

            if (mathString[i] is >= '0' and <= '9' || mathString[i] == _decimalSeparator) //number
            {
                if (isOperand)
                    return EvaluateDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = mathString.ParseDecimalNumber(_numberFormat, ref i);
                continue;
            }

            switch (mathString[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var startParenthesis = i;
                    i++;
                    var result = EvaluateDecimal(mathString, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    mathString.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiationDecimal(mathString, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    break;
                case '+' when mathString.Length == i + 1 || mathString[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return value;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    value += EvaluateDecimal(mathString, ref i, separator, closingSymbol, p, isOperand);
                    if (isOperand)
                        return value;
                    break;
                case '-' when mathString.Length == i + 1 || mathString[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return value;

                    var isNegativity = start == i;
                    i++;
                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    result = EvaluateDecimal(mathString, ref i, separator, closingSymbol, p, isOperand);
                    value = isNegativity ? -result : value - result; //it keeps sign
                    if (isOperand)
                        return value;
                    break;
                case '*' when mathString.Length == i + 1 || mathString[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= EvaluateDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when mathString.Length == i + 1 || mathString[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= EvaluateDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = FirstMathEntity(mathString[i..]);
                    if (entity == null && mathString.TryParseCurrency(_numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    value = EvaluateMathEntityDecimal(mathString, ref i, separator, closingSymbol, precedence, value, entity);
                    if (isOperand)
                        return value;
                    break;
            }
        }

        if (value == default)
            mathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return value;
    }

    private decimal EvaluateOperandDecimal(ReadOnlySpan<char> mathString, ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = EvaluateDecimal(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (value == default)
            mathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return value;
    }

    private decimal EvaluateMathEntityDecimal(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, int precedence, decimal value, IMathEntity? entity = null, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryEvaluateEntityDecimal(mathString, entity, ref i, separator, closingSymbol, ref value))
                return value;

            var doubleValue = (double)value;
            if (TryEvaluateEntity(mathString, entity, ref i, separator, closingSymbol, ref doubleValue))
                return (decimal)doubleValue;
        }

        if (throwError)
            mathString.ThrowExceptionInvalidToken(i);

        return value;
    }

    private decimal EvaluateExponentiationDecimal(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, decimal value)
    {
        mathString.SkipMeaningless(ref i);
        if (mathString.Length <= i)
            return value;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = FirstMathEntity(mathString[i..]);
        return EvaluateMathEntityDecimal(mathString, ref i, separator, closingSymbol, precedence, value, entity, false);
    }

    private bool TryEvaluateEntityDecimal(ReadOnlySpan<char> mathString, IMathEntity entity,
        ref int i, char? separator, char? closingSymbol, ref decimal value)
    {
        var start = i;
        switch (entity)
        {
            case MathConstant<decimal> mathConstant:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiationDecimal(mathString, ref i, separator, closingSymbol, mathConstant.Value);
                    value = value == default ? result : value * result;
                    return true;
                }
            case MathVariable<decimal> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiationDecimal(mathString, ref i, separator, closingSymbol, mathVariable.Value);
                    value = value == default ? result : value * result;
                    return true;
                }
            case MathOperandOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var result = mathOperator.IsProcessingLeft
                        ? fn(value)
                        : fn(EvaluateOperandDecimal(mathString, ref i, separator, closingSymbol));
                    value = EvaluateExponentiationDecimal(mathString, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateOperandDecimal(mathString, ref i, separator, closingSymbol);
                    right = EvaluateExponentiationDecimal(mathString, ref i, separator, closingSymbol, right);
                    value = fn(value, right);
                    return true;
                }
            case MathOperator<decimal> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateDecimal(mathString, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, right);
                    return true;
                }
            case MathGetValueFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.SkipParenthesis(ref i);
                    var result = mathFunction.Fn();
                    result = EvaluateExponentiationDecimal(mathString, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    return true;
                }
            case MathUnaryFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    if (mathFunction.OpeningSymbol.HasValue)
                        mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol.Value, start, ref i);

                    var arg = mathFunction.ClosingSymbol.HasValue
                        ? EvaluateDecimal(mathString, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown)
                        : EvaluateOperandDecimal(mathString, ref i, separator, closingSymbol);

                    if (mathFunction.ClosingSymbol.HasValue)
                        mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol.Value, start, ref i);

                    var result = mathFunction.Fn(arg);
                    result = EvaluateExponentiationDecimal(mathString, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    return true;
                }
            case MathFunction<decimal> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol, start, ref i);

                    var args = new List<decimal>();
                    while (mathString.Length > i)
                    {
                        var arg = EvaluateDecimal(mathString, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
                        args.Add(arg);

                        if (mathString[i] == mathFunction.Separator)
                        {
                            i++; //other param
                            continue;
                        }
                        break;
                    }

                    mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol, start, ref i);

                    var result = mathFunction.Fn([.. args]);
                    result = EvaluateExponentiationDecimal(mathString, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    return true;
                }
            default:
                return false;
        }
    }
}