using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Relational.Columns.Tests;

public sealed class RenameLensTests : SymmetricLensTestingFramework<Column, Column>
{
    protected override Column _left => StringColumn.Cons("TestColumn");

    protected override Column _right => StringColumn.Cons("RenamedColumn");

    protected override (Column originalSource, Column expectedOriginalTarget, Column updatedTarget, Column expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, StringColumn.Cons("RerenamedColumn"), _left);

    protected override (Column originalSource, Column expectedOriginalTarget, Column updatedTarget, Column expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (_right, _left, StringColumn.Cons("RerenamedColumn"), _right);

    protected override ISymmetricLens<Column, Column> _lens => RenameLens.Cons(_left.Name, _right.Name);
}
