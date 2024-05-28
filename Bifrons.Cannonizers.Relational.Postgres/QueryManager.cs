using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class QueryManager : IQueryManager
{
    public Result<TableData> GetAllFrom(Table table)
    {
        throw new NotImplementedException();
    }

    public Result<TableData> GetFrom(Table table, ColumnData key)
    {
        throw new NotImplementedException();
    }

    public Result<TableData> GetFrom(Table table, Func<RowData, bool> predicate)
    {
        throw new NotImplementedException();
    }
}
