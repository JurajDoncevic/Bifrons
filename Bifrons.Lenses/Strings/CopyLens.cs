using System.Text.RegularExpressions;

namespace Bifrons.Lenses;

public class CopyLens : BaseAsymmetricLens<string, string>
{

    private Regex _matchRegex;

    public CopyLens(string matchRegex)
    {
        _matchRegex = new Regex(matchRegex ?? string.Empty);
    }

    public override Func<string, Result<string>> Get => 
        source => Results.OnFailure<string>("Not implemented");
    public override Func<string, Option<string>, Result<string>> Put => 
        (updatedView, originalSource) => Results.OnFailure<string>("Not implemented");
}
