using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class IterateLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "John\nPaul\nAlice\nGeorge\nDicky\nStuart\nPete";

    protected override string _right => "John\nPaul\nAlice\nGeorge\nDicky\nStuart\nPete";

    protected override BaseSymmetricLens<string, string> _lens =>
        IterateLens.Cons(
            "\n",
            IdentityLens.Cons(@"\w+")
        );
}
