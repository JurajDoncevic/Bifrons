namespace Bifrons.Lenses.DateTimes;

public abstract class DisconnectLens
    : SymmetricDateTimeLens
{
    public readonly static DateTime UNIT_VALUE = DateTime.MinValue;
    public readonly static DateTime INIT_VALUE = DateTime.MaxValue;

    protected readonly DateTime _leftDefault;
    protected readonly DateTime _rightDefault;

    protected DisconnectLens(DateTime leftDefault, DateTime rightDefault)
    {
        _leftDefault = leftDefault;
        _rightDefault = rightDefault;
    }

    public override Func<DateTime, Option<DateTime>, Result<DateTime>> PutLeft =>
    (updatedSource, originalTarget)
        => originalTarget.Match(
            target => Result.Success(target),
            () => CreateLeft(updatedSource)
            )!;

    public override Func<DateTime, Option<DateTime>, Result<DateTime>> PutRight =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
                )!;

    public override Func<DateTime, Result<DateTime>> CreateRight =>
        source => Result.Success(_rightDefault);

    public override Func<DateTime, Result<DateTime>> CreateLeft =>
        source => Result.Success(_leftDefault);
}

