
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Asymmetric.Strings.Tests;

public class ConstantLensTests : AsymmetricLensTestingFramework<string, string>
{
    protected override string _source => "Hello, World!";
    protected override string _view => "Hello!";
    protected override string _updatedView => "Hello!";
    protected override string _updatedSource => "Hello, World!";

    protected override BaseAsymmetricLens<string, string> _lens => ConstantLens.Cons("Hello!", @"\w+!");
}
