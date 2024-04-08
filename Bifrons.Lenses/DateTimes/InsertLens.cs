namespace Bifrons.Lenses.DateTimes;

public sealed class InsertLens
    : DisconnectLens
{
    private InsertLens(DateTime value) 
        :base(UNIT_VALUE, value)
    {
    }

    public static InsertLens Cons(DateTime? value = null)
        => new(value ?? INIT_VALUE);
}
