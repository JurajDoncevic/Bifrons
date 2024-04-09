using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Decimals.Tests;

public sealed class InsertLensTests : SymmetricLensTestingFramework<double, double>
{
    protected override double _left => InsertLens.UNIT_VALUE;

    protected override double _right => 42.0;

    private readonly double _updatedValue = 23.0;

    protected override (double originalSource, double expectedOriginalTarget, double updatedTarget, double expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _left);

    protected override (double originalSource, double expectedOriginalTarget, double updatedTarget, double expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedValue, _right);

    protected override ISymmetricLens<double, double> _lens => InsertLens.Cons(_right);
}
