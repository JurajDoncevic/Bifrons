namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Describes a lens union combinator.
/// </summary>
public sealed class OrLens
    : BaseSymmetricLens<Either<string, string>, Either<string, string>>
{
    private readonly SymmetricStringLens _lhsLens;
    private readonly SymmetricStringLens _rhsLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="lhsLens">Left-hand side lens operand</param>
    /// <param name="rhsLens">Right-hand side lens operand</param>
    private OrLens(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
    {
        _lhsLens = lhsLens;
        _rhsLens = rhsLens;
    }

    public override Func<Either<string, string>, Option<Either<string, string>>, Result<Either<string, string>>> PutLeft =>
        (updatedRight, originalLeft) =>
            updatedRight.Map(
                leftLensRight => _lhsLens.PutLeft(leftLensRight, originalLeft.Map(l => l.Left)),
                rightLensRight => _rhsLens.PutLeft(rightLensRight, originalLeft.Map(r => r.Right))
            ).Unfold();

    public override Func<Either<string, string>, Option<Either<string, string>>, Result<Either<string, string>>> PutRight =>
        (updatedLeft, originalRight) =>
            updatedLeft.Map(
                leftLensLeft => _lhsLens.PutRight(leftLensLeft, originalRight.Map(l => l.Left)),
                rightLensLeft => _rhsLens.PutRight(rightLensLeft, originalRight.Map(r => r.Right))
            ).Unfold();

    public override Func<Either<string, string>, Result<Either<string, string>>> CreateRight =>
        left => left.Map(
            leftLensLeft => _lhsLens.CreateRight(leftLensLeft),
            rightLensLeft => _rhsLens.CreateRight(rightLensLeft)
        ).Unfold();

    public override Func<Either<string, string>, Result<Either<string, string>>> CreateLeft =>
        right => right.Map(
            leftLensRight => _lhsLens.CreateLeft(leftLensRight),
            rightLensRight => _rhsLens.CreateLeft(rightLensRight)
        ).Unfold();

    /// <summary>
    /// Constructs a new lens that is the union of two lenses.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static OrLens Cons(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => new(lhsLens, rhsLens);
}
