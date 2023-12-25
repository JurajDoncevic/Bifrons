using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class DeleteLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "Some left test text";

    protected override string _right => "Some right test text";

    protected override string _updatedLeft => "Some left updated text";

    protected override string _updatedRight => "Some right updated text";

    protected override BaseSymmetricLens<string, string> _lens => DeleteLens.Cons("test");
}
