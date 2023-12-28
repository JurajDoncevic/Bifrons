
using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Asymmetric.Strings;

/// <summary>
/// Basic asymmetric lens that gives a predetermined constant as view and ignores it in the put. The put returns the original source
/// As presented in https://www.cs.cornell.edu/~jnfoster/papers/jnfoster-dissertation.pdf p.23
/// </summary>
public sealed class ConstantLens : BaseAsymmetricLens<string, string>
{
    private readonly string _constant;
    private readonly Regex _matchRegex; // needed for the create function create = representative(Regex)

    private ConstantLens(string constant, string matchRegex)
    {
        _constant = constant ?? string.Empty;
        _matchRegex = new Regex(matchRegex ?? string.Empty);
    }

    public override Func<string, Option<string>, Result<string>> Put =>
        (updatedView, originalSource) => originalSource ? Results.OnSuccess(originalSource.Value) : Create(updatedView);

    public override Func<string, Result<string>> Get =>
        source => Results.OnSuccess(_constant);

    public override Func<string, Result<string>> Create =>
        (view) => Results.OnSuccess(Helpers.GetRepresentative(_matchRegex.ToString()));


    public static ConstantLens Cons(string constant, string matchRegex)
    {
        return new ConstantLens(constant, matchRegex ?? string.Empty);
    }
}
