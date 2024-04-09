using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Integers.Tests;

public class DeleteLensTests : SymmetricLensTestingFramework<int, int>
{
    protected override int _left => 42;

    protected override int _right => DeleteLens.UNIT_VALUE;

    private readonly int _updatedValue = 23;

    protected override (int originalSource, int expectedOriginalTarget, int updatedTarget, int expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _left);

    protected override (int originalSource, int expectedOriginalTarget, int updatedTarget, int expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedValue, _right);

    protected override ISymmetricLens<int, int> _lens => DeleteLens.Cons(_left);
}
