
namespace Bifrons.Lenses.Longs;

public abstract class DisconnectLens
    : SymmetricLongLens
{
    public const long UNIT_VALUE = 0;
    public const long INIT_VALUE = long.MaxValue;

    private readonly long _leftDefault;
    private readonly long _rightDefault;

    protected DisconnectLens(long leftDefault, long rightDefault)
    {
        _leftDefault = leftDefault;
        _rightDefault = rightDefault;
    }

    public override Func<long, Option<long>, Result<long>> PutLeft => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
                )!;

    public override Func<long, Option<long>, Result<long>> PutRight => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
                )!;

    public override Func<long, Result<long>> CreateRight => 
        source => Result.Success(_rightDefault);

    public override Func<long, Result<long>> CreateLeft => 
        source => Result.Success(_leftDefault);
}
