using System.Text.RegularExpressions;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Npgsql;

namespace Bifrons.Cannonizers.Relational.Postgres;

/// <summary>
/// The command manager for PostgreSQL. The command manager is responsible for executing commands to the data store.
/// </summary>
public sealed class CommandManager : ICommandManager
{
    private readonly NpgsqlConnection _connection;
    private readonly bool _useAtomicConnection;

    /// <summary>
    /// The connection path to the database.
    /// </summary>
    internal string ConnectionPath => _connection.DataSource + "/" + _connection.Database;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use atomic connection for each operation.</param>
    private CommandManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connection = new NpgsqlConnection(connectionString);
    }

    public Result<Unit> Delete(Table table, Func<RowData, bool> predicate)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                var rowsToDelete = new List<NpgsqlTypes.NpgsqlTid>();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT ctid, * FROM \"{table.Name}\"";

                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var rowColumnData = new List<ColumnData>();
                        var rowid = reader.GetFieldValue<NpgsqlTypes.NpgsqlTid>(0); // rowid is at column 0

                        for (var i = 1; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            var columnOpt = table[fieldName];
                            if (!columnOpt)
                            {
                                return Result.Failure<Unit>($"Column {fieldName} not found in table {table.Name}");
                            }
                            var column = columnOpt.Value;
                            var value = reader.GetValue(i).AdaptFromPostgresValue(column.DataType);

                            var columnDataResult = ColumnData.Cons(column, value);
                            if (!columnDataResult)
                            {
                                return Result.Failure<Unit>(columnDataResult.Message);
                            }
                            rowColumnData.Add(columnDataResult.Data);
                        }
                        var rowData = RowData.Cons(rowColumnData);
                        if (predicate(rowData))
                        {
                            rowsToDelete.Add(rowid);
                        }
                    }
                }
                var rowsDeleted = 0;
                foreach (var rowid in rowsToDelete)
                {
                    using var deleteCommand = connection.CreateCommand();
                    deleteCommand.CommandText = $"DELETE FROM \"{table.Name}\" WHERE ctid = @rowId";
                    deleteCommand.Parameters.AddWithValue("rowId", NpgsqlTypes.NpgsqlDbType.Tid, rowid);
                    rowsDeleted += deleteCommand.ExecuteNonQuery();
                }

                if (rowsDeleted != rowsToDelete.Count)
                {
                    return Result.Failure<Unit>($"Failed to delete {rowsToDelete.Count - rowsDeleted} rows");
                }
                return Result.Success(Unit());
            }));

    public Result<Unit> Insert(Table table, RowData row)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO \"{table.Name}\" ({string.Join(", ", table.Columns.Select(c => $"\"{c.Name}\""))}) VALUES ({string.Join(", ", table.Columns.Select(c => $"@{c.Name}"))})";

                foreach (var column in table.Columns)
                {
                    var colDataOpt = row[column.Name];
                    if (!colDataOpt)
                    {
                        return Result.Failure<Unit>($"Column {column.Name} not found in row of table {table.Name}");
                    }
                    var value = colDataOpt.Value.BoxedData ?? DBNull.Value;
                    command.Parameters.AddWithValue(column.Name, value);
                }

                return command.ExecuteNonQuery() > 0
                    ? Result.Success(Unit())
                    : Result.Failure<Unit>("Failed to insert row");

            }));


    public Result<Unit> Update(Table table, Func<RowData, bool> predicate, RowData row)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                var rowsToUpdate = new List<NpgsqlTypes.NpgsqlTid>();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT ctid, * FROM \"{table.Name}\"";

                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var rowColumnData = new List<ColumnData>();
                        var rowid = reader.GetFieldValue<NpgsqlTypes.NpgsqlTid>(0); // rowid is at column 0

                        for (var i = 1; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            var columnOpt = table[fieldName];
                            if (!columnOpt)
                            {
                                return Result.Failure<Unit>($"Column {fieldName} not found in table {table.Name}");
                            }
                            var column = columnOpt.Value;
                            var value = reader.GetValue(i).AdaptFromPostgresValue(column.DataType);

                            var columnDataResult = ColumnData.Cons(column, value);
                            if (columnDataResult.IsFailure)
                            {
                                return Result.Failure<Unit>(columnDataResult.Message);
                            }
                            rowColumnData.Add(columnDataResult.Data);
                        }
                        var rowData = RowData.Cons(rowColumnData);
                        if (predicate(rowData))
                        {
                            rowsToUpdate.Add(rowid);
                        }
                    }
                }

                var results = new List<Result<Unit>>();
                foreach (var rowid in rowsToUpdate)
                {
                    using var updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = $"UPDATE \"{table.Name}\" SET {string.Join(", ", table.Columns.Select(c => $"\"{c.Name}\" = @{c.Name}"))} WHERE ctid = @rowId";

                    foreach (var column in table.Columns)
                    {
                        var colDataOpt = row[column.Name];
                        if (!colDataOpt)
                        {
                            return Result.Failure<Unit>($"Column {column.Name} not found in row of table {table.Name}");
                        }
                        var value = colDataOpt.Value.BoxedData ?? DBNull.Value;
                        updateCommand.Parameters.AddWithValue(column.Name, value);
                    }
                    updateCommand.Parameters.AddWithValue("rowId", NpgsqlTypes.NpgsqlDbType.Tid, rowid);

                    var result = updateCommand.ExecuteNonQuery() > 0 ? Result.Success(Unit()) : Result.Failure<Unit>("Failed to update row");
                    results.Add(result);
                }
                return results.Unfold()
                    .Map(_ => Unit());
            }));


    public Result<Unit> SyncIntoDatabase(Table table, IEnumerable<Column> identityColumns, IEnumerable<RowData> rows)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                var tableName = table.Name;
                var identityColumnNames = identityColumns.Select(c => c.Name).ToArray();
                var allColumnNames = table.Columns.Select(c => c.Name).ToArray();

                var results = new List<Result<Unit>>();
                foreach (var row in rows)
                {
                    var columnNames = row.ColumnData.Map(cd => cd.Name).ToArray();
                    var columnValues = row.ColumnData.Map(cd => cd.BoxedData).ToArray();

                    var insertColumns = string.Join(", ", columnNames.Map(c => $"\"{c}\""));
                    var insertValues = string.Join(", ", columnNames.Select((_, i) => $"@p{i}"));
                    var updateSet = string.Join(", ", columnNames.Select((col, i) => $"\"{col}\" = @p{i}"));

                    var conflictColumns = string.Join(", ", identityColumnNames.Map(c => $"\"{c}\""));
                    var sql = $@"
                    INSERT INTO {"\"" + tableName + "\""} ({insertColumns})
                    VALUES({insertValues})
                    ON CONFLICT({conflictColumns})
                    DO UPDATE SET {updateSet}; ";

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        for (int i = 0; i < columnValues.Length; i++)
                        {
                            command.Parameters.AddWithValue($"@p{i}", columnValues[i] ?? DBNull.Value);
                        }

                        var result = command.ExecuteNonQuery() > 0 ? Result.Success(Unit()) : Result.Failure<Unit>("Failed to insert or update row");
                        results.Add(result);
                    }
                }

                return results.Unfold().Map(_ => Unit());
            }));

    /// <summary>
    /// Constructs a new instance of the PostgreSQL command manager.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use atomic connection for each operation.</param>
    public static Result<CommandManager> Cons(string connectionString, bool useAtomicConnection = true)
        => Result.AsResult(() =>
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return Result.Failure<CommandManager>("Connection string is required.");
            }

            var hostPattern = @"Host=.*;?";
            var portPattern = @"Port=.*;?";
            var databasePattern = @"Database=.*;?";
            var usernamePattern = @"Username=.*;?";
            var passwordPattern = @"Password=.*;?";

            if (!Regex.IsMatch(connectionString, hostPattern, RegexOptions.IgnoreCase) ||
                !Regex.IsMatch(connectionString, portPattern, RegexOptions.IgnoreCase) ||
                !Regex.IsMatch(connectionString, databasePattern, RegexOptions.IgnoreCase) ||
                !Regex.IsMatch(connectionString, usernamePattern, RegexOptions.IgnoreCase) ||
                !Regex.IsMatch(connectionString, passwordPattern, RegexOptions.IgnoreCase))
            {
                return Result.Failure<CommandManager>("Connection string is invalid.");
            }

            return Result.Success(new CommandManager(connectionString, useAtomicConnection));
        });
}
