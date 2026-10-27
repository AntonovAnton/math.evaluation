using MathEvaluation.Entities;
using System;
using System.Collections.Generic;

// ReSharper disable UnusedMember.Global

namespace MathEvaluation.Context;

/// <summary>
///     The composite math context combines several math contexts into one,
///     e.g. <see cref="ScientificMathContext" /> with a custom <see cref="MathContext" />.
///     The entities bound directly to the composite context are searched first, then the combined contexts in the order they were added.
///     When several entities match, the one with the longest key wins; on a tie, the one found first wins.
/// </summary>
public class CompositeMathContext : MathContext
{
    private readonly List<MathContext> _contexts = [];

    /// <summary>Initializes a new instance of the <see cref="CompositeMathContext" /> class.</summary>
    /// <param name="contexts">The math contexts to combine, in the order of their priority.</param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ArgumentException" />
    public CompositeMathContext(params MathContext[] contexts)
    {
        ArgumentNullException.ThrowIfNull(contexts);

        foreach (var context in contexts)
        {
            Add(context);
        }
    }

    /// <summary>Gets the combined math contexts.</summary>
    /// <value>The combined math contexts in the order of their priority.</value>
    public IReadOnlyList<MathContext> Contexts => _contexts;

    /// <summary>Adds the math context with the lowest priority.</summary>
    /// <param name="context">The math context.</param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ArgumentException">The context contains the composite context itself.</exception>
    public void Add(MathContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (Contains(context, this))
            throw new ArgumentException("The context cannot contain the composite context itself.", nameof(context));

        _contexts.Add(context);
    }

    /// <inheritdoc />
    internal override IMathEntity? FirstMathEntity(ReadOnlySpan<char> mathString)
    {
        var entity = base.FirstMathEntity(mathString);
        foreach (var context in _contexts)
        {
            var e = context.FirstMathEntity(mathString);
            if (e != null && (entity == null || e.Key.Length > entity.Key.Length))
                entity = e;
        }

        return entity;
    }

    private static bool Contains(MathContext context, MathContext target)
    {
        if (ReferenceEquals(context, target))
            return true;

        if (context is not CompositeMathContext composite)
            return false;

        foreach (var c in composite._contexts)
        {
            if (Contains(c, target))
                return true;
        }

        return false;
    }
}
