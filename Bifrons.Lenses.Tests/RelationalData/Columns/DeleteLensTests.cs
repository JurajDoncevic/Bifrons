using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Columns.Delete.Tests;

public sealed class StringDeleteLensTests : SymmetricLensTestingFramework<StringDataColumn, UnitDataColumn>
{
    protected override StringDataColumn _left => StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Bob", "Charlie"]);

    protected override UnitDataColumn _right => UnitDataColumn.Cons();


    protected override (StringDataColumn originalSource, UnitDataColumn expectedOriginalTarget, UnitDataColumn updatedTarget, StringDataColumn expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _right, _left);

    protected override (UnitDataColumn originalSource, StringDataColumn expectedOriginalTarget, StringDataColumn updatedTarget, UnitDataColumn expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _left, _right);

    protected override ISymmetricLens<StringDataColumn, UnitDataColumn> _lens 
        => StringDeleteLens.Cons(Relational.Columns.DeleteLens.Cons("Name"), Strings.VarDeleteLens.Cons(".*", ""));

}
