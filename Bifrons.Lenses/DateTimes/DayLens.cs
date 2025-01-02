using System;

namespace Bifrons.Lenses.DateTimes;

/// <summary>
/// Defines a lens that focuses on the day of a <see cref="DateTime"/> object.
/// DateTime <=> int
/// </summary>
public class DayLens : ISymmetricLens<DateTime, int>
{
    private readonly DateTime _defaultDateTime;

    private DayLens(DateTime defaultDateTime)
    {
        _defaultDateTime = defaultDateTime;
    }

    public Func<int, Option<DateTime>, Result<DateTime>> PutLeft =>
        (updatedSource, originalTarget) => originalTarget.Match(
            target => updatedSource >= 1 && updatedSource <= DateTime.DaysInMonth(target.Year, target.Month)
            ? Result.Success(
                new DateTime(
                    target.Year,
                    target.Month,
                    updatedSource,
                    target.Hour,
                    target.Minute,
                    target.Second
                    ))
            : Result.Failure<DateTime>($"Day {updatedSource} is not valid for month of {target.Month}-{target.Year}."),
            () => updatedSource >= 1 && updatedSource <= DateTime.DaysInMonth(_defaultDateTime.Year, _defaultDateTime.Month)
            ? Result.Success(
                new DateTime(
                    _defaultDateTime.Year,
                    _defaultDateTime.Month,
                    updatedSource,
                    _defaultDateTime.Hour,
                    _defaultDateTime.Minute,
                    _defaultDateTime.Second
                    ))
            : Result.Failure<DateTime>($"Day {updatedSource} is not valid for month of {_defaultDateTime.Month}-{_defaultDateTime.Year}.")
            );

    public Func<DateTime, Option<int>, Result<int>> PutRight =>
        (updatedSource, _) => Result.Success(updatedSource.Day);

    public Func<DateTime, Result<int>> CreateRight =>
        source => Result.Success(source.Day);

    public Func<int, Result<DateTime>> CreateLeft =>
        day => day >= 1 && day <= DateTime.DaysInMonth(_defaultDateTime.Year, _defaultDateTime.Month)
            ? Result.Success(
            new DateTime(
                _defaultDateTime.Year,
                _defaultDateTime.Month,
                day,
                _defaultDateTime.Hour,
                _defaultDateTime.Minute,
                _defaultDateTime.Second
                ))
                : Result.Failure<DateTime>($"Day {day} is not valid for month of {_defaultDateTime.Month}-{_defaultDateTime.Year}.");

    /// <summary>
    /// Constructs a day lens.
    /// </summary>
    public static DayLens Cons(DateTime defaultDateTime) => new(defaultDateTime);

    /// <summary>
    /// Constructs a day lens.
    /// </summary>
    public static DayLens Cons() => new(DateTime.MinValue);

}
