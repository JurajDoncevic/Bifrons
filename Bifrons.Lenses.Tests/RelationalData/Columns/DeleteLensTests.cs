using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Columns.Delete.Tests;

public sealed class StringDeleteLensTests : SymmetricLensTestingFramework<StringDataColumn, StringDataColumn>
{
    protected override StringDataColumn _left => StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Bob", "Charlie"]);

    protected override StringDataColumn _right => StringDataColumn.Cons(StringColumn.Cons("Name"));

    protected override (StringDataColumn originalSource, StringDataColumn expectedOriginalTarget, StringDataColumn updatedTarget, StringDataColumn expectedUpdatedSource) _roundTripWithRightSideUpdateData => throw new NotImplementedException();

    protected override (StringDataColumn originalSource, StringDataColumn expectedOriginalTarget, StringDataColumn updatedTarget, StringDataColumn expectedUpdatedSource) _roundTripWithLeftSideUpdateData => throw new NotImplementedException();

    protected override ISymmetricLens<StringDataColumn, StringDataColumn> _lens 
        => StringDeleteLens.Cons(Relational.Columns.DeleteLens.Cons("Name"), Strings.DeleteLens.Cons(".*")); 
}
