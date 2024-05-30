using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Npgsql;

namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class QueryManager : IQueryManager
{

    private readonly string _connectionString;
    private readonly NpgsqlConnection _connection;
    private readonly bool _useAtomicConnection;

    public QueryManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new NpgsqlConnection(_connectionString);
    }

    public Result<TableData> GetFrom(Table table, ColumnData key)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {table.Name} WHERE {key.Name} = $value";
                command.Parameters.AddWithValue("$value", key.BoxedData ?? DBNull.Value);
                var rowData = new List<RowData>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var rowColumnData = new List<ColumnData>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var column = table.Columns[i];
                        var value = reader.GetValue(i).AdaptFromPostgresValue(column.DataType);

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
                        var value = reader.GetValue(i).AdaptFromPostgresValue(column.DataType);

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

    public Result<TableData> GetFrom(Table table, Func<RowData, bool> predicate)
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
                        var value = reader.GetValue(i).AdaptFromPostgresValue(column.DataType);

                        var columnDataResult = ColumnData.Cons(column, value);
                        if (columnDataResult.IsFailure)
                        {
                            return Result.Failure<TableData>(columnDataResult.Message);
                        }
                        rowColumnData.Add(columnDataResult.Data);
                    }
                    var row = RowData.Cons(rowColumnData);
                    if (predicate(row))
                    {
                        rowData.Add(row);
                    }
                }
                return TableData.Cons(table, rowData);
            }));
}
