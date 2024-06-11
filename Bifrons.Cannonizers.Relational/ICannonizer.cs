namespace Bifrons.Cannonizers.Relational;

public interface ICannonizer
{
    public ICommandManager CommandManager { get; }
    public IQueryManager QueryManager { get; }
    public IMetadataManager MetadataManager { get; }
}
