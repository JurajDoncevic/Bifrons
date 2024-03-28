using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Relational.Columns.Tests;

public sealed class InsertLensTests : SymmetricLensTestingFramework<Column, Column>
{
    protected override Column _left => UnitColumn.Cons();

    protected override Column _right => StringColumn.Cons("TestColumn");

    protected override (Column originalSource, Column expectedOriginalTarget, Column updatedTarget, Column expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (Column originalSource, Column expectedOriginalTarget, Column updatedTarget, Column expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, UnitColumn.Cons("TestColumn"), UnitColumn.Cons("TestedColumn"), _right);

    protected override ISymmetricLens<Column, Column> _lens => InsertLens.Cons(_right.Name, _right.DataType);
}
