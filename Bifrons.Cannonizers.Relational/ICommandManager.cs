using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational;

public interface ICommandManager
{
    Result<Unit> Insert(Table table, RowData row);
    Result<Unit> Update(Table table, Func<RowData, bool> predicate, RowData row);
    Result<Unit> Delete(Table table, Func<RowData, bool> predicate);
}
