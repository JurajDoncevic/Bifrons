using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational.Mysql;

public sealed class CommandManager : ICommandManager
{
    private readonly string _connectionString;

    public CommandManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Result<Unit> Delete(Table table, Func<RowData, bool> predicate)
        => Result.Failure<Unit>("Not implemented");


    public Result<Unit> Insert(Table table, RowData row)
        => Result.Failure<Unit>("Not implemented");

    public Result<Unit> Update(Table table, Func<RowData, bool> predicate, RowData row)
        => Result.Failure<Unit>("Not implemented");
}
