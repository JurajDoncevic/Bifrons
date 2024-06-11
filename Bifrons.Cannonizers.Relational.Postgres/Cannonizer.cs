namespace Bifrons.Cannonizers.Relational.Postgres;

public sealed class Cannonizer : ICannonizer
{
    private readonly MetadataManager _metadataManager;
    private readonly QueryManager _queryManager;
    private readonly CommandManager _commandManager;

    private readonly bool _useAtomicConnection;

    public ICommandManager CommandManager => throw new NotImplementedException();

    public IQueryManager QueryManager => throw new NotImplementedException();

    public IMetadataManager MetadataManager => throw new NotImplementedException();

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
