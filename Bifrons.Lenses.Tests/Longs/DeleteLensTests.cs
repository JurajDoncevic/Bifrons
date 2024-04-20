using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Longs.Tests;

public class DeleteLensTests : SymmetricLensTestingFramework<long, long>
{
    protected override long _left => 42;

    protected override long _right => DeleteLens.UNIT_VALUE;

    private readonly long _updatedValue = 23;

    protected override (long originalSource, long expectedOriginalTarget, long updatedTarget, long expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _left);

    protected override (long originalSource, long expectedOriginalTarget, long updatedTarget, long expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedValue, _right);

    protected override ISymmetricLens<long, long> _lens => DeleteLens.Cons(_left);
}
