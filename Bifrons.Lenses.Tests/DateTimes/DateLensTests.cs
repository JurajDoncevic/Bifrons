using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.DateTimes.Tests;

public sealed class DateLensTests : SymmetricLensTestingFramework<DateTime, DateOnly>
{
    protected override DateTime _left => new(1992, 12, 31, 11, 40, 33);

    protected override DateOnly _right => new(1992, 12, 31);

    private readonly DateTime _defaultedLeft = new(1992, 12, 31);

    private readonly DateTime _expectedLeft = new(1999, 5, 20, 11, 40, 33);

    private readonly DateOnly _updatedRight = new(1999, 5, 20);

    private readonly DateTime _updatedLeft = new(1999, 5, 20, 11, 40, 33);

    private readonly DateOnly _expectedRight = new(1999, 5, 20);

    protected override (DateTime originalSource, DateOnly expectedOriginalTarget, DateOnly updatedTarget, DateTime expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => (_left, _right, _updatedRight, _expectedLeft);

    protected override (DateOnly originalSource, DateTime expectedOriginalTarget, DateTime updatedTarget, DateOnly expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _defaultedLeft, _updatedLeft, _expectedRight);

    protected override DateLens _lens => DateLens.Cons();
}

