using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational;

public interface IQueryManager
{
    Result<TableData> GetAllFrom(Table table);
    Result<TableData> GetFrom(Table table, ColumnData key);
    Result<TableData> GetFrom(Table table, Func<RowData, bool> predicate);
}
