using System.Text.RegularExpressions;
using Bifrons.Lenses.Relational.Model;
using Npgsql;

namespace Bifrons.Cannonizers.Relational.Postgres;

/// <summary>
/// Metadata manager for PostgreSQL. Metadata manager is a class that provides a way to interact with the database metadata.
/// </summary>
public sealed class MetadataManager : IMetadataManager
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
    private MetadataManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connection = new NpgsqlConnection(connectionString);
    }

    public Result<Unit> CreateTable(Table table)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var commandText =
                    $"CREATE TABLE \"{table.Name}\" ("
                    + string.Join(", ", table.Columns.Select(c => $"\"{c.Name}\" {c.DataType.ToPostgresTypeName()}"))
                    + ")";

                using var command = conn.CreateCommand();
                command.CommandText = commandText;

                command.ExecuteNonQuery();

                return Result.Success(Unit());
            }));

    public Result<Unit> DropTable(string tableName)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var commandText = $"DROP TABLE \"{tableName}\"";

                using var command = conn.CreateCommand();
                command.CommandText = commandText;

                command.ExecuteNonQuery();

                return Result.Success(Unit());
            }));

    public Result<IEnumerable<Table>> GetAllTables()
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var tableNames = new List<string>();
                var getTablesCommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'";
                using (var getTablesCommand = conn.CreateCommand())
                {
                    getTablesCommand.CommandText = getTablesCommandText;
                    using var tableReader = getTablesCommand.ExecuteReader();

                    while (tableReader.Read())
                    {
                        tableNames.Add(tableReader.GetFieldValue<string>(0));
                    }
                }

                var tables = new List<Table>();
                foreach (var tableName in tableNames)
                {
                    var getColumnsCommandText = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}'";
                    using var getColumnsCommand = conn.CreateCommand();
                    getColumnsCommand.CommandText = getColumnsCommandText;

                    using var columnsReader = getColumnsCommand.ExecuteReader();

                    var columns = new List<Column>();
                    while (columnsReader.Read())
                    {
                        columns.Add(Column.Cons(columnsReader.GetFieldValue<string>(0), columnsReader.GetFieldValue<string>(1).FromPostgresTypeName()));
                    }

                    tables.Add(Table.Cons(tableName, columns));
                }

                return Result.Success(tables.AsEnumerable());

            }));

    public Result<Table> GetTable(string tableName)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                if(!TableExists(tableName))
                {
                    return Result.Failure<Table>($"Table {tableName} does not exist.");
                }
                
                var getColumnsCommandText = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}' ORDER BY ordinal_position";
                using var getColumnsCommand = new NpgsqlCommand(getColumnsCommandText, conn);

                using var columnsReader = getColumnsCommand.ExecuteReader();

                var columns = new List<Column>();
                while (columnsReader.Read())
                {
                    columns.Add(Column.Cons(columnsReader.GetFieldValue<string>(0), columnsReader.GetFieldValue<string>(1).FromPostgresTypeName()));
                }

                return Result.Success(Table.Cons(tableName, columns));

            }));

    public Result<Unit> TableExists(string tableName)
        => Result.AsResult(() =>
            _connection.WithConnection(_useAtomicConnection, conn =>
            {
                var getTablesCommandText = $"SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_name = '{tableName}' LIMIT 1";
                using var getTablesCommand = new NpgsqlCommand(getTablesCommandText, conn);

                using var reader = getTablesCommand.ExecuteReader();

                return reader.HasRows
                    ? Result.Success(Unit())
                    : Result.Failure<Unit>($"Table {tableName} does not exist.");

            }));

    /// <summary>
    /// Constructs a new instance of the PostgreSQL metadata manager.
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
                return Result.Failure<MetadataManager>("Connection string is invalid.");
            }

            return Result.Success(new MetadataManager(connectionString, useAtomicConnection));
        });
}
