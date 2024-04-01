using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Relational.Tables.Tests;

public sealed class DeleteLensTests : SymmetricLensTestingFramework<Table, Table>
{
    protected override Table _left 
        => Table.Cons(
                "TestTable", 
                IntegerColumn.Cons("Id"),
                StringColumn.Cons("Name"),
                StringColumn.Cons("Alias"),
                DateTimeColumn.Cons("CreatedOn")
            ); 

    protected override Table _right 
        => Table.ConsUnit(); 

    protected override (Table originalSource, Table expectedOriginalTarget, Table updatedTarget, Table expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (Table originalSource, Table expectedOriginalTarget, Table updatedTarget, Table expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, Table.ConsUnit("TestTable"), _left, _right);

    protected override ISymmetricLens<Table, Table> _lens => DeleteLens.Cons("TestTable");
}
