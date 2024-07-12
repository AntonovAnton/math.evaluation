﻿using System;
using System.Runtime.CompilerServices;
using MathEvaluation.Context;

namespace MathEvaluation;

public static class StringExtensions
{
    public static MathEvaluator SetContext(this string expression, IMathContext context)
    {
        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator Bind(this string expression, object args)
    {
        var context = new MathContext();
        context.Bind(args);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindVariable(this string expression, double value, char key)
        => BindVariable(expression, value, key.ToString());

    public static MathEvaluator BindVariable(this string expression, double value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        var context = new MathContext();
        context.BindVariable(value, key);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindVariable(this string expression, Func<double> getValue, char key)
        => BindVariable(expression, getValue, key.ToString());

    public static MathEvaluator BindVariable(this string expression, Func<double> getValue,
        [CallerArgumentExpression(nameof(getValue))] string? key = null)
    {
        var context = new MathContext();
        context.BindVariable(getValue, key);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<double, double> fn, char key)
        => BindFunction(expression, fn, key.ToString());

    public static MathEvaluator BindFunction(this string expression, Func<double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        var context = new MathContext();
        context.BindFunction(fn, key);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<double, double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<double, double, double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<double[], double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindVariable(this string expression, decimal value, char key)
        => BindVariable(expression, value, key.ToString());

    public static MathEvaluator BindVariable(this string expression, decimal value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        var context = new MathContext();
        context.BindVariable(value, key);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindVariable(this string expression, Func<decimal> getValue, char key)
        => BindVariable(expression, getValue, key.ToString());

    public static MathEvaluator BindVariable(this string expression, Func<decimal> getValue,
        [CallerArgumentExpression(nameof(getValue))] string? key = null)
    {
        var context = new MathContext();
        context.BindVariable(getValue, key);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal> fn, char key)
        => BindFunction(expression, fn, key.ToString());

    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        var context = new MathContext();
        context.BindFunction(fn, key);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<decimal, decimal, decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static MathEvaluator BindFunction(this string expression, Func<decimal[], decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        var context = new MathContext();
        context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);

        return new MathEvaluator(expression)
        {
            Context = context
        };
    }

    public static double Evaluate(this string expression, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(expression, provider);

    public static double Evaluate(this string expression, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(expression, context, provider);

    public static double Evaluate(this ReadOnlySpan<char> span, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(span, provider);

    public static double Evaluate(this ReadOnlySpan<char> span, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.Evaluate(span, context, provider);

    public static decimal EvaluateDecimal(this string expression, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(expression, provider);

    public static decimal EvaluateDecimal(this string expression, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(expression, context, provider);

    public static decimal EvaluateDecimal(this ReadOnlySpan<char> span, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(span, provider);

    public static decimal EvaluateDecimal(this ReadOnlySpan<char> span, IMathContext? context, IFormatProvider? provider = null) =>
        MathEvaluator.EvaluateDecimal(span, context, provider);
}