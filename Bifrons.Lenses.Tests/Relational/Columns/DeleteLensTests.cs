using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Relational.Columns.Tests;

public sealed class DeleteLensTests : SymmetricLensTestingFramework<Column, Column>
{
    protected override Column _left => StringColumn.Cons("TestColumn");

    protected override Column _right => UnitColumn.Cons("TestColumn");

    protected override (Column originalSource, Column expectedOriginalTarget, Column updatedTarget, Column expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (Column originalSource, Column expectedOriginalTarget, Column updatedTarget, Column expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, UnitColumn.Cons("TestColumn"), StringColumn.Cons("TestedColumn"), _right);

    protected override ISymmetricLens<Column, Column> _lens => DeleteLens.Cons(_left.Name);
}
