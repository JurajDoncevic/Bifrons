﻿namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Combinators for simple symmetric lenses.
/// </summary>
public static class Combinators
{
    /// <summary>
    /// Constructs an invert lens
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <param name="lens">Original lens</param>
    public static InvertLens<TRight, TLeft> Invert<TLeft, TRight>(this BaseSymmetricLens<TLeft, TRight> lens)
        => InvertLens.Cons(lens);

    /// <summary>
    /// Constructs a sequentially composed lens.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static BaseSymmetricLens<TLeft, TRight> Compose<TLeft, TMid, TRight>(
        this BaseSymmetricLens<TLeft, TMid> lhsLens,
        BaseSymmetricLens<TMid, TRight> rhsLens)
        => ComposeLens.Cons(lhsLens, rhsLens);

    /// <summary>
    /// Constructs an or lens from two lenses.
    /// </summary>
    /// <typeparam name="TLeftSource"></typeparam>
    /// <typeparam name="TRightSource"></typeparam>
    /// <typeparam name="TLeftTarget"></typeparam>
    /// <typeparam name="TRightTarget"></typeparam>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static OrLens<TLeftSource, TRightSource, TLeftTarget, TRightTarget> Or<TLeftSource, TRightSource, TLeftTarget, TRightTarget>(
        this BaseSymmetricLens<TLeftSource, TLeftTarget> lhsLens,
        BaseSymmetricLens<TRightSource, TRightTarget> rhsLens)
        => OrLens.Cons(lhsLens, rhsLens);


    /// <summary>
    /// Constructs an iterate lens from a an item lens
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static IterateLens<TLeft, TRight> Iterate<TLeft, TRight>(
        BaseSymmetricLens<TLeft, TRight> itemLens)
        => IterateLens.Cons(itemLens);
}
