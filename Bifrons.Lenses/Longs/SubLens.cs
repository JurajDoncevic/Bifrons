namespace Bifrons.Lenses.Longs;

/// <summary>
/// Describes a lens that subtracts a constant value from an long. This is in effect a special case of AddLens and an inverse of AddLens.
/// sub(value) : long <=> long
/// </summary>
public sealed class SubLens : AddLens
{
    private readonly long _subValue;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="subValue">The value to subtract from the source (expected positive value)</param>
    private SubLens(long subValue) : base(-subValue)
    {
        _subValue = subValue;
    }

    /// <summary>
    /// Constructs a subtraction lens
    /// </summary>
    /// <param name="subValue">Subtraction value</param>
    public static new SubLens Cons(long subValue) => new(subValue);
}
