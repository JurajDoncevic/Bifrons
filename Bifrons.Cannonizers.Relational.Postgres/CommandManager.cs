using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Npgsql;

namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class CommandManager : ICommandManager
{
    private readonly string _connectionString;
    private readonly NpgsqlConnection _connection;
    private readonly bool _useAtomicConnection;

    public CommandManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new NpgsqlConnection(_connectionString);
    }

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
