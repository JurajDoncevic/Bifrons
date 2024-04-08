namespace Bifrons.Lenses.Booleans;

public sealed class DeleteLens
    : DisconnectLens
{
    private DeleteLens(bool defaultValue) 
        : base(defaultValue, UNIT_VALUE)
    {
    }

    public static DeleteLens Cons(bool defaultValue = INIT_VALUE)
        => new(defaultValue);
}
