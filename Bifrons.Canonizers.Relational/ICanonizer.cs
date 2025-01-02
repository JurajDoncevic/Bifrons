namespace Bifrons.Canonizers.Relational;

/// <summary>
/// Interface for the canonizer. The canonizer is responsible for adapting the standard (cannonized) data and structures for and from data store.
/// </summary>
public interface ICanonizer
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
