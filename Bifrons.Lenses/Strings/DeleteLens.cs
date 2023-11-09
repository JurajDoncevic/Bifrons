
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
        if (!originalSource)
        {
            return Create(updatedView);
        }

        var view = Get(originalSource.Value);

        if (!view)
        {
            return view;
        }

        var firstIndex = originalSource.Value.IndexOf(view.Data);

        if (firstIndex == -1)
        {
            return Results.OnFailure<string>("View not found in original source");
        }

        var lastIndex = firstIndex + view.Data.Length;

        var result = originalSource.Value.Substring(0, firstIndex) + updatedView + originalSource.Value.Substring(lastIndex);

        return Results.OnSuccess(result);
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

    public override Func<string, Result<string>> Create => 
        view => Results.OnFailure<string>("Not implemented representative for regex");

    public static DeleteLens Cons(string matchRegex) => new(matchRegex ?? string.Empty);
}
