using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Describes a merge (left and right) lens. Used to merge the sides of an OrLens.
/// Desides on the side to use based on the regex match.
/// <c>L : string <=> string</c>
/// </summary>
public class MergeLens : SymmetricStringLens
{
    private readonly OrLens _orLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="orLens">OrLens to merge</param>
    private MergeLens(OrLens orLens)
    {
        _orLens = orLens;
    }

    public override Regex LeftRegex => _orLens.LeftRegex;

    public override Regex RightRegex => _orLens.RightRegex;

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (updatedSource, originalTarget) =>
            (_orLens.IsLhsRightRegexMatch(updatedSource)
                ? _orLens.PutLeft(Either.Left<string, string>(updatedSource), originalTarget.Map(Either.Left<string, string>))
                : _orLens.IsRhsRightRegexMatch(updatedSource)
                    ? _orLens.PutLeft(Either.Right<string, string>(updatedSource), originalTarget.Map(Either.Right<string, string>))
                    : Results.Failure<Either<string, string>>($"Source string '{updatedSource}' does not match either regex."))
            .Map(either => either.Match(l => l, r => r));

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) =>
            (_orLens.IsLhsLeftRegexMatch(updatedSource)
                ? _orLens.PutRight(Either.Left<string, string>(updatedSource), originalTarget.Map(Either.Left<string, string>))
                : _orLens.IsRhsLeftRegexMatch(updatedSource)
                    ? _orLens.PutRight(Either.Right<string, string>(updatedSource), originalTarget.Map(Either.Right<string, string>))
                    : Results.Failure<Either<string, string>>($"Source string '{updatedSource}' does not match either regex."))
            .Map(either => either.Match(l => l, r => r));

    public override Func<string, Result<string>> CreateRight =>
        source =>
            (_orLens.IsLhsLeftRegexMatch(source)
                ? _orLens.CreateRight(Either.Left<string, string>(source))
                : _orLens.IsRhsLeftRegexMatch(source)
                    ? _orLens.CreateRight(Either.Right<string, string>(source))
                    : Results.Failure<Either<string, string>>($"Source string '{source}' does not match either regex."))
            .Map(either => either.Match(l => l, r => r));

    public override Func<string, Result<string>> CreateLeft =>
        source =>
            (_orLens.IsLhsRightRegexMatch(source)
                ? _orLens.CreateLeft(Either.Left<string, string>(source))
                : _orLens.IsRhsRightRegexMatch(source)
                    ? _orLens.CreateLeft(Either.Right<string, string>(source))
                    : Results.Failure<Either<string, string>>($"Source string '{source}' does not match either regex."))
            .Map(either => either.Match(l => l, r => r));

    /// <summary>
    /// Constructs a MergeLens from an OrLens
    /// </summary>
    /// <param name="orLens">OrLens to merge</param>
    public static MergeLens Cons(OrLens orLens)
        => new(orLens);
}
