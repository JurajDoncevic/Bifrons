
namespace Bifrons.Lenses.Integers;

public sealed class InsertLens
    : DisconnectLens
{
    private InsertLens(int value) 
        :base(UNIT_VALUE, value)
    {
    }

    public static InsertLens Cons(int value = INIT_VALUE)
        => new(value);
}
