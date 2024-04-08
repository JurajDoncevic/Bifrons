namespace Bifrons.Lenses.Booleans;

public sealed class IdentityLens : SymmetricBooleanLens
{
    private IdentityLens()
    {
    }

    public override Func<bool, Option<bool>, Result<bool>> PutLeft =>
        (updatedView, _) => CreateLeft(updatedView);

    public override Func<bool, Option<bool>, Result<bool>> PutRight =>
        (updatedView, _) => CreateRight(updatedView);

    public override Func<bool, Result<bool>> CreateRight =>
        source => source;

    public override Func<bool, Result<bool>> CreateLeft =>
        source => source;

    public static IdentityLens Cons()
        => new();
}
