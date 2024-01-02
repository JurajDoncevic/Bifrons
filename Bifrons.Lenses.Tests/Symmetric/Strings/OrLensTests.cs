using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class OrLensTests : SymmetricLensTestingFramework<Either<string, string>, Either<string, string>>
{
    protected override Either<string, string> _left => Either.Left<string, string>("John");

    protected override Either<string, string> _right => Either.Right<string, string>("123");

    protected override BaseSymmetricLens<Either<string, string>, Either<string, string>> _lens =>
        IdentityLens.Cons(@"\w+") | IdentityLens.Cons(@"\d+");
}
