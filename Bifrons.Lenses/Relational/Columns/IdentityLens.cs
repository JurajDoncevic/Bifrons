using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column identity lens. This lens is used to represent a column that is not changed during a transformation.
/// id: Column <=> Column
/// </summary>
public class IdentityLens : SymmetricColumnLens
{
    private readonly string _columnName;

    public override string MatchesColumnNameLeft => _columnName;
    public override string MatchesColumnNameRight => _columnName;

    public override bool MatchesLeft => true;

    public override bool MatchesRight => true;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="columnName">Target column name</param>
    protected IdentityLens(string columnName)
    {
        _columnName = columnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => Result.Success(Column.Cons(_columnName, updatedSource.DataType))
                );

    public override Func<Column, Option<Column>, Result<Column>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => Result.Success(Column.Cons(_columnName, updatedSource.DataType))
                );

    public override Func<Column, Result<Column>> CreateRight =>
        source => Result.Success(Column.Cons(_columnName, source.DataType));

    public override Func<Column, Result<Column>> CreateLeft =>
        source => Result.Success(Column.Cons(_columnName, source.DataType));

    /// <summary>
    /// Constructs a new IdentityLens
    /// </summary>
    /// <param name="columnName"></param>
    /// <returns></returns>
    public static IdentityLens Cons(string columnName)
        => new(columnName);
}