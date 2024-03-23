using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Describes a date-time-string lens that tranforms date-times to strings and vice versa.
/// DateTimeStr : DateTime <=> string
/// </summary>
public sealed class DateTimeStringLens : ISimpleSymmetricLens<DateTime, string>
{
    private readonly Regex _dateTimeRegex;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dateTimeRegexString">The regex string to use for date-time matching</param>
    private DateTimeStringLens(string dateTimeRegexString)
    {
        _dateTimeRegex = new Regex(dateTimeRegexString);
    }

    public Func<string, Option<DateTime>, Result<DateTime>> PutLeft =>
        (updatedSource, _) =>
        {
            var match = _dateTimeRegex.Match(updatedSource);
            if (!match.Success)
            {
                return Result.Failure<DateTime>("No date-time found in string");
            }
            return Result.Success(DateTime.Parse(match.Value));
        };

    public Func<DateTime, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateRight(updatedSource);
            }

            var updatedSourceString = updatedSource.ToString();

            return Result.AsResult<string>(() => _dateTimeRegex.Replace(originalTarget.Value, updatedSourceString, 1));
        };

    public Func<DateTime, Result<string>> CreateRight =>
        source => Result.Success(source.ToString());

    public Func<string, Result<DateTime>> CreateLeft =>
        source =>
        {
            var match = _dateTimeRegex.Match(source);
            if (!match.Success)
            {
                return Result.Failure<DateTime>("No date-time found in string");
            }
            return Result.Success(DateTime.Parse(match.Value));
        };

    /// <summary>
    /// Constructs a date-time-string lens
    /// </summary>
    /// <param name="dateTimeRegexString">The regex string to use for date-time matching</param>
    public static DateTimeStringLens Cons(string dateTimeRegexString = @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}")
        => new(dateTimeRegexString);
}

/// <summary>
/// Describes a string-date-time lens that tranforms strings to date-times and vice versa.
/// StrDateTime : string <=> DateTime
/// </summary>
public sealed class StringDateTimeLens : InvertLens<string, DateTime>
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dateTimeRegexString">The regex string to use for date-time matching</param>
    private StringDateTimeLens(string dateTimeRegexString) : base(DateTimeStringLens.Cons(dateTimeRegexString))
    {
    }

    /// <summary>
    /// Constructs a string-date-time lens
    /// </summary>
    /// <param name="dateTimeRegexString">The regex string to use for date-time matching</param>
    public static StringDateTimeLens Cons(string dateTimeRegexString = @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}")
        => new(dateTimeRegexString);
}
