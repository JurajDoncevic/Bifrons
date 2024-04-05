using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public abstract class SymmetricDataTableLens : ISymmetricLens<DataTable, DataTable>
{
    public abstract Func<DataTable, Option<DataTable>, Result<DataTable>> PutLeft { get; }
    public abstract Func<DataTable, Option<DataTable>, Result<DataTable>> PutRight { get; }
    public abstract Func<DataTable, Result<DataTable>> CreateRight { get; }
    public abstract Func<DataTable, Result<DataTable>> CreateLeft { get; }
}