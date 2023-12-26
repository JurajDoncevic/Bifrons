
using Bifrons.Lenses.Symmetric.Strings;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class ConcatLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "Jane Doe";

    protected override string _right => "Jane ";

    protected override string _updatedLeft => "Janine Doe";

    protected override string _updatedRight => "Janine ";

    protected override BaseSymmetricLens<string, string> _lens
        => ConcatLens.Cons(
            IdentityLens.Cons(@"\w+"),
            IdentityLens.Cons(@" ")
        );
}
