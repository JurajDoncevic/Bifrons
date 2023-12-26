using Bifrons.Lenses.Symmetric.Strings;

namespace Bifrons.Lenses.Tests;

public class Experiments
{
    [Fact]
    public void CreateRight_WithConcatLens()
    {
        var lens = IdentityLens.Cons(@"\w+") |
            IdentityLens.Cons(@"\s+") |
            IdentityLens.Cons(@"(?!\w+\s+)\w+") |
            DeleteLens.Cons(@"(?!\w+\s+):\s+") |
            InsertLens.Cons(@",");

        var source = "Jane Doe: ";
        var expectedTarget = "Jane Doe,";

        var target = lens.CreateRight(source);

        Assert.Equal(expectedTarget, target);

    }

    [Fact]
    public void PutRight_OnConcatLens_WithNoOriginalSource()
    {
        var lens = IdentityLens.Cons(@"\w+") |
            IdentityLens.Cons(@"\s+") |
            IdentityLens.Cons(@"(?!\w+\s+)\w+") |
            DeleteLens.Cons(@"(?!\w+\s+):\s+") |
            InsertLens.Cons(@",");

        var source = "Jane Doe: ";
        var expectedTarget = "Jane Doe,";

        var target = lens.PutRight(source, Option.None<string>());

        Assert.Equal(expectedTarget, target);

    }
}
