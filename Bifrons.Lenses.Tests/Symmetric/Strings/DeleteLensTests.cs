using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class DeleteLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "test";

    protected override string _right => "";

    protected override BaseSymmetricLens<string, string> _lens => DeleteLens.Cons("test");
}
