using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class VarInsertLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "";

    protected override string _right => "1234";

    protected override BaseSymmetricLens<string, string> _lens =>
        VarInsertLens.Cons(@"\d+", "0000");

    [Fact]
    public void CreateRightDefault()
    {
        var result = _lens.CreateRight("");
        Assert.True(result);
        Assert.Equal("0000", result.Data);
    }
}