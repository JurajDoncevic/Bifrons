namespace Bifrons.Lenses.Decimals;

public sealed class DeleteLens
    : DisconnectLens
{
    private DeleteLens(double defaultValue) 
        :base(defaultValue, UNIT_VALUE)
    {
    }

    public static DeleteLens Cons(double defaultValue = INIT_VALUE)
        => new(defaultValue);
}
