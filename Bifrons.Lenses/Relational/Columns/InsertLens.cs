using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column insert lens. This lens is used to represent a column that is inserted during a transformation.
/// insert: Column[Unit] <=> Column
/// </summary>
public sealed class InsertLens : SymmetricColumnLens
{
    private readonly string _columnName;
    private readonly DataTypes _defaultDataType;
    public override string TargetColumnName => _columnName;
    /// <summary>
    /// Default data type for the column that is to be inserted on the right
    /// </summary>
    public DataTypes DefaultDataType => _defaultDataType;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="columnName">Target column name</param>
    private InsertLens(string columnName, DataTypes defaultDataType = DataTypes.UNIT)
    {
        _columnName = columnName;
        _defaultDataType = defaultDataType;
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
        source => Result.Success(Column.Cons(_columnName, _defaultDataType));

    public override Func<Column, Result<Column>> CreateLeft =>
        source => Result.Success(UnitColumn.Cons(_columnName) as Column);

    /// <summary>
    /// Constructs a new InsertLens
    /// </summary>
    public static InsertLens Cons(string columnName, DataTypes defaultDataType = DataTypes.UNIT)
        => new(columnName, defaultDataType);



}
