namespace Bifrons.Lenses.Symmetric.Integers;

/// <summary>
/// Abstract class describing a simple symmetric lens between two integers.
/// <c>L : int <=> int</c>
/// </summary>
public abstract class SymmetricIntegerLens : BaseSymmetricLens<int, int>
{
    /// <summary>
    /// Composes two integer lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static SymmetricIntegerLens operator>>(SymmetricIntegerLens left, SymmetricIntegerLens right) 
        => ComposeLens.Cons(left, right);
}
