using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Relational.Tables.Tests;

public sealed class InsertLensTests : SymmetricLensTestingFramework<Table, Table>
{
    protected override Table _left => Table.ConsUnit();

    protected override Table _right => 
        Table.Cons(
            "TestTable", 
            IntegerColumn.Cons("Id"),
            StringColumn.Cons("Name"),
            StringColumn.Cons("Alias"),
            DateTimeColumn.Cons("CreatedOn")
        );

    private readonly Table _updatedRight =
        Table.Cons(
            "TestTable", 
            IntegerColumn.Cons("Id"),
            StringColumn.Cons("Name"),
            StringColumn.Cons("Alias"),
            DateTimeColumn.Cons("CreatedOn"),
            DateTimeColumn.Cons("UpdatedOn")
        );

    protected override (Table originalSource, Table expectedOriginalTarget, Table updatedTarget, Table expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _left);

    protected override (Table originalSource, Table expectedOriginalTarget, Table updatedTarget, Table expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _left, _right);

    protected override ISymmetricLens<Table, Table> _lens => InsertLens.Cons("TestTable", _right);
}
