using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Integers.Tests;

public sealed class InsertLensTests : SymmetricLensTestingFramework<int, int>
{
    protected override int _left => InsertLens.UNIT_VALUE;

    protected override int _right => 42;

    private readonly int _updatedValue = 23;

    protected override (int originalSource, int expectedOriginalTarget, int updatedTarget, int expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _left);

    protected override (int originalSource, int expectedOriginalTarget, int updatedTarget, int expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedValue, _right);

    protected override ISymmetricLens<int, int> _lens => InsertLens.Cons(_right);
}
