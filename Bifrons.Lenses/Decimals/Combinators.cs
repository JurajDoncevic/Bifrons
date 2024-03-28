namespace Bifrons.Lenses.Decimals;

/// <summary>
/// Combinators for decimal lenses
/// </summary>
public static class Combinators
{
    /// <summary>
    /// Constructs a composition lens for decimal lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static ComposeLens Compose(SymmetricDecimalLens left, SymmetricDecimalLens right) 
        => ComposeLens.Cons(left, right);
}
