using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational.Mysql;

public sealed class QueryManager : IQueryManager
{
    private readonly string _connectionString;

    public QueryManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Result<TableData> GetAllFrom(Table table)
        => Result.Failure<TableData>("Not implemented");

    public Result<TableData> GetFrom(Table table, ColumnData key)
        => Result.Failure<TableData>("Not implemented");

    public Result<TableData> GetFrom(Table table, Func<RowData, bool> predicate)
        => Result.Failure<TableData>("Not implemented");
}
