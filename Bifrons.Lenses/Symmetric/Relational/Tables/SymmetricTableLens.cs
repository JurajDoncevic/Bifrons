using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Tables;

public abstract class SymmetricTableLens : BaseSymmetricLens<Table, Table>
{
    public abstract string TargetTableName { get; }

}
