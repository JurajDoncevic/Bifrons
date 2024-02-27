namespace Bifrons.Lenses.Symmetric.Decimals;

/// <summary>
/// Describes an identity lens. The identity lens propagates decimal data.
/// <c>id()</c>
/// </summary>
public sealed class IdentityLens : SymmetricDecimalLens
{
    public override Func<double, Option<double>, Result<double>> PutLeft => 
        (double updatedView, Option<double> _) => updatedView;

    public override Func<double, Option<double>, Result<double>> PutRight => 
        (double updatedView, Option<double> _) => updatedView;

    public override Func<double, Result<double>> CreateRight => 
        source => source;

    public override Func<double, Result<double>> CreateLeft => 
        source => source;

    /// <summary>
    /// Constructs an identity lens.
    /// </summary>
    public static IdentityLens Cons() => new();
}
