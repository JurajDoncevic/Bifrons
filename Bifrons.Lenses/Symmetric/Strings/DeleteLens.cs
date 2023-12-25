namespace Bifrons.Lenses.Symmetric.Strings;

public sealed class DeleteLens : DisconnectLens
{
    private DeleteLens(string sourceString)
        : base(sourceString, string.Empty, sourceString, string.Empty) { }

    public static DeleteLens Cons(string sourceString)
        => new(sourceString);
}
