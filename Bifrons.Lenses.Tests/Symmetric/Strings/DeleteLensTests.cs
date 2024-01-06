using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class DeleteLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "delete this";

    protected override string _right => "";

    protected override BaseSymmetricLens<string, string> _lens => DeleteLens.Cons("delete this");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => ("delete this", "", "added ", "delete this");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => ("", "delete this", "delete this", "");
}
