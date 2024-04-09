using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Booleans.Tests;

public sealed class IdentityLensTests : SymmetricLensTestingFramework<bool, bool>
{
    protected override bool _left => true;

    protected override bool _right => true;

    private readonly bool _updatedValue = false;

    protected override (bool originalSource, bool expectedOriginalTarget, bool updatedTarget, bool expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _updatedValue);

    protected override (bool originalSource, bool expectedOriginalTarget, bool updatedTarget, bool expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedValue, _updatedValue);

    protected override ISymmetricLens<bool, bool> _lens => IdentityLens.Cons();
}
