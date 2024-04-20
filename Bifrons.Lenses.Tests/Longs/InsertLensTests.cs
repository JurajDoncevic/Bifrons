using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Longs.Tests;

public sealed class InsertLensTests : SymmetricLensTestingFramework<long, long>
{
    protected override long _left => InsertLens.UNIT_VALUE;

    protected override long _right => 42;

    private readonly long _updatedValue = 23;

    protected override (long originalSource, long expectedOriginalTarget, long updatedTarget, long expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _left);

    protected override (long originalSource, long expectedOriginalTarget, long updatedTarget, long expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedValue, _right);

    protected override ISymmetricLens<long, long> _lens => InsertLens.Cons(_right);
}
