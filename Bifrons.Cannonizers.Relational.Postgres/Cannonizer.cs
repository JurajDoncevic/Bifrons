namespace Bifrons.Cannonizers.Relational.Postgres;

public class Cannonizer : ICannonizer
{
    private readonly MetadataManager _metadataManager;
    private readonly QueryManager _queryManager;
    private readonly CommandManager _commandManager;

    public ICommandManager CommandManager => throw new NotImplementedException();

    public IQueryManager QueryManager => throw new NotImplementedException();

    public IMetadataManager MetadataManager => throw new NotImplementedException();

    public Cannonizer(MetadataManager metadataManager, QueryManager queryManager, CommandManager commandManager)
    {
        _metadataManager = metadataManager;
        _queryManager = queryManager;
        _commandManager = commandManager;
    }
}
