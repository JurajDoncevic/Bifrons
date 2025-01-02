using System.Text.RegularExpressions;

namespace Bifrons.Canonizers.Relational.Mysql;

/// <summary>
/// The canonizer to MySQL. The canonizer is responsible for adapting the standard (cannonized) data and structures for and from data store.
public sealed class Canonizer : ICanonizer
{
    private readonly MetadataManager _metadataManager;
    private readonly QueryManager _queryManager;
    private readonly CommandManager _commandManager;

    public ICommandManager CommandManager => _commandManager;

    public IQueryManager QueryManager => _queryManager;

    public IMetadataManager MetadataManager => _metadataManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="metadataManager">The metadata manager.</param>
    /// <param name="queryManager">The query manager.</param>
    /// <param name="commandManager">The command manager.</param>
    private Canonizer(MetadataManager metadataManager, QueryManager queryManager, CommandManager commandManager)
    {
        _metadataManager = metadataManager;
        _queryManager = queryManager;
        _commandManager = commandManager;
    }

    /// <summary>
    /// Constructs a new instance of the MySQL canonizer.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <param name="useAtomicConnection">Whether to use atomic connection for each operation.</param>
    /// <returns>The canonizer.</returns>
    public static Result<Canonizer> Cons(string connectionString, bool useAtomicConnection = true)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return Result.Failure<Canonizer>("Connection string is required.");
        }

        // Regular expression patterns to check if the connection string contains "Server", "Database", "Uid", and "Pwd"
        var serverPattern = @"Server=.*;";
        var databasePattern = @"Database=.*;";
        var uidPattern = @"Uid=.*;";
        var pwdPattern = @"Pwd=.*;";

        if (!Regex.IsMatch(connectionString, serverPattern, RegexOptions.IgnoreCase) ||
            !Regex.IsMatch(connectionString, databasePattern, RegexOptions.IgnoreCase) ||
            !Regex.IsMatch(connectionString, uidPattern, RegexOptions.IgnoreCase) ||
            !Regex.IsMatch(connectionString, pwdPattern, RegexOptions.IgnoreCase))
        {
            return Result.Failure<Canonizer>("Invalid MySQL connection string. It must contain 'Server', 'Database', 'Uid', and 'Pwd'.");
        }

        var metadataManager = Mysql.MetadataManager.Cons(connectionString, useAtomicConnection);
        var queryManager = Mysql.QueryManager.Cons(connectionString, useAtomicConnection);
        var commandManager = Mysql.CommandManager.Cons(connectionString, useAtomicConnection);

        var canonizerCreation =
            Mysql.MetadataManager.Cons(connectionString, useAtomicConnection)
            .Bind(metadataManager => Mysql.QueryManager.Cons(connectionString, useAtomicConnection).Map(_ => (metadataManager, queryManager: _)))
            .Bind(managers => Mysql.CommandManager.Cons(connectionString, useAtomicConnection).Map(_ => (managers.metadataManager, managers.queryManager, commandManager: _)))
            .Bind(managers => Canonizer.Cons(managers.metadataManager, managers.queryManager, managers.commandManager));

        return canonizerCreation;
    }

    /// <summary>
    /// Constructs a new instance of the MySQL canonizer.
    /// </summary>
    /// <param name="metadataManager">The metadata manager.</param>
    /// <param name="queryManager">The query manager.</param>
    /// <param name="commandManager">The command manager.</param>
    public static Result<Canonizer> Cons(MetadataManager metadataManager, QueryManager queryManager, CommandManager commandManager)
    {

        if (metadataManager.ConnectionPath != queryManager.ConnectionPath
            || queryManager.ConnectionPath != commandManager.ConnectionPath
            || commandManager.ConnectionPath != metadataManager.ConnectionPath)
        {
            return Result.Failure<Canonizer>("All managers must point to the same server and database.");
        }
        return Result.Success(new Canonizer(metadataManager, queryManager, commandManager));
    }
}
