
namespace Bifrons.Lenses.Symmetric.Strings;

public sealed class ConcatLens : BaseSymmetricLens<string, string>
{
    private readonly BaseSymmetricLens<string, string> _leftLens;
    private readonly BaseSymmetricLens<string, string> _rightLens;

    private ConcatLens(BaseSymmetricLens<string, string> leftLens, BaseSymmetricLens<string, string> rightLens)
    {
        _leftLens = leftLens;
        _rightLens = rightLens;
    }

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (updatedRight, originalLeft) =>
        {
            var defaultLeft = originalLeft
                .Match(
                    res => res,
                    () => CreateLeft(updatedRight).Data ?? string.Empty
                );
            var defaultRight = originalLeft
                .Match(
                    res => CreateRight(res)
                        .Match(
                            res => res,
                            _ => string.Empty
                        ),
                    () => CreateRight(defaultLeft)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );

            var result = _leftLens.PutLeft(updatedRight, originalLeft)
                .Bind(leftLensRes => _rightLens.PutLeft(updatedRight, originalLeft)
                                        .Map(rightLensRes => leftLensRes + rightLensRes)
                                        .Map(res => defaultLeft.Replace(defaultRight, res)));

            return result;
        };

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (updatedLeft, originalRight) =>
        {
            var defaultRight = originalRight
                .Match(
                    res => res,
                    () => CreateRight(updatedLeft).Data ?? string.Empty
                );
            var defaultLeft = originalRight
                .Match(
                    res => CreateLeft(res)
                        .Match(
                            res => res,
                            _ => string.Empty
                        ),
                    () => CreateLeft(defaultRight)
                        .Match(
                            res => res,
                            _ => string.Empty
                        )
                );
            var result = _rightLens.PutRight(updatedLeft, originalRight)
                .Bind(rightLensRes => _leftLens.PutRight(updatedLeft, originalRight)
                                        .Map(leftLensRes => leftLensRes + rightLensRes)
                                        .Map(res => defaultRight.Replace(defaultLeft, res)));

            return result;
        };

    public override Func<string, Result<string>> CreateRight =>
        originalLeft =>
        {
            var result = _leftLens.CreateRight(originalLeft)
                .Bind(leftLensRes => _rightLens.CreateRight(originalLeft)
                                        .Map(rightLensRes => leftLensRes + rightLensRes));

            return result;
        };

    public override Func<string, Result<string>> CreateLeft =>
        originalRight =>
        {
            var result = _rightLens.CreateLeft(originalRight)
                .Bind(rightLensRes => _leftLens.CreateLeft(originalRight)
                                        .Map(leftLensRes => leftLensRes + rightLensRes));

            return result;
        };

    public static ConcatLens Cons(BaseSymmetricLens<string, string> leftLens, BaseSymmetricLens<string, string> rightLens)
        => new(leftLens, rightLens);
}
