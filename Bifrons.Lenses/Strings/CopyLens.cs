using System.Text.RegularExpressions;

namespace Bifrons.Lenses;

public class CopyLens : BaseAsymmetricLens<string, string>
{
    private Regex _matchRegex;

    private CopyLens(string matchRegex)
    {
        _matchRegex = new Regex(matchRegex ?? string.Empty);
    }

    public override Func<string, Result<string>> Get =>
        source =>
        {
            var match = _matchRegex.Match(source);

            if (match.Success)
            {
                return Results.OnSuccess(match.Value);
            }
            else
            {
                return Results.OnFailure<string>("No match found");
            }
        };

    public override Func<string, Option<string>, Result<string>> Put =>
        (updatedView, originalSource) =>
        {
            var view = Get(originalSource.Value);
            if (originalSource)
            {
                if (view)
                {
                    var splits = originalSource.Value.Split(view.Data);
                    return Results.OnSuccess(splits[0] + updatedView + splits[1]);
                }
                else
                {
                    return Results.OnFailure<string>("No match found");
                }
            }
            else
            {
                if (view)
                {
                    return Results.OnSuccess(updatedView);
                }
                else
                {
                    return Results.OnFailure<string>("No match found");
                }
            }
        };

    public static CopyLens Create(string matchRegex) => new(matchRegex);
}
