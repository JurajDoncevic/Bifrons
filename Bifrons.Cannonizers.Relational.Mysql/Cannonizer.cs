namespace Bifrons.Cannonizers.Relational.Mysql;

public sealed class Cannonizer : ICannonizer
{
    private readonly MetadataManager _metadataManager;
    private readonly QueryManager _queryManager;
    private readonly CommandManager _commandManager;

    public ICommandManager CommandManager => _commandManager;

    public IQueryManager QueryManager => _queryManager;

    public IMetadataManager MetadataManager => _metadataManager;

    public Cannonizer(MetadataManager metadataManager, QueryManager queryManager, CommandManager commandManager)
    {
        _metadataManager = metadataManager;
        _queryManager = queryManager;
        _commandManager = commandManager;
    }

    public Cannonizer(string connectionString, bool useAtomicConnection = true)
    {
        _metadataManager = new MetadataManager(connectionString, useAtomicConnection);
        _queryManager = new QueryManager(connectionString, useAtomicConnection);
        _commandManager = new CommandManager(connectionString, useAtomicConnection);
    }
}
