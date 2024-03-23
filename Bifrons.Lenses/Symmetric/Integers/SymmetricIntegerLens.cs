
namespace Bifrons.Lenses.Symmetric.Integers;

/// <summary>
/// Abstract class describing a simple symmetric lens between two integers.
/// <c>L : int <=> int</c>
/// </summary>
public abstract class SymmetricIntegerLens : ISimpleSymmetricLens<int, int>
{
    public abstract Func<int, Option<int>, Result<int>> PutLeft { get; }
    public abstract Func<int, Option<int>, Result<int>> PutRight { get; }
    public abstract Func<int, Result<int>> CreateRight { get; }
    public abstract Func<int, Result<int>> CreateLeft { get; }

    /// <summary>
    /// Composes two integer lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static SymmetricIntegerLens operator >>(SymmetricIntegerLens left, SymmetricIntegerLens right)
        => ComposeLens.Cons(left, right);
}
