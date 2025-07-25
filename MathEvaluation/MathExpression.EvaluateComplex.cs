﻿using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;
using System;
using System.Numerics;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Evaluate(object?)" />
    public Complex EvaluateComplex(object? parameters = null)
        => EvaluateComplex(parameters != null ? new MathParameters(parameters) : null);

    /// <inheritdoc cref="Evaluate(MathParameters?)" />
    public Complex EvaluateComplex(MathParameters? parameters)
    {
        _parameters = parameters;
        _evaluatingStep = 0;

        try
        {
            var i = 0;
            var value = EvaluateComplex(ref i, null, null);

            if (_evaluatingStep == 0)
                OnEvaluating(0, i, value);

            return value;
        }
        catch (Exception ex)
        {
            throw CreateException(ex, parameters);
        }
    }

    internal Complex EvaluateComplex(ref int i, char? separator, char? closingSymbol,
        int precedence = (int)EvalPrecedence.Unknown, bool isOperand = false)
    {
        var span = MathString.AsSpan();
        var value = default(Complex);

        var start = i;
        while (span.Length > i)
        {
            if ((separator.HasValue && IsParamSeparator(separator.Value, start, i)) ||
                (closingSymbol.HasValue && span[i] == closingSymbol.Value))
            {
                if (value == default)
                    MathString.ThrowExceptionIfNotEvaluated(true, start, i);

                return value;
            }

            if (span[i] is >= '0' and <= '9' || span[i] == _decimalSeparator || //the real part of a complex number.
                (span[i] is 'i' && (span.Length == i + 1 || !char.IsLetterOrDigit(span[i + 1])))) //the imaginary part of a complex number.
            {
                if (isOperand)
                    return EvaluateComplex(ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                var tokenPosition = i;
                value = span.ParseComplexNumber(_numberFormat, ref i);

                if (value.Imaginary != default)
                    OnEvaluating(tokenPosition, i, value);
                continue;
            }

            switch (span[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var tokenPosition = i;
                    i++;
                    var result = EvaluateComplex(ref i, null, ')');
                    MathString.ThrowExceptionIfNotClosed(')', tokenPosition, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiationComplex(tokenPosition, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;

                    if (value != result && !double.IsNaN(value.Real) && !double.IsNaN(value.Imaginary))
                        OnEvaluating(start, i, value);
                    break;
                case '+' when span.Length == i + 1 || span[i + 1] != '+':
                    if (isOperand || (precedence >= (int)EvalPrecedence.LowestBasic && !MathString.IsMeaningless(start, i)))
                        return value;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    value += EvaluateComplex(ref i, separator, closingSymbol, p, isOperand);

                    OnEvaluating(start, i, value);
                    if (isOperand)
                        return value;

                    break;
                case '-' when span.Length == i + 1 || span[i + 1] != '-':
                    var isMeaningless = MathString.IsMeaningless(start, i);
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !isMeaningless)
                        return value;

                    i++;
                    var numberPosition = i;

                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    result = EvaluateComplex(ref i, separator, closingSymbol, p, isOperand);

                    //it keeps sign of the part of the complex number, correct sign is important in complex analysis.
                    if (isMeaningless && span[numberPosition..i].IsComplexNumberPart(_numberFormat, out var isImaginaryPart))
                        value = isImaginaryPart ? Complex.Conjugate(result) : new Complex(-result.Real, result.Imaginary);
                    else
                        value -= result;

                    OnEvaluating(start, i, value);
                    if (isOperand)
                        return value;

                    break;
                case '*' when span.Length == i + 1 || span[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= EvaluateComplex(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);

                    OnEvaluating(start, i, value);
                    break;
                case '/' when span.Length == i + 1 || span[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= EvaluateComplex(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);

                    OnEvaluating(start, i, value);
                    break;
                case ' ' or '\t' or '\n' or '\r': //whitespace, tab, LF, or CR
                    i++;
                    break;
                default:
                    var entity = FirstMathEntity(span[i..]);
                    if (entity == null)
                    {
                        if (span.TryParseCurrency(_numberFormat, ref i))
                            break;

                        throw CreateExceptionInvalidToken(span, i);
                    }

                    //highest precedence is evaluating first
                    if (precedence >= entity.Precedence)
                        return value;

                    value = entity.Evaluate(this, start, ref i, separator, closingSymbol, value);

                    if (isOperand)
                        return value;

                    break;
            }
        }

        if (value == default)
            MathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return value;
    }

    internal Complex EvaluateOperandComplex(ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = EvaluateComplex(ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (value == default)
            MathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return value;
    }

    internal Complex EvaluateExponentiationComplex(int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        MathString.SkipMeaningless(ref i);
        if (MathString.Length <= i)
            return value;

        var entity = FirstMathEntity(MathString.AsSpan(i));
        return entity is { Precedence: >= (int)EvalPrecedence.Exponentiation }
            ? entity.Evaluate(this, start, ref i, separator, closingSymbol, value)
            : value;
    }
}