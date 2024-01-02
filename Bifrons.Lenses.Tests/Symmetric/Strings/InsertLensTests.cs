using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class InsertLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "";

    protected override string _right => "Added";

    protected override BaseSymmetricLens<string, string> _lens => InsertLens.Cons("Added");
}
