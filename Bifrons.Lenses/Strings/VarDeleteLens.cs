namespace Bifrons.Lenses.Strings;

/// <summary>
/// Describes a variable delete lens that uses a regex to delete part of a string. In case of no source, the default value is provided.
/// vardel(regex, default) : string <=> string
/// </summary>
public class VarDeleteLens : DisconnectLens
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="deleteRegex">Regex to delete</param>
    /// <param name="defaultConstant">Default value to provide in case of no source. Should match regex!</param>
    protected VarDeleteLens(string deleteRegex, string defaultConstant)
        : base(deleteRegex, string.Empty, defaultConstant, string.Empty)
    { }

    /// <summary>
    /// Constructs a variable delete lens
    /// </summary>
    /// <param name="deleteRegex">Regex to delete</param>
    /// <param name="defaultConstant">Default value to provide in case of no source. Should match regex!</param>
    public static VarDeleteLens Cons(string deleteRegex, string defaultConstant)
        => new(deleteRegex, defaultConstant);
}
