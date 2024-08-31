using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Microsoft.Data.Sqlite;

namespace Bifrons.Cannonizers.Relational.Sqlite;

/// <summary>
/// Command manager for SQLite. Command manager is a class that provides a way to interact with the database.
/// </summary>
public sealed class CommandManager : ICommandManager
{
    private readonly SqliteConnection _connection;
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
        _connection = new SqliteConnection(connectionString);
    }

    public Result<Unit> Delete(Table table, Func<RowData, bool> predicate)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT rowid, * FROM {table.Name}";

                using var reader = command.ExecuteReader();
                var rowsToDelete = new List<long>();
                while (reader.Read())
                {
                    var rowColumnData = new List<ColumnData>();
                    var rowid = reader.GetInt64(0); // rowid is at column 0

                    for (var i = 1; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        var columnOpt = table[fieldName];
                        if (!columnOpt)
                        {
                            return Result.Failure<Unit>($"Column {fieldName} not found in table {table.Name}");
                        }
                        var column = columnOpt.Value;
                        var value = reader.GetValue(i).AdaptFromSqliteValue(column.DataType);

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
                        rowsToDelete.Add(rowid);
                    }
                }

                foreach (var rowid in rowsToDelete)
                {
                    using var deleteCommand = connection.CreateCommand();
                    deleteCommand.CommandText = $"DELETE FROM {table.Name} WHERE rowid = {rowid}";
                    deleteCommand.ExecuteNonQuery();
                }
                return Unit();
            }));


    public Result<Unit> Insert(Table table, RowData row)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                using (var command = connection.CreateCommand())
                {
                    var columnNames = string.Join(", ", table.Columns.Select(c => c.Name));
                    var columnValues = string.Join(", ", table.Columns.Select(c => row[c.Name].Value.BoxedData.AdaptToSqliteValue(c.DataType) ?? "NULL"));
                    command.CommandText = $"INSERT INTO {table.Name} ({columnNames}) VALUES ({columnValues})";

                    return command.ExecuteNonQuery() == 1
                        ? Unit()
                        : Result.Failure<Unit>("Failed to insert row");
                }
            }));

    public Result<Unit> Update(Table table, Func<RowData, bool> predicate, RowData row)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT rowid, * FROM {table.Name}";
                    using (var reader = command.ExecuteReader())
                    {
                        var rowsToUpdate = new List<long>();
                        while (reader.Read())
                        {
                            var rowid = reader.GetInt64(0); // rowid is at column 0
                            var rowColumnData = new List<ColumnData>();
                            for (var i = 1; i < reader.FieldCount; i++)
                            {
                                var fieldName = reader.GetName(i);
                                var columnOpt = table[fieldName];
                                if (!columnOpt)
                                {
                                    return Result.Failure<Unit>($"Column {fieldName} not found in table {table.Name}");
                                }
                                var column = columnOpt.Value;
                                var value = reader.GetValue(i).AdaptFromSqliteValue(column.DataType);

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

                        foreach (var rowid in rowsToUpdate)
                        {
                            using (var updateCommand = connection.CreateCommand())
                            {
                                var setValues = new List<string>();
                                for (var i = 0; i < table.Columns.Count; i++)
                                {
                                    var fieldName = reader.GetName(i);
                                    var columnOpt = table[fieldName];
                                    if (!columnOpt)
                                    {
                                        return Result.Failure<Unit>($"Column {fieldName} not found in table {table.Name}");
                                    }
                                    var column = columnOpt.Value;
                                    var value = row[row[column.Name].Value.Name].Value.BoxedData;
                                    var adaptedValue = value.AdaptToSqliteValue(column.DataType);
                                    setValues.Add($"{column.Name} = {adaptedValue}");
                                }
                                var setClause = string.Join(", ", setValues);
                                updateCommand.CommandText = $"UPDATE {table.Name} SET {setClause} WHERE rowid = {rowid}";
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                return Unit();
            }));


    public Result<Unit> SyncIntoDatabase(Table table, IEnumerable<Column> identityColumns, IEnumerable<RowData> rows)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                var tableName = table.Name;
                var allColumnNames = table.Columns.Select(c => c.Name).ToArray();

                var results = new List<Result<Unit>>();
                foreach (var row in rows)
                {
                    var columnNames = row.ColumnData.Map(cd => cd.Name).ToArray();
                    var columnValues = row.ColumnData.Map(cd => cd.BoxedData).ToArray();

                    var insertColumns = string.Join(", ", columnNames);
                    var insertValues = string.Join(", ", columnNames.Select((_, i) => $"@p{i}"));

                    var sql = $@"
                        INSERT OR REPLACE INTO {tableName} ({insertColumns})
                        VALUES ({insertValues});";

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        for (int i = 0; i < columnValues.Length; i++)
                        {
                            command.Parameters.AddWithValue($"@p{i}", columnValues[i]);
                        }

                        var result = command.ExecuteNonQuery() > 0 ? Result.Success(Unit()) : Result.Failure<Unit>("Failed to insert or replace row");
                        results.Add(result);
                    }
                }

                return results.Unfold().Map(_ => Unit());
            }));

    /// <summary>
    /// Constructs a new instance of the SQLite command manager.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use atomic connection for each operation.</param>
    public static Result<CommandManager> Cons(string connectionString, bool useAtomicConnection = true)
        => Result.AsResult(() =>
        {
            var commandManager = new CommandManager(connectionString, useAtomicConnection);
            return Result.Success(commandManager);
        });
}
