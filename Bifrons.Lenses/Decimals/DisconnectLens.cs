namespace Bifrons.Lenses.Decimals;

public abstract class DisconnectLens
    : SymmetricDecimalLens
{
    public const double UNIT_VALUE = 0.0;
    public const double INIT_VALUE = double.MaxValue;

    protected readonly double _leftDefault;
    protected readonly double _rightDefault;

    protected DisconnectLens(double leftDefault, double rightDefault)
    {
        _leftDefault = leftDefault;
        _rightDefault = rightDefault;
    }

    public override Func<double, Option<double>, Result<double>> PutLeft =>
    (updatedSource, originalTarget)
        => originalTarget.Match(
            target => Result.Success(target),
            () => CreateLeft(updatedSource)
            )!;

    public override Func<double, Option<double>, Result<double>> PutRight =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
                )!;

    public override Func<double, Result<double>> CreateRight =>
        source => Result.Success(_rightDefault);

    public override Func<double, Result<double>> CreateLeft =>
        source => Result.Success(_leftDefault);
}
