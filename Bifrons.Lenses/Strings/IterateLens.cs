using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Strings;

/// <summary>
/// A simple symmetric string lens that iterates over a string and applies a lens to each item.
/// <c>iterate(r, L), L : string <=> string; iterate : string <=> string </c>
public class IterateLens : SymmetricStringLens
{
    private readonly string _separator;
    private readonly SymmetricStringLens _itemLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="separator">String for the separator</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    private IterateLens(string separator, SymmetricStringLens itemLens)
    {
        _separator = separator;
        _itemLens = itemLens;
    }

    public override Regex LeftRegex => new Regex($"({_itemLens.LeftRegex}({_separator})?)*");

    public override Regex RightRegex => new Regex($"({_itemLens.RightRegex}({_separator})?)*");

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateLeft(updatedSource);
            }

            var updatedItems = updatedSource.Split(_separator);
            var originalItems = originalTarget.Value.Split(_separator);

            var results = updatedItems.Mapi((idx, item) =>
            {
                var originalItem = originalItems.ElementAtOrDefault((int)idx) ?? string.Empty;
                return _itemLens.PutLeft(item, originalItem);
            })
            .Unfold()
            .Map(rs => string.Join(_separator, rs));

            return results;
        };


    public override Func<string, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateRight(updatedSource);
            }

            var updatedItems = updatedSource.Split(_separator);
            var originalItems = originalTarget.Value.Split(_separator);

            var results = updatedItems.Mapi((idx, item) =>
            {
                var originalItem = originalItems.ElementAtOrDefault((int)idx) ?? string.Empty;
                return _itemLens.PutRight(item, originalItem);
            })
            .Unfold()
            .Map(rs => string.Join(_separator, rs));

            return results;
        };

    public override Func<string, Result<string>> CreateRight =>
        source => source.Split(_separator)
            .Map(_itemLens.CreateRight)
            .Unfold()
            .Map(rs => string.Join(_separator, rs));

    public override Func<string, Result<string>> CreateLeft =>
        source => source.Split(_separator)
            .Map(_itemLens.CreateLeft)
            .Unfold()
            .Map(rs => string.Join(_separator, rs));

    /// <summary>
    /// Constructs an iterate lens
    /// </summary>
    /// <param name="separator">The separator to use</param>
    /// <param name="itemLens">The lens to use for each item</param>
    public static IterateLens Cons(string separator, SymmetricStringLens itemLens) =>
        new IterateLens(separator, itemLens);
}
