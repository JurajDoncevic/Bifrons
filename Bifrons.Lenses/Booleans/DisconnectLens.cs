namespace Bifrons.Lenses.Booleans;

public abstract class DisconnectLens
    : SymmetricBooleanLens
{
    public const bool UNIT_VALUE = false;
    public const bool INIT_VALUE = false;

    private readonly bool _leftDefault;
    private readonly bool _rightDefault;

    protected DisconnectLens(bool leftDefault, bool rightDefault)
    {
        _leftDefault = leftDefault;
        _rightDefault = rightDefault;
    }

    public override Func<bool, Option<bool>, Result<bool>> PutLeft =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
                )!;

    public override Func<bool, Option<bool>, Result<bool>> PutRight =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
                )!;

    public override Func<bool, Result<bool>> CreateRight =>
        source => Result.Success(_rightDefault);

    public override Func<bool, Result<bool>> CreateLeft =>
        source => Result.Success(_leftDefault);
}
