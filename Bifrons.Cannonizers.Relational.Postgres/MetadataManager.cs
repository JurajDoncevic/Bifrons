using Bifrons.Lenses.Relational.Model;
using Npgsql;

namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class MetadataManager : IMetadataManager
{

    private readonly string _connectionString;
    private readonly NpgsqlConnection _connection;
    private readonly bool _useAtomicConnection;

    public MetadataManager(string connectionString, bool useAtomicConnection = true)
    {
        _useAtomicConnection = useAtomicConnection;
        _connectionString = connectionString;
        _connection = new NpgsqlConnection(_connectionString);
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
                var getColumnsCommandText = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}'";
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
}
