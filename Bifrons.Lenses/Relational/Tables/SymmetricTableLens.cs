using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

public abstract class SymmetricTableLens : ISymmetricLens<Table, Table>
{
    public abstract string TargetTableName { get; }
    public abstract Func<Table, Option<Table>, Result<Table>> PutLeft { get; }
    public abstract Func<Table, Option<Table>, Result<Table>> PutRight { get; }
    public abstract Func<Table, Result<Table>> CreateRight { get; }
    public abstract Func<Table, Result<Table>> CreateLeft { get; }
}
