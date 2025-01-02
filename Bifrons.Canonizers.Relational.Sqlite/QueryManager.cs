using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Microsoft.Data.Sqlite;

namespace Bifrons.Canonizers.Relational.Sqlite;

/// <summary>
/// Query manager for SQLite. Query manager is a class that provides a way to interact with the database.
/// </summary>
public sealed class QueryManager : IQueryManager
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
    private QueryManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connection = new SqliteConnection(connectionString);
    }

    public Result<TableData> GetFrom(Table table, ColumnData key)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM \"{table.Name}\" WHERE \"{key.Name}\" = $value";
                command.Parameters.AddWithValue("$value", key.BoxedData);
                var rowData = new List<RowData>();
                var reader = command.ExecuteReader();
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
                command.CommandText = $"SELECT * FROM \"{table.Name}\"";
                var reader = command.ExecuteReader();
                var rowData = new List<RowData>();
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
                command.CommandText = $"SELECT * FROM \"{table.Name}\"";
                var reader = command.ExecuteReader();
                var rowData = new List<RowData>();
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

    /// <summary>
    /// Constructs an SQLite query manager.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use atomic connection for each operation.</param>
    public static Result<QueryManager> Cons(string connectionString, bool useAtomicConnection = true)
        => Result.AsResult(() =>
        {
            var queryManager = new QueryManager(connectionString, useAtomicConnection);
            return Result.Success(queryManager);
        });
}
