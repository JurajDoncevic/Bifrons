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
    /// <param name="lhsLens">Left lens</param>
    /// <param name="rhsLens">Right lens</param>
    public static SymmetricStringLens Concat(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => ConcatLens.Cons(lhsLens, rhsLens);

    /// <summary>
    /// Iterates over a string by a separator and applies a lens to each item.
    /// </summary>
    /// <param name="separatorRegexString">String for separator regex</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static IterateLens Iterate(string separatorRegexString, SymmetricStringLens itemLens)
        => IterateLens.Cons(separatorRegexString, itemLens);

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


    /// <summary>
    /// Iterates over a string by a separator and applies a lens to each item to create a anonymous <c>SymmetricLens</c>.
    /// </summary>
    /// <param name="separatorRegexString">String for separator regex</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static BaseSymmetricLens<string, IEnumerable<string>> IterateAnon(string separatorRegexString, SymmetricStringLens itemLens)
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
}
