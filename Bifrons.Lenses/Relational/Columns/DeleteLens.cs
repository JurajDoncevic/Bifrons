using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column delete lens. This lens is used to represent a column that is deleted during a transformation.
/// delete: Column <=> Column[Unit]
/// </summary>
public sealed class DeleteLens : DisconnectLens
{
    private readonly string _columnName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="columnName">Target column name</param>
    private DeleteLens(string columnName)
        : base(columnName, UnitColumn.DEFAULT_NAME, DataTypes.UNIT, DataTypes.UNIT)
    {
        _columnName = columnName;
    }

    public override string ToString()
    {
        return $"[delete('{_columnName}'): Column <=> Column]";
    }

    /// <summary>
    /// Constructs a new DeleteLens
    /// </summary>
    /// <param name="columnName">Target column name</param>
    public static DeleteLens Cons(string columnName)
        => new(columnName);

}

