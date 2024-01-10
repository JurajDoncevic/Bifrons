namespace Bifrons.Lenses.Symmetric.Integers;

public sealed class SubLens : AddLens
{
    private readonly int _subValue;

    private SubLens(int subValue) : base(-subValue)
    {
        _subValue = subValue;
    }

    public static new SubLens Cons(int subValue) => new(subValue);
}
