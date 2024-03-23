using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Tables;

public abstract class SymmetricTableLens : ISimpleSymmetricLens<Table, Table>
{
    public abstract string TargetTableName { get; }
    public abstract Func<Table, Option<Table>, Result<Table>> PutLeft { get; }
    public abstract Func<Table, Option<Table>, Result<Table>> PutRight { get; }
    public abstract Func<Table, Result<Table>> CreateRight { get; }
    public abstract Func<Table, Result<Table>> CreateLeft { get; }
}
