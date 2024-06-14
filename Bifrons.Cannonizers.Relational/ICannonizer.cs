namespace Bifrons.Cannonizers.Relational;

/// <summary>
/// Interface for the cannonizer. The cannonizer is responsible for adapting the standard (cannonized) data and structures for and from data store.
/// </summary>
public interface ICannonizer
{
    /// <summary>
    /// The command manager.
    /// </summary>
    public ICommandManager CommandManager { get; }

    /// <summary>
    /// The query manager.
    /// </summary>
    public IQueryManager QueryManager { get; }

    /// <summary>
    /// The metadata manager.
    /// </summary>
    public IMetadataManager MetadataManager { get; }
}
