using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Integers.Tests;

public class AddLensTests : SymmetricLensTestingFramework<int, int>
{
    protected override int _left => 1;

    protected override int _right => 6;

    protected override (int originalSource, int expectedOriginalTarget, int updatedTarget, int expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => (1, 6, 9, 4);

    protected override (int originalSource, int expectedOriginalTarget, int updatedTarget, int expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (7, 2, 5, 10);

    protected override ISimpleSymmetricLens<int, int> _lens => AddLens.Cons(5);
}
