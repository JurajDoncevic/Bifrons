using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Decimals.Tests;

public sealed class IdentityLensTests : SymmetricLensTestingFramework<double, double>
{
    protected override double _left => 42.0;

    protected override double _right => 42.0;

    private readonly double _updatedValue = 23.0;

    protected override (double originalSource, double expectedOriginalTarget, double updatedTarget, double expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _updatedValue);

    protected override (double originalSource, double expectedOriginalTarget, double updatedTarget, double expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left,  _updatedValue, _updatedValue);

    protected override ISymmetricLens<double, double> _lens => IdentityLens.Cons();
}
