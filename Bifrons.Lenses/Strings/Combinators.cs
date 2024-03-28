namespace Bifrons.Lenses.Strings;

/// <summary>
/// Combinators for simple symmetric string lenses.
/// </summary>
public static class Combinators
{
    /// <summary>
    /// Concatenates two simple symmetric string lenses. Lens regexes have to take into account the preceding lens regexes of the concat.
    /// e.g. <c>id(\w+) | id((?!\w+\s)\w+)</c> is a valid lens, but <c>id(\w+) | id(\w+)</c> is not.
    /// </summary>
    /// <param name="lhsLens">Left lens</param>
    /// <param name="rhsLens">Right lens</param>
    public static SymmetricStringLens Concat(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => ConcatLens.Cons(lhsLens, rhsLens);

    /// <summary>
    /// Enumerates a string by a separator and applies a lens to each item.
    /// </summary>
    /// <param name="separatorRegexString">String for separator regex</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static EnumerateLens Enumerate(string separatorRegexString, SymmetricStringLens itemLens)
        => EnumerateLens.Cons(separatorRegexString, itemLens);

    /// <summary>
    /// Joins a string enumerable with a separator and applies a lens to each item.
    /// </summary>
    /// <param name="separatorRegexString">String for separator regex</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static JoinLens Join(string separatorRegexString, SymmetricStringLens itemLens)
        => JoinLens.Cons(separatorRegexString, itemLens);

    /// <summary>
    /// Iterates over a string by a separator and applies a lens to each item.
    /// </summary>
    /// <param name="separatorRegexString">String for the separator - CONSTANT</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static IterateLens Iterate(string separator, SymmetricStringLens itemLens)
        => IterateLens.Cons(separator, itemLens);

    /// <summary>
    /// Inverts a simple symmetric string lens.
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    public static SymmetricStringLens Invert(SymmetricStringLens originalLens)
        => InvertLens.Cons(originalLens);

    /// <summary>
    /// Composes two simple symmetric string lenses.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static SymmetricStringLens Compose(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => ComposeLens.Cons(lhsLens, rhsLens);

    /// <summary>
    /// Creates a union lens from two simple symmetric string lenses.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static OrLens Or(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => OrLens.Cons(lhsLens, rhsLens);

    /// <summary>
    /// Merges the left side of a or lens with a symmetric string lens.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static MergeLeft MergeL(SymmetricStringLens lhsLens, OrLens rhsLens)
        => MergeLeft.Cons(rhsLens, lhsLens);

    /// <summary>
    /// Merges the right side of a or lens with a symmetric string lens.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static MergeRight MergeR(OrLens lhsLens, SymmetricStringLens rhsLens)
        => MergeRight.Cons(lhsLens, rhsLens);

    /// <summary>
    /// Merges both side of a OrLens to a MergeLens (in effect a symmetric string lens).
    /// Side to use is decided by the regex match.
    /// </summary>
    /// <param name="orLens">OrLens to merge</param>
    public static MergeLens Merge(OrLens orLens)
        => MergeLens.Cons(orLens);

    /// <summary>
    /// Concatenates two simple symmetric string lenses to a anonymous <c>SymmetricLens</c>. Lens regexes have to take into account the preceding lens regexes of the concat.
    /// e.g. <c>id(\w+) | id((?!\w+\s)\w+)</c> is a valid lens, but <c>id(\w+) | id(\w+)</c> is not.
    /// </summary>
    public static ISymmetricLens<string, string> ConcatAnon(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
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

    /// <summary>
    /// Iterates over a string by a separator and applies a lens to each item to create a anonymous <c>SymmetricLens</c>.
    /// </summary>
    /// <param name="separatorRegexString">String for separator regex</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static ISymmetricLens<string, IEnumerable<string>> IterateAnon(string separatorRegexString, SymmetricStringLens itemLens)
    {
        System.Text.RegularExpressions.Regex separatorRegex = new System.Text.RegularExpressions.Regex(separatorRegexString);

        Func<IEnumerable<string>, Option<string>, Result<string>> putLeft =
        (updatedRight, originalLeft) =>
        {
            var leftElements = originalLeft.Match(
                left => separatorRegex.Split(left).AsEnumerable(),
                () => Enumerable.Empty<string>()
            );
            var results = updatedRight.Mapi((idx, right) => itemLens.PutLeft(right, leftElements.ElementAtOrDefault((int)idx) ?? Option.None<string>()))
                .Unfold()
                .Map(rs => string.Join(separatorRegex.ToString(), rs));

            return results;
        };

        Func<string, Option<IEnumerable<string>>, Result<IEnumerable<string>>> PutRight =
            (updatedLeft, originalRight) =>
            {
                var updatedItems = separatorRegex.Split(updatedLeft).AsEnumerable();
                var originalItems = originalRight.Match(
                    items => items,
                    () => separatorRegex.Split(updatedLeft).AsEnumerable()
                );

                var results = updatedItems.Mapi((idx, item) =>
                {
                    var originalItem = originalItems.ElementAtOrDefault((int)idx) ?? string.Empty;
                    return itemLens.PutRight(item, originalItem);
                }).Unfold();

                return results;
            };

        Func<string, Result<IEnumerable<string>>> createRight =
        left =>
        {
            var items = separatorRegex.Split(left).AsEnumerable();
            var results = items.Map(itemLens.CreateRight)
                .Unfold();

            return results;

        };

        Func<IEnumerable<string>, Result<string>> createLeft =
            right =>
            {
                var result = right.Map(itemLens.CreateLeft)
                    .Unfold()
                    .Map(rs => string.Join(separatorRegex.ToString(), rs));

                return result;
            };

        return SymmetricLens.Cons(
            putLeft,
            PutRight,
            createRight,
            createLeft
        );
    }

    /// <summary>
    /// Inverts a simple symmetric string lens to a anonymous <c>SymmetricLens</c>.
    /// </summary> 
    /// <param name="originalLens">The original lens to invert</param>
    public static ISymmetricLens<string, string> InvertAnon(SymmetricStringLens originalLens)
    {
        Func<string, Result<string>> createLeft = originalLeft =>
        {
            var result = originalLens.CreateRight(originalLeft)
                .Map(rightLensRes => originalLeft + rightLensRes);

            return result;
        };

        Func<string, Result<string>> createRight = originalRight =>
        {
            var result = originalLens.CreateLeft(originalRight)
                .Map(leftLensRes => leftLensRes + originalRight);

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
            var result = originalLens.PutRight(updatedLeft, originalRight)
                .Map(rightLensRes => defaultRight.Replace(defaultLeft, rightLensRes));

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

            var result = originalLens.PutLeft(updatedRight, originalLeft)
                .Map(leftLensRes => defaultLeft.Replace(defaultRight, leftLensRes));

            return result;
        };

        return SymmetricLens.Cons(
            putLeft,
            putRight,
            createRight,
            createLeft
        );
    }

    /// <summary>
    /// Composes two simple symmetric string lenses to a anonymous <c>SymmetricLens</c>.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static ISymmetricLens<string, string> ComposeAnon(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
    {
        Func<string, Result<string>> createLeft = originalLeft =>
        {
            var result = lhsLens.CreateLeft(originalLeft)
                .Bind(leftLensRes => rhsLens.CreateLeft(leftLensRes));

            return result;
        };

        Func<string, Result<string>> createRight = originalRight =>
        {
            var result = rhsLens.CreateRight(originalRight)
                .Bind(rightLensRes => lhsLens.CreateRight(rightLensRes));

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
                .Bind(rightLensRes => lhsLens.PutRight(rightLensRes, originalRight)
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
                .Bind(leftLensRes => rhsLens.PutLeft(leftLensRes, originalLeft)
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
