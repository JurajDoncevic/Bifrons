using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Npgsql;

namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class CommandManager : ICommandManager
{
    private readonly string _connectionString;
    private readonly NpgsqlConnection _connection;
    private readonly bool _useAtomicConnection;

    public CommandManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new NpgsqlConnection(_connectionString);
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
                var updatedRows = 0;
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
}
