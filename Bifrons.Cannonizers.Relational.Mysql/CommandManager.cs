using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using MySql.Data.MySqlClient;

namespace Bifrons.Cannonizers.Relational.Mysql;

public sealed class CommandManager : ICommandManager
{
    private readonly string _connectionString;
    private readonly bool _useAtomicConnection;
    private readonly MySqlConnection _connection;

    public CommandManager(string connectionString, bool useAtomicConnection = true)
    {
        _connectionString = connectionString;
        _connection = new MySqlConnection(_connectionString);
        _useAtomicConnection = useAtomicConnection;
    }

    public Result<Unit> Delete(Table table, Func<RowData, bool> predicate)
    => Result.AsResult(() =>
        _connection.WithConnection(_useAtomicConnection, connection =>
        {
            var rowsToDelete = new List<string>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"
                SELECT 
                    SHA1(CONCAT({string.Join(", ", table.Columns.Select(c => $"`{table.Name}`.`{c.Name}`"))})) AS row_hash,
                    {string.Join(", ", table.Columns.Select(c => $"`{table.Name}`.`{c.Name}`"))} 
                FROM `{table.Name}`";
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var rowHash = reader.GetString(0);
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
                        var value = reader.GetValue(i).AdaptFromMysqlValue(column.DataType);

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
                        rowsToDelete.Add($"'{rowHash}'");
                    }
                }
            }

            using var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = $@"
            DELETE FROM `{table.Name}`
            WHERE SHA1(CONCAT({string.Join(", ", table.Columns.Select(c => $"`{c.Name}`"))})) IN ({string.Join(", ", rowsToDelete)})";
            
            var rowsDeleted = deleteCommand.ExecuteNonQuery();
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
                command.CommandText = $"INSERT INTO `{table.Name}` ({string.Join(", ", table.Columns.Select(c => $"`{c.Name}`"))}) VALUES ({string.Join(", ", table.Columns.Select(c => $"@{c.Name}"))})";

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
                var rowsToUpdate = new List<string>();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                    SELECT 
                        SHA1(CONCAT({string.Join(", ", table.Columns.Select(c => $"`{table.Name}`.`{c.Name}`"))})) AS row_hash,
                        {string.Join(", ", table.Columns.Select(c => $"`{table.Name}`.`{c.Name}`"))} 
                    FROM `{table.Name}`";

                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var rowColumnData = new List<ColumnData>();
                        var rowHash = reader.GetString(0); // rowid is at column 0

                        for (var i = 1; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            var columnOpt = table[fieldName];
                            if (!columnOpt)
                            {
                                return Result.Failure<Unit>($"Column {fieldName} not found in table {table.Name}");
                            }
                            var column = columnOpt.Value;
                            var value = reader.GetValue(i).AdaptFromMysqlValue(column.DataType);

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
                            rowsToUpdate.Add(rowHash);
                        }
                    }
                }
                using var updateCommand = connection.CreateCommand();
                updateCommand.CommandText = $@"
                    UPDATE `{table.Name}` 
                    SET {string.Join(", ", table.Columns.Select(c => $"{c.Name} = @{c.Name}"))}
                    WHERE SHA1(CONCAT({string.Join(", ", table.Columns.Select(c => $"`{c.Name}`"))})) IN ({string.Join(", ", rowsToUpdate)})";
                
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

                var rowsUpdated = updateCommand.ExecuteNonQuery();
                if (rowsUpdated != rowsToUpdate.Count)
                {
                    return Result.Failure<Unit>($"Failed to update {rowsToUpdate.Count - rowsUpdated} rows");
                }
                return Result.Success(Unit());
            }));
}
