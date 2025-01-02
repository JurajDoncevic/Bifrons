using System;

namespace Bifrons.Lenses.DateTimes;


/// <summary>
/// Defines a lens that focuses on the month of a <see cref="DateTime"/> object.
/// DateTime <=> int
/// </summary>
public sealed class MonthLens : ISymmetricLens<DateTime, int>
{
    private readonly DateTime _defaultDateTime;

    private MonthLens(DateTime defaultDateTime)
    {
        _defaultDateTime = defaultDateTime;
    }

    public Func<int, Option<DateTime>, Result<DateTime>> PutLeft =>
        (updatedSource, originalTarget) => originalTarget.Match(
            target => updatedSource >= 1 && updatedSource <= 12
            ? Result.Success(
                new DateTime(
                    target.Year,
                    updatedSource,
                    target.Day,
                    target.Hour,
                    target.Minute,
                    target.Second
                    ))
            : Result.Failure<DateTime>($"Month {updatedSource} is not valid."),
            () => updatedSource >= 1 && updatedSource <= 12
            ? Result.Success(
                new DateTime(
                    _defaultDateTime.Year,
                    updatedSource,
                    _defaultDateTime.Day,
                    _defaultDateTime.Hour,
                    _defaultDateTime.Minute,
                    _defaultDateTime.Second
                    ))
            : Result.Failure<DateTime>($"Month {updatedSource} is not valid.")
            );

    public Func<DateTime, Option<int>, Result<int>> PutRight =>
        (updatedSource, _) => Result.Success(updatedSource.Month);

    public Func<DateTime, Result<int>> CreateRight =>
        source => Result.Success(source.Month);

    public Func<int, Result<DateTime>> CreateLeft =>
        month =>
            month >= 1 && month <= 12
            ? Result.Success(
                new DateTime(
                    _defaultDateTime.Year,
                    month,
                    _defaultDateTime.Day,
                    _defaultDateTime.Hour,
                    _defaultDateTime.Minute,
                    _defaultDateTime.Second
                    ))
            : Result.Failure<DateTime>($"Month {month} is not valid.");

    /// <summary>
    /// Constructs a month lens.
    /// </summary>
    public static MonthLens Cons(DateTime defaultDateTime) => new(defaultDateTime);

    /// <summary>
    /// Constructs a month lens.
    /// </summary>
    public static MonthLens Cons() => new(DateTime.MinValue);
}
