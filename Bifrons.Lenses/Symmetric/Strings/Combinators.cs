namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Combinators for simple symmetric string lenses.
/// </summary>
public static class Combinators
{
    /// <summary>
    /// Concatenates two simple symmetric string lenses. Lens regexes have to take into account the preceding lens regexes of the concat.
    /// e.g. <c>id(\w+) | id((?!\w+\s)\w+)</c> is a valid lens, but <c>id(\w+) | id(\w+)</c> is not.
    /// </summary>
    public static SymmetricStringLens Concat(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => ConcatLens.Cons(lhsLens, rhsLens);

    /// <summary>
    /// Concatenates two simple symmetric string lenses to a anonymous <c>SymmetricLens</c>. Lens regexes have to take into account the preceding lens regexes of the concat.
    /// e.g. <c>id(\w+) | id((?!\w+\s)\w+)</c> is a valid lens, but <c>id(\w+) | id(\w+)</c> is not.
    /// </summary>
    public static BaseSymmetricLens<string, string> ConcatAnon(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
    {
        Func<string, Result<string>> createLeft = originalLeft =>
        {
            var result = lhsLens.CreateRight(originalLeft)
                .Bind(leftLensRes => rhsLens.CreateRight(originalLeft)
                                        .Map(rightLensRes => leftLensRes + rightLensRes));

            return result;
        };

        Func<string, Result<string>> createRight = originalRight =>
        {
            var result = rhsLens.CreateLeft(originalRight)
                .Bind(rightLensRes => lhsLens.CreateLeft(originalRight)
                                        .Map(leftLensRes => rightLensRes + leftLensRes));

            return result;
        };

        Func<string, Option<string>, Result<string>> putRight =
        (updatedLeft, originalRight) =>
        {
            var defaultRight = originalRight
                .Match(
                    res => res,
                    () => createRight(updatedLeft)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );
            var defaultLeft = originalRight
                .Match(
                    res => createLeft(res)
                        .Match(
                            res => res,
                            _ => string.Empty
                        ),
                    () => createLeft(defaultRight)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );
            var result = rhsLens.PutRight(updatedLeft, originalRight)
                .Bind(rightLensRes => lhsLens.PutRight(updatedLeft, originalRight)
                                        .Map(leftLensRes => leftLensRes + rightLensRes)
                                        .Map(res => defaultRight.Replace(defaultLeft, res)));

            return result;
        };

        Func<string, Option<string>, Result<string>> putLeft =
        (updatedRight, originalLeft) =>
        {
            var defaultLeft = originalLeft
                .Match(
                    res => res,
                    () => createLeft(updatedRight)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );
            var defaultRight = originalLeft
                .Match(
                    res => createRight(res)
                        .Match(
                            res => res,
                            _ => string.Empty
                        ),
                    () => createRight(defaultLeft)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );

            var result = lhsLens.PutLeft(updatedRight, originalLeft)
                .Bind(leftLensRes => rhsLens.PutLeft(updatedRight, originalLeft)
                                        .Map(rightLensRes => leftLensRes + rightLensRes)
                                        .Map(res => defaultLeft.Replace(defaultRight, res)));

            return result;
        };

        return SymmetricLens.Cons(
            putLeft,
            putRight,
            createRight,
            createLeft
        );
    }

}
