namespace Bifrons.Lenses.Longs;

/// <summary>
/// Combinators for integer lenses
/// </summary>
public static class Combinators
{
    /// <summary>
    /// Composes two integer lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static SymmetricLongLens Compose(SymmetricLongLens left, SymmetricLongLens right)
        => ComposeLens.Cons(left, right);
}
