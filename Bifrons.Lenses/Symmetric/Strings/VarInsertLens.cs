namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Describes a variable insert lens that uses a regex to match updates of the inserted value on the target. Inserts the default value. The default value should match the regex!
/// This allows the inserted value to be changed as long as it fits the original regex.
/// varins(regex, default) : string <=> string
public class VarInsertLens : DisconnectLens
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="insertRegex">Regex to follow in the target</param>
    /// <param name="defaultConstant">Default value to insert. Should match regex!</param>
    protected VarInsertLens(string insertRegex, string defaultConstant)
        : base(string.Empty, insertRegex, string.Empty, defaultConstant)
    { }

    /// <summary>
    /// Constructs a variable insert lens
    /// </summary>
    /// <param name="insertRegex">Regex to follow in the target</param>
    /// <param name="defaultConstant">Default value to insert. Should match regex!</param>
    public static VarInsertLens Cons(string insertRegex, string defaultConstant)
        => new(insertRegex, defaultConstant);
}
