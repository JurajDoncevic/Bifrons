using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Columns.Tests;

public sealed class StringRenameLensTests : SymmetricLensTestingFramework<StringColumnData, StringColumnData>
{
    protected override StringColumnData _left => StringColumnData.Cons(StringColumn.Cons("Name"), "Alice");

    protected override StringColumnData _right => StringColumnData.Cons(StringColumn.Cons("ReName"), "Alice");

    private StringColumnData _updatedLeft = StringColumnData.Cons(StringColumn.Cons("Name"), "Bob");

    private StringColumnData _updatedRight = StringColumnData.Cons(StringColumn.Cons("ReName"), "Bob");

    protected override (StringColumnData originalSource, StringColumnData expectedOriginalTarget, StringColumnData updatedTarget, StringColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _updatedLeft);

    protected override (StringColumnData originalSource, StringColumnData expectedOriginalTarget, StringColumnData updatedTarget, StringColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _updatedRight);

    protected override ISymmetricLens<StringColumnData, StringColumnData> _lens 
        => StringRenameLens.Cons(Relational.Columns.RenameLens.Cons("Name", "ReName"), Strings.IdentityLens.Cons(".*"));
}


public sealed class IntegerRenameLensTests : SymmetricLensTestingFramework<IntegerColumnData, IntegerColumnData>
{
    protected override IntegerColumnData _left => IntegerColumnData.Cons(IntegerColumn.Cons("ID"), 37);

    protected override IntegerColumnData _right => IntegerColumnData.Cons(IntegerColumn.Cons("ReID"), 37);

    private readonly IntegerColumnData _updatedLeft = IntegerColumnData.Cons(IntegerColumn.Cons("ID"), 42);

    private readonly IntegerColumnData _updatedRight = IntegerColumnData.Cons(IntegerColumn.Cons("ReID"), 42);

    protected override (IntegerColumnData originalSource, IntegerColumnData expectedOriginalTarget, IntegerColumnData updatedTarget, IntegerColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _updatedLeft);

    protected override (IntegerColumnData originalSource, IntegerColumnData expectedOriginalTarget, IntegerColumnData updatedTarget, IntegerColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updatedLeft, _updatedRight);

    protected override ISymmetricLens<IntegerColumnData, IntegerColumnData> _lens 
        => IntegerRenameLens.Cons(Relational.Columns.RenameLens.Cons("ID", "ReID"), Integers.IdentityLens.Cons());
}

public sealed class DateTimeRenameLensTests : SymmetricLensTestingFramework<DateTimeColumnData, DateTimeColumnData>
{
    protected override DateTimeColumnData _left => DateTimeColumnData.Cons(DateTimeColumn.Cons("Date"), DateTime.Parse("2021-01-01"));

    protected override DateTimeColumnData _right => DateTimeColumnData.Cons(DateTimeColumn.Cons("ReDate"), DateTime.Parse("2021-01-01"));

    private readonly DateTimeColumnData _updatedLeft = DateTimeColumnData.Cons(DateTimeColumn.Cons("Date"), DateTime.Parse("2021-01-02"));
    private readonly DateTimeColumnData _updatedRight = DateTimeColumnData.Cons(DateTimeColumn.Cons("ReDate"), DateTime.Parse("2021-01-02"));

    protected override (DateTimeColumnData originalSource, DateTimeColumnData expectedOriginalTarget, DateTimeColumnData updatedTarget, DateTimeColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _updatedLeft);

    protected override (DateTimeColumnData originalSource, DateTimeColumnData expectedOriginalTarget, DateTimeColumnData updatedTarget, DateTimeColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updatedLeft, _updatedRight);

    protected override ISymmetricLens<DateTimeColumnData, DateTimeColumnData> _lens 
        => DateTimeRenameLens.Cons(Relational.Columns.RenameLens.Cons("Date", "ReDate"), DateTimes.IdentityLens.Cons());
}

public sealed class BooleanRenameLensTests : SymmetricLensTestingFramework<BooleanColumnData, BooleanColumnData>
{
    protected override BooleanColumnData _left => BooleanColumnData.Cons(BooleanColumn.Cons("IsTrue"), false);

    protected override BooleanColumnData _right => BooleanColumnData.Cons(BooleanColumn.Cons("ReIsTrue"), false);

    private readonly BooleanColumnData _updatedLeft = BooleanColumnData.Cons(BooleanColumn.Cons("IsTrue"), true);
    private readonly BooleanColumnData _updatedRight = BooleanColumnData.Cons(BooleanColumn.Cons("ReIsTrue"), true);

    protected override (BooleanColumnData originalSource, BooleanColumnData expectedOriginalTarget, BooleanColumnData updatedTarget, BooleanColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _updatedLeft);

    protected override (BooleanColumnData originalSource, BooleanColumnData expectedOriginalTarget, BooleanColumnData updatedTarget, BooleanColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updatedLeft, _updatedRight);

    protected override ISymmetricLens<BooleanColumnData, BooleanColumnData> _lens 
        => BooleanRenameLens.Cons(Relational.Columns.RenameLens.Cons("IsTrue", "ReIsTrue"), Booleans.IdentityLens.Cons());
}

public sealed class DecimalRenameLensTests : SymmetricLensTestingFramework<DecimalColumnData, DecimalColumnData>
{
    protected override DecimalColumnData _left => DecimalColumnData.Cons(DecimalColumn.Cons("Amount"), 1.1);

    protected override DecimalColumnData _right => DecimalColumnData.Cons(DecimalColumn.Cons("ReAmount"), 1.1);

    private readonly DecimalColumnData _updatedLeft = DecimalColumnData.Cons(DecimalColumn.Cons("Amount"), 2.2);
    private readonly DecimalColumnData _updatedRight = DecimalColumnData.Cons(DecimalColumn.Cons("ReAmount"), 2.2);

    protected override (DecimalColumnData originalSource, DecimalColumnData expectedOriginalTarget, DecimalColumnData updatedTarget, DecimalColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _updatedLeft);

    protected override (DecimalColumnData originalSource, DecimalColumnData expectedOriginalTarget, DecimalColumnData updatedTarget, DecimalColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updatedLeft, _updatedRight);

    protected override ISymmetricLens<DecimalColumnData, DecimalColumnData> _lens 
        => DecimalRenameLens.Cons(Relational.Columns.RenameLens.Cons("Amount", "ReAmount"), Decimals.IdentityLens.Cons());
}