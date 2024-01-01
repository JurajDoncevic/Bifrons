using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class ConcatLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "12345;Jane Doe";

    protected override string _right => "Jane ";

    protected override string _updatedLeft => "12345;Janine Doe";

    protected override string _updatedRight => "Janine ";

    protected override BaseSymmetricLens<string, string> _lens
        => IdentityLens.Cons(@"[a-zA-Z]+") | IdentityLens.Cons(@" ");
}
