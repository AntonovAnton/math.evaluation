using System;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Context;

public interface IMathContext
{
    public const char DefaultParamsSeparator = ',';
    public const char DefaultOpeningSymbol = '(';
    public const char DefaultClosingSymbol = ')';

    void Bind(object args);

    void BindVariable(double value, char key);

    void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null);

    void BindVariable(Func<double> getValue, char key);

    void BindVariable(Func<double> getValue, [CallerArgumentExpression(nameof(getValue))] string? key = null);

    void BindFunction(Func<double, double> value, char key);

    void BindFunction(Func<double, double> value, [CallerArgumentExpression(nameof(value))] string? key = null);

    void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = DefaultOpeningSymbol, char separator = DefaultParamsSeparator, char closingSymbol = DefaultClosingSymbol);

    internal IMathEntity? FindMathEntity(ReadOnlySpan<char> expression);
}
