using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// A simple symmetric string lens that iterates over a string and applies a lens to each item.
/// <c>enumerate(R, L), L : string <=> string, enumerate: string <=> IEnumerable</c>
/// </summary>
public class EnumerateLens : BaseSymmetricLens<string, IEnumerable<string>>
{
    private readonly Regex _separatorRegex;
    private readonly SymmetricStringLens _itemLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="separatorRegexString">String for separator regex</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    private EnumerateLens(string separatorRegexString, SymmetricStringLens itemLens)
    {
        _separatorRegex = new Regex(separatorRegexString);
        _itemLens = itemLens;
    }

    public override Func<IEnumerable<string>, Option<string>, Result<string>> PutLeft =>
        (updatedRight, originalLeft) =>
        {
            if (!originalLeft)
            {
                return CreateLeft(updatedRight);
            }

            var leftElements = originalLeft.Match(
                left => _separatorRegex.Split(left).AsEnumerable(),
                () => Enumerable.Empty<string>()
            );
            var results = updatedRight.Mapi((idx, right) => _itemLens.PutLeft(right, leftElements.ElementAtOrDefault((int)idx) ?? Option.None<string>()))
                .Unfold()
                .Map(rs => string.Join(_separatorRegex.ToString(), rs));

            return results;
        };

    public override Func<string, Option<IEnumerable<string>>, Result<IEnumerable<string>>> PutRight =>
        (updatedLeft, originalRight) =>
        {
            if (!originalRight)
            {
                return CreateRight(updatedLeft);
            }
            var updatedItems = _separatorRegex.Split(updatedLeft).AsEnumerable();
            var originalItems = originalRight.Match(
                items => items,
                () => _separatorRegex.Split(updatedLeft).AsEnumerable()
            );

            var results = updatedItems.Mapi((idx, item) =>
            {
                var originalItem = originalItems.ElementAtOrDefault((int)idx) ?? string.Empty;
                return _itemLens.PutRight(item, originalItem);
            }).Unfold();

            return results;
        };

    public override Func<string, Result<IEnumerable<string>>> CreateRight =>
        left =>
        {
            var items = _separatorRegex.Split(left).AsEnumerable();
            var results = items.Map(_itemLens.CreateRight)
                .Unfold();

            return results;

        };

    public override Func<IEnumerable<string>, Result<string>> CreateLeft =>
        right =>
        {
            var result = right.Map(_itemLens.CreateLeft)
                .Unfold()
                .Map(rs => string.Join(_separatorRegex.ToString(), rs));

            return result;
        };

    /// <summary>
    /// Constructs an iterate lens
    /// </summary>
    /// <param name="separatorRegexString">String for separator regex</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static EnumerateLens Cons(string separatorRegexString, SymmetricStringLens itemLens)
        => new(separatorRegexString, itemLens);
}
