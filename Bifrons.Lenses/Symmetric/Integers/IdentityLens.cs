namespace Bifrons.Lenses.Symmetric.Integers;

public sealed class IdentityLens : SymmetricIntegerLens
{
    public override Func<int, Option<int>, Result<int>> PutLeft => 
        (int updatedView, Option<int> _) => updatedView;

    public override Func<int, Option<int>, Result<int>> PutRight => 
        (int updatedView, Option<int> _) => updatedView;

    public override Func<int, Result<int>> CreateRight => 
        source => source;

    public override Func<int, Result<int>> CreateLeft => 
        source => source;

    public static IdentityLens Cons() => new();
}
