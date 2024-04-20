using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational;

public interface IQueryManager
{
    Result<TableData> GetAllFrom(Table table);
    Result<RowData> GetFrom(Table table, ColumnData key);
}
