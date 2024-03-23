namespace Bifrons.Lenses.Symmetric.Booleans;

public sealed class OrLens : SymmetricBoolLens
{
    private readonly bool _defaultOriginalSource;
    private OrLens(bool defaultOriginalSource)
    {
        _defaultOriginalSource = defaultOriginalSource;
    }

    public override Func<bool, Option<bool>, Result<bool>> PutLeft =>
        (updatedView, originalSource) =>
            originalSource.Match(
                value => updatedView || value,
                () => updatedView || _defaultOriginalSource
                );

    public override Func<bool, Option<bool>, Result<bool>> PutRight =>
        (updatedView, originalSource) =>
            originalSource.Match(
                value => updatedView || value,
                () => updatedView || _defaultOriginalSource
                );

    public override Func<bool, Result<bool>> CreateRight =>
        source => source;

    public override Func<bool, Result<bool>> CreateLeft =>
        source => source;

    public static OrLens Cons(bool defaultOriginalSource = false)
        => new(defaultOriginalSource);
}
