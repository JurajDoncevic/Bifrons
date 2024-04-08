namespace Bifrons.Lenses.Decimals;

public sealed class InsertLens
    : DisconnectLens
{
    private InsertLens(double value) 
        :base(UNIT_VALUE, value)
    {
    }

    public static InsertLens Cons(double value = INIT_VALUE)
        => new(value);
}
