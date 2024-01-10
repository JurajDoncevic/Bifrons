using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.CrossType;

public sealed class IntegerStringLens : BaseSymmetricLens<int, string>
{
    private readonly Regex _numberRegex = new(@"-?\d+");

    private IntegerStringLens()
    {
    }

    public override Func<string, Option<int>, Result<int>> PutLeft =>
        (updatedSource, _) =>
        {
            var match = _numberRegex.Match(updatedSource);
            if (!match.Success)
            {
                return Results.OnFailure<int>("No integer found in string");
            }
            return Results.OnSuccess(int.Parse(match.Value));
        };

    public override Func<int, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateRight(updatedSource);
            }

            var updatedSourceString = updatedSource.ToString();

            return Results.AsResult<string>(() => _numberRegex.Replace(originalTarget.Value, updatedSourceString, 1));
        };

    public override Func<int, Result<string>> CreateRight =>
        source => Results.OnSuccess(source.ToString());

    public override Func<string, Result<int>> CreateLeft =>
        source =>
        {
            var match = _numberRegex.Match(source);
            if (!match.Success)
            {
                return Results.OnFailure<int>("No integer found in string");
            }
            return Results.OnSuccess(int.Parse(match.Value));
        };
    
    public static IntegerStringLens Cons() => new();
}

public sealed class StringIntegerLens : InvertLens<string, int>
{
    private readonly Regex _numberRegex = new(@"-?\d+");

    private StringIntegerLens() : base(IntegerStringLens.Cons())
    {
    }

    public static StringIntegerLens Cons() => new();
}
