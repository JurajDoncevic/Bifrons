using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Longs.Tests;

public class SubLensTests : SymmetricLensTestingFramework<long, long>
{
    protected override long _left => 6;

    protected override long _right => 1;

    protected override (long originalSource, long expectedOriginalTarget, long updatedTarget, long expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => (7, 2, 5, 10);

    protected override (long originalSource, long expectedOriginalTarget, long updatedTarget, long expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (1, 6, 9, 4);

    protected override ISymmetricLens<long, long> _lens => SubLens.Cons(5);
}
