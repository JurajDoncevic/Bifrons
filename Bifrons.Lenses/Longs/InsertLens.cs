
namespace Bifrons.Lenses.Longs;

public sealed class InsertLens
    : DisconnectLens
{
    private InsertLens(long value) 
        :base(UNIT_VALUE, value)
    {
    }

    public static InsertLens Cons(long value = INIT_VALUE)
        => new(value);
}
