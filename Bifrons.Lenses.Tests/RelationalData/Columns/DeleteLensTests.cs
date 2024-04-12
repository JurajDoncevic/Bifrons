using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Columns.Delete.Tests;

public sealed class StringDeleteLensTests : SymmetricLensTestingFramework<StringColumnData, UnitColumnData>
{
    protected override StringColumnData _left => StringColumnData.Cons(StringColumn.Cons("Name"), "Alice");

    protected override UnitColumnData _right => UnitColumnData.Cons();

    private readonly StringColumnData _updatedLeft = StringColumnData.Cons(StringColumn.Cons("Name"), "Bob");

    protected override (StringColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, StringColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (UnitColumnData originalSource, StringColumnData expectedOriginalTarget, StringColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _right);

    protected override ISymmetricLens<StringColumnData, UnitColumnData> _lens 
        => StringDeleteLens.Cons(Relational.Columns.DeleteLens.Cons("Name", DataTypes.STRING), "Alice");

}

public sealed class IntegerDeleteLensTests : SymmetricLensTestingFramework<IntegerColumnData, UnitColumnData>
{
    protected override IntegerColumnData _left => IntegerColumnData.Cons(IntegerColumn.Cons("Age"), 42);

    protected override UnitColumnData _right => UnitColumnData.Cons();

    private readonly IntegerColumnData _updatedLeft = IntegerColumnData.Cons(IntegerColumn.Cons("Age"), 37);

    
    protected override (IntegerColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, IntegerColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (UnitColumnData originalSource, IntegerColumnData expectedOriginalTarget, IntegerColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _right);

    protected override ISymmetricLens<IntegerColumnData, UnitColumnData> _lens 
        => IntegerDeleteLens.Cons(Relational.Columns.DeleteLens.Cons("Age", DataTypes.INTEGER), 42);

}

public sealed class BooleanDeleteLensTests : SymmetricLensTestingFramework<BooleanColumnData, UnitColumnData>
{
    protected override BooleanColumnData _left => BooleanColumnData.Cons(BooleanColumn.Cons("IsAdmin"), true);

    protected override UnitColumnData _right => UnitColumnData.Cons();

    private readonly BooleanColumnData _updatedLeft = BooleanColumnData.Cons(BooleanColumn.Cons("IsAdmin"), false);

    
    protected override (BooleanColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, BooleanColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (UnitColumnData originalSource, BooleanColumnData expectedOriginalTarget, BooleanColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _right);

    protected override ISymmetricLens<BooleanColumnData, UnitColumnData> _lens 
        => BooleanDeleteLens.Cons(Relational.Columns.DeleteLens.Cons("IsAdmin", DataTypes.BOOLEAN), true);

}

public sealed class DecimalDeleteLensTests : SymmetricLensTestingFramework<DecimalColumnData, UnitColumnData>
{
    protected override DecimalColumnData _left => DecimalColumnData.Cons(DecimalColumn.Cons("Price"), 42.42);

    protected override UnitColumnData _right => UnitColumnData.Cons();

    private readonly DecimalColumnData _updatedLeft = DecimalColumnData.Cons(DecimalColumn.Cons("Price"), 37.37);

    
    protected override (DecimalColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, DecimalColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (UnitColumnData originalSource, DecimalColumnData expectedOriginalTarget, DecimalColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _right);

    protected override ISymmetricLens<DecimalColumnData, UnitColumnData> _lens 
        => DecimalDeleteLens.Cons(Relational.Columns.DeleteLens.Cons("Price", DataTypes.DECIMAL), 42.42);

}


public sealed class DateTimeDeleteLensTests : SymmetricLensTestingFramework<DateTimeColumnData, UnitColumnData>
{
    protected override DateTimeColumnData _left => DateTimeColumnData.Cons(DateTimeColumn.Cons("CreatedAt"), DateTime.Parse("2021-01-01"));

    protected override UnitColumnData _right => UnitColumnData.Cons();

    private readonly DateTimeColumnData _updatedLeft = DateTimeColumnData.Cons(DateTimeColumn.Cons("CreatedAt"), DateTime.Parse("2021-01-02"));

    
    protected override (DateTimeColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, DateTimeColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (UnitColumnData originalSource, DateTimeColumnData expectedOriginalTarget, DateTimeColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _right);

    protected override ISymmetricLens<DateTimeColumnData, UnitColumnData> _lens 
        => DateTimeDeleteLens.Cons(Relational.Columns.DeleteLens.Cons("CreatedAt", DataTypes.DATETIME), DateTime.Parse("2021-01-01"));

}