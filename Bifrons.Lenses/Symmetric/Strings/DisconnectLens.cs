using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Describes a disconnect lens. A disconnect lens does not propagate any data at all from one format to the other.
/// <c>disconnect(R, S, s, t)</c>
/// </summary>
public class DisconnectLens : SymmetricStringLens
{
    private readonly Regex _leftRegex;
    private readonly Regex _rightRegex;
    private readonly string _rightDefault;
    private readonly string _leftDefault;

    public override Regex LeftRegex => _leftRegex;
    public override Regex RightRegex => _rightRegex;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="leftRegex">Regex string that is used for PutR</param>
    /// <param name="rightRegex">Regex string that is used for PutL</param>
    protected DisconnectLens(string leftRegex, string rightRegex, string leftDefault, string rightDefault)
    {
        _leftRegex = new Regex(leftRegex ?? string.Empty);
        _rightRegex = new Regex(rightRegex ?? string.Empty);
        _rightDefault = rightDefault ?? string.Empty;
        _leftDefault = leftDefault ?? string.Empty;
    }
    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (updatedSource, originalTarget) => originalTarget
            ? Results.Success(_leftRegex.Match(originalTarget.Value).Value)
            : CreateLeft(updatedSource);

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) => originalTarget
            ? Results.Success(_rightRegex.Match(originalTarget.Value).Value)
            : CreateRight(updatedSource);

    public override Func<string, Result<string>> CreateRight =>
        source => Results.Success(_rightDefault);

    public override Func<string, Result<string>> CreateLeft =>
        source => Results.Success(_leftDefault);

    /// <summary>
    /// Constructs a DisconnectLens.
    /// </summary>
    /// <param name="matchToRightRegex">String for the right match regex</param>
    /// <param name="matchToLeftRegex">String for the left match regex</param>
    /// <param name="leftDefault">Left default value</param>
    /// <param name="rightDefault">Right default value</param>
    /// <returns>A disconnect lens</returns>
    public static DisconnectLens Cons(string matchToRightRegex, string matchToLeftRegex, string leftDefault, string rightDefault)
        => new(matchToRightRegex, matchToLeftRegex, leftDefault, rightDefault);
}
