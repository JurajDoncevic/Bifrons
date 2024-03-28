namespace Bifrons.Lenses.DateTimes;

/// <summary>
/// Combinators for symmetric date time lenses
/// </summary>
public static class Combinators
{
    /// <summary>
    /// Composes two integer lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static SymmetricDateTimeLens Compose(SymmetricDateTimeLens left, SymmetricDateTimeLens right)
        => ComposeLens.Cons(left, right);
}
