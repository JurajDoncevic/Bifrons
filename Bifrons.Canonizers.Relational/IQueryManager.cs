using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Canonizers.Relational;

/// <summary>
/// Interface for the query manager. The query manager is responsible for executing data retrieval commands on the database.
/// </summary>
public interface IQueryManager
{
    /// <summary>
    /// Gets all rows from the specified table.
    /// </summary>
    /// <param name="table">The table to get the rows from.</param>
    Result<TableData> GetAllFrom(Table table);

    /// <summary>
    /// Gets a row from the specified table with the specified key.
    /// </summary>
    /// <param name="table">The table to get the row from.</param>
    /// <param name="key">The key of the row to get.</param>
    Result<TableData> GetFrom(Table table, ColumnData key);

    /// <summary>
    /// Gets all rows from the specified table that match the specified predicate.
    /// </summary>
    /// <param name="table">The table to get the rows from.</param>
    /// <param name="predicate">The predicate to match the rows to get.</param>
    Result<TableData> GetFrom(Table table, Func<RowData, bool> predicate);
}
