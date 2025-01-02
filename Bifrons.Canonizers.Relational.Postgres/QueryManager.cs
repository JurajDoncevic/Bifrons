using System.Text.RegularExpressions;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Npgsql;

namespace Bifrons.Canonizers.Relational.Postgres;

/// <summary>
/// Query manager for PostgreSQL. Query manager is a class that provides a way to interact with the database.
/// </summary>
public sealed class QueryManager : IQueryManager
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
    private QueryManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connection = new NpgsqlConnection(connectionString);
    }

    public Result<TableData> GetFrom(Table table, ColumnData key)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, connection =>
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM \"{table.Name}\" WHERE \"{key.Name}\" = @keyValue";
                command.Parameters.AddWithValue("keyValue", key.BoxedData ?? DBNull.Value);
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
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM \"{table.Name}\"";
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
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM \"{table.Name}\"";
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

    /// <summary>
    /// Constructs a new instance of the PostgreSQL query manager.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use atomic connection for each operation.</param>
    public static Result<QueryManager> Cons(string connectionString, bool useAtomicConnection = true)
        => Result.AsResult(() =>
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return Result.Failure<QueryManager>("Connection string is required.");
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
                return Result.Failure<QueryManager>("Connection string is invalid.");
            }

            return Result.Success(new QueryManager(connectionString, useAtomicConnection));
        });
}
