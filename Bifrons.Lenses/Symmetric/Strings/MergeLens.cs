using System.Text.RegularExpressions;
using Bifrons.Lenses.Symmetric.Strings;

namespace Bifrons.Lenses;

public class MergeLens : SymmetricStringLens
{
    private readonly OrLens _orLens;

    public MergeLens(OrLens orLens)
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
                    : Results.OnFailure<Either<string, string>>($"Source string '{updatedSource}' does not match either regex."))
            .Map(either => either.Match(l => l, r => r));

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) =>
            (_orLens.IsLhsLeftRegexMatch(updatedSource)
                ? _orLens.PutRight(Either.Left<string, string>(updatedSource), originalTarget.Map(Either.Left<string, string>))
                : _orLens.IsRhsLeftRegexMatch(updatedSource)
                    ? _orLens.PutRight(Either.Right<string, string>(updatedSource), originalTarget.Map(Either.Right<string, string>))
                    : Results.OnFailure<Either<string, string>>($"Source string '{updatedSource}' does not match either regex."))
            .Map(either => either.Match(l => l, r => r));

    public override Func<string, Result<string>> CreateRight =>
        source =>
            (_orLens.IsLhsLeftRegexMatch(source)
                ? _orLens.CreateRight(Either.Left<string, string>(source))
                : _orLens.IsRhsLeftRegexMatch(source)
                    ? _orLens.CreateRight(Either.Right<string, string>(source))
                    : Results.OnFailure<Either<string, string>>($"Source string '{source}' does not match either regex."))
            .Map(either => either.Match(l => l, r => r));

    public override Func<string, Result<string>> CreateLeft =>
        source =>
            (_orLens.IsLhsRightRegexMatch(source)
                ? _orLens.CreateLeft(Either.Left<string, string>(source))
                : _orLens.IsRhsRightRegexMatch(source)
                    ? _orLens.CreateLeft(Either.Right<string, string>(source))
                    : Results.OnFailure<Either<string, string>>($"Source string '{source}' does not match either regex."))
            .Map(either => either.Match(l => l, r => r));

    public static MergeLens Cons(OrLens orLens)
        => new(orLens);
}
