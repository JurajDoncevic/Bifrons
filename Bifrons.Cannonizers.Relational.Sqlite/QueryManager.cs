using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Microsoft.Data.Sqlite;

namespace Bifrons.Cannonizers.Relational.Sqlite;

public sealed class QueryManager : IQueryManager
{
    private readonly string _connectionString;
    private readonly SqliteConnection _connection;
    private readonly bool _useAtomicConnection;

    public QueryManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new SqliteConnection(_connectionString);
    }

    public Result<RowData> GetFrom(Table table, ColumnData key)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {table.Name} WHERE {key.Name} = $value";
                command.Parameters.AddWithValue("$value", key.BoxedData);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var rowColumnData = new List<ColumnData>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var column = table.Columns[i];
                        var value = reader.GetValue(i).AdaptSqliteValue(column.DataType);

                        var columnDataResult = ColumnData.Cons(column, value);
                        if (columnDataResult.IsFailure)
                        {
                            return Result.Failure<RowData>(columnDataResult.Message);
                        }
                        rowColumnData.Add(columnDataResult.Data);
                    }
                    return RowData.Cons(rowColumnData);
                } else {
                    return Result.Failure<RowData>($"No row found in table {table.Name} with key {key}");
                }
            }));

    public Result<TableData> GetAllFrom(Table table)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {table.Name}";
                var reader = command.ExecuteReader();
                var rowData = new List<RowData>();
                while (reader.Read())
                {
                    var rowColumnData = new List<ColumnData>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var column = table.Columns[i];
                        var value = reader.GetValue(i).AdaptSqliteValue(column.DataType);

                        var columnDataResult = ColumnData.Cons(column, value);
                        if (columnDataResult.IsFailure)
                        {
                            return Result.Failure<TableData>(columnDataResult.Message);
                        }
                        rowColumnData.Add(columnDataResult.Data);
                    }
                    rowData.Add(RowData.Cons(rowColumnData));
                }
                return TableData.Cons(table, rowData);
            }));
}
