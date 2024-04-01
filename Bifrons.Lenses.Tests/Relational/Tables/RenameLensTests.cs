using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Relational.Tables.Tests;

public sealed class RenameLensTests : SymmetricLensTestingFramework<Table, Table>
{
    protected override Table _left 
        => Table.Cons(
                "LeftTestTable", 
                IntegerColumn.Cons("Id"),
                StringColumn.Cons("Name"),
                StringColumn.Cons("Alias"),
                DateTimeColumn.Cons("CreatedOn")
            );

    protected override Table _right 
        => Table.Cons(
                "RightTestTable",
                IntegerColumn.Cons("Id"),
                StringColumn.Cons("QualName"),
                StringColumn.Cons("Description"),
                DateTimeColumn.Cons("CreatedOn")
            );
    
    private readonly Table _updatedLeft
        = Table.Cons(
                "LeftUpdatedTable",
                IntegerColumn.Cons("Id"),
                StringColumn.Cons("Name"),
                StringColumn.Cons("Alias"),
                DateTimeColumn.Cons("CreatedOn")
            );

    private readonly Table _updatedRight 
        = Table.Cons(
                "RightUpdatedTable",
                IntegerColumn.Cons("Id"),
                StringColumn.Cons("QualName"),
                StringColumn.Cons("Description"),
                DateTimeColumn.Cons("CreatedOn")
            );
    


    protected override (Table originalSource, Table expectedOriginalTarget, Table updatedTarget, Table expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _left);

    protected override (Table originalSource, Table expectedOriginalTarget, Table updatedTarget, Table expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, 
            Table.Cons( // because Alias DataType can't be determined R -> L 
                "LeftTestTable", 
                IntegerColumn.Cons("Id"),
                StringColumn.Cons("Name"),
                UnitColumn.Cons("Alias"),
                DateTimeColumn.Cons("CreatedOn")
            ), 
            _updatedLeft, 
            _right);
    protected override ISymmetricLens<Table, Table> _lens 
        => RenameLens.Cons(
                "LeftTestTable",
                "RightTestTable",
                Columns.IdentityLens.Cons("Id"),
                Columns.RenameLens.Cons("Name", "QualName"),
                Columns.DeleteLens.Cons("Alias"),
                Columns.InsertLens.Cons("Description", DataTypes.STRING),
                Columns.IdentityLens.Cons("CreatedOn")
            );
}
