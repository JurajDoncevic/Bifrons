using System;

namespace Bifrons.Lenses.DateTimes;

/// <summary>
/// Defines a lens that focuses on the date part of a <see cref="DateTime"/> object.
/// DateTime <=> DateOnly
/// </summary>
public sealed class DateLens : ISymmetricLens<DateTime, DateOnly>
{

    public Func<DateOnly, Option<DateTime>, Result<DateTime>> PutLeft =>
        (updatedSource, originalTarget) => originalTarget.Match(
            target => Result.Success(new DateTime(updatedSource.Year, updatedSource.Month, updatedSource.Day, target.Hour, target.Minute, target.Second)),
            () => Result.Success(new DateTime(updatedSource.Year, updatedSource.Month, updatedSource.Day))
            );

    public Func<DateTime, Option<DateOnly>, Result<DateOnly>> PutRight =>
        (updatedSource, _) => Result.Success(new DateOnly(updatedSource.Year, updatedSource.Month, updatedSource.Day));

    public Func<DateTime, Result<DateOnly>> CreateRight =>
        source => Result.Success(new DateOnly(source.Year, source.Month, source.Day));

    public Func<DateOnly, Result<DateTime>> CreateLeft =>
        source => Result.Success(new DateTime(source.Year, source.Month, source.Day));

    /// <summary>
    /// Constructs a date lens.
    /// </summary>
    public static DateLens Cons() => new();
}
