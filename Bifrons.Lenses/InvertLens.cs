﻿namespace Bifrons.Lenses;

/// <summary>
/// Describes an invert lens. L : R <=> L
/// </summary>
public class InvertLens<TLeft, TRight> : ISymmetricLens<TLeft, TRight>
{
    private readonly ISymmetricLens<TRight, TLeft> _originalLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    internal InvertLens(ISymmetricLens<TRight, TLeft> originalLens)
    {
        _originalLens = originalLens;
    }

    public Func<TRight, Option<TLeft>, Result<TLeft>> PutLeft => _originalLens.PutRight;

    public Func<TLeft, Option<TRight>, Result<TRight>> PutRight => _originalLens.PutLeft;

    public Func<TLeft, Result<TRight>> CreateRight => _originalLens.CreateLeft;

    public Func<TRight, Result<TLeft>> CreateLeft => _originalLens.CreateRight;
}

/// <summary>
/// Static class for creating invert lenses
/// </summary>
public static class InvertLens
{
    /// <summary>
    /// Constructs an invert lens
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    public static InvertLens<TRight, TLeft> Cons<TRight, TLeft>(ISymmetricLens<TLeft, TRight> originalLens)
        => new(originalLens);
}