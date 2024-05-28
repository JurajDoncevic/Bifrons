using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class CommandManager : ICommandManager
{
    public Result<Unit> Delete(Table table, Func<RowData, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> Insert(Table table, RowData row)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> Update(Table table, Func<RowData, bool> predicate, RowData row)
    {
        throw new NotImplementedException();
    }
}
