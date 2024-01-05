using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class ComposeLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "abcd1234";

    protected override string _right => "1234";

    protected override BaseSymmetricLens<string, string> _lens =>
        ComposeLens.Cons(
            IdentityLens.Cons(@"[a-z0-9]+"),
            IdentityLens.Cons(@"[0-9]+")
        );
}