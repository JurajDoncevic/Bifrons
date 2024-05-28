using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class MetadataManager : IMetadataManager
{
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
