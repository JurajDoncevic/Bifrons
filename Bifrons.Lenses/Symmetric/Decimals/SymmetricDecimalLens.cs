
namespace Bifrons.Lenses.Symmetric.Decimals;

/// <summary>
/// Abstract class describing a simple symmetric lens between two decimals.
/// <c>L : decimal <=> decimal</c>
/// </summary>
public abstract class SymmetricDecimalLens : ISimpleSymmetricLens<double, double>
{
    public abstract Func<double, Option<double>, Result<double>> PutLeft { get; }
    public abstract Func<double, Option<double>, Result<double>> PutRight { get; }
    public abstract Func<double, Result<double>> CreateRight { get; }
    public abstract Func<double, Result<double>> CreateLeft { get; }

    /// <summary>
    /// Composes two decimal lenses
    /// </summary>
    public static SymmetricDecimalLens operator >>(SymmetricDecimalLens left, SymmetricDecimalLens right)
        => Combinators.Compose(left, right);
}
