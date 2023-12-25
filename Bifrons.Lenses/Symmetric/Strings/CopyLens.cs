using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

public sealed class CopyLens : BaseSymmetricLens<string, string>
{
    private readonly Regex _matchToRightRegex;
    private readonly Regex _matchToLeftRegex;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="matchToRightRegex">Regex string that is used for PutR</param>
    /// <param name="matchToLeftRegex">Regex string that is used for PutL</param>
    private CopyLens(string matchToRightRegex, string matchToLeftRegex)
    {
        _matchToRightRegex = new Regex(matchToRightRegex ?? string.Empty);
        _matchToLeftRegex = new Regex(matchToLeftRegex ?? string.Empty);
    }

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (updatedRight, originalLeft) =>
        {
            var defaultRight = originalLeft.Bind(left => CreateRight(left).ToOption());
            if (originalLeft)
            {
                if (defaultRight)
                {
                    var matched = _matchToLeftRegex.Match(updatedRight);
                    return Results.OnSuccess(originalLeft.Value.Replace(defaultRight.Value, matched.Value));
                }
                else
                {
                    return Results.OnFailure<string>("No match found");
                }
            }
            else
            {
                if (defaultRight)
                {
                    var matched = _matchToLeftRegex.Match(updatedRight);
                    return Results.OnSuccess(matched.Value);
                }
                else
                {
                    return Results.OnFailure<string>("No match found");
                }
            }
        };

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (updatedLeft, originalRight) =>
        {
            var defaultLeft = originalRight.Bind(right => CreateLeft(right).ToOption());
            if (originalRight)
            {
                if (defaultLeft)
                {
                    var matched = _matchToRightRegex.Match(updatedLeft);
                    return Results.OnSuccess(originalRight.Value.Replace(defaultLeft.Value, matched.Value));
                }
                else
                {
                    return Results.OnFailure<string>("No match found");
                }
            }
            else
            {
                if (defaultLeft)
                {
                    var matched = _matchToRightRegex.Match(updatedLeft);
                    return Results.OnSuccess(matched.Value);
                }
                else
                {
                    return Results.OnFailure<string>("No match found");
                }
            }
        };

    public override Func<string, Result<string>> CreateRight =>
        left =>
        {
            var match = _matchToRightRegex.Match(left);

            if (match.Success)
            {
                return Results.OnSuccess(match.Value);
            }
            else
            {
                return Results.OnFailure<string>("No match found");
            }
        };

    public override Func<string, Result<string>> CreateLeft =>
        right =>
        {
            var match = _matchToLeftRegex.Match(right);

            if (match.Success)
            {
                return Results.OnSuccess(match.Value);
            }
            else
            {
                return Results.OnFailure<string>("No match found");
            }
        };

    /// <summary>
    /// Constructs a Copy lens
    /// </summary>
    /// <param name="matchToRightRegex">Regex string for PutR</param>
    /// <param name="matchToLeftRegex">Regex string for PutL</param>
    /// <returns>CopyLens instance</returns>
    public static CopyLens Cons(string matchToRightRegex, string matchToLeftRegex) => new(matchToRightRegex, matchToLeftRegex);
}
