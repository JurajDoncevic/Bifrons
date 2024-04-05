using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Columns.Tests;

public sealed class StringIdentityLensTests : SymmetricLensTestingFramework<StringDataColumn, StringDataColumn>
{
    protected override StringDataColumn _left => StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Bob", "Charlie"]);

    protected override StringDataColumn _right => StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Bob", "Charlie"]);

    private StringDataColumn _updatedLeft = StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Bob", "Dickie"]);

    private StringDataColumn _updatedRight = StringDataColumn.Cons(StringColumn.Cons("Name"), ["Alice", "Dickie", "Charlie"]);

    protected override (StringDataColumn originalSource, StringDataColumn expectedOriginalTarget, StringDataColumn updatedTarget, StringDataColumn expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, _updatedRight, _updatedRight);

    protected override (StringDataColumn originalSource, StringDataColumn expectedOriginalTarget, StringDataColumn updatedTarget, StringDataColumn expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, _updatedLeft, _updatedLeft);

    protected override ISymmetricLens<StringDataColumn, StringDataColumn> _lens 
        => StringIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Name"), Strings.IdentityLens.Cons());
}