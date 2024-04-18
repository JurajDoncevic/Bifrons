using Bifrons.Base;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational;

public interface ICommandManager
{
    Result<Unit> Insert(string tableName, RowData row);
    Result<Unit> Update<TKey>(string tableName, TKey key, RowData row);
    Result<Unit> Update(string tableName, Func<RowData, ColumnData> keySelector, RowData row);
    Result<Unit> Update(string tableName, Func<RowData, bool> predicate, RowData row);
    Result<Unit> Delete<TKey>(string tableName, TKey key);
    Result<Unit> Delete(string tableName, Func<RowData, ColumnData> keySelector);
    Result<Unit> Delete(string tableName, Func<RowData, bool> predicate);
}
