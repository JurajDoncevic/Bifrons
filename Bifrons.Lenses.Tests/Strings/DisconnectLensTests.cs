using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Strings.Tests;

public class DisconnectLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "15000€";

    protected override string _right => "";

    private readonly string _salaryRegex = @"[1-9][0-9]*€";
    private readonly string _anythingRegex = @"";

    protected override ISymmetricLens<string, string> _lens => DisconnectLens.Cons(_salaryRegex, _anythingRegex, "unk", "");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => ("15000€", "", "anything ", "15000€");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => ("", "unk", "unk", "");
}
