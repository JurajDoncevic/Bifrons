using Bifrons.Lenses.Symmetric;
using Bifrons.Lenses.Symmetric.Relational.Columns;
using Bifrons.Lenses.Symmetric.Relational.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Relational.Columns.Tests;

public class IdentityLensTests : SymmetricLensTestingFramework<Column, Column>
{
    protected override Column _left => Column.Cons("StringColumn", DataTypes.STRING);

    protected override Column _right => Column.Cons("StringColumn", DataTypes.STRING);

    protected override (Column originalSource, Column expectedOriginalTarget, Column updatedTarget, Column expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => (Column.Cons("StringColumn", DataTypes.STRING), Column.Cons("StringColumn", DataTypes.STRING), Column.Cons("StringColumn", DataTypes.STRING), Column.Cons("StringColumn", DataTypes.STRING));

    protected override (Column originalSource, Column expectedOriginalTarget, Column updatedTarget, Column expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => (Column.Cons("StringColumn", DataTypes.STRING), Column.Cons("StringColumn", DataTypes.STRING), Column.Cons("StringColumn", DataTypes.STRING), Column.Cons("StringColumn", DataTypes.STRING));

    protected override ISymmetricLens<Column, Column> _lens => IdentityLens.Cons("StringColumn");
}
