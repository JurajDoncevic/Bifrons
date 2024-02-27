namespace Bifrons.Lenses.Symmetric.Decimals;

/// <summary>
/// Abstract class describing a simple symmetric lens between two decimals.
/// <c>L : decimal <=> decimal</c>
/// </summary>
public abstract class SymmetricDecimalLens : BaseSymmetricLens<double, double>
{
    /// <summary>
    /// Composes two decimal lenses
    /// </summary>
    public static SymmetricDecimalLens operator >>(SymmetricDecimalLens left, SymmetricDecimalLens right)
        => Combinators.Compose(left, right);
}
