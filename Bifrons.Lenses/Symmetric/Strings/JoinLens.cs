using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Lens that joins a list of strings into a single string, and vice-versa.
/// <c>join(R, L), L : IEnumerable&lt;string&gt; <=> string, join: IEnumerable <=> string</c>
/// </summary>
public class JoinLens : ISymmetricLens<IEnumerable<string>, string>
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

    public Func<string, Option<IEnumerable<string>>, Result<IEnumerable<string>>> PutLeft =>
        (updatedSource, originalTarget) =>
        {
            var updatedItems = _separatorRegex.Split(updatedSource).AsEnumerable();
            var originalItems = originalTarget.Match(
                items => items,
                () => _separatorRegex.Split(updatedSource).AsEnumerable()
            );
            var results = updatedItems.Mapi((idx, item) =>
            {
                var originalItem = originalItems.ElementAtOrDefault((int)idx) ?? string.Empty;
                return _itemLens.PutRight(item, originalItem);
            }).Unfold();

            return results;
        };


    public Func<IEnumerable<string>, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) =>
        {
            var sourceElements = originalTarget.Match(
                source => _separatorRegex.Split(source).AsEnumerable(),
                () => Enumerable.Empty<string>()
            );
            var results = updatedSource.Mapi((idx, right) => _itemLens.PutLeft(right, sourceElements.ElementAtOrDefault((int)idx) ?? Option.None<string>()))
                .Unfold()
                .Map(rs => string.Join(_separatorRegex.ToString(), rs));

            return results;
        };

    public Func<IEnumerable<string>, Result<string>> CreateRight =>
        source =>
        {
            var result = source.Map(_itemLens.CreateLeft)
                .Unfold()
                .Map(rs => string.Join(_separatorRegex.ToString(), rs));

            return result;
        };

    public Func<string, Result<IEnumerable<string>>> CreateLeft =>
        source =>
        {
            var items = _separatorRegex.Split(source).AsEnumerable();
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
