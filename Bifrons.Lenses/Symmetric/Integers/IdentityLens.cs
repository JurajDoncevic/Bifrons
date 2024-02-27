namespace Bifrons.Lenses.Symmetric.Integers;

/// <summary>
/// Describes an identity lens. The identity lens propagates integer data.
/// <c>id()</c>
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

    /// <summary>
    /// Constructs an identity lens.
    /// </summary>
    public static IdentityLens Cons() => new();
}
