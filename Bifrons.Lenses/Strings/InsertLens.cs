namespace Bifrons.Lenses.Strings;

/// <summary>
/// Describes an insert lens. The insert lens inserts a constant going left to right. Defined through a disconnect lens.
/// <c>ins(t) = disconnect("", t, "", t)</c>
/// </summary>
public sealed class InsertLens : DisconnectLens
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="targetString">CONSTANT string that is inserted L => R</param>
    private InsertLens(string targetString)
        : base(string.Empty, targetString, string.Empty, targetString) { }

    /// <summary>
    /// Constructs an insert lens
    /// </summary>
    /// <param name="targetString">CONSTANT string that is inserted L => R</param>
    public static InsertLens Cons(string targetString)
        => new(targetString);
}

