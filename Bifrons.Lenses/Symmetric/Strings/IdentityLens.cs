using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

public sealed class IdentityLens : SymmetricStringLens
{
    private readonly Regex _identityRegex;
    private IdentityLens(string identityRegexString)
    {
        _identityRegex = new Regex(identityRegexString ?? @".*");
    }

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (right, _) => Results.OnSuccess(_identityRegex.Match(right).Value);

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (left, _) => Results.OnSuccess(_identityRegex.Match(left).Value);

    public override Func<string, Result<string>> CreateRight =>
        left => Results.OnSuccess(_identityRegex.Match(left).Value);

    public override Func<string, Result<string>> CreateLeft =>
        right => Results.OnSuccess(_identityRegex.Match(right).Value);

    public static IdentityLens Cons(string identityRegexString = @".*") => new(identityRegexString);
}
