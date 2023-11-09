
using System.Text.RegularExpressions;

namespace Bifrons.Lenses;

/// <summary>
/// Basic asymmetric lens that gives a predetermined constant as view and ignores it in the put. The put returns the original source
/// As presented in https://www.cs.cornell.edu/~jnfoster/papers/jnfoster-dissertation.pdf p.24
/// </summary>
public sealed class DeleteLens : BaseAsymmetricLens<string, string>
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

            var firstIndex = originalSource.Value.IndexOf(view.Data);
            var lastIndex = firstIndex + view.Data.Length;

            var result = updatedView;

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

    public static DeleteLens Create(string matchRegex)
    {
        return new DeleteLens(matchRegex ?? string.Empty);
    }
}
