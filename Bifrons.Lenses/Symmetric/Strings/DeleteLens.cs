namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// A lens that deletes string values through a regex going left to right. Defined through a disconnect lens.
/// <c>del(s) = disconnect(s, "", s, "")</c>
/// </summary>
public sealed class DeleteLens : DisconnectLens
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="sourceString">CONSTANT string that is used for PutR</param>
    private DeleteLens(string sourceString)
        : base(sourceString, string.Empty, sourceString, string.Empty) { }

    /// <summary>
    /// Constructs a DeleteLens from a regex string
    /// </summary>
    /// <param name="sourceString">CONSTANT string that is used for PutR</param>
    public static DeleteLens Cons(string sourceString)
        => new(sourceString);
}
