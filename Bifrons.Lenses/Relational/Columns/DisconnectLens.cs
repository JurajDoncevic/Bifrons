using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column disconnect lens. This lens is used to represent a column that is disconnected during a transformation.
/// disconnect: Column <=> Column
/// </summary>
public class DisconnectLens : SymmetricColumnLens
{
    private readonly DataTypes _leftDataTypeDefault;
    private readonly DataTypes _rightDataTypeDefault;
    protected readonly string _leftColumnName;
    protected readonly string _rightColumnName;

    public override string MatchesColumnNameLeft => _leftColumnName;
    public override string MatchesColumnNameRight => _rightColumnName;

    public override bool MatchesLeft => !_leftColumnName.Equals(UnitColumn.DEFAULT_NAME);

    public override bool MatchesRight => !_rightColumnName.Equals(UnitColumn.DEFAULT_NAME);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="columnName">Target column name</param>
    /// <param name="leftDefault">Default column for the left side</param>
    /// <param name="rightDefault">Default column for the right side</param>
    protected DisconnectLens(string leftColumnName, string rightColumnName, DataTypes leftDataTypeDefault, DataTypes rightDataTypeDefault)
    {
        _leftColumnName = leftColumnName;
        _rightColumnName = rightColumnName;
        _leftDataTypeDefault = leftDataTypeDefault;
        _rightDataTypeDefault = rightDataTypeDefault;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
            );

    public override Func<Column, Option<Column>, Result<Column>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
            );
    public override Func<Column, Result<Column>> CreateRight =>
        source => Result.Success(Column.Cons(_rightColumnName, _rightDataTypeDefault));

    public override Func<Column, Result<Column>> CreateLeft =>
        source => Result.Success(Column.Cons(_leftColumnName, _leftDataTypeDefault));

    public override string ToString()
    {
        return $"[disconnect('{_leftColumnName}'|'{_rightColumnName}'): Column <=> Column]";
    }


    /// <summary>
    /// Constructs a new DisconnectLens
    /// </summary>
    /// <param name="leftColumnName">Left column name</param>
    /// <param name="rightColumnName">Right column name</param>
    /// <param name="leftDefault">Default data type for the left column</param>
    /// <param name="rightDefault">Default data type for the right column</param>
    public static DisconnectLens Cons(string leftColumnName, string rightColumnName, DataTypes leftDefault = DataTypes.UNIT, DataTypes rightDefault = DataTypes.UNIT)
        => new(leftColumnName, rightColumnName, leftDefault, rightDefault);
}
