
namespace Bifrons.Lenses.Symmetric.Booleans;

public sealed class NotLens : SymmetricBoolLens
{
    private NotLens()
    {
    }

    public override Func<bool, Option<bool>, Result<bool>> PutLeft => 
        (updatedView, _) => CreateRight(updatedView);

    public override Func<bool, Option<bool>, Result<bool>> PutRight => 
        (updatedView, _) => CreateLeft(updatedView);

    public override Func<bool, Result<bool>> CreateRight => 
        source => !source;

    public override Func<bool, Result<bool>> CreateLeft => 
        source => !source;

    public static NotLens Cons()
        => new();
}
