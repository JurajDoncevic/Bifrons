using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using MySql.Data.MySqlClient;

namespace Bifrons.Cannonizers.Relational.Mysql;

public sealed class QueryManager : IQueryManager
{
    private readonly string _connectionString;
    private readonly bool _useAtomicConnection;
    private readonly MySqlConnection _connection;

    public QueryManager(string connectionString, bool useAtomicConnection = true)
    {
        _connectionString = connectionString;
        _connection = new MySqlConnection(_connectionString);
        _useAtomicConnection = useAtomicConnection;
    }

    public Result<TableData> GetFrom(Table table, ColumnData key)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM `{table.Name}` WHERE `{key.Name}` = @keyValue";
                command.Parameters.AddWithValue("@keyValue", key.BoxedData ?? DBNull.Value);
                var rowData = new List<RowData>();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var rowColumnData = new List<ColumnData>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        var columnOpt = table[fieldName];
                        if (!columnOpt)
                        {
                            return Result.Failure<TableData>($"Column {fieldName} not found in table {table.Name}");
                        }
                        var column = columnOpt.Value;
                        var value = reader.GetValue(i).AdaptFromMysqlValue(column.DataType);

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
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM `{table.Name}`";
                using var reader = command.ExecuteReader();
                var rowData = new List<RowData>();
                while (reader.Read())
                {
                    var rowColumnData = new List<ColumnData>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        var columnOpt = table[fieldName];
                        if (!columnOpt)
                        {
                            return Result.Failure<TableData>($"Column {fieldName} not found in table {table.Name}");
                        }
                        var column = columnOpt.Value;
                        var value = reader.GetValue(i).AdaptFromMysqlValue(column.DataType);

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
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM `{table.Name}`";
                using var reader = command.ExecuteReader();
                var rowData = new List<RowData>();
                while (reader.Read())
                {
                    var rowColumnData = new List<ColumnData>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        var columnOpt = table[fieldName];
                        if (!columnOpt)
                        {
                            return Result.Failure<TableData>($"Column {fieldName} not found in table {table.Name}");
                        }
                        var column = columnOpt.Value;
                        var value = reader.GetValue(i).AdaptFromMysqlValue(column.DataType);

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