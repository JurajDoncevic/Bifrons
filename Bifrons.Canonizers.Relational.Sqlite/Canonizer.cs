namespace Bifrons.Canonizers.Relational.Sqlite;

/// <summary>
/// Canonizer for SQLite. Canonizer is a class that provides a way to interact with the database.
/// </summary>
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
    /// <param name="metadataManager">Metadata manager.</param>
    /// <param name="queryManager">Query manager.</param>
    /// <param name="commandManager">Command manager.</param>
    private Canonizer(MetadataManager metadataManager, QueryManager queryManager, CommandManager commandManager)
    {
        _metadataManager = metadataManager;
        _queryManager = queryManager;
        _commandManager = commandManager;
    }

    /// <summary>
    /// Constructs a new instance of the SQLite canonizer.
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
        var metadataManager = Sqlite.MetadataManager.Cons(connectionString, useAtomicConnection);
        var queryManager = Sqlite.QueryManager.Cons(connectionString, useAtomicConnection);
        var commandManager = Sqlite.CommandManager.Cons(connectionString, useAtomicConnection);

        var canonizerCreation =
            Sqlite.MetadataManager.Cons(connectionString, useAtomicConnection)
            .Bind(metadataManager => Sqlite.QueryManager.Cons(connectionString, useAtomicConnection).Map(_ => (metadataManager, queryManager: _)))
            .Bind(managers => Sqlite.CommandManager.Cons(connectionString, useAtomicConnection).Map(_ => (managers.metadataManager, managers.queryManager, commandManager: _)))
            .Bind(managers => Canonizer.Cons(managers.metadataManager, managers.queryManager, managers.commandManager));

        return canonizerCreation;
    }

    /// <summary>
    /// Constructs a new instance of the SQLite canonizer.
    /// </summary>
    /// <param name="metadataManager">The metadata manager.</param>
    /// <param name="queryManager">The query manager.</param>
    /// <param name="commandManager">The command manager.</param>
    public static Result<Canonizer> Cons(MetadataManager metadataManager, QueryManager queryManager, CommandManager commandManager)
    {

        if (metadataManager.ConnectionPath == queryManager.ConnectionPath
            && queryManager.ConnectionPath == commandManager.ConnectionPath
            && commandManager.ConnectionPath == metadataManager.ConnectionPath)
        {
            return Result.Failure<Canonizer>("All managers must point to the same server and database.");
        }

        return Result.Success(new Canonizer(metadataManager, queryManager, commandManager));
    }
}
