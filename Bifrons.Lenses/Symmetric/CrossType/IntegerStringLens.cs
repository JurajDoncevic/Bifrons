using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.CrossType;

/// <summary>
/// Describes an integer-string lens that tranforms integers to strings and vice versa.
/// IntStr : int <=> string
public sealed class IntegerStringLens : ISymmetricLens<int, string>
{
    private readonly Regex _numberRegex = new(@"-?\d+");

    /// <summary>
    /// Constructor
    /// </summary>
    private IntegerStringLens()
    {
    }

    public Func<string, Option<int>, Result<int>> PutLeft =>
        (updatedSource, _) =>
        {
            var match = _numberRegex.Match(updatedSource);
            if (!match.Success)
            {
                return Result.Failure<int>("No integer found in string");
            }
            return Result.Success(int.Parse(match.Value));
        };

    public Func<int, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateRight(updatedSource);
            }

            var updatedSourceString = updatedSource.ToString();

            return Result.AsResult<string>(() => _numberRegex.Replace(originalTarget.Value, updatedSourceString, 1));
        };

    public Func<int, Result<string>> CreateRight =>
        source => Result.Success(source.ToString());

    public Func<string, Result<int>> CreateLeft =>
        source =>
        {
            var match = _numberRegex.Match(source);
            if (!match.Success)
            {
                return Result.Failure<int>("No integer found in string");
            }
            return Result.Success(int.Parse(match.Value));
        };

    /// <summary>
    /// Constructs an integer-string lens
    /// </summary>
    public static IntegerStringLens Cons() => new();
}

/// <summary>
/// Describes an integer-string lens that tranforms strings to integers and vice versa.
/// StrInt() : string <=> int
/// </summary>
public sealed class StringIntegerLens : InvertLens<string, int>
{
    /// <summary>
    /// Constructor
    /// </summary>
    private StringIntegerLens() : base(IntegerStringLens.Cons())
    {
    }

    /// <summary>
    /// Constructs an integer-string lens
    /// </summary>
    public static StringIntegerLens Cons() => new();
}
