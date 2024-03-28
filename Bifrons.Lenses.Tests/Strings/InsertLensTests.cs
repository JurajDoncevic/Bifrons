using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Strings.Tests;

public class InsertLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "";

    protected override string _right => "Added";

    protected override ISymmetricLens<string, string> _lens => InsertLens.Cons("Added");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => ("", "Added", "Added 1234", "");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => ("Added", "", "Anything ", "Added");
}
