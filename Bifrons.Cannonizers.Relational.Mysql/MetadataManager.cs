using System.Text.RegularExpressions;
using Bifrons.Lenses.Relational.Model;
using MySql.Data.MySqlClient;

namespace Bifrons.Cannonizers.Relational.Mysql;

/// <summary>
/// The metadata manager for MySQL. The metadata manager is responsible for managing the metadata of the data store.
/// </summary>
public sealed class MetadataManager : IMetadataManager
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
    private MetadataManager(string connectionString, bool useAtomicConnection = true)
    {
        _connection = new MySqlConnection(connectionString);
        _useAtomicConnection = useAtomicConnection;
    }

    public Result<Unit> CreateTable(Table table)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var columns = table.Columns.Select(c => $"{c.Name} {c.DataType.ToMysqlTypeName()}").ToArray();
                var query = $"CREATE TABLE {table.Name} ({string.Join(", ", columns)})";
                using var command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
                return Result.Success(Unit());
            }));

    public Result<Unit> DropTable(string tableName)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var query = $"DROP TABLE {tableName}";
                using var command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
                return Result.Success(Unit());
            }));

    public Result<IEnumerable<Table>> GetAllTables()
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var tableNames = new List<string>();
                var tablesQuery = "SHOW TABLES";
                using (var tablesCommand = new MySqlCommand(tablesQuery, conn))
                {
                    using var reader = tablesCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var tableName = reader.GetString(0);
                        tableNames.Add(tableName);
                    }
                }
                var tables = new List<Table>();
                // get columns for each tableName
                foreach (var tableName in tableNames)
                {
                    var colsQuery = $"SHOW COLUMNS FROM {tableName}";
                    using var colsCommand = new MySqlCommand(colsQuery, conn);
                    using var colsReader = colsCommand.ExecuteReader();
                    var columns = new List<Column>();
                    while (colsReader.Read())
                    {
                        var name = colsReader.GetString(0);
                        var type = colsReader.GetString(1);
                        var dataType = type.FromMysqlTypeName();
                        columns.Add(Column.Cons(name, dataType));
                    }
                    tables.Add(Table.Cons(tableName, columns));
                }

                return Result.Success(tables.AsEnumerable());

            }));

    public Result<Table> GetTable(string tableName)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var tableQuery = $"SHOW TABLES LIKE '{tableName}'";
                using (var tableCommand = new MySqlCommand(tableQuery, conn))
                {
                    using var tableReader = tableCommand.ExecuteReader();
                    if (!tableReader.HasRows)
                    {
                        return Result.Failure<Table>($"Table {tableName} does not exist");
                    }
                }

                var colsQuery = $"SHOW COLUMNS FROM {tableName}";
                using var colsCommand = new MySqlCommand(colsQuery, conn);
                using var colsReader = colsCommand.ExecuteReader();
                var columns = new List<Column>();
                while (colsReader.Read())
                {
                    var name = colsReader.GetString(0);
                    var type = colsReader.GetString(1);
                    var dataType = type.FromMysqlTypeName();
                    columns.Add(Column.Cons(name, dataType));
                }
                return Result.Success(Table.Cons(tableName, columns));
            }));

    public Result<Unit> TableExists(string tableName)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var query = $"SHOW TABLES LIKE '{tableName}'";
                using var command = new MySqlCommand(query, conn);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return Result.Success(Unit());
                }
                return Result.Failure<Unit>($"Table named {tableName} does not exist");
            }));

    /// <summary>
    /// Constructs a new instance of the MySQL metadata manager.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use atomic connection for each operation.</param>
    public static Result<MetadataManager> Cons(string connectionString, bool useAtomicConnection = true)
        => Result.AsResult(() =>
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return Result.Failure<MetadataManager>("Connection string is required.");
            }

            var serverPattern = @"Server=.*;?";
            var databasePattern = @"Database=.*;?";
            var uidPattern = @"Uid=.*;?";
            var pwdPattern = @"Pwd=.*;?";

            if (!Regex.IsMatch(connectionString, serverPattern, RegexOptions.IgnoreCase) ||
                !Regex.IsMatch(connectionString, databasePattern, RegexOptions.IgnoreCase) ||
                !Regex.IsMatch(connectionString, uidPattern, RegexOptions.IgnoreCase) ||
                !Regex.IsMatch(connectionString, pwdPattern, RegexOptions.IgnoreCase))
            {
                return Result.Failure<MetadataManager>("Connection string is invalid.");
            }

            return Result.Success(new MetadataManager(connectionString, useAtomicConnection));
        });
}
