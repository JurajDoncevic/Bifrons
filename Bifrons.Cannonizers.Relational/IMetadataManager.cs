using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Cannonizers.Relational;

/// <summary>
/// Interface for the metadata manager. The metadata manager is responsible for managing the metamodel of the database; primarily table structures.
/// </summary>
public interface IMetadataManager
{
    /// <summary>
    /// Checks if a table with the specified name exists in the database.
    /// </summary>
    /// <param name="tableName">The name of the table to check for.</param>
    Result<Unit> TableExists(string tableName);

    /// <summary>
    /// Gets the table with the specified name.
    /// </summary>
    /// <param name="tableName">The name of the table to get.</param>
    Result<Table> GetTable(string tableName);

    /// <summary>
    /// Gets all tables in the database.
    /// </summary>
    Result<IEnumerable<Table>> GetAllTables();

    /// <summary>
    /// Creates a table with the specified structure.
    /// </summary>
    /// <param name="table">The structure of the table to create.</param>
    Result<Unit> CreateTable(Table table);

    /// <summary>
    /// Drops the table with the specified name.
    /// </summary>
    /// <param name="tableName">The name of the table to drop.</param>
    Result<Unit> DropTable(string tableName);
}
