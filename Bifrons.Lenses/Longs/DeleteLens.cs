
namespace Bifrons.Lenses.Longs;

public sealed class DeleteLens
    : DisconnectLens
{    
    private DeleteLens(long defaultValue) 
        : base(defaultValue, UNIT_VALUE)
    {
    }

    public static DeleteLens Cons(long defaultValue = UNIT_VALUE)
        => new(defaultValue);
}
