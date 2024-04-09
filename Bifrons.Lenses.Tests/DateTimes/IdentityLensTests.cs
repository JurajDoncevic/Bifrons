using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.DateTimes.Tests;

public sealed class IdentityLensTests : SymmetricLensTestingFramework<DateTime, DateTime>
{
    protected override DateTime _left => new (1992, 12, 31);

    protected override DateTime _right => new (1992, 12, 31);

    private readonly DateTime _updatedValue = new (1999, 5, 20);

    protected override (DateTime originalSource, DateTime expectedOriginalTarget, DateTime updatedTarget, DateTime expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedValue, _updatedValue);

    protected override (DateTime originalSource, DateTime expectedOriginalTarget, DateTime updatedTarget, DateTime expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left,  _updatedValue, _updatedValue);

    protected override ISymmetricLens<DateTime, DateTime> _lens => IdentityLens.Cons();
}
