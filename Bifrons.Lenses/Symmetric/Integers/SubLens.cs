namespace Bifrons.Lenses.Symmetric.Integers;

/// <summary>
/// Describes a lens that subtracts a constant value from an integer. This is in effect a special case of AddLens and an inverse of AddLens.
/// sub(value) : int <=> int
/// </summary>
public sealed class SubLens : AddLens
{
    private readonly int _subValue;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="subValue">The value to subtract from the source (expected positive value)</param>
    private SubLens(int subValue) : base(-subValue)
    {
        _subValue = subValue;
    }

    /// <summary>
    /// Constructs a subtraction lens
    /// </summary>
    /// <param name="subValue">Subtraction value</param>
    public static new SubLens Cons(int subValue) => new(subValue);
}
