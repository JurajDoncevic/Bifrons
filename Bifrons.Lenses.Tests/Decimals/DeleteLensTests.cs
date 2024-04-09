using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Decimals.Tests;

public class DeleteLensTests : SymmetricLensTestingFramework<double, double>
{
    protected override double _left => 42.0;

    protected override double _right => DeleteLens.UNIT_VALUE;

    private readonly double _updatedValue = 23.0;

    protected override (double originalSource, double expectedOriginalTarget, double updatedTarget, double expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _left);

    protected override (double originalSource, double expectedOriginalTarget, double updatedTarget, double expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedValue, _right);

    protected override ISymmetricLens<double, double> _lens => DeleteLens.Cons(_left);
}
