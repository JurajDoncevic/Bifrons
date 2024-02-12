namespace Bifrons.Lenses.Symmetric.DateTimes;

public sealed class IdentityLens : SymmetricDateTimeLens
{
    public override Func<DateTime, Option<DateTime>, Result<DateTime>> PutLeft => 
        (DateTime updatedView, Option<DateTime> _) => updatedView;

    public override Func<DateTime, Option<DateTime>, Result<DateTime>> PutRight => 
        (DateTime updatedView, Option<DateTime> _) => updatedView;

    public override Func<DateTime, Result<DateTime>> CreateRight => 
        source => source;

    public override Func<DateTime, Result<DateTime>> CreateLeft => 
        source => source;

    public static IdentityLens Cons() => new();
}
