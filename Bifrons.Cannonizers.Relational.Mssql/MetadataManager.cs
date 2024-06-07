using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Cannonizers.Relational.Mssql;

public sealed class MetadataManager : IMetadataManager
{
    private readonly string _connectionString;

    public MetadataManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Result<Unit> CreateTable(Table table)
        => Result.Failure<Unit>("Not implemented");

    public Result<Unit> DropTable(string tableName)
        => Result.Failure<Unit>("Not implemented");

    public Result<IEnumerable<Table>> GetAllTables()
        => Result.Failure<IEnumerable<Table>>("Not implemented");

    public Result<Table> GetTable(string tableName)
        => Result.Failure<Table>("Not implemented");

    public Result<Unit> TableExists(string tableName)
        => Result.Failure<Unit>("Not implemented");
}
