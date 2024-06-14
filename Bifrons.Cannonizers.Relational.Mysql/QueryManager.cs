using System.Text.RegularExpressions;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using MySql.Data.MySqlClient;

namespace Bifrons.Cannonizers.Relational.Mysql;

/// <summary>
/// The query manager for MySQL. The query manager is responsible for executing queries to the data store.
/// </summary>
public sealed class QueryManager : IQueryManager
{
    private readonly bool _useAtomicConnection;
    private readonly MySqlConnection _connection;

    /// <summary>
    /// The connection path to the database.
    /// </summary>
    internal string ConnectionPath => _connection.DataSource + "/" + _connection.Database;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use an atomic connection for each operation.</param>
    private QueryManager(string connectionString, bool useAtomicConnection = true)
    {
        _connection = new MySqlConnection(connectionString);
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

    /// <summary>
    /// Constructs a new instance of the MySQL query manager.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use an atomic connection for each operation.</param>
    public static Result<QueryManager> Cons(string connectionString, bool useAtomicConnection = true)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return Result.Failure<QueryManager>("Connection string is required.");
        }

        // Regular expression patterns to check if the connection string contains "Server", "Database", "Uid", and "Pwd"
        var serverPattern = @"Server=.*;?";
        var databasePattern = @"Database=.*;?";
        var uidPattern = @"Uid=.*;?";
        var pwdPattern = @"Pwd=.*;?";

        if (!Regex.IsMatch(connectionString, serverPattern, RegexOptions.IgnoreCase) ||
            !Regex.IsMatch(connectionString, databasePattern, RegexOptions.IgnoreCase) ||
            !Regex.IsMatch(connectionString, uidPattern, RegexOptions.IgnoreCase) ||
            !Regex.IsMatch(connectionString, pwdPattern, RegexOptions.IgnoreCase))
        {
            return Result.Failure<QueryManager>("Connection string is invalid.");
        }

        return Result.Success(new QueryManager(connectionString, useAtomicConnection));
    }
}