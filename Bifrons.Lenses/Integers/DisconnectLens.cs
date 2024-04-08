namespace Bifrons.Lenses.Integers;

public abstract class DisconnectLens
    : SymmetricIntegerLens
{
    public const int UNIT_VALUE = 0;
    public const int INIT_VALUE = int.MaxValue;

    private readonly int _leftDefault;
    private readonly int _rightDefault;

    protected DisconnectLens(int leftDefault, int rightDefault)
    {
        _leftDefault = leftDefault;
        _rightDefault = rightDefault;
    }

    public override Func<int, Option<int>, Result<int>> PutLeft => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
                )!;

    public override Func<int, Option<int>, Result<int>> PutRight => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
                )!;

    public override Func<int, Result<int>> CreateRight => 
        source => Result.Success(_rightDefault);

    public override Func<int, Result<int>> CreateLeft => 
        source => Result.Success(_leftDefault);
}
