using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

/// <summary>
/// Table delete lens. This lens is used to delete a table.
/// delete: Table <=> Table[Unit]
/// </summary>
public sealed class DeleteLens : DisconnectLens
{
    private readonly string _tableName;
    /// <summary>
    /// Name of the table matched by the lens
    /// </summary>
    public string TableName => _tableName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tableName">Name of the table matched by the lens</param>
    private DeleteLens(string tableName)
        : base(tableName, Table.UNIT_NAME, Table.ConsUnit(tableName), Table.ConsUnit())
    {
        _tableName = tableName;
    }

    /// <summary>
    /// Constructs a new delete lens
    /// </summary>
    /// <param name="tableName">Name of the table matched by the lens</param>
    public static DeleteLens Cons(string tableName)
        => new(tableName);
}
