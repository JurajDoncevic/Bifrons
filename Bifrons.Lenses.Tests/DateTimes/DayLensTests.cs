using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.DateTimes.Tests;

public sealed class DayLensTests : SymmetricLensTestingFramework<DateTime, int>
{
    protected override DateTime _left => new(1992, 12, 31, 11, 40, 33);

    protected override int _right => 31;

    private readonly DateTime _defaultedLeft = new(1, 1, 31, 0, 0, 0);

    private readonly DateTime _expectedLeft = new(1992, 12, 20, 11, 40, 33);

    private readonly int _updatedRight = 20;

    private readonly DateTime _updatedLeft = new(1999, 5, 20, 11, 40, 33);

    private readonly int _expectedRight = 20;

    protected override (DateTime originalSource, int expectedOriginalTarget, int updatedTarget, DateTime expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => (_left, _right, _updatedRight, _expectedLeft);

    protected override (int originalSource, DateTime expectedOriginalTarget, DateTime updatedTarget, int expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _defaultedLeft, _updatedLeft, _expectedRight);

    protected override DayLens _lens => DayLens.Cons();
}
