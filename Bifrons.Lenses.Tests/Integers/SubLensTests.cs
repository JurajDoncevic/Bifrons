using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Integers.Tests;

public class SubLensTests : SymmetricLensTestingFramework<int, int>
{
    protected override int _left => 6;

    protected override int _right => 1;

    protected override (int originalSource, int expectedOriginalTarget, int updatedTarget, int expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => (7, 2, 5, 10);

    protected override (int originalSource, int expectedOriginalTarget, int updatedTarget, int expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (1, 6, 9, 4);

    protected override ISymmetricLens<int, int> _lens => SubLens.Cons(5);
}
