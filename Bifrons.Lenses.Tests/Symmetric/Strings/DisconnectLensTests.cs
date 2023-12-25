using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class DisconnectLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "15000kn";

    protected override string _right => "unk";

    protected override string _updatedLeft => "10000kn";

    protected override string _updatedRight => "unknown";

    protected override BaseSymmetricLens<string, string> _lens => DisconnectLens.Cons(@"\+dkn", "", "unk", "");
}
