using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Columns.Identity.Tests;


public sealed class StringIdentityLensTests : SymmetricLensTestingFramework<StringColumnData, StringColumnData>
{
    protected override StringColumnData _left => StringColumnData.Cons(StringColumn.Cons("Name"), "Alice");

    protected override StringColumnData _right => StringColumnData.Cons(StringColumn.Cons("Name"), "Alice");

    private StringColumnData _updatedLeft = StringColumnData.Cons(StringColumn.Cons("Name"), "Bob");

    private StringColumnData _updatedRight = StringColumnData.Cons(StringColumn.Cons("Name"), "Bob");

    protected override (StringColumnData originalSource, StringColumnData expectedOriginalTarget, StringColumnData updatedTarget, StringColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _updatedRight);

    protected override (StringColumnData originalSource, StringColumnData expectedOriginalTarget, StringColumnData updatedTarget, StringColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _updatedLeft);

    protected override ISymmetricLens<StringColumnData, StringColumnData> _lens 
        => StringIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Name"), Strings.IdentityLens.Cons(".*"));
}


public sealed class IntegerIdentityLensTests : SymmetricLensTestingFramework<IntegerColumnData, IntegerColumnData>
{
    protected override IntegerColumnData _left => IntegerColumnData.Cons(IntegerColumn.Cons("ID"), 42);

    protected override IntegerColumnData _right => IntegerColumnData.Cons(IntegerColumn.Cons("ID"), 42);

    private readonly IntegerColumnData _updated = IntegerColumnData.Cons(IntegerColumn.Cons("ID"), 37);

    protected override (IntegerColumnData originalSource, IntegerColumnData expectedOriginalTarget, IntegerColumnData updatedTarget, IntegerColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updated, _updated);

    protected override (IntegerColumnData originalSource, IntegerColumnData expectedOriginalTarget, IntegerColumnData updatedTarget, IntegerColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updated, _updated);

    protected override ISymmetricLens<IntegerColumnData, IntegerColumnData> _lens 
        => IntegerIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("ID"), Integers.IdentityLens.Cons());
}

public sealed class DateTimeIdentityLensTests : SymmetricLensTestingFramework<DateTimeColumnData, DateTimeColumnData>
{
    protected override DateTimeColumnData _left => DateTimeColumnData.Cons(DateTimeColumn.Cons("Date"), DateTime.Parse("2021-01-01"));

    protected override DateTimeColumnData _right => DateTimeColumnData.Cons(DateTimeColumn.Cons("Date"), DateTime.Parse("2021-01-01"));

    private readonly DateTimeColumnData _updated = DateTimeColumnData.Cons(DateTimeColumn.Cons("Date"), DateTime.Parse("2021-01-02"));

    protected override (DateTimeColumnData originalSource, DateTimeColumnData expectedOriginalTarget, DateTimeColumnData updatedTarget, DateTimeColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updated, _updated);

    protected override (DateTimeColumnData originalSource, DateTimeColumnData expectedOriginalTarget, DateTimeColumnData updatedTarget, DateTimeColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updated, _updated);

    protected override ISymmetricLens<DateTimeColumnData, DateTimeColumnData> _lens 
        => DateTimeIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Date"), DateTimes.IdentityLens.Cons());
}

public sealed class BooleanIdentityLensTests : SymmetricLensTestingFramework<BooleanColumnData, BooleanColumnData>
{
    protected override BooleanColumnData _left => BooleanColumnData.Cons(BooleanColumn.Cons("IsTrue"), false);

    protected override BooleanColumnData _right => BooleanColumnData.Cons(BooleanColumn.Cons("IsTrue"), false);

    private readonly BooleanColumnData _updated = BooleanColumnData.Cons(BooleanColumn.Cons("IsTrue"), true);

    protected override (BooleanColumnData originalSource, BooleanColumnData expectedOriginalTarget, BooleanColumnData updatedTarget, BooleanColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updated, _updated);

    protected override (BooleanColumnData originalSource, BooleanColumnData expectedOriginalTarget, BooleanColumnData updatedTarget, BooleanColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updated, _updated);

    protected override ISymmetricLens<BooleanColumnData, BooleanColumnData> _lens 
        => BooleanIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("IsTrue"), Booleans.IdentityLens.Cons());
}

public sealed class DecimalIdentityLensTests : SymmetricLensTestingFramework<DecimalColumnData, DecimalColumnData>
{
    protected override DecimalColumnData _left => DecimalColumnData.Cons(DecimalColumn.Cons("Amount"), 1.1);

    protected override DecimalColumnData _right => DecimalColumnData.Cons(DecimalColumn.Cons("Amount"), 1.1);

    private readonly DecimalColumnData _updated = DecimalColumnData.Cons(DecimalColumn.Cons("Amount"), 2.2);

    protected override (DecimalColumnData originalSource, DecimalColumnData expectedOriginalTarget, DecimalColumnData updatedTarget, DecimalColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updated, _updated);

    protected override (DecimalColumnData originalSource, DecimalColumnData expectedOriginalTarget, DecimalColumnData updatedTarget, DecimalColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updated, _updated);

    protected override ISymmetricLens<DecimalColumnData, DecimalColumnData> _lens 
        => DecimalIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Amount"), Decimals.IdentityLens.Cons());
}