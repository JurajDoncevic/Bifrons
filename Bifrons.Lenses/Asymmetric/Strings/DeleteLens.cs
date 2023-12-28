
using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Asymmetric.Strings;

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

        var match = _matchRegex.Match(originalSource.Value);

        if (!match.Success)
        {
            return Results.OnFailure<string>("View not found in original source");
        }

        var prefixChar = match.Index == 0 ? Option.None<char>() : Option.Some(originalSource.Value[match.Index - 1]);
        var suffixChar = match.Index + match.Length == originalSource.Value.Length ? Option.None<char>() : Option.Some(originalSource.Value[match.Index + match.Length]);

        var prefixIndex = updatedView.LastIndexOf(prefixChar.Value);
        var suffixIndex = updatedView.IndexOf(suffixChar.Value, prefixIndex + 1);

        if (prefixIndex >= 0 && suffixIndex >= 0)
        {
            var matchValue = originalSource.Value.Substring(match.Index, match.Length);
            var prefixLength = prefixIndex + 1;
            var suffixLength = suffixIndex - prefixLength;

            var result = updatedView.Substring(0, prefixLength) + matchValue + updatedView.Substring(prefixLength + suffixLength);
            return Results.OnSuccess(result);
        }
        else
        {
            return Results.OnFailure<string>("Regex prefix or suffix character not found in updated view. No position for deleted data found for updated source");
        }

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
        view => Results.OnSuccess<string>(view);

    public static DeleteLens Cons(string matchRegex) => new(matchRegex ?? string.Empty);
}
