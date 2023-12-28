using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Asymmetric.Strings.Tests;

public class DeleteLensTests : AsymmetricLensTestingFramework<string, string>
{
    protected override string _source => "Jean Sibelius, 1865-1957, Finnish";

    protected override string _view => "Jean Sibelius, Finnish";

    protected override string _updatedView => "Benjamin Britten, English";

    protected override string _updatedSource => "Benjamin Britten, 1865-1957, English";

    protected override BaseAsymmetricLens<string, string> _lens => DeleteLens.Cons(@"\d{4}-\d{4}, ");


}
