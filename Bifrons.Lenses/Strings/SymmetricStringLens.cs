﻿using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Strings;

/// <summary>
/// Abstract class describing a simple symmetric lens between two strings.
/// <c>L : string <=> string</c>
/// </summary>
public abstract class SymmetricStringLens : ISymmetricLens<string, string>
{
    /// <summary>
    /// Regex describing the strings on the left-hand side of the lens.
    /// </summary>
    public abstract Regex LeftRegex { get; }
    /// <summary>
    /// Regex describing the strings on the right-hand side of the lens.
    /// </summary>
    public abstract Regex RightRegex { get; }

    public abstract Func<string, Option<string>, Result<string>> PutLeft { get; }
    public abstract Func<string, Option<string>, Result<string>> PutRight { get; }
    public abstract Func<string, Result<string>> CreateRight { get; }
    public abstract Func<string, Result<string>> CreateLeft { get; }

    /// <summary>
    /// Concatenates two simple symmetric string lenses. Lens regexes have to take into account the preceding lens regexes.
    /// e.g. <c>id(\w+)</c> | <c>id((?!\w+\s)\w+)</c> is a valid lens, but <c>id(\w+)</c> | <c>id(\w+)</c> is not.
    /// </summary>
    /// <param name="lhsLens">Left-hand operand lens</param>
    /// <param name="rhsLens">Right-hand operand lens</param>
    /// <returns>SymmetricStringLens</returns>
    public static SymmetricStringLens operator &(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => Combinators.Concat(lhsLens, rhsLens);

    /// <summary>
    /// Inverts the lens
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    public static SymmetricStringLens operator !(SymmetricStringLens originalLens)
        => Combinators.Invert(originalLens);

    /// <summary>
    /// Composition of two simple symmetric string lenses.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static SymmetricStringLens operator >>(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => Combinators.Compose(lhsLens, rhsLens);

    /// <summary>
    /// Creates a new lens that is the union of two lenses.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static OrLens operator |(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => Combinators.Or(lhsLens, rhsLens);

    /// <summary>
    /// Creates a string lens that iterates over a string and applies a lens to each item according to the constant separator.
    /// </summary>
    /// <param name="lhsRegexString">String for separator - CONSTANT</param>
    /// <param name="rhsLens">Lens to be applied on each item</param>
    public static IterateLens operator *(string lhsSeparatorString, SymmetricStringLens rhsLens)
        => Combinators.Iterate(lhsSeparatorString, rhsLens);


    /// <summary>
    /// Implicitly merges an OrLens into a SymmetricStringLens (MergeLens).
    /// </summary>
    /// <param name="orLens">OrLens to merge</param>
    public static implicit operator SymmetricStringLens(OrLens orLens)
        => Combinators.Merge(orLens);

}
