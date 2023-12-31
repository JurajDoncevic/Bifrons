using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Describes a disconnect lens. A disconnect lens does not propagate any data at all from one format to the other.
/// <c>disconnect(R, S, s, t)</c>
/// </summary>
public class DisconnectLens : SymmetricStringLens
{
    private readonly Regex _matchToRightRegex;
    private readonly Regex _matchToLeftRegex;
    private readonly string _rightDefault;
    private readonly string _leftDefault;

    public override Regex LeftRegex => _matchToLeftRegex;
    public override Regex RightRegex => _matchToRightRegex;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="matchToRightRegex">Regex string that is used for PutR</param>
    /// <param name="matchToLeftRegex">Regex string that is used for PutL</param>
    protected DisconnectLens(string matchToRightRegex, string matchToLeftRegex, string leftDefault, string rightDefault)
    {
        _matchToRightRegex = new Regex(matchToRightRegex ?? string.Empty);
        _matchToLeftRegex = new Regex(matchToLeftRegex ?? string.Empty);
        _rightDefault = rightDefault ?? string.Empty;
        _leftDefault = leftDefault ?? string.Empty;
    }
    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (_, originalLeft) => originalLeft
            ? Results.OnSuccess(originalLeft.Value)
            : Results.OnSuccess(string.Empty);

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (_, originalRight) => originalRight
            ? Results.OnSuccess(originalRight.Value)
            : Results.OnSuccess(string.Empty);

    public override Func<string, Result<string>> CreateRight =>
        left => Results.OnSuccess(_rightDefault);

    public override Func<string, Result<string>> CreateLeft =>
        right => Results.OnSuccess(_leftDefault);

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
