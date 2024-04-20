using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Microsoft.Data.Sqlite;

namespace Bifrons.Cannonizers.Relational.Sqlite;

public sealed class CommandManager : ICommandManager
{

    private readonly string _connectionString;
    private readonly SqliteConnection _connection;
    private readonly bool _useAtomicConnection;

    public CommandManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new SqliteConnection(_connectionString);
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
                        var column = table.Columns[i-1];
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
                            var rowColumnData = new List<ColumnData>();
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                var column = table.Columns[i];
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
                                rowsToUpdate.Add(reader.GetInt64(0)); // rowid is at column 0
                            }
                        }

                        foreach (var rowid in rowsToUpdate)
                        {
                            using (var updateCommand = connection.CreateCommand())
                            {
                                var setValues = new List<string>();
                                for (var i = 0; i < table.Columns.Count; i++)
                                {
                                    var column = table.Columns[i];
                                    var value = row[row[column.Name].Value.Name].Value;
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
}
