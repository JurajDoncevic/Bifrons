using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Strings;

/// <summary>
/// Describes a lens union combinator.
/// </summary>
public sealed class OrLens
    : ISymmetricLens<Either<string, string>, Either<string, string>>
{
    private readonly SymmetricStringLens _lhsLens;
    private readonly SymmetricStringLens _rhsLens;

    /// <summary>
    /// Regex describing the strings on the left-hand side of the lens.
    /// </summary>
    public Regex LeftRegex => new Regex(_lhsLens.LeftRegex.ToString() + "|" + _rhsLens.LeftRegex.ToString());

    /// <summary>
    /// Regex describing the strings on the right-hand side of the lens.
    /// </summary>
    public Regex RightRegex => new Regex(_lhsLens.RightRegex.ToString() + "|" + _rhsLens.RightRegex.ToString());

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

    public Func<Either<string, string>, Option<Either<string, string>>, Result<Either<string, string>>> PutLeft =>
        (updatedSource, originalTarget) =>
            updatedSource.Map(
                leftLensRight => _lhsLens.PutLeft(leftLensRight, originalTarget.Map(l => l.Left)),
                rightLensRight => _rhsLens.PutLeft(rightLensRight, originalTarget.Map(r => r.Right))
            ).Unfold();

    public Func<Either<string, string>, Option<Either<string, string>>, Result<Either<string, string>>> PutRight =>
        (updatedSource, originalTarget) =>
            updatedSource.Map(
                leftLensLeft => _lhsLens.PutRight(leftLensLeft, originalTarget.Map(l => l.Left)),
                rightLensLeft => _rhsLens.PutRight(rightLensLeft, originalTarget.Map(r => r.Right))
            ).Unfold();

    public Func<Either<string, string>, Result<Either<string, string>>> CreateRight =>
        source => source.Map(
            leftLensLeft => _lhsLens.CreateRight(leftLensLeft),
            rightLensLeft => _rhsLens.CreateRight(rightLensLeft)
        ).Unfold();

    public Func<Either<string, string>, Result<Either<string, string>>> CreateLeft =>
        source => source.Map(
            leftLensRight => _lhsLens.CreateLeft(leftLensRight),
            rightLensRight => _rhsLens.CreateLeft(rightLensRight)
        ).Unfold();

    /// <summary>
    /// Checks if the source string completely matches the left-side operand lenses left-side regex.
    /// </summary>
    internal bool IsLhsLeftRegexMatch(string source)
        => _lhsLens.LeftRegex.Match(source).Value.Equals(source);

    /// <summary>
    /// Checks if the source string completely matches the left-side operand lenses right-side regex.
    /// </summary>
    internal bool IsLhsRightRegexMatch(string source)
        => _lhsLens.RightRegex.Match(source).Value.Equals(source);

    /// <summary>
    /// Checks if the source string completely matches the right-side operand lenses left-side regex.
    /// </summary>
    internal bool IsRhsLeftRegexMatch(string source)
        => _rhsLens.LeftRegex.Match(source).Value.Equals(source);

    /// <summary>
    /// Checks if the source string completely matches the right-side operand lenses right-side regex.
    /// </summary>
    internal bool IsRhsRightRegexMatch(string source)
        => _rhsLens.RightRegex.Match(source).Value.Equals(source);

    /// <summary>
    /// Constructs a new lens that is the union of two lenses.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static OrLens Cons(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => new(lhsLens, rhsLens);
}
