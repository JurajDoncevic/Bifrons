using Bifrons.Base;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational;

public interface IQueryManager
{
    Result<TableData> GetAll(string tableName);
    Result<RowData> Get<TKey>(string tableName, TKey key);
    Result<RowData> Get(string tableName, Func<RowData, ColumnData> keySelector);
    Result<RowData> Get(string tableName, Func<RowData, bool> predicate);
}
