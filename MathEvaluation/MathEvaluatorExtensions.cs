using System;
using System.Runtime.CompilerServices;
using MathEvaluation.Context;

namespace MathEvaluation;

public static class MathEvaluatorExtensions
{
    public static MathEvaluator Bind(this MathEvaluator evaluator, object args)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.Bind(args);
        return evaluator;
    }

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, double value, char key)
        => BindVariable(evaluator, value, key.ToString());

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, double value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(value, key);
        return evaluator;
    }

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, Func<double> getValue, char key)
        => BindVariable(evaluator, getValue, key.ToString());

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, Func<double> getValue,
        [CallerArgumentExpression(nameof(getValue))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(getValue, key);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double> fn, char key)
        => BindFunction(evaluator, fn, key.ToString());

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double, double, double, double, double, double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<double[], double> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, decimal value, char key)
        => BindVariable(evaluator, value, key.ToString());

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, decimal value,
        [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(value, key);
        return evaluator;
    }

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, Func<decimal> getValue, char key)
        => BindVariable(evaluator, getValue, key.ToString());

    public static MathEvaluator BindVariable(this MathEvaluator evaluator, Func<decimal> getValue,
        [CallerArgumentExpression(nameof(getValue))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindVariable(getValue, key);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal> fn, char key)
        => BindFunction(evaluator, fn, key.ToString());

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal, decimal, decimal, decimal, decimal, decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }

    public static MathEvaluator BindFunction(this MathEvaluator evaluator, Func<decimal[], decimal> fn,
        [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
    {
        evaluator.Context ??= new MathContext();
        evaluator.Context.BindFunction(fn, key, openingSymbol, separator, closingSymbol);
        return evaluator;
    }
}