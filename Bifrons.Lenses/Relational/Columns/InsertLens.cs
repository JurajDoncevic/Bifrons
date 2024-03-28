using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column insert lens. This lens is used to represent a column that is inserted during a transformation.
/// insert: Column[Unit] <=> Column
/// </summary>
public sealed class InsertLens : SymmetricColumnLens
{
    private readonly string _columnName;

    public override string TargetColumnName => _columnName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="columnName">Target column name</param>
    private InsertLens(string columnName)
    {
        _columnName = columnName;
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
        source => Result.Success(Column.Cons(_columnName, source.DataType));

    public override Func<Column, Result<Column>> CreateLeft =>
        source => Result.Success(UnitColumn.Cons(_columnName) as Column);

    /// <summary>
    /// Constructs a new InsertLens
    /// </summary>
    public static InsertLens Cons(string columnName)
        => new(columnName);



}
