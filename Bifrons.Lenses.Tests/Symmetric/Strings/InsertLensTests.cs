using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class InsertLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "Some left text";

    protected override string _right => "test";

    protected override string _updatedLeft => "Some left updated text";

    protected override string _updatedRight => "Some right updated text";

    protected override BaseSymmetricLens<string, string> _lens => InsertLens.Cons("test");
}
