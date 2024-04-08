
namespace Bifrons.Lenses.Integers;

public sealed class DeleteLens
    : DisconnectLens
{    
    private DeleteLens(int defaultValue) 
        : base(defaultValue, UNIT_VALUE)
    {
    }

    public static DeleteLens Cons(int defaultValue = UNIT_VALUE)
        => new(defaultValue);
}
