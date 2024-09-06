using System;
using System.Collections.Generic;
using System.Globalization;
using MathEvaluation.Context;
using MathEvaluation.Entities;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;

namespace MathEvaluation;

/// <summary>
/// Evaluates and compiles mathematical expressions from strings.
/// </summary>
public partial class MathExpression
{
    private readonly NumberFormatInfo? _numberFormat;
    private readonly char _decimalSeparator;

    private IMathParameters? _parameters;

    /// <summary>Gets the math expression string.</summary>
    /// <value>The math expression string.</value>
    public string MathString { get; }

    /// <summary>Gets the math context.</summary>
    /// <value>The instance of the <see cref="IMathContext"/> interface.</value>
    public IMathContext? Context { get; }

    /// <summary>Gets the specified format provider.</summary>
    /// <value>The specified format provider.</value>
    public IFormatProvider? Provider { get; }

    /// <summary>Initializes a new instance of the <see cref="MathExpression" /> class.</summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="context">The math context.</param>
    /// <param name="provider">The specified format provider.</param>
    /// <exception cref="System.ArgumentNullException">mathString</exception>
    /// <exception cref="System.ArgumentException">Expression string is empty or white space. - mathString</exception>
    public MathExpression(string mathString, IMathContext? context = null, IFormatProvider? provider = null)
    {
        if (mathString == null)
            throw new ArgumentNullException(nameof(mathString));

        if (string.IsNullOrWhiteSpace(mathString))
            throw new ArgumentException("Expression string is empty or white space.", nameof(mathString));

        MathString = mathString;
        Context = context;
        Provider = provider;

        _numberFormat = provider != null ? NumberFormatInfo.GetInstance(provider) : null;
        _decimalSeparator = _numberFormat?.NumberDecimalSeparator?.Length > 0 ? _numberFormat.NumberDecimalSeparator[0] : '.';
    }

    /// <inheritdoc cref="Evaluate(IMathParameters?)"/>
    /// <exception cref="NotSupportedException">parameters</exception>
    public double Evaluate(object? parameters = null)
        => Evaluate(parameters != null ? new MathParameters(parameters) : null);

    /// <summary>Evaluates the <see cref="MathString">math expression string</see>.</summary>
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <returns>Value of the math expression.</returns>
    /// <exception cref="MathExpressionException"/>
    public double Evaluate(IMathParameters? parameters)
    {
        _parameters = parameters;

        try
        {
            var i = 0;
            return Evaluate(MathString, ref i, null, null, (int)EvalPrecedence.Unknown);
        }
        catch (Exception ex)
        {
            throw CreateException(ex, MathString, Context, Provider, parameters);
        }
    }

    private double Evaluate(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, int precedence, bool isOperand = false, double value = default)
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
                    return Evaluate(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Function);

                value = mathString.ParseNumber(_numberFormat, ref i);
                continue;
            }

            switch (mathString[i])
            {
                case '(':
                    if (precedence >= (int)EvalPrecedence.Function)
                        return value;

                    var startParenthesis = i;
                    i++;
                    var result = Evaluate(mathString, ref i, null, ')', (int)EvalPrecedence.Unknown);
                    mathString.ThrowExceptionIfNotClosed(')', startParenthesis, ref i);
                    if (isOperand)
                        return result;

                    result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    break;
                case '+' when mathString.Length == i + 1 || mathString[i + 1] != '+':
                    if (isOperand || precedence >= (int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return value;

                    i++;
                    var p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    value += Evaluate(mathString, ref i, separator, closingSymbol, p, isOperand);
                    if (isOperand)
                        return value;
                    break;
                case '-' when mathString.Length == i + 1 || mathString[i + 1] != '-':
                    if (precedence >= (int)EvalPrecedence.LowestBasic && !mathString.IsMeaningless(start, i))
                        return value;

                    var isNegativity = start == i;
                    i++;
                    p = precedence > (int)EvalPrecedence.LowestBasic ? precedence : (int)EvalPrecedence.LowestBasic;
                    result = Evaluate(mathString, ref i, separator, closingSymbol, p, isOperand);
                    value = isNegativity ? -result : value - result; //it keeps sign
                    if (isOperand)
                        return value;
                    break;
                case '*' when mathString.Length == i + 1 || mathString[i + 1] != '*':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value *= Evaluate(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
                    break;
                case '/' when mathString.Length == i + 1 || mathString[i + 1] != '/':
                    if (precedence >= (int)EvalPrecedence.Basic)
                        return value;

                    i++;
                    value /= Evaluate(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic);
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

                    value = EvaluateMathEntity(mathString, ref i, separator, closingSymbol, precedence, value, entity);
                    if (isOperand)
                        return value;
                    break;
            }
        }

        if (value == default)
            mathString.ThrowExceptionIfNotEvaluated(isOperand, start, i);

        return value;
    }

    private double EvaluateOperand(ReadOnlySpan<char> mathString, ref int i, char? separator, char? closingSymbol)
    {
        var start = i;
        var value = Evaluate(mathString, ref i, separator, closingSymbol, (int)EvalPrecedence.Basic, true);
        if (value == default)
            mathString.ThrowExceptionIfNotEvaluated(true, start, i);

        return value;
    }

    private double EvaluateMathEntity(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, int precedence, double value, IMathEntity? entity, bool throwError = true)
    {
        if (entity != null && entity.Precedence >= precedence)
        {
            if (TryEvaluateEntity(mathString, entity, ref i, separator, closingSymbol, ref value))
                return value;

            var decimalValue = (decimal)value;
            if (TryEvaluateEntityDecimal(mathString, entity, ref i, separator, closingSymbol, ref decimalValue))
                return (double)decimalValue;
        }

        if (throwError)
            mathString.ThrowExceptionInvalidToken(i);

        return value;
    }

    private double EvaluateExponentiation(ReadOnlySpan<char> mathString,
        ref int i, char? separator, char? closingSymbol, double value)
    {
        mathString.SkipMeaningless(ref i);
        if (mathString.Length <= i)
            return value;

        var precedence = (int)EvalPrecedence.Exponentiation;
        var entity = FirstMathEntity(mathString[i..]);
        return EvaluateMathEntity(mathString, ref i, separator, closingSymbol, precedence, value, entity, false);
    }

    private bool TryEvaluateEntity(ReadOnlySpan<char> mathString, IMathEntity entity,
        ref int i, char? separator, char? closingSymbol, ref double value)
    {
        var start = i;
        switch (entity)
        {
            case MathConstant<double> mathConstant:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, mathConstant.Value);
                    value = value == default ? result : value * result;
                    return true;
                }
            case MathVariable<double> mathVariable:
                {
                    i += entity.Key.Length;
                    var result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, mathVariable.Value);
                    value = value == default ? result : value * result;
                    return true;
                }
            case MathOperandOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var result = mathOperator.IsProcessingLeft
                        ? fn(value)
                        : fn(EvaluateOperand(mathString, ref i, separator, closingSymbol));
                    value = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, result);
                    return true;
                }
            case MathOperandsOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = EvaluateOperand(mathString, ref i, separator, closingSymbol);
                    right = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, right);
                    value = fn(value, right);
                    return true;
                }
            case MathOperator<double> mathOperator:
                {
                    i += entity.Key.Length;
                    var fn = mathOperator.Fn;
                    var right = Evaluate(mathString, ref i, separator, closingSymbol, mathOperator.Precedence);
                    value = fn(value, right);
                    return true;
                }
            case MathGetValueFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.SkipParenthesis(ref i);

                    var result = mathFunction.Fn();
                    result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    return true;
                }
            case MathUnaryFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    if (mathFunction.OpeningSymbol.HasValue)
                        mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol.Value, start, ref i);

                    var arg = mathFunction.ClosingSymbol.HasValue
                        ? Evaluate(mathString, ref i, null, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown)
                        : EvaluateOperand(mathString, ref i, separator, closingSymbol);

                    if (mathFunction.ClosingSymbol.HasValue)
                        mathString.ThrowExceptionIfNotClosed(mathFunction.ClosingSymbol.Value, start, ref i);

                    var result = mathFunction.Fn(arg);
                    result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    return true;
                }
            case MathFunction<double> mathFunction:
                {
                    i += entity.Key.Length;
                    mathString.ThrowExceptionIfNotOpened(mathFunction.OpeningSymbol, start, ref i);

                    var args = new List<double>();
                    while (mathString.Length > i)
                    {
                        var arg = Evaluate(mathString, ref i, mathFunction.Separator, mathFunction.ClosingSymbol, (int)EvalPrecedence.Unknown);
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
                    result = EvaluateExponentiation(mathString, ref i, separator, closingSymbol, result);
                    value = value == default ? result : value * result;
                    return true;
                }
            default:
                return false;
        }
    }

    private static MathExpressionException CreateException(Exception ex,
        string mathString, IMathContext? context, IFormatProvider? provider, object? parameters)
    {
        ex = ex is not MathExpressionException ? new MathExpressionException(ex.Message, ex) : ex;
        ex.Data[nameof(mathString)] = mathString;
        ex.Data[nameof(context)] = context;
        ex.Data[nameof(provider)] = provider;
        ex.Data[nameof(parameters)] = parameters;
        throw ex;
    }

    private IMathEntity? FirstMathEntity(ReadOnlySpan<char> mathString)
        => Context?.FirstMathEntity(mathString) ?? _parameters?.FirstMathEntity(mathString);
}