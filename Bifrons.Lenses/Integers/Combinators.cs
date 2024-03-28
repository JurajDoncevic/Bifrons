namespace Bifrons.Lenses.Integers;

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
    public static SymmetricIntegerLens Compose(SymmetricIntegerLens left, SymmetricIntegerLens right)
        => ComposeLens.Cons(left, right);
}
