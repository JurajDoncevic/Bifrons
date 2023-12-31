using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Describes an identity lens. The identity lens propagates tha data that matches the regex R.
/// <c>id(R)</c>
/// </summary>
public sealed class IdentityLens : SymmetricStringLens
{
    private readonly Regex _identityRegex;

    public override Regex LeftRegex => _identityRegex;
    public override Regex RightRegex => _identityRegex;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="identityRegexString">Identity regex string <c>R</c></param>
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

    /// <summary>
    /// Constructs an identity lens.
    /// </summary>
    /// <param name="identityRegexString">Identity regex string <c>R</c>; defaults to <c>.*</c></param>
    public static IdentityLens Cons(string identityRegexString = @".*") => new(identityRegexString);
}
