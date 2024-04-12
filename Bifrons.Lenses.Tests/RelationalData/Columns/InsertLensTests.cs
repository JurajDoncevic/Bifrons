using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Columns.Insert.Tests;

public sealed class StringInsertLensTests : SymmetricLensTestingFramework<UnitColumnData, StringColumnData>
{
    protected override UnitColumnData _left => UnitColumnData.Cons(UnitColumn.Cons());

    protected override StringColumnData _right => StringColumnData.Cons(StringColumn.Cons("Name"), "Alice");

    private StringColumnData _updatedRight = StringColumnData.Cons(StringColumn.Cons("Name"), "Bob");


    protected override (UnitColumnData originalSource, StringColumnData expectedOriginalTarget, StringColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _left);

    protected override (StringColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, StringColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _left, _right);

    protected override ISymmetricLens<UnitColumnData, StringColumnData> _lens 
        => StringInsertLens.Cons(Relational.Columns.InsertLens.Cons("Name", DataTypes.STRING), "Alice");
}

public sealed class IntegerInsertLensTests : SymmetricLensTestingFramework<UnitColumnData, IntegerColumnData>
{
    protected override UnitColumnData _left => UnitColumnData.Cons(UnitColumn.Cons());

    protected override IntegerColumnData _right => IntegerColumnData.Cons(IntegerColumn.Cons("Age"), 42);

    private IntegerColumnData _updatedRight = IntegerColumnData.Cons(IntegerColumn.Cons("Age"), 37);

    
    protected override (UnitColumnData originalSource, IntegerColumnData expectedOriginalTarget, IntegerColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _left);

    protected override (IntegerColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, IntegerColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _left, _right);

    protected override ISymmetricLens<UnitColumnData, IntegerColumnData> _lens 
        => IntegerInsertLens.Cons(Relational.Columns.InsertLens.Cons("Age", DataTypes.INTEGER), 42);

}

public sealed class DecimalInsertLensTests : SymmetricLensTestingFramework<UnitColumnData, DecimalColumnData>
{
    protected override UnitColumnData _left => UnitColumnData.Cons(UnitColumn.Cons());

    protected override DecimalColumnData _right => DecimalColumnData.Cons(DecimalColumn.Cons("Price"), 42.42);

    private DecimalColumnData _updatedRight = DecimalColumnData.Cons(DecimalColumn.Cons("Price"), 37.37);

    
    protected override (UnitColumnData originalSource, DecimalColumnData expectedOriginalTarget, DecimalColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _left);

    protected override (DecimalColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, DecimalColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _left, _right);

    protected override ISymmetricLens<UnitColumnData, DecimalColumnData> _lens 
        => DecimalInsertLens.Cons(Relational.Columns.InsertLens.Cons("Price", DataTypes.DECIMAL), 42.42);

}

public sealed class BooleanInsertLensTests : SymmetricLensTestingFramework<UnitColumnData, BooleanColumnData>
{
    protected override UnitColumnData _left => UnitColumnData.Cons(UnitColumn.Cons());

    protected override BooleanColumnData _right => BooleanColumnData.Cons(BooleanColumn.Cons("IsAdmin"), true);

    private BooleanColumnData _updatedRight = BooleanColumnData.Cons(BooleanColumn.Cons("IsAdmin"), false);

    
    protected override (UnitColumnData originalSource, BooleanColumnData expectedOriginalTarget, BooleanColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _left);

    protected override (BooleanColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, BooleanColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _left, _right);

    protected override ISymmetricLens<UnitColumnData, BooleanColumnData> _lens 
        => BooleanInsertLens.Cons(Relational.Columns.InsertLens.Cons("IsAdmin", DataTypes.BOOLEAN), true);

}

public sealed class DateTimeInsertLensTests : SymmetricLensTestingFramework<UnitColumnData, DateTimeColumnData>
{
    protected override UnitColumnData _left => UnitColumnData.Cons(UnitColumn.Cons());

    protected override DateTimeColumnData _right => DateTimeColumnData.Cons(DateTimeColumn.Cons("CreatedAt"), DateTime.Parse("2021-01-01"));

    private DateTimeColumnData _updatedRight = DateTimeColumnData.Cons(DateTimeColumn.Cons("CreatedAt"), DateTime.Parse("2021-01-02"));

    
    protected override (UnitColumnData originalSource, DateTimeColumnData expectedOriginalTarget, DateTimeColumnData updatedTarget, UnitColumnData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _left);

    protected override (DateTimeColumnData originalSource, UnitColumnData expectedOriginalTarget, UnitColumnData updatedTarget, DateTimeColumnData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _left, _right);

    protected override ISymmetricLens<UnitColumnData, DateTimeColumnData> _lens 
        => DateTimeInsertLens.Cons(Relational.Columns.InsertLens.Cons("CreatedAt", DataTypes.DATETIME), DateTime.Parse("2021-01-01"));

}

