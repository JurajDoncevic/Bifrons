using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column delete lens. This lens is used to represent a column that is deleted during a transformation.
/// delete: Column <=> Column[Unit]
/// </summary>
public sealed class DeleteLens : SymmetricColumnLens
{
    private readonly string _columnName;

    public override string TargetColumnName => _columnName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="columnName">Target column name</param>
    private DeleteLens(string columnName)
    {
        _columnName = columnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                source => Result.Success(source),
                () => Result.Success(UnitColumn.Cons(_columnName) as Column)
                );

    public override Func<Column, Option<Column>, Result<Column>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                source => Result.Success(source),
                () => Result.Success(UnitColumn.Cons(_columnName) as Column)
                );

    public override Func<Column, Result<Column>> CreateRight =>
        source => UnitColumn.Cons(_columnName);

    public override Func<Column, Result<Column>> CreateLeft =>
        source => source;

    /// <summary>
    /// Constructs a new DeleteLens
    /// </summary>
    /// <param name="columnName">Target column name</param>
    public static DeleteLens Cons(string columnName)
        => new(columnName);

}

