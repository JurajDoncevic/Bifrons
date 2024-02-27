namespace Bifrons.Lenses.Symmetric.DateTimes;

/// <summary>
/// Abstract class describing a simple symmetric lens between two DateTimes.
/// <c>L : DateTime <=> DateTime</c>
/// </summary>
public abstract class SymmetricDateTimeLens : BaseSymmetricLens<DateTime, DateTime>
{
    /// <summary>
    /// Composes two datetime lenses
    /// </summary>
    public static SymmetricDateTimeLens operator >>(SymmetricDateTimeLens left, SymmetricDateTimeLens right)
        => Combinators.Compose(left, right);
}
