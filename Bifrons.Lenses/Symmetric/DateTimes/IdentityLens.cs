namespace Bifrons.Lenses.Symmetric.DateTimes;

/// <summary>
/// Describes an identity lens. The identity lens propagates date time data.
/// <c>id()</c>
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

    /// <summary>
    /// Constructs an identity lens.
    /// </summary>
    public static IdentityLens Cons() => new();
}
