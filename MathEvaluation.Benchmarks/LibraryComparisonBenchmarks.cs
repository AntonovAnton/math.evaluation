using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using MathEvaluation.Compilation;
using MathEvaluation.Parameters;
using NCalc.LambdaCompilation;
using NoStringEvaluating.Models.Values;
using CalcNumber = CalcExpr.Expressions.Terminals.Number;
using XFuncParameters = xFunc.Maths.Expressions.Parameters.ExpressionParameters;

namespace MathEvaluation.Benchmarks;

/// <summary>
///     Shared setup for comparing MathEvaluator with other .NET expression libraries.
///     The expression uses all four arithmetic operators, operator precedence, nested parentheses,
///     decimal literals and variables, limited to the syntax that every compared library supports.
/// </summary>
public static class LibraryComparison
{
    public const string MathString = "(a * b + c) / (a - b) * 2.5 - c / (b + 0.75) + 1000";

    public const double A = 5;
    public const double B = 10;
    public const double C = 60.5;

    public static readonly double ExpectedResult = UniqueExpectedResult(1000);

    /// <summary>
    ///     Unique math expression strings, so that no library can be served from a cache of parsed expressions.
    ///     Some caches can not be disabled: Jace reuses compiled code for a repeated math expression string even with its cache turned off.
    ///     The pool is large enough that the slow libraries (Flee, Jace) never wrap around within a benchmark run.
    /// </summary>
    public const int UniqueMathStringsCount = 100_000;

    public static string UniqueMathString(long constant) => $"(a * b + c) / (a - b) * 2.5 - c / (b + 0.75) + {constant}";

    public static double UniqueExpectedResult(long constant) => (A * B + C) / (A - B) * 2.5 - C / (B + 0.75) + constant;

    public static string[] CreateUniqueMathStrings()
    {
        var mathStrings = new string[UniqueMathStringsCount];
        for (var i = 0; i < mathStrings.Length; i++)
            mathStrings[i] = UniqueMathString(1000 + i);

        return mathStrings;
    }

    public static void Verify(string library, double result, double expected)
    {
        if (Math.Abs(result - expected) > 1e-9 * Math.Max(1, Math.Abs(expected)))
            throw new InvalidOperationException($"{library}: unexpected result {result}, expected {expected}.");
    }

    public sealed class Variables
    {
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable InconsistentNaming
        public double a { get; set; } = A;
        public double b { get; set; } = B;
        public double c { get; set; } = C;
        // ReSharper restore InconsistentNaming
#pragma warning restore IDE1006 // Naming Styles
    }

    public static Dictionary<string, double> NewDictionary() => new() { ["a"] = A, ["b"] = B, ["c"] = C };

    public static Dictionary<string, EvaluatorValue> NewNoStringVariables() => new() { ["a"] = A, ["b"] = B, ["c"] = C };

    public static XFuncParameters NewXFuncParameters() => new() { { "a", A }, { "b", B }, { "c", C } };

    public static CalcExpr.Context.ExpressionContext NewCalcExprContext()
    {
        var context = new CalcExpr.Context.ExpressionContext();
        context.SetVariable("a", new CalcNumber(A));
        context.SetVariable("b", new CalcNumber(B));
        context.SetVariable("c", new CalcNumber(C));
        return context;
    }

    public static NTDLS.ExpressionParser.Expression NewNtdls(string mathString, NTDLS.ExpressionParser.ExpressionOptions? options = null)
    {
        var expression = new NTDLS.ExpressionParser.Expression(mathString, options);
        expression.SetParameter("a", A);
        expression.SetParameter("b", B);
        expression.SetParameter("c", C);
        return expression;
    }

    /// <summary>
    ///     MathosParser 2.0.0 is published as a non-optimized (Debug) build, which BenchmarkDotNet rejects by default.
    /// </summary>
    public sealed class Config : ManualConfig
    {
        public Config() => WithOptions(ConfigOptions.DisableOptimizationsValidator);
    }

    public static NCalc.Expression NewNCalc(string mathString, NCalc.ExpressionOptions options = NCalc.ExpressionOptions.None)
    {
        var expression = new NCalc.Expression(mathString, options);
        expression.Parameters["a"] = A;
        expression.Parameters["b"] = B;
        expression.Parameters["c"] = C;
        return expression;
    }
}

/// <summary>
///     Scenario 1: parse + evaluate. Every call parses a math expression string never seen before, sets the variables
///     and evaluates it once. Caches of parsed expressions are disabled. Results are per call.
/// </summary>
[Config(typeof(LibraryComparison.Config))]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByJob)]
[HideColumns("Error", "StdDev", "Median", "RatioSD", "Alloc Ratio")]
public class ParseAndEvaluateComparisonBenchmarks
{
    private const int CallsPerInvoke = 1000;

    private string[] _mathStrings = null!;
    private int _next;

    private readonly NTDLS.ExpressionParser.ExpressionOptions _ntdlsOptions = new() { UseCompileCache = false };
    private readonly Flee.PublicTypes.ExpressionContext _fleeContext = new();
    private readonly Jace.CalculationEngine _jaceEngine = new(new Jace.JaceOptions { CacheEnabled = false });
    private readonly xFunc.Maths.Processor _xFuncProcessor = new();
    private readonly NoStringEvaluating.Contract.INoStringEvaluator _noStringEvaluator = null!;
    private readonly NoStringEvaluating.Contract.IFormulaParser _noStringParser = null!;
    private readonly CalcExpr.Parsing.Parser _calcExprParser = new();
    private readonly Mathos.Parser.MathParser _mathosParser = new();

    public ParseAndEvaluateComparisonBenchmarks()
    {
        var facade = NoStringEvaluating.NoStringEvaluator.CreateFacade(_ => { });
        _noStringEvaluator = facade.Evaluator;
        _noStringParser = facade.FormulaParser;
    }

    [GlobalSetup]
    public void Setup()
    {
        _mathStrings = LibraryComparison.CreateUniqueMathStrings();

        // Each library must produce the right result.
        const long constant = 123456;
        var mathString = LibraryComparison.UniqueMathString(constant);
        var expected = LibraryComparison.UniqueExpectedResult(constant);
        LibraryComparison.Verify("MathEvaluator", MathEvaluator(mathString), expected);
        LibraryComparison.Verify("NTDLS.ExpressionParser", Ntdls(mathString), expected);
        LibraryComparison.Verify("NCalc", NCalc(mathString), expected);
        LibraryComparison.Verify("Flee", Flee(mathString), expected);
        LibraryComparison.Verify("Jace", Jace(mathString), expected);
        LibraryComparison.Verify("xFunc", XFunc(mathString), expected);
        LibraryComparison.Verify("NoStringEvaluating", NoString(mathString), expected);
        LibraryComparison.Verify("CalcExpr.NET", CalcExpr(mathString), expected);
        LibraryComparison.Verify("Mathos Parser", Mathos(mathString), expected);
    }

    private string NextMathString()
    {
        var mathString = _mathStrings[_next];
        if (++_next == _mathStrings.Length)
            _next = 0;

        return mathString;
    }

    [Benchmark(Baseline = true, OperationsPerInvoke = CallsPerInvoke, Description = "MathEvaluator")]
    public double MathEvaluator_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += MathEvaluator(NextMathString());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = CallsPerInvoke, Description = "NTDLS.ExpressionParser")]
    public double Ntdls_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += Ntdls(NextMathString());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = CallsPerInvoke, Description = "NCalc")]
    public double NCalc_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += NCalc(NextMathString());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = CallsPerInvoke, Description = "Flee")]
    public double Flee_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += Flee(NextMathString());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = CallsPerInvoke, Description = "Jace")]
    public double Jace_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += Jace(NextMathString());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = CallsPerInvoke, Description = "xFunc")]
    public double XFunc_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += XFunc(NextMathString());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = CallsPerInvoke, Description = "NoStringEvaluating")]
    public double NoString_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += NoString(NextMathString());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = CallsPerInvoke, Description = "CalcExpr.NET")]
    public double CalcExpr_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += CalcExpr(NextMathString());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = CallsPerInvoke, Description = "Mathos Parser")]
    public double Mathos_ParseAndEvaluate()
    {
        var sum = 0d;
        for (var i = 0; i < CallsPerInvoke; i++)
            sum += Mathos(NextMathString());
        return sum;
    }

    /// <summary>For a single evaluation MathEvaluator's interpreter is much faster than compiling.</summary>
    private static double MathEvaluator(string mathString)
    {
        var parameters = new MathParameters();
        parameters.BindVariable(LibraryComparison.A, "a");
        parameters.BindVariable(LibraryComparison.B, "b");
        parameters.BindVariable(LibraryComparison.C, "c");
        return new MathExpression(mathString).Evaluate(parameters);
    }

    private double Ntdls(string mathString)
        => LibraryComparison.NewNtdls(mathString, _ntdlsOptions).Evaluate() ?? double.NaN;

    private static double NCalc(string mathString)
        => Convert.ToDouble(LibraryComparison.NewNCalc(mathString, global::NCalc.ExpressionOptions.NoCache).Evaluate());

    /// <summary>Flee has no interpreter: every new expression is compiled to IL, which is JIT-compiled on first evaluation.</summary>
    private double Flee(string mathString)
    {
        _fleeContext.Variables["a"] = LibraryComparison.A;
        _fleeContext.Variables["b"] = LibraryComparison.B;
        _fleeContext.Variables["c"] = LibraryComparison.C;
        return _fleeContext.CompileGeneric<double>(mathString).Evaluate();
    }

    /// <summary>Jace compiles every new expression to a delegate.</summary>
    private double Jace(string mathString)
        => _jaceEngine.Build(mathString)(LibraryComparison.NewDictionary());

    private double XFunc(string mathString)
        => ((xFunc.Maths.Expressions.NumberValue)_xFuncProcessor.Parse(mathString).Execute(LibraryComparison.NewXFuncParameters())).Number;

    /// <summary>Parses with the FormulaParser directly, which bypasses the evaluator's formula cache.</summary>
    private double NoString(string mathString)
        => _noStringEvaluator.CalcNumber(_noStringParser.Parse(mathString), LibraryComparison.NewNoStringVariables());

    /// <summary>The parser caches parsed expressions by math expression string, so its cache is cleared before every parse.</summary>
    private double CalcExpr(string mathString)
    {
        _calcExprParser.ClearCache();
        return ((CalcNumber)_calcExprParser.Parse(mathString).Evaluate(LibraryComparison.NewCalcExprContext())).Value;
    }

    private double Mathos(string mathString)
    {
        _mathosParser.LocalVariables["a"] = LibraryComparison.A;
        _mathosParser.LocalVariables["b"] = LibraryComparison.B;
        _mathosParser.LocalVariables["c"] = LibraryComparison.C;
        return _mathosParser.Parse(mathString);
    }
}

/// <summary>
///     Scenario 2: evaluate many times. Each library parses (or compiles) the expression once, untimed,
///     then only the evaluation is measured. Results are per evaluation; evaluations are looped so that
///     a call of a compiled delegate (~1-2 ns) is not lost in the benchmark overhead.
/// </summary>
[Config(typeof(LibraryComparison.Config))]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByJob)]
[HideColumns("Error", "StdDev", "Median", "RatioSD", "Alloc Ratio")]
public class EvaluateComparisonBenchmarks
{
    private const int EvaluationsPerInvoke = 1000;

    private readonly LibraryComparison.Variables _variables = new();
    private readonly Dictionary<string, double> _dictionary = LibraryComparison.NewDictionary();
    private readonly Dictionary<string, EvaluatorValue> _noStringVariables = LibraryComparison.NewNoStringVariables();
    private readonly XFuncParameters _xFuncParameters = LibraryComparison.NewXFuncParameters();
    private readonly CalcExpr.Context.ExpressionContext _calcExprContext = LibraryComparison.NewCalcExprContext();

    private Func<LibraryComparison.Variables, double> _mathEvaluatorFn = null!;
    private Func<LibraryComparison.Variables, double> _mathEvaluatorFecFn = null!;
    private NTDLS.ExpressionParser.Expression _ntdls = null!;
    private NCalc.Expression _nCalc = null!;
    private Func<LibraryComparison.Variables, double> _nCalcLambda = null!;
    private Flee.PublicTypes.IGenericExpression<double> _flee = null!;
    private Func<IDictionary<string, double>, double> _jace = null!;
    private xFunc.Maths.Expressions.IExpression _xFunc = null!;
    private NoStringEvaluating.Contract.INoStringEvaluator _noStringEvaluator = null!;
    private NoStringEvaluating.Nodes.Common.FormulaNodes _noStringFormula = null!;
    private CalcExpr.Expressions.IExpression _calcExpr = null!;
    private Mathos.Parser.MathParser _mathosParser = null!;
    private System.Collections.ObjectModel.ReadOnlyCollection<string> _mathosTokens = null!;

    [GlobalSetup]
    public void Setup()
    {
        const string mathString = LibraryComparison.MathString;

        _mathEvaluatorFn = new MathExpression(mathString).Compile(_variables);
        _mathEvaluatorFecFn = new MathExpression(mathString, null, null, new FastMathExpressionCompiler()).Compile(_variables);

        _ntdls = LibraryComparison.NewNtdls(mathString);
        _nCalc = LibraryComparison.NewNCalc(mathString);
        _nCalcLambda = new NCalc.Expression(mathString).ToLambda<LibraryComparison.Variables, double>();

        var fleeContext = new Flee.PublicTypes.ExpressionContext();
        fleeContext.Variables["a"] = LibraryComparison.A;
        fleeContext.Variables["b"] = LibraryComparison.B;
        fleeContext.Variables["c"] = LibraryComparison.C;
        _flee = fleeContext.CompileGeneric<double>(mathString);

        _jace = new Jace.CalculationEngine().Build(mathString);
        _xFunc = new xFunc.Maths.Processor().Parse(mathString);

        var facade = NoStringEvaluating.NoStringEvaluator.CreateFacade(_ => { });
        _noStringEvaluator = facade.Evaluator;
        _noStringFormula = facade.FormulaParser.Parse(mathString);

        _calcExpr = new CalcExpr.Parsing.Parser().Parse(mathString);

        _mathosParser = new Mathos.Parser.MathParser();
        _mathosParser.LocalVariables["a"] = LibraryComparison.A;
        _mathosParser.LocalVariables["b"] = LibraryComparison.B;
        _mathosParser.LocalVariables["c"] = LibraryComparison.C;
        _mathosTokens = _mathosParser.GetTokens(mathString);

        // Each library must produce the right result.
        var expected = LibraryComparison.ExpectedResult * EvaluationsPerInvoke;
        LibraryComparison.Verify("MathEvaluator", MathEvaluator_Compiled(), expected);
        LibraryComparison.Verify("MathEvaluator.FastExpressionCompiler", MathEvaluator_FastExpressionCompiler_Compiled(), expected);
        LibraryComparison.Verify("NTDLS.ExpressionParser", Ntdls_Evaluate(), expected);
        LibraryComparison.Verify("NCalc", NCalc_Evaluate(), expected);
        LibraryComparison.Verify("NCalc.LambdaCompilation", NCalc_Lambda(), expected);
        LibraryComparison.Verify("Flee", Flee_Evaluate(), expected);
        LibraryComparison.Verify("Jace", Jace_Evaluate(), expected);
        LibraryComparison.Verify("xFunc", XFunc_Evaluate(), expected);
        LibraryComparison.Verify("NoStringEvaluating", NoString_Evaluate(), expected);
        LibraryComparison.Verify("CalcExpr.NET", CalcExpr_Evaluate(), expected);
        LibraryComparison.Verify("Mathos Parser", Mathos_Evaluate(), expected);
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Baseline = true, Description = "MathEvaluator")]
    public double MathEvaluator_Compiled()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += _mathEvaluatorFn(_variables);
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "MathEvaluator.FastExpressionCompiler")]
    public double MathEvaluator_FastExpressionCompiler_Compiled()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += _mathEvaluatorFecFn(_variables);
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "NTDLS.ExpressionParser")]
    public double Ntdls_Evaluate()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += _ntdls.Evaluate() ?? double.NaN;
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "NCalc")]
    public double NCalc_Evaluate()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += Convert.ToDouble(_nCalc.Evaluate());
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "NCalc.LambdaCompilation")]
    public double NCalc_Lambda()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += _nCalcLambda(_variables);
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "Flee")]
    public double Flee_Evaluate()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += _flee.Evaluate();
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "Jace")]
    public double Jace_Evaluate()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += _jace(_dictionary);
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "xFunc")]
    public double XFunc_Evaluate()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += ((xFunc.Maths.Expressions.NumberValue)_xFunc.Execute(_xFuncParameters)).Number;
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "NoStringEvaluating")]
    public double NoString_Evaluate()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += _noStringEvaluator.CalcNumber(_noStringFormula, _noStringVariables);
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "CalcExpr.NET")]
    public double CalcExpr_Evaluate()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += ((CalcNumber)_calcExpr.Evaluate(_calcExprContext)).Value;
        return sum;
    }

    [Benchmark(OperationsPerInvoke = EvaluationsPerInvoke, Description = "Mathos Parser")]
    public double Mathos_Evaluate()
    {
        var sum = 0d;
        for (var i = 0; i < EvaluationsPerInvoke; i++)
            sum += _mathosParser.Parse(_mathosTokens);
        return sum;
    }
}

/// <summary>
///     Scenario 3: compile cost. Every call compiles a math expression string never seen before into a delegate.
///     With <see cref="FirstCall" /> the delegate is also evaluated once, which includes JIT-compiling the generated code,
///     i.e. the real cost of a compiled expression before it starts paying off. Only libraries that compile are compared.
/// </summary>
[Config(typeof(LibraryComparison.Config))]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByJob, BenchmarkLogicalGroupRule.ByParams)]
[HideColumns("Error", "StdDev", "Median", "RatioSD", "Alloc Ratio")]
public class CompileComparisonBenchmarks
{
    private readonly LibraryComparison.Variables _variables = new();
    private readonly FastMathExpressionCompiler _fastCompiler = new();
    private readonly Flee.PublicTypes.ExpressionContext _fleeContext = new();
    private readonly Jace.CalculationEngine _jaceEngine = new(new Jace.JaceOptions { CacheEnabled = false });

    private string[] _mathStrings = null!;
    private int _next;
    private object? _compiled;

    [Params(false, true)]
    public bool FirstCall { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _mathStrings = LibraryComparison.CreateUniqueMathStrings();

        _fleeContext.Variables["a"] = LibraryComparison.A;
        _fleeContext.Variables["b"] = LibraryComparison.B;
        _fleeContext.Variables["c"] = LibraryComparison.C;

        // Each library must produce the right result.
        const long constant = 123456;
        var mathString = LibraryComparison.UniqueMathString(constant);
        var expected = LibraryComparison.UniqueExpectedResult(constant);
        LibraryComparison.Verify("MathEvaluator", CompileMathEvaluator(mathString)(_variables), expected);
        LibraryComparison.Verify("MathEvaluator.FastExpressionCompiler", CompileMathEvaluatorFec(mathString)(_variables), expected);
        LibraryComparison.Verify("NCalc.LambdaCompilation", CompileNCalc(mathString)(_variables), expected);
        LibraryComparison.Verify("Flee", CompileFlee(mathString).Evaluate(), expected);
        LibraryComparison.Verify("Jace", CompileJace(mathString)(LibraryComparison.NewDictionary()), expected);
    }

    private string NextMathString()
    {
        var mathString = _mathStrings[_next];
        if (++_next == _mathStrings.Length)
            _next = 0;

        return mathString;
    }

    /// <summary>Keeps the compiled delegate alive, so the compilation can not be eliminated.</summary>
    private double Keep(object compiled)
    {
        _compiled = compiled;
        return 0;
    }

    [Benchmark(Baseline = true, Description = "MathEvaluator")]
    public double MathEvaluator_Compile()
    {
        var fn = CompileMathEvaluator(NextMathString());
        return FirstCall ? fn(_variables) : Keep(fn);
    }

    [Benchmark(Description = "MathEvaluator.FastExpressionCompiler")]
    public double MathEvaluator_FastExpressionCompiler_Compile()
    {
        var fn = CompileMathEvaluatorFec(NextMathString());
        return FirstCall ? fn(_variables) : Keep(fn);
    }

    [Benchmark(Description = "NCalc.LambdaCompilation")]
    public double NCalc_ToLambda()
    {
        var fn = CompileNCalc(NextMathString());
        return FirstCall ? fn(_variables) : Keep(fn);
    }

    [Benchmark(Description = "Flee")]
    public double Flee_Compile()
    {
        var expression = CompileFlee(NextMathString());
        return FirstCall ? expression.Evaluate() : Keep(expression);
    }

    [Benchmark(Description = "Jace")]
    public double Jace_Build()
    {
        var fn = CompileJace(NextMathString());
        return FirstCall ? fn(LibraryComparison.NewDictionary()) : Keep(fn);
    }

    private Func<LibraryComparison.Variables, double> CompileMathEvaluator(string mathString)
        => new MathExpression(mathString).Compile(_variables);

    private Func<LibraryComparison.Variables, double> CompileMathEvaluatorFec(string mathString)
        => new MathExpression(mathString, null, null, _fastCompiler).Compile(_variables);

    private static Func<LibraryComparison.Variables, double> CompileNCalc(string mathString)
        => new NCalc.Expression(mathString, NCalc.ExpressionOptions.NoCache).ToLambda<LibraryComparison.Variables, double>();

    private Flee.PublicTypes.IGenericExpression<double> CompileFlee(string mathString)
        => _fleeContext.CompileGeneric<double>(mathString);

    private Func<IDictionary<string, double>, double> CompileJace(string mathString)
        => _jaceEngine.Build(mathString);
}
