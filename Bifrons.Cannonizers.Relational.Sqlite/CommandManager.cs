using Bifrons.Lenses.RelationalData.Model;
using Microsoft.Data.Sqlite;

namespace Bifrons.Cannonizers.Relational.Sqlite;

public sealed class CommandManager : ICommandManager
{

    private readonly string _connectionString;
    private readonly SqliteConnection _connection;
    private readonly bool _useAtomicConnection;

    public CommandManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new SqliteConnection(_connectionString);
    }

    public Result<Unit> Delete<TKey>(string tableName, TKey key)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> Delete(string tableName, Func<RowData, ColumnData> keySelector)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> Delete(string tableName, Func<RowData, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> Insert(string tableName, RowData row)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> Update<TKey>(string tableName, TKey key, RowData row)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> Update(string tableName, Func<RowData, ColumnData> keySelector, RowData row)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> Update(string tableName, Func<RowData, bool> predicate, RowData row)
    {
        throw new NotImplementedException();
    }
}
