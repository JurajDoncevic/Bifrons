using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Cannonizers.Relational;

public interface IMetadataManager
{
    Result<Table> GetTable(string tableName);
    Result<IEnumerable<Table>> GetAllTables();
    Result<Unit> CreateTable(Table table);
    Result<Unit> DropTable(string tableName);
}
