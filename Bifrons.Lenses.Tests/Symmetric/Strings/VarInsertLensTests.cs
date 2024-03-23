using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class VarInsertLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "";

    protected override string _right => "1234";

    protected override ISimpleSymmetricLens<string, string> _lens =>
        VarInsertLens.Cons(@"\d{4}", "0000");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => ("", "0000", "1234", "");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => ("1234", "", "", "1234");

    [Fact]
    public void CreateRightDefault()
    {
        var result = _lens.CreateRight("");
        Assert.True(result);
        Assert.Equal("0000", result.Data);
    }
}