namespace Bifrons.Lenses.Booleans;

public sealed class AndLens : SymmetricBoolLens
{
    private readonly bool _defaultOriginalSource;
    private AndLens(bool defaultOriginalSource)
    {
        _defaultOriginalSource = defaultOriginalSource;
    }

    public override Func<bool, Option<bool>, Result<bool>> PutLeft =>
        (updatedView, originalSource) =>
            originalSource.Match(
                value => updatedView && value,
                () => updatedView && _defaultOriginalSource
                );

    public override Func<bool, Option<bool>, Result<bool>> PutRight =>
        (updatedView, originalSource) =>
            originalSource.Match(
                value => updatedView && value,
                () => updatedView && _defaultOriginalSource
                );

    public override Func<bool, Result<bool>> CreateRight =>
        source => source;

    public override Func<bool, Result<bool>> CreateLeft =>
        source => source;

    public static AndLens Cons(bool defaultOriginalSource = true)
        => new(defaultOriginalSource);
}