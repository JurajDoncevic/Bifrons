using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Booleans.Tests;

public class DeleteLensTests : SymmetricLensTestingFramework<bool, bool>
{
    protected override bool _left => true;

    protected override bool _right => DeleteLens.UNIT_VALUE;

    private readonly bool _updatedValue = false;

    protected override (bool originalSource, bool expectedOriginalTarget, bool updatedTarget, bool expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _left);

    protected override (bool originalSource, bool expectedOriginalTarget, bool updatedTarget, bool expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedValue, _right);

    protected override ISymmetricLens<bool, bool> _lens => DeleteLens.Cons(_left);
}
