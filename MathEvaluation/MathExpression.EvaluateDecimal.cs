using System;
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
            return EvaluateDecimal(ref i, null, null);
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }

    internal decimal EvaluateDecimal(ref int i, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
    {
        var span = MathString.AsSpan();
        var value = default(decimal);

        var start = i;
        while (span.Length > i)
        {
            if (separator.HasValue && IsParamSeparator(separator.Value, start, i) ||
                closingSymbol.HasValue && span[i] == closingSymbol.Value)
            {
                if (value == default)
                    MathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return value;
            }

            if (span[i] is >= '0' and <= '9' || span[i] == _decimalSeparator) //number
            {
                if (isOperand)
                    return EvaluateDecimal(ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = span.ParseDecimalNumber(_numberFormat, ref i);
                continue;
            }

            switch (span[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var startParenthesis = i;
                    i++;
                    var result = EvaluateDecimal(ref i, null, ')');
                    MathString.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiationDecimal(ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsMeaningless(start, i))
                        return value;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    value += EvaluateDecimal(ref i, separator, closingSymbol, p, isOperand);
                    if (isOperand)
                        return value;
                    break;
                case '-' when span.Length == i + 1 || span[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsMeaningless(start, i))
                        return value;

                    var isNegativity = start == i;
                    i++;
                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    result = EvaluateDecimal(ref i, separator, closingSymbol, p, isOperand);
                    value = isNegativity ? -result : value - result; //it keeps sign
                    if (isOperand)
                        return value;
                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= EvaluateDecimal(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= EvaluateDecimal(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case ' ' or '\t' or '\n' or '\r': //space or tab or LF or CR
                    i++;
                    break;
                default:
                    var entity = FirstMathEntity(span[i..]);
                    if (entity == null && span.TryParseCurrency(_numberFormat, ref i))
                        break;

                    //highest precedence is evaluating first
                    if (precedence >= entity?.Precedence)
                        return value;

                    if (entity != null)
                        value = entity.Evaluate(this, ref i, separator, closingSymbol, value);
                    else
                        MathString.ThrowExceptionInvalidToken(i);

                    if (isOperand)
                        return value;
                    break;
            }
        }

        if (value == default)
            MathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return value;
    }

    internal decimal EvaluateOperandDecimal(ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = EvaluateDecimal(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (value == default)
            MathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return value;
    }

    internal decimal EvaluateExponentiationDecimal(ref int i, char? separator, char? closingSymbol, decimal value)
    {
        MathString.SkipMeaningless(ref i);
        if (MathString.Length <= i)
            return value;

        var entity = FirstMathEntity(MathString.AsSpan(i));
        if (entity != null && entity.Precedence >= (int)EvalPrecedence.Exponentiation)
            return entity.Evaluate(this, ref i, separator, closingSymbol, value);

        return value;
    }
}