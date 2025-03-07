﻿namespace Bifrons.Lenses;

/// <summary>
/// Describes a lens union combinator.
/// </summary>
public sealed class OrLens<TLeftSource, TRightSource, TLeftTarget, TRightTarget>
    : ISymmetricLens<Either<TLeftSource, TRightSource>, Either<TLeftTarget, TRightTarget>>
{
    private readonly ISymmetricLens<TLeftSource, TLeftTarget> _lhsLens;
    private readonly ISymmetricLens<TRightSource, TRightTarget> _rhsLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="lhsLens">Left-hand side lens operand</param>
    /// <param name="rhsLens">Right-hand side lens operand</param>
    internal OrLens(ISymmetricLens<TLeftSource, TLeftTarget> lhsLens, ISymmetricLens<TRightSource, TRightTarget> rhsLens)
    {
        _lhsLens = lhsLens;
        _rhsLens = rhsLens;
    }

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    public Func<Either<TLeftTarget, TRightTarget>, Option<Either<TLeftSource, TRightSource>>, Result<Either<TLeftSource, TRightSource>>> PutLeft =>
        (updatedSource, originalTarget) =>
            updatedSource.Map(
                leftLensRight => _lhsLens.PutLeft(leftLensRight, originalTarget.Map(l => l.Left)),
                rightLensRight => _rhsLens.PutLeft(rightLensRight, originalTarget.Map(r => r.Right))
            ).Unfold();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    public Func<Either<TLeftSource, TRightSource>, Option<Either<TLeftTarget, TRightTarget>>, Result<Either<TLeftTarget, TRightTarget>>> PutRight =>
        (updatedSource, originalTarget) =>
            updatedSource.Map(
                leftLensLeft => _lhsLens.PutRight(leftLensLeft, originalTarget.Map(l => l.Left)),
                rightLensLeft => _rhsLens.PutRight(rightLensLeft, originalTarget.Map(r => r.Right))
            ).Unfold();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    public Func<Either<TLeftSource, TRightSource>, Result<Either<TLeftTarget, TRightTarget>>> CreateRight =>
        source => source.Map(
            leftLensLeft => _lhsLens.CreateRight(leftLensLeft),
            rightLensLeft => _rhsLens.CreateRight(rightLensLeft)
        ).Unfold();

    public Func<Either<TLeftTarget, TRightTarget>, Result<Either<TLeftSource, TRightSource>>> CreateLeft =>
        source => source.Map(
            leftLensRight => _lhsLens.CreateLeft(leftLensRight),
            rightLensRight => _rhsLens.CreateLeft(rightLensRight)
        ).Unfold();
}

/// <summary>
/// Extension methods for a lens union combinator.
/// </summary>
public static class OrLens
{
    /// <summary>
    /// Constructs a new lens that is the union of two lenses.
    /// </summary>
    /// <param name="leftLens">Left-hand side lens operand</param>
    /// <param name="rightLens">Right-hand side lens operand</param>
    public static OrLens<TLeftSource, TRightSource, TLeftTarget, TRightTarget> Cons<TLeftSource, TRightSource, TLeftTarget, TRightTarget>(
        ISymmetricLens<TLeftSource, TLeftTarget> leftLens,
        ISymmetricLens<TRightSource, TRightTarget> rightLens)
        => new(leftLens, rightLens);
}
