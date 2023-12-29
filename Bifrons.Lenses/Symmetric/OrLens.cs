namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Describes a lens union combinator.
/// </summary>
public sealed class OrLens<TLeftSource, TRightSource, TLeftTarget, TRightTarget>
    : BaseSymmetricLens<Either<TLeftSource, TRightSource>, Either<TLeftTarget, TRightTarget>>
{
    private readonly BaseSymmetricLens<TLeftSource, TLeftTarget> _lhsLens;
    private readonly BaseSymmetricLens<TRightSource, TRightTarget> _rhsLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="lhsLens">Left-hand side lens operand</param>
    /// <param name="rhsLens">Right-hand side lens operand</param>
    internal OrLens(BaseSymmetricLens<TLeftSource, TLeftTarget> lhsLens, BaseSymmetricLens<TRightSource, TRightTarget> rhsLens)
    {
        _lhsLens = lhsLens;
        _rhsLens = rhsLens;
    }

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    public override Func<Either<TLeftTarget, TRightTarget>, Option<Either<TLeftSource, TRightSource>>, Result<Either<TLeftSource, TRightSource>>> PutLeft =>
        (updatedRight, originalLeft) =>
            updatedRight.Map(
                leftLensRight => _lhsLens.PutLeft(leftLensRight, originalLeft.Map(l => l.Left)),
                rightLensRight => _rhsLens.PutLeft(rightLensRight, originalLeft.Map(r => r.Right))
            ).Unfold();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    public override Func<Either<TLeftSource, TRightSource>, Option<Either<TLeftTarget, TRightTarget>>, Result<Either<TLeftTarget, TRightTarget>>> PutRight =>
        (updatedLeft, originalRight) =>
            updatedLeft.Map(
                leftLensLeft => _lhsLens.PutRight(leftLensLeft, originalRight.Map(l => l.Left)),
                rightLensLeft => _rhsLens.PutRight(rightLensLeft, originalRight.Map(r => r.Right))
            ).Unfold();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    public override Func<Either<TLeftSource, TRightSource>, Result<Either<TLeftTarget, TRightTarget>>> CreateRight =>
        left => left.Map(
            leftLensLeft => _lhsLens.CreateRight(leftLensLeft),
            rightLensLeft => _rhsLens.CreateRight(rightLensLeft)
        ).Unfold();

    public override Func<Either<TLeftTarget, TRightTarget>, Result<Either<TLeftSource, TRightSource>>> CreateLeft =>
        right => right.Map(
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
        BaseSymmetricLens<TLeftSource, TLeftTarget> leftLens,
        BaseSymmetricLens<TRightSource, TRightTarget> rightLens)
        => new(leftLens, rightLens);
}
