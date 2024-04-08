
namespace Bifrons.Lenses.DateTimes;

public sealed class DeleteLens
    : DisconnectLens
{
    private DeleteLens(DateTime defaultValue) 
        :base(defaultValue, UNIT_VALUE)
    {
    }

    public static DeleteLens Cons(DateTime? defaultvValue = null)
        => new(defaultvValue ?? INIT_VALUE);
}
