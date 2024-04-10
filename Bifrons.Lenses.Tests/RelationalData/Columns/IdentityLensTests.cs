using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Columns.Identity.Tests;


public sealed class StringIdentityLensTests : SymmetricLensTestingFramework<StringDataColumn, StringDataColumn>
{
    protected override StringDataColumn _left => StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Bob", "Charlie"]);

    protected override StringDataColumn _right => StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Bob", "Charlie"]);

    private StringDataColumn _updatedLeft = StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Bob", "Dickie"]);

    private StringDataColumn _updatedRight = StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Dickie", "Charlie"]);

    protected override (StringDataColumn originalSource, StringDataColumn expectedOriginalTarget, StringDataColumn updatedTarget, StringDataColumn expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _updatedRight);

    protected override (StringDataColumn originalSource, StringDataColumn expectedOriginalTarget, StringDataColumn updatedTarget, StringDataColumn expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _updatedLeft);

    protected override ISymmetricLens<StringDataColumn, StringDataColumn> _lens 
        => StringIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Name"), Strings.IdentityLens.Cons());
}


public sealed class IntegerIdentityLensTests : SymmetricLensTestingFramework<IntegerDataColumn, IntegerDataColumn>
{
    protected override IntegerDataColumn _left => IntegerDataColumn.Cons(IntegerColumn.Cons("ID"), [1, 2, 3]);

    protected override IntegerDataColumn _right => IntegerDataColumn.Cons(IntegerColumn.Cons("ID"), [1, 2, 3]);

    private readonly IntegerDataColumn _updated = IntegerDataColumn.Cons(IntegerColumn.Cons("ID"), [1, 4, 3]);

    protected override (IntegerDataColumn originalSource, IntegerDataColumn expectedOriginalTarget, IntegerDataColumn updatedTarget, IntegerDataColumn expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updated, _updated);

    protected override (IntegerDataColumn originalSource, IntegerDataColumn expectedOriginalTarget, IntegerDataColumn updatedTarget, IntegerDataColumn expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updated, _updated);

    protected override ISymmetricLens<IntegerDataColumn, IntegerDataColumn> _lens 
        => IntegerIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("ID"), Integers.IdentityLens.Cons());
}

public sealed class DateTimeIdentityLensTests : SymmetricLensTestingFramework<DateTimeDataColumn, DateTimeDataColumn>
{
    protected override DateTimeDataColumn _left => DateTimeDataColumn.Cons(DateTimeColumn.Cons("Date"), [DateTime.Parse("2021-01-01"), DateTime.Parse("2021-01-02"), DateTime.Parse("2021-01-03")]);

    protected override DateTimeDataColumn _right => DateTimeDataColumn.Cons(DateTimeColumn.Cons("Date"), [DateTime.Parse("2021-01-01"), DateTime.Parse("2021-01-02"), DateTime.Parse("2021-01-03")]);

    private readonly DateTimeDataColumn _updated = DateTimeDataColumn.Cons(DateTimeColumn.Cons("Date"), [DateTime.Parse("2021-01-01"), DateTime.Parse("2021-01-04"), DateTime.Parse("2021-01-03")]);

    protected override (DateTimeDataColumn originalSource, DateTimeDataColumn expectedOriginalTarget, DateTimeDataColumn updatedTarget, DateTimeDataColumn expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updated, _updated);

    protected override (DateTimeDataColumn originalSource, DateTimeDataColumn expectedOriginalTarget, DateTimeDataColumn updatedTarget, DateTimeDataColumn expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updated, _updated);

    protected override ISymmetricLens<DateTimeDataColumn, DateTimeDataColumn> _lens 
        => DateTimeIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Date"), DateTimes.IdentityLens.Cons());
}

public sealed class BooleanIdentityLensTests : SymmetricLensTestingFramework<BooleanDataColumn, BooleanDataColumn>
{
    protected override BooleanDataColumn _left => BooleanDataColumn.Cons(BooleanColumn.Cons("IsTrue"), [true, false, true]);

    protected override BooleanDataColumn _right => BooleanDataColumn.Cons(BooleanColumn.Cons("IsTrue"), [true, false, true]);

    private readonly BooleanDataColumn _updated = BooleanDataColumn.Cons(BooleanColumn.Cons("IsTrue"), [true, true, true]);

    protected override (BooleanDataColumn originalSource, BooleanDataColumn expectedOriginalTarget, BooleanDataColumn updatedTarget, BooleanDataColumn expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updated, _updated);

    protected override (BooleanDataColumn originalSource, BooleanDataColumn expectedOriginalTarget, BooleanDataColumn updatedTarget, BooleanDataColumn expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updated, _updated);

    protected override ISymmetricLens<BooleanDataColumn, BooleanDataColumn> _lens 
        => BooleanIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("IsTrue"), Booleans.IdentityLens.Cons());
}

public sealed class DecimalIdentityLensTests : SymmetricLensTestingFramework<DecimalDataColumn, DecimalDataColumn>
{
    protected override DecimalDataColumn _left => DecimalDataColumn.Cons(DecimalColumn.Cons("Amount"), [1.1, 2.2, 3.3]);

    protected override DecimalDataColumn _right => DecimalDataColumn.Cons(DecimalColumn.Cons("Amount"), [1.1, 2.2, 3.3]);

    private readonly DecimalDataColumn _updated = DecimalDataColumn.Cons(DecimalColumn.Cons("Amount"), [1.1, 4.4, 3.3]);

    protected override (DecimalDataColumn originalSource, DecimalDataColumn expectedOriginalTarget, DecimalDataColumn updatedTarget, DecimalDataColumn expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updated, _updated);

    protected override (DecimalDataColumn originalSource, DecimalDataColumn expectedOriginalTarget, DecimalDataColumn updatedTarget, DecimalDataColumn expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, _updated, _updated);

    protected override ISymmetricLens<DecimalDataColumn, DecimalDataColumn> _lens 
        => DecimalIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Amount"), Decimals.IdentityLens.Cons());
}