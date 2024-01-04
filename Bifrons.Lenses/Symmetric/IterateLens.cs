using Bifrons.Lenses.Symmetric;

namespace Bifrons.Lenses;

/// <summary>
/// Describes a lens that iterates over an IEnumerable and applies a lens to each item
/// </summary>
public class IterateLens<TLeft, TRight> : BaseSymmetricLens<IEnumerable<TLeft>, IEnumerable<TRight>>
{
    private readonly BaseSymmetricLens<TLeft, TRight> _itemLens;

    internal IterateLens(BaseSymmetricLens<TLeft, TRight> itemLens)
    {
        _itemLens = itemLens;
    }

    public override Func<IEnumerable<TRight>, Option<IEnumerable<TLeft>>, Result<IEnumerable<TLeft>>> PutLeft =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateLeft(updatedSource);
            }

            var updatedItems = updatedSource;
            var originalItems = originalTarget.Value;

            var results = updatedItems.Mapi((idx, item) =>
            {
                var originalItem = originalItems.ElementAtOrDefault((int)idx);
                return _itemLens.PutLeft(item, originalItem!);
            })
            .Unfold()
            .Map(rs => rs.AsEnumerable());

            return results;
        };

    public override Func<IEnumerable<TLeft>, Option<IEnumerable<TRight>>, Result<IEnumerable<TRight>>> PutRight =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateRight(updatedSource);
            }

            var updatedItems = updatedSource;
            var originalItems = originalTarget.Value;

            var results = updatedItems.Mapi((idx, item) =>
            {
                var originalItem = originalItems.ElementAtOrDefault((int)idx);
                return _itemLens.PutRight(item, originalItem!);
            })
            .Unfold()
            .Map(rs => rs.AsEnumerable());

            return results;
        };

    public override Func<IEnumerable<TLeft>, Result<IEnumerable<TRight>>> CreateRight =>
        (source) => source.Map(_itemLens.CreateRight)
            .Unfold()
            .Map(rs => rs.AsEnumerable());

    public override Func<IEnumerable<TRight>, Result<IEnumerable<TLeft>>> CreateLeft =>
        (source) => source.Map(_itemLens.CreateLeft)
            .Unfold()
            .Map(rs => rs.AsEnumerable());


}


public static class IterateLens
{
    /// <summary>
    /// Iterates over an IEnumerable and applies a lens to each item
    /// </summary>
    /// <param name="itemLens">Individual item lens</param>
    public static IterateLens<TLeft, TRight> Cons<TLeft, TRight>(BaseSymmetricLens<TLeft, TRight> itemLens)
        => new(itemLens);
}