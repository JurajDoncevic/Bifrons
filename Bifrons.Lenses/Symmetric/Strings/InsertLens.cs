namespace Bifrons.Lenses.Symmetric.Strings;

public sealed class InsertLens : DisconnectLens
{
    private InsertLens(string targetString)
        : base(string.Empty, targetString, string.Empty, targetString) { }

    public static InsertLens Cons(string targetString)
        => new(targetString);
}

