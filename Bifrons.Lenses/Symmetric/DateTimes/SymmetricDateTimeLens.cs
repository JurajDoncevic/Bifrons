
namespace Bifrons.Lenses.Symmetric.DateTimes;

/// <summary>
/// Abstract class describing a simple symmetric lens between two DateTimes.
/// <c>L : DateTime <=> DateTime</c>
/// </summary>
public abstract class SymmetricDateTimeLens : ISimpleSymmetricLens<DateTime, DateTime>
{
    public abstract Func<DateTime, Option<DateTime>, Result<DateTime>> PutLeft { get; }
    public abstract Func<DateTime, Option<DateTime>, Result<DateTime>> PutRight { get; }
    public abstract Func<DateTime, Result<DateTime>> CreateRight { get; }
    public abstract Func<DateTime, Result<DateTime>> CreateLeft { get; }

    /// <summary>
    /// Composes two datetime lenses
    /// </summary>
    public static SymmetricDateTimeLens operator >>(SymmetricDateTimeLens left, SymmetricDateTimeLens right)
        => Combinators.Compose(left, right);
}
