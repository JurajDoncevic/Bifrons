using Bifrons.Lenses.Relational.Model;
using Npgsql;

namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class MetadataManager : IMetadataManager
{

    private readonly string _connectionString;
    private readonly NpgsqlConnection _connection;
    private readonly bool _useAtomicConnection;

    public MetadataManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new NpgsqlConnection(_connectionString);
    }

    public Result<Unit> CreateTable(Table table)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> DropTable(string tableName)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<Table>> GetAllTables()
    {
        throw new NotImplementedException();
    }

    public Result<Table> GetTable(string tableName)
    {
        throw new NotImplementedException();
    }

    public Result<Unit> TableExists(string tableName)
    {
        throw new NotImplementedException();
    }
}
