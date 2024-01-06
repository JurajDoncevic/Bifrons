using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class VarDeleteLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "1234";

    protected override string _right => "";

    protected override BaseSymmetricLens<string, string> _lens =>
        VarDeleteLens.Cons(@"\d{4}", "0000");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => ("1234", "", "", "1234");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => ("", "0000", "1234", "");

    [Fact]
    public void CreateLeftDefault()
    {
        var result = _lens.CreateLeft("");
        Assert.True(result);
        Assert.Equal("0000", result.Data);
    }
}
