namespace Bifrons.Lenses.Booleans;

public sealed class InsertLens
    : DisconnectLens
{
    private InsertLens(bool value) 
        :base(UNIT_VALUE, value)
    {
    }

    public static InsertLens Cons(bool? value = null)
        => new(value ?? INIT_VALUE);
}
