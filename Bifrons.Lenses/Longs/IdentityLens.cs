namespace Bifrons.Lenses.Longs;

/// <summary>
/// Describes an identity lens. The identity lens propagates long data.
/// <c>id()</c>
public sealed class IdentityLens : SymmetricLongLens
{
    public override Func<long, Option<long>, Result<long>> PutLeft => 
        (long updatedView, Option<long> _) => updatedView;

    public override Func<long, Option<long>, Result<long>> PutRight => 
        (long updatedView, Option<long> _) => updatedView;

    public override Func<long, Result<long>> CreateRight => 
        source => source;

    public override Func<long, Result<long>> CreateLeft => 
        source => source;

    /// <summary>
    /// Constructs an identity lens.
    /// </summary>
    public static IdentityLens Cons() => new();
}
