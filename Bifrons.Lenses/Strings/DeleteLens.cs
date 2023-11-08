
using System.Text.RegularExpressions;

namespace Bifrons.Lenses;

public class DeleteLens : BaseAsymmetricLens<string, string>
{
    private Regex _matchRegex;

    private DeleteLens(string matchRegex)
    {
        _matchRegex = new Regex(matchRegex ?? string.Empty);
    }

    public override Func<string, Option<string>, Result<string>> Put =>
        (updatedView, originalSource) =>
        {
            var view = Get(originalSource.Value);
            if (!view || !originalSource)
            {
                return view;
            }
            return Results.OnFailure<string>("Not implemented");
        };

    public override Func<string, Result<string>> Get =>
        source =>
        {
            var match = _matchRegex.Match(source);

            if (match.Success)
            {
                var view = source.Replace(match.Value, string.Empty);
                return Results.OnSuccess(view);
            }
            else
            {
                return Results.OnFailure<string>("No match found");
            }
        };
}
