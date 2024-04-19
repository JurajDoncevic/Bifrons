using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Microsoft.Data.Sqlite;

namespace Bifrons.Cannonizers.Relational.Sqlite;

public sealed class MetadataManager : IMetadataManager
{
    private readonly string _connectionString;
    private readonly SqliteConnection _connection;
    private readonly bool _useAtomicConnection;

    public MetadataManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new SqliteConnection(_connectionString);
    }

    public Result<Unit> CreateTable(Table table)
        => _connection.WithConnection(_useAtomicConnection, connection =>
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"CREATE TABLE {table.Name} ({string.Join(", ", table.Columns.Select(c => $"{c.Name} {c.DataType.ToSqliteType()}"))})";
            try
            {
                return command.ExecuteNonQuery() > 0
                    ? Result.Success(Unit())
                    : Result.Failure<Unit>("Failed to create table");
            }
            catch (SqliteException e)
            {
                return Result.Exception<Unit>(e);
            }

        });

    public Result<Unit> DropTable(string tableName)
        => _connection.WithConnection(_useAtomicConnection, connection =>
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"DROP TABLE {tableName}";
            try
            {
                return command.ExecuteNonQuery() > 0
                    ? Result.Success(Unit())
                    : Result.Failure<Unit>("Failed to drop table");
            }
            catch (SqliteException e)
            {
                return Result.Exception<Unit>(e);
            }
        });

    public Result<IEnumerable<Table>> GetAllTables()
        => _connection.WithConnection(_useAtomicConnection, connection =>
        {
            var tablesAndColumns = new Dictionary<string, List<(string name, DataTypes dataType)>>();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM sqlite_schema WHERE type='table' AND name NOT LIKE 'sqlite%'";
            try
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tableName = reader.GetString(0);
                    var columns = new List<(string name, DataTypes dataType)>();

                    using var columnCommand = connection.CreateCommand();
                    columnCommand.CommandText = $"PRAGMA table_info({tableName})";
                    using var columnReader = columnCommand.ExecuteReader();
                    while (columnReader.Read())
                    {
                        var colName = columnReader.GetString(1); // Column name is in the second column of the result set
                        var colDataType = Utils.FromDbString(columnReader.GetString(2)).ToDataType(); // Column data type is in the third column of the result set
                        columns.Add((colName, colDataType));
                    }

                    tablesAndColumns[tableName] = columns;
                }
                var tables = tablesAndColumns.Map(kv => Table.Cons(kv.Key, kv.Value.Map(col => Column.Cons(col.name, col.dataType))));

                return Result.Success(tables);
            }
            catch (SqliteException e)
            {
                return Result.Exception<IEnumerable<Table>>(e);
            }
        });

    public Result<Table> GetTable(string tableName)
        => _connection.WithConnection(_useAtomicConnection, connection =>
        {
            var tableExists = TableExists(tableName);
            if (!tableExists)
            {
                return tableExists.Map(_ => (Table)null!);
            }

            using var command = connection.CreateCommand();
            command.CommandText = $"PRAGMA table_info({tableName})";
            try
            {
                using var reader = command.ExecuteReader();
                var columns = new List<(string name, DataTypes dataType)>();
                while (reader.Read())
                {
                    var colName = reader.GetString(1); // Column name is in the second column of the result set
                    var colDataType = Utils.FromDbString(reader.GetString(2)).ToDataType(); // Column data type is in the third column of the result set
                    columns.Add((colName, colDataType));
                }

                return Result.Success(Table.Cons(tableName, columns.Map(col => Column.Cons(col.name, col.dataType))));
            }
            catch (SqliteException e)
            {
                return Result.Exception<Table>(e);
            }
        });

    public Result<Unit> TableExists(string tableName)
        => _connection.WithConnection(_useAtomicConnection, connection =>
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT EXISTS(" +
                "SELECT 1 " +
                "FROM sqlite_schema " +
                "WHERE type = 'table' AND name NOT LIKE 'sqlite%' AND name = $table_name)";

            command.Parameters.AddWithValue("$table_name", tableName);

            try
            {
                using var reader = command.ExecuteReader();

                return reader.Read()
                    ? reader.GetBoolean(0)
                        ? Result.Success(Unit(), $"Table {tableName} exists")
                        : Result.Failure<Unit>($"Table {tableName} doesn't exist")
                    : Result.Failure<Unit>("Failed to get result");

            }
            catch (SqliteException e)
            {
                return Result.Exception<Unit>(e);
            }
        });
}
