
namespace Bifrons.Lenses.Longs;

/// <summary>
/// Abstract class describing a simple symmetric lens between two longes.
/// <c>L : long <=> long</c>
/// </summary>
public abstract class SymmetricLongLens : ISymmetricLens<long, long>
{
    public abstract Func<long, Option<long>, Result<long>> PutLeft { get; }
    public abstract Func<long, Option<long>, Result<long>> PutRight { get; }
    public abstract Func<long, Result<long>> CreateRight { get; }
    public abstract Func<long, Result<long>> CreateLeft { get; }

    /// <summary>
    /// Composes two long lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static SymmetricLongLens operator >>(SymmetricLongLens left, SymmetricLongLens right)
        => ComposeLens.Cons(left, right);
}
