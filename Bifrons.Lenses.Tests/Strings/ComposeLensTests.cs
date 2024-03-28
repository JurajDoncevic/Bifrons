using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Strings.Tests;

public class ComposeLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "abcd1234";

    protected override string _right => "1234";

    protected override ISymmetricLens<string, string> _lens =>
        ComposeLens.Cons(
            IdentityLens.Cons(@"[a-z0-9]+"),
            IdentityLens.Cons(@"[0-9]+")
        );

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => ("abcd1234", "1234", "12345", "abcd12345");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => ("1234", "1234", "abcd12345", "12345");
}