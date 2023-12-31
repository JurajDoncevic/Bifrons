namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Concatenates two simple symmetric string lenses. Lens regexes have to take into account the preceding lens regexes of the concat.
/// e.g. <c>id(\w+) | id((?!\w+\s)\w+)</c> is a valid lens, but <c>id(\w+) | id(\w+)</c> is not.
/// </summary>
public sealed class ConcatLens : SymmetricStringLens
{
    /// <summary>
    /// Left-hand operand lens of the concat.
    /// </summary>
    private readonly SymmetricStringLens _leftLens;

    /// <summary>
    /// Right-hand operand lens of the concat.
    /// </summary>
    private readonly SymmetricStringLens _rightLens;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="leftLens">Left-hand operand lens of the concat.</param>
    /// <param name="rightLens">Right-hand operand lens of the concat.</param>
    private ConcatLens(SymmetricStringLens leftLens, SymmetricStringLens rightLens)
    {
        _leftLens = leftLens;
        _rightLens = rightLens;
    }

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (updatedRight, originalLeft) =>
        {
            var defaultLeft = originalLeft
                .Match(
                    res => res,
                    () => CreateLeft(updatedRight)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );
            var defaultRight = originalLeft
                .Match(
                    res => CreateRight(res)
                        .Match(
                            res => res,
                            _ => string.Empty
                        ),
                    () => CreateRight(defaultLeft)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );

            var result = _leftLens.PutLeft(updatedRight, originalLeft)
                .Bind(leftLensRes => _rightLens.PutLeft(updatedRight, originalLeft)
                                        .Map(rightLensRes => leftLensRes + rightLensRes)
                                        .Map(res => defaultLeft.Replace(defaultRight, res)));

            return result;
        };

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (updatedLeft, originalRight) =>
        {
            var defaultRight = originalRight
                .Match(
                    res => res,
                    () => CreateRight(updatedLeft)
                        .Match(
                            _ => _,
                            _ => string.Empty
                        )
                );
            var defaultLeft = originalRight
                .Match(
                    res => CreateLeft(res)
                        .Match(
                            res => res,
                            _ => string.Empty
                        ),
                    () => CreateLeft(defaultRight)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );
            var result = _rightLens.PutRight(updatedLeft, originalRight)
                .Bind(rightLensRes => _leftLens.PutRight(updatedLeft, originalRight)
                                        .Map(leftLensRes => leftLensRes + rightLensRes)
                                        .Map(res => defaultRight.Replace(defaultLeft, res)));

            return result;
        };

    public override Func<string, Result<string>> CreateRight =>
        originalLeft =>
        {
            var result = _leftLens.CreateRight(originalLeft)
                .Bind(leftLensRes => _rightLens.CreateRight(originalLeft)
                                        .Map(rightLensRes => leftLensRes + rightLensRes));

            return result;
        };

    public override Func<string, Result<string>> CreateLeft =>
        originalRight =>
        {
            var result = _rightLens.CreateLeft(originalRight)
                .Bind(rightLensRes => _leftLens.CreateLeft(originalRight)
                                        .Map(leftLensRes => leftLensRes + rightLensRes));

            return result;
        };

    /// <summary>
    /// Constructs a concat lens.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    /// <returns>A concat lens</returns>
    public static ConcatLens Cons(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => new(lhsLens, rhsLens);
}
