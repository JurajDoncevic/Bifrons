using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column insert lens. This lens is used to represent a column that is inserted during a transformation.
/// insert: Column[Unit] <=> Column
/// </summary>
public sealed class InsertLens : DisconnectLens
{
    private readonly DataTypes _columnDataType;
    private readonly string _columnName;
    /// <summary>
    /// Default data type for the column that is to be inserted on the right
    /// </summary>
    public DataTypes ColumnDataType => _columnDataType;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="columnName">Target column name</param>
    private InsertLens(string columnName, DataTypes columnDataType = DataTypes.UNIT)
        : base(UnitColumn.DEFAULT_NAME, columnName, DataTypes.UNIT, columnDataType)
    {
        _columnDataType = columnDataType;
        _columnName = columnName;
    }

    public override string ToString()
    {
        return $"[insert('{_columnName}'): Column <=> Column]";
    }

    /// <summary>
    /// Constructs a new InsertLens
    /// </summary>
    public static InsertLens Cons(string columnName, DataTypes defaultDataType = DataTypes.UNIT)
        => new(columnName, defaultDataType);



}
