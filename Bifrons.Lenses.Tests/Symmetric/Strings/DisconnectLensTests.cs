using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class DisconnectLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "15000kn";

    protected override string _right => "";

    private readonly string _salaryRegex = @"[1-9][0-9]*kn";
    private readonly string _anythingRegex = @"";

    protected override BaseSymmetricLens<string, string> _lens => DisconnectLens.Cons(_salaryRegex, _anythingRegex, "unk", "");
}
