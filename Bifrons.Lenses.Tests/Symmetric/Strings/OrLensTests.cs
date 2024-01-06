using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class OrLensTests : SymmetricLensTestingFramework<Either<string, string>, Either<string, string>>
{
    protected override Either<string, string> _left => Either.Left<string, string>("John");

    protected override Either<string, string> _right => Either.Right<string, string>("123");

    protected override BaseSymmetricLens<Either<string, string>, Either<string, string>> _lens =>
        IdentityLens.Cons(@"\w+") | IdentityLens.Cons(@"\d+");

    protected override (Either<string, string> originalSource, Either<string, string> expectedOriginalTarget, Either<string, string> updatedTarget, Either<string, string> expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => (Either.Left<string, string>("John"),
            Either.Left<string, string>("John"),
            Either.Left<string, string>("James"),
            Either.Left<string, string>("James"));

    protected override (Either<string, string> originalSource, Either<string, string> expectedOriginalTarget, Either<string, string> updatedTarget, Either<string, string> expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (Either.Right<string, string>("1234"),
            Either.Right<string, string>("1234"),
            Either.Right<string, string>("12345"),
            Either.Right<string, string>("12345"));
}
