using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational;

/// <summary>
/// Interface for the command manager. The command manager is responsible for executing data manipulation commands on the database.
/// </summary>
public interface ICommandManager
{
    /// <summary>
    /// Inserts a row into the specified table.
    /// </summary>
    /// <param name="table">The table to insert the row into.</param>
    /// <param name="row">The row to insert.</param>
    Result<Unit> Insert(Table table, RowData row);

    /// <summary>
    /// Updates a row in the specified table according to the specified predicate.
    /// </summary>
    /// <param name="table">The table to update the row in.</param>
    /// <param name="predicate">The predicate to match the row to update.</param>
    /// <param name="row">The row to update the matched row with.</param>
    Result<Unit> Update(Table table, Func<RowData, bool> predicate, RowData row);

    /// <summary>
    /// Deletes a row from the specified table according to the specified predicate.
    /// </summary>
    /// <param name="table">The table to delete the row from.</param>
    /// <param name="predicate">The predicate to match the row to delete.</param>
    Result<Unit> Delete(Table table, Func<RowData, bool> predicate);


    /// <summary>
    /// Synchronizes the specified rows into the database. If a row exists by the identity columns, it will be updated. If it does not exist, it will be inserted. No rows are deleted.
    /// </summary>
    /// <param name="table">The table to synchronize the rows into.</param>
    /// <param name="identityColumns">The identity columns of the table.</param>
    /// <param name="rows">The rows to synchronize.</param>
    Result<Unit> SyncIntoDatabase(Table table, IEnumerable<Column> identityColumns, IEnumerable<RowData> rows);
}
