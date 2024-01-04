using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Lens that joins a list of strings into a single string, and vice-versa.
/// <c>join(R, L), L : IEnumerable&lt;string&gt; <=> string, join: IEnumerable <=> string</c>
/// </summary>
public class JoinLens : BaseSymmetricLens<IEnumerable<string>, string>
{
    private readonly Regex _separatorRegex;
    private readonly SymmetricStringLens _itemLens;

    /// <summary>
    /// Constructor
    /// </summary>
    private JoinLens(string separatorRegexString, SymmetricStringLens itemLens)
    {
        _separatorRegex = new Regex(separatorRegexString);
        _itemLens = itemLens;
    }

    public override Func<string, Option<IEnumerable<string>>, Result<IEnumerable<string>>> PutLeft =>
        (updatedLeft, originalRight) =>
        {
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


    public override Func<IEnumerable<string>, Option<string>, Result<string>> PutRight =>
        (updatedRight, originalLeft) =>
        {
            var leftElements = originalLeft.Match(
                left => _separatorRegex.Split(left).AsEnumerable(),
                () => Enumerable.Empty<string>()
            );
            var results = updatedRight.Mapi((idx, right) => _itemLens.PutLeft(right, leftElements.ElementAtOrDefault((int)idx) ?? Option.None<string>()))
                .Unfold()
                .Map(rs => string.Join(_separatorRegex.ToString(), rs));

            return results;
        };

    public override Func<IEnumerable<string>, Result<string>> CreateRight =>
        left =>
        {
            var result = left.Map(_itemLens.CreateLeft)
                .Unfold()
                .Map(rs => string.Join(_separatorRegex.ToString(), rs));

            return result;
        };

    public override Func<string, Result<IEnumerable<string>>> CreateLeft =>
        right =>
        {
            var items = _separatorRegex.Split(right).AsEnumerable();
            var results = items.Map(_itemLens.CreateRight)
                .Unfold();

            return results;

        };

    /// <summary>
    /// Constructs a join lens
    /// </summary>
    /// <param name="separatorRegexString">String for separator regex</param>
    /// <param name="itemLens">Item lens to be applied on each item</param>
    public static JoinLens Cons(string separatorRegexString, SymmetricStringLens itemLens)
        => new(separatorRegexString, itemLens);
}
